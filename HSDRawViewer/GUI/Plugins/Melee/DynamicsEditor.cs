using HSDRaw;
using HSDRaw.Common;
using HSDRaw.Melee.Pl;
using HSDRawViewer.Properties;
using HSDRawViewer.Rendering;
using HSDRawViewer.Rendering.Models;
using HSDRawViewer.Rendering.Renderers;
using HSDRawViewer.Rendering.Widgets;
using HSDRawViewer.Tools.Physics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HSDRawViewer.GUI.Plugins.Melee
{
    [SupportedTypes(new Type[] { typeof(SBM_PhysicsGroup) })]
    public partial class DynamicsEditor : PluginBase, IDrawableInterface
    {
        public class Settings
        {
            [Category("Bone Chain")]
            [DisplayName("Bone Radius")]
            [Description("The radius of each selected bone sphere.")]
            public float BoneRadius { get; set; } = 1;

            [Category("Bone Chain")]
            [DisplayName("Selected Color")]
            [Description("")]
            public Color BoneSelected { get; set; } = Color.Yellow;

            [Category("Bone Chain")]
            [DisplayName("Color1")]
            [Description("")]
            public Color BoneColor1 { get; set; } = Color.White;

            [Category("Bone Chain")]
            [DisplayName("Color2")]
            [Description("")]
            public Color BoneColor2 { get; set; } = Color.Red;

            public Vector3 GravityDirection { get; set; } = -Vector3.UnitY;
        }

        public readonly Settings _settings = new Settings();

        public override DataNode Node
        {
            get => _node; set
            {
                _node = value;
                Physics = value.Accessor as SBM_PhysicsGroup;
                LoadData();

                if (value.Parent is DataNode parent &&
                    parent.Accessor is SBM_FighterData ft &&
                    ft.MetalModel != null)
                {
                    LoadModel(ft.MetalModel);
                }
            }
        }

        private readonly ViewportControl viewport;

        private SBM_PhysicsGroup Physics;

        public DynamicGroup[] groups { get; set; }
        public SBM_DynamicHitBubble[] bubbles { get; set; }

        private GLTextRenderer text;
        private readonly RenderJObj RenderJObj = new();

        private List<Dynamics> dynamics = new List<Dynamics>();

        public DrawOrder DrawOrder => DrawOrder.First;

        private DataNode _node;

        // widgets
        private List<IWidget> Widgets = new List<IWidget>();
        private TranslationWidget _transW;
        //private ArcballWidget _arcball;

        private bool _isHitbubbleTransparent { get; set; } = true;
        private bool _isHitbubbleVisible { get; set; } = true;
        private bool _isSelectedChainVisible { get; set; } = true;
        public bool _isControlGravity { get; set; } = true;
        public bool _isSimulation { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        private void Button_UpdateVisibility(object sender, EventArgs args)
        {
            RenderJObj._settings.RenderBones = toggleViewBones.Checked;
            _isHitbubbleVisible = toggleHitbubbles.Checked;
            _isSelectedChainVisible = toggleBoneChain.Checked;
            _isControlGravity = toggleControlGravity.Checked;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="joint"></param>
        /// <returns></returns>
        private Matrix4 GetJointMatrix(int joint)
        {
            if (RenderJObj.RootJObj != null)
                return RenderJObj.RootJObj.GetJObjAtIndex(joint).WorldTransform;

            return Matrix4.Identity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        private void InitDynamics(DynamicGroup g)
        {
            Dynamics d = new Dynamics();

            // init params
            d.StartBone = g.BoneIndex;
            d.StiffnessMultiplier = g.StiffnessMultiplier;
            d.BoneLengthScaleFactor = g.GravityMultiplier;
            d.DampingMultiplier = g.DragMultiplier;

            // init bones
            if (RenderJObj.RootJObj != null)
            {
                for (int i = 0; i < g._chain.Length; i++)
                {
                    var b = g._chain[i];
                    LiveJObj joint = RenderJObj.RootJObj.GetJObjAtIndex(g.BoneIndex + i);
                    if (joint == null)
                        break;

                    d.Bones.Add(new DynamicJoint(joint)
                    {
                        FollowDamping = b.FollowDamping,
                        RestPoseStiffness = b.Stiffness,
                        RestRotation = new Vector3(b.RotX, b.RotY, b.RotZ),
                        RotationLimit = b.RotationLimit,
                        InertiaDamping = b.InertiaDamping,
                        Resistance = b.Resistance,
                        WorldPosition = joint.WorldTransform.ExtractTranslation(),
                    });
                }
                d.CalculateBoneLengthsAndAxes();
            }

            dynamics.Add(d);
        }
        /// <summary>
        /// 
        /// </summary>
        private void RefreshDynamics()
        {
            dynamics.Clear();
            if (!_isSimulation)
            {
                RenderJObj.ResetDefaultStateAll();
                return;
            }
            foreach (var d in groups)
                InitDynamics(d);
        }
        /// <summary>
        /// 
        /// </summary>
        public DynamicsEditor()
        {
            InitializeComponent();

            _transW = new TranslationWidget();
            _transW.Transform = Matrix4.CreateTranslation(0, 20, 0);
            Widgets.Add(_transW);

            //_arcball = new ArcballWidget();
            //Widgets.Add(_arcball);

            text = new GLTextRenderer();

            viewport = new ViewportControl();
            viewport.Dock = DockStyle.Fill;
            viewport.AnimationTrackEnabled = false;
            viewport.AddRenderer(this);
            viewport.DisplayGrid = true;

            groupBoxViewport.Controls.Add(viewport);
            viewport.RefreshSize();
            viewport.BringToFront();

            viewport.FrameChange += (f) =>
            {
                RenderJObj.RequestAnimationUpdate(FrameFlags.All, f);
            };

            arrayGroup.SelectedObjectChanged += (e, s) =>
            {
                arrayEditor.SetArrayFromProperty(arrayGroup.SelectedObject, nameof(DynamicGroup._chain));

                // reset model
                RenderJObj.ResetDefaultStateAll();

                // init dynamics render
                RefreshDynamics();
            };

            arrayGroup.ArrayUpdated += (e, s) =>
            {
                RefreshDynamics();
            };

            arrayEditor.ArrayUpdated += (e, s) =>
            {
                RefreshDynamics();
            };

            viewport.Visible = false;

            dropDownView.DropDown.Closing += (s, e) =>
            {
                if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                    e.Cancel = true;
            };

            FormClosing += (sender, args) =>
            {
                viewport.Dispose();
            };
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadModel(HSD_JOBJ model)
        {
            RenderJObj.LoadJObj(model);
            viewport.Visible = true;
            loadAnimationToolStripMenuItem.Enabled = true;
            RefreshDynamics();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        public void Draw(Camera cam, int windowWidth, int windowHeight)
        {
            // TODO: chain debug info (Follow Decay, Stiffness, Limit, Inertia Decay, Resistenace)

            // process physics
            if (RenderJObj.RootJObj != null)
            {
                Vector3 gravity = _settings.GravityDirection;

                if (_isControlGravity)
                {
                    Vector3 start = _transW.Transform.ExtractTranslation();
                    Vector3 end = Vector3.Zero;
                    gravity = (end - start).Normalized();
                }

                text.RenderText($"D: ({gravity.X}, {gravity.Y}, {gravity.Z})", windowWidth, windowHeight, 0, 0, StringAlignment.Near, true);

                //dynamics.Wind = _transW.Transform.ExtractTranslation();
                foreach (var d in dynamics)
                {
                    d.Gravity = gravity;
                    d.Think(RenderJObj.RootJObj, bubbles);
                }
            }

            // render model
            RenderJObj.Render(cam);

            GL.Disable(EnableCap.DepthTest);

            // selected bubble
            if (_isHitbubbleVisible)
            {
                var sel = arrayHitbubble.SelectedObject as SBM_DynamicHitBubble;
                foreach (var b in bubbles)
                {
                    Matrix4 joint = RenderJObj.RootJObj.GetJObjAtIndex(b.BoneIndex).WorldTransform;
                    joint *= Matrix4.CreateTranslation(b.X, b.Y, b.Z);
                    DrawShape.DrawSphere(joint, b.Size, 8, 8,
                        sel == b ? _settings.BoneSelected.ToTKVector() : Vector3.UnitZ,
                        _isHitbubbleTransparent ? 0.5f : 1f);
                }
            }

            if (_isSelectedChainVisible)
                DrawPhysicsChain();

            // render widgets
            if (_isControlGravity)
                DrawGravityVector(cam);
        }
        /// <summary>
        /// 
        /// </summary>
        private void DrawPhysicsChain()
        {
            if (RenderJObj.RootJObj == null ||
                arrayGroup.SelectedObject is not DynamicGroup group)
                return;

            var start_bone = RenderJObj.RootJObj.GetJObjAtIndex(group.BoneIndex);

            Vector3 startColor = _settings.BoneColor1.ToTKVector();
            Vector3 endColor = _settings.BoneColor2.ToTKVector();
            Vector3 selectedColor = _settings.BoneSelected.ToTKVector();
            var selected = arrayEditor.SelectedIndex;

            // Draw Lines
            var bone = start_bone;
            GL.LineWidth(2 * 2);
            for (int i = 0;
                bone != null &&
                i < group._chain.Length;
                i++, bone = bone.Child)
            {
                if (bone.Parent != null)
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color3(startColor);
                    GL.Vertex3(bone.Parent.WorldTransform.ExtractTranslation());
                    GL.Color3(endColor);
                    GL.Vertex3(bone.WorldTransform.ExtractTranslation());
                    GL.End();
                }
            }

            // Draw Bone Markers
            bone = start_bone;
            for (int i = 0;
                bone != null &&
                i < group._chain.Length;
                i++, bone = bone.Child)
            {
                bool sel = (i == selected);
                DrawShape.DrawSphere(bone.WorldTransform, _settings.BoneRadius, 8, 8, sel ? selectedColor : startColor, 0.5f);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cam"></param>
        private void DrawGravityVector(Camera cam)
        {
            _transW.Render(cam);
            text.RenderText(cam, "Force", Matrix4.CreateTranslation(Vector3.UnitY) * _transW.Transform, StringAlignment.Center, true);

            Vector3 start = _transW.Transform.ExtractTranslation();
            Vector3 end = start - start.Normalized() * 3;
            Vector3 arrowColor = new Vector3(1, 1, 0);

            GL.LineWidth(2 * 2);
            GL.Color3(arrowColor);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(end);
            GL.Vertex3(start);
            GL.End();

            GL.PushMatrix();

            Matrix4 rot = Math3D.CreateRotationFromDirection((end - start).Normalized());
            Matrix4 trans = rot * Matrix4.CreateTranslation(end);

            GL.MultMatrix(ref trans);

            DrawShape.DrawCone(
                3 * 0.25f,
                3 * 0.08f,
                16,
                arrowColor);

            GL.PopMatrix();
        }
        /// <summary>
        /// 
        /// </summary>
        public void GLInit()
        {
            text.InitializeRender(@"Consolas.bff");
            RenderJObj.Invalidate();
        }
        /// <summary>
        /// 
        /// </summary>
        public void GLFree()
        {
            text.Dispose();
            text = null;

            RenderJObj.FreeResources();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="kbState"></param>
        public void ViewportKeyPress(KeyEventArgs kbState)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <param name="pick"></param>
        public void ScreenClick(MouseButtons button, PickInformation pick)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pick"></param>
        public void ScreenDoubleClick(PickInformation pick)
        {
            if (_isSelectedChainVisible)
            {
                // select bone from chain
                if (RenderJObj.RootJObj == null ||
                    arrayGroup.SelectedObject is not DynamicGroup group)
                    return;

                var start_bone = RenderJObj.RootJObj.GetJObjAtIndex(group.BoneIndex);

                var bone = start_bone;
                float min_distance = float.MaxValue;
                object selected = null;
                for (int i = 0;
                    bone != null &&
                    i < group._chain.Length;
                    i++, bone = bone.Child)
                {
                    if (pick.CheckSphereHitDistance(bone.WorldTransform.ExtractTranslation(), _settings.BoneRadius, out float d))
                    {
                        if (d < min_distance)
                        {
                            min_distance = d;
                            selected = group._chain[i];
                        }
                    }
                }
                arrayEditor.SelectObject(selected);

                if (selected != null)
                    return;
            }

            if (_isHitbubbleVisible)
            {
                // select hitbubble
                float min_distance = float.MaxValue;
                object selected = null;
                foreach (var b in bubbles)
                {
                    if (pick.CheckSphereHitDistance(Vector3.TransformPosition(new Vector3(b.X, b.Y, b.Z), GetJointMatrix(b.BoneIndex)), b.Size, out float d))
                    {
                        if (d < min_distance &&
                            (object)b != arrayHitbubble.SelectedObject)
                        {
                            min_distance = d;
                            selected = b;
                        }
                    }
                }
                arrayHitbubble.SelectObject(selected);

                if (selected != null)
                    return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="pick"></param>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public void ScreenDrag(MouseEventArgs args, PickInformation pick, float deltaX, float deltaY)
        {
            if (_isControlGravity)
            {
                if (args.Button == MouseButtons.Left)
                {
                    _transW.MouseDown(pick);
                }
                else
                {
                    _transW.MouseUp();
                }

                _transW.Drag(pick);

                if (_transW.PendingUpdate)
                {
                    _transW.PendingUpdate = false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void ScreenSelectArea(PickInformation start, PickInformation end)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool FreezeCamera()
        {
            return Widgets.Any(e => e.Interacting);
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadData()
        {
            if (Physics == null)
            {
                arrayHitbubble.SetArrayFromProperty(this, "");
                arrayGroup.SetArrayFromProperty(this, "");
                arrayEditor.SetArrayFromProperty(this, "");
                return;
            }

            if (Physics.Hitbubbles != null)
                bubbles = Physics.Hitbubbles.Array;
            else
                bubbles = Array.Empty<SBM_DynamicHitBubble>();
            arrayHitbubble.SetArrayFromProperty(this, nameof(bubbles));
            if (bubbles.Length > 0)
                arrayHitbubble.SelectObject(bubbles[0]);

            if (Physics.DynamicDesc != null)
                groups = Physics.DynamicDesc.Array.Select(e => new DynamicGroup(e)).ToArray();
            else
                groups = Array.Empty<DynamicGroup>();
            arrayGroup.SetArrayFromProperty(this, nameof(groups));
            if (groups.Length > 0)
                arrayGroup.SelectObject(groups[0]);
        }
        /// <summary>
        /// 
        /// </summary>
        private void SaveData()
        {
            if (Physics == null)
                return;

            if (bubbles == null || bubbles.Length == 0)
            {
                Physics.Hitbubbles = null;
            }
            else
            {
                Physics.Hitbubbles = new HSDRaw.HSDArrayAccessor<SBM_DynamicHitBubble>()
                {
                    Array = bubbles
                };
            }

            if (groups == null || groups.Length == 0)
            {
                Physics.DynamicDesc = null;
            }
            else
            {
                Physics.DynamicDesc = new HSDRaw.HSDArrayAccessor<SBM_DynamicDesc>()
                {
                    Array = groups.Select(e => e.ToDesc()).ToArray(),
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadModel_Click(object sender, EventArgs e)
        {
            //string path = System.IO.Path.GetDirectoryName(MainForm.Instance.FilePath);
            //string filename = System.IO.Path.GetFileName(MainForm.Instance.FilePath).Replace(".dat", "Nr.dat");

            //path = System.IO.Path.Combine(path, filename);

            //if (System.IO.File.Exists(path))
            //{
            //    HSDRawFile hsd = new(path);
            //    if (hsd.Roots[0].Data is HSD_JOBJ jobj)
            //        LoadModel(jobj);
            //}
            //else
            {
                string f = Tools.FileIO.OpenFile(ApplicationSettings.HSDFileFilter);

                if (f != null)
                {
                    HSDRawFile hsd = new(f);
                    if (hsd.Roots[0].Data is HSD_JOBJ jobj)
                        LoadModel(jobj);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class DynamicGroup
        {
            [Browsable(false)]
            public SBM_DynamicParams[] _chain { get; set; }

            [DisplayName("Start Bone Index")]
            [Description("Starting index of bone chain.")]
            public int BoneIndex { get; set; }

            [DisplayName("Drag Damping Multiplier")]
            [Description("Controls how strongly the bone resists following motion from its parent. Higher values create more lag and softer movement.")]
            public float DragMultiplier { get; set; }

            [DisplayName("Stiffness Multiplier")]
            [Description("Controls how strongly the bone returns toward its rest pose. Higher values make the bone appear stiffer and less flexible.")]
            public float StiffnessMultiplier { get; set; }

            [DisplayName("Gravity Multiplier")]
            [Description("Controls how strongly the bone is affected by gravity.")]
            public float GravityMultiplier { get; set; }

            public DynamicGroup()
            {
                BoneIndex = 0;
                DragMultiplier = 1;
                StiffnessMultiplier = 1;
                GravityMultiplier = 1;
                _chain = Array.Empty<SBM_DynamicParams>();
            }

            public DynamicGroup(SBM_DynamicDesc desc)
            {
                BoneIndex = desc.BoneIndex;
                DragMultiplier = desc.DragMultiplier;
                StiffnessMultiplier = desc.StiffnessMultiplier;
                GravityMultiplier = desc.GravityLengthCompensation;
                _chain = desc.Parameters;
            }

            public SBM_DynamicDesc ToDesc()
            {
                return new SBM_DynamicDesc()
                {
                    BoneIndex = BoneIndex,
                    DragMultiplier = DragMultiplier,
                    StiffnessMultiplier = StiffnessMultiplier,
                    GravityLengthCompensation = GravityMultiplier,
                    Parameters = _chain,
                };
            }

            public override string ToString()
            {
                return $"Bone: {BoneIndex}";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBubbleTransparency_Click(object sender, EventArgs e)
        {
            _isHitbubbleTransparent = !_isHitbubbleTransparent;

            if (_isHitbubbleTransparent)
            {
                buttonBubbleTransparency.Image = Resources.ts_transparent;
            }
            else
            {
                buttonBubbleTransparency.Image = Resources.ts_solid;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            _isSimulation = !_isSimulation;

            if (_isSimulation)
            {
                buttonPlay.Image = Resources.ts_pause;
            }
            else
            {
                buttonPlay.Image = Resources.ts_play;
            }

            RefreshDynamics();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonResetGravity_Click(object sender, EventArgs e)
        {
            _transW.Transform = Matrix4.CreateTranslation(0, 20, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var anim = JointAnimManager.LoadFromFile(null);

            if (anim != null)
            {
                RenderJObj.LoadAnimation(anim, null, null);
                viewport.AnimationTrackEnabled = true;
                viewport.MaxFrame = anim.FrameCount;
                clearAnimationToolStripItem.Enabled = true;
            }
            else
            {
                clearAnimationToolStripItem.Enabled = false;
                viewport.AnimationTrackEnabled = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAnimationToolStripItem_Click(object sender, EventArgs e)
        {
            RenderJObj.ClearAnimation(FrameFlags.All);
            clearAnimationToolStripItem.Enabled = false;
            viewport.AnimationTrackEnabled = false;
        }
    }
}
