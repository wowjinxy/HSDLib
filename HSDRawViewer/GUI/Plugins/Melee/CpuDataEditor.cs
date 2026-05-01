using HSDRaw;
using HSDRaw.Common;
using HSDRaw.Melee;
using HSDRawViewer.Rendering;
using HSDRawViewer.Rendering.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace HSDRawViewer.GUI.Plugins.Melee
{
    [SupportedTypes(new Type[] { typeof(HSDArrayAccessor<SBM_CPUChance>) })]
    public partial class CpuDataEditor : PluginBase, IDrawable
    {
        public override DataNode Node
        {
            get => node;
            set
            {
                node = value;

                if (node.Accessor is HSDArrayAccessor<SBM_CPUChance> arr)
                {
                    editor.SetArrayFromProperty(arr, "Array");
                    _preview = editor.SelectedObject as SBM_CPUChance;
                }
            }
        }

        public DrawOrder DrawOrder => DrawOrder.Last;

        private readonly ViewportControl viewport;

        private SBM_CPUChance _preview = null;

        private DataNode node;

        private readonly RenderJObj RenderJObj = new();

        public CpuDataEditor()
        {
            InitializeComponent();

            viewport = new ViewportControl();
            viewport.Dock = DockStyle.Fill;
            viewport.AnimationTrackEnabled = false;
            viewport.AddRenderer(this);
            viewport.DisplayGrid = true;

            viewportBox.Controls.Add(viewport);
            viewport.RefreshSize();
            viewport.BringToFront();

            viewport.Load += (e, s) =>
            {
                viewport.Camera.DefaultRotationY = (float)Math.PI / 2;
                viewport.Camera.RestoreDefault();
            };

            FormClosing += (sender, args) =>
            {
                viewport.Dispose();
            };

            editor.SelectedObjectChanged += (e, s) =>
            {
                _preview = editor.SelectedObject as SBM_CPUChance;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public void GLInit()
        {
            RenderJObj.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void GLFree()
        {
            RenderJObj.FreeResources();
        }

        private Color DrawColor = Color.Red;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        public void Draw(Camera cam, int windowWidth, int windowHeight)
        {
            RenderJObj.Render(cam);

            //hurtboxRenderer.Render(RenderJObj.RootJObj, list, (HSDAccessor)selected);
            if (_preview != null)
            {
                DrawShape.DrawLedgeBox(_preview.X1, _preview.Y1, _preview.X2, _preview.Y2, DrawColor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadModel(HSD_JOBJ jobj)
        {
            RenderJObj.LoadJObj(jobj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadModel_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.GetDirectoryName(MainForm.Instance.FilePath);
            string filename = System.IO.Path.GetFileName(MainForm.Instance.FilePath).Replace(".dat", "Nr.dat");

            path = System.IO.Path.Combine(path, filename);

            if (System.IO.File.Exists(path))
            {
                HSDRawFile hsd = new(path);
                if (hsd.Roots[0].Data is HSD_JOBJ jobj)
                    LoadModel(jobj);
            }
            else
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
    }
}
