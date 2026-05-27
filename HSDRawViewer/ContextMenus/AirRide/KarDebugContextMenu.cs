using HSDRaw.AirRide.Db;
using HSDRawViewer.Tools;
using System;
using System.Windows.Forms;

namespace HSDRawViewer.ContextMenus.AirRide
{
    public class KarDebugContextMenu : CommonContextMenu
    {
        public override Type[] SupportedTypes { get; } = new Type[] { typeof(KAR_dbEffectData) };

        public KarDebugContextMenu() : base()
        {
            ToolStripMenuItem export = new("Export To TXT");
            export.Click += (sender, args) =>
            {
                string f = FileIO.SaveFile(@"Text (*.txt)|*.txt;");

                if (f == null) return;

                if (MainForm.SelectedDataNode.Accessor is KAR_dbEffectData data)
                {
                    data.DumpToFile(f);
                }
            };
            Items.Add(export);
        }

    }
}
