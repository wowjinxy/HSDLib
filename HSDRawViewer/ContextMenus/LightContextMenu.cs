using HSDRaw.Common;
using System;
using System.Windows.Forms;

namespace HSDRawViewer.ContextMenus
{
    internal class LightContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(HSD_Light) };

        public LightContextMenu() : base()
        {
            ToolStripMenuItem addAnimation = new("Add Animation");
            addAnimation.Click += (sender, args) =>
            {
                if (MainForm.SelectedDataNode.Accessor is HSD_Light root)
                {
                    if (root.AnimPointer == null)
                        root.AnimPointer = new HSDRaw.HSDNullPointerArrayAccessor<HSD_LightAnimPointer>();

                    root.AnimPointer.Add(new HSD_LightAnimPointer()
                    {
                        LightAnim = new HSDRaw.Common.Animation.HSD_AOBJ(),
                        InterestAnim = new HSD_WOBJAnim(),
                        PositionAnim = new HSD_WOBJAnim(),
                    });
                }
            };
            Items.Add(addAnimation);
        }
    }
}
