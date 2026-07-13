using HSDRawViewer.Tools;
using HSDRawViewer.Tools.AirRide;
using System;
using System.IO;
using System.Windows.Forms;

namespace HSDRawViewer.GUI.Extra
{
    public class A2DPackageTool : Form
    {
        private readonly ListView _resourceList = new();
        private readonly Label _summaryLabel = new();
        private readonly ToolStripMenuItem _saveToolStripMenuItem = new("Save");
        private readonly ToolStripMenuItem _saveAsToolStripMenuItem = new("Save As");
        private readonly ToolStripMenuItem _exportResourceToolStripMenuItem = new("Export Selected Resource");
        private readonly ToolStripMenuItem _replaceResourceToolStripMenuItem = new("Replace Selected Resource (Same Size)");

        private A2DPackage _package;

        public string FilePath { get; private set; }

        public A2DPackageTool()
        {
            Text = "A2D Package";
            Width = 820;
            Height = 520;
            CenterToScreen();

            MenuStrip menuStrip = new();
            ToolStripMenuItem fileMenu = new("File");
            fileMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                _saveToolStripMenuItem,
                _saveAsToolStripMenuItem,
                new ToolStripSeparator(),
                _exportResourceToolStripMenuItem,
                _replaceResourceToolStripMenuItem,
            });
            menuStrip.Items.Add(fileMenu);
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);

            _summaryLabel.Dock = DockStyle.Top;
            _summaryLabel.Height = 42;
            _summaryLabel.Padding = new Padding(8, 8, 8, 0);

            _resourceList.Dock = DockStyle.Fill;
            _resourceList.FullRowSelect = true;
            _resourceList.GridLines = true;
            _resourceList.HideSelection = false;
            _resourceList.MultiSelect = false;
            _resourceList.View = View.Details;
            _resourceList.Columns.Add("#", 48);
            _resourceList.Columns.Add("Name", 360);
            _resourceList.Columns.Add("Kind", 80);
            _resourceList.Columns.Add("Name Offset", 100);
            _resourceList.Columns.Add("Data Offset", 100);
            _resourceList.Columns.Add("Size", 100);

            Controls.Add(_resourceList);
            Controls.Add(_summaryLabel);

            _saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            _saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            _exportResourceToolStripMenuItem.Click += exportResourceToolStripMenuItem_Click;
            _replaceResourceToolStripMenuItem.Click += replaceResourceToolStripMenuItem_Click;
        }

        public void OpenPackage(string readPath, string savePath = null)
        {
            if (!A2DPackage.TryOpen(readPath, out _package, out string error))
            {
                MessageBox.Show(error, "A2D Package", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            FilePath = savePath ?? readPath;
            RefreshResourceList();
            UpdateTitle();
        }

        private A2DPackageEntry SelectedEntry
        {
            get
            {
                if (_resourceList.SelectedItems.Count == 0)
                    return null;

                return _resourceList.SelectedItems[0].Tag as A2DPackageEntry;
            }
        }

        private void RefreshResourceList()
        {
            _resourceList.BeginUpdate();
            _resourceList.Items.Clear();

            foreach (A2DPackageEntry entry in _package.Entries)
            {
                ListViewItem item = new(entry.Index.ToString())
                {
                    Tag = entry,
                };
                item.SubItems.Add(entry.Name);
                item.SubItems.Add(entry.Kind);
                item.SubItems.Add($"0x{entry.NameOffset:X}");
                item.SubItems.Add($"0x{entry.DataOffset:X}");
                item.SubItems.Add($"0x{entry.Size:X}");
                _resourceList.Items.Add(item);
            }

            _resourceList.EndUpdate();

            string sizeMode = _package.HasFileSizeWord ? "file-size header" : "zero header";
            _summaryLabel.Text = $"{Path.GetFileName(FilePath)} - {_package.Entries.Count} resources, 0x{_package.Data.Length:X} bytes, {sizeMode}";
        }

        private void UpdateTitle()
        {
            Text = $"A2D Package - {Path.GetFileName(FilePath)}{(_package?.Modified == true ? " *" : "")}";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_package == null || string.IsNullOrEmpty(FilePath))
                return;

            string savePath = MainForm.Instance?.GetProjectSavePath(FilePath) ?? FilePath;
            if (MainForm.Instance != null && !MainForm.Instance.ValidateProjectWritePath(savePath))
                return;

            _package.Save(savePath);
            FilePath = savePath;
            MainForm.Instance?.RefreshProjectExplorer();
            RefreshResourceList();
            UpdateTitle();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_package == null)
                return;

            string f = FileIO.SaveFile("A2D DAT (*.dat)|*.dat|All Files (*.*)|*.*", Path.GetFileName(FilePath), "Save A2D Package As", Path.GetDirectoryName(MainForm.Instance?.GetProjectSavePath(FilePath) ?? FilePath));
            if (f == null)
                return;

            if (MainForm.Instance != null && !MainForm.Instance.ValidateProjectWritePath(f))
                return;

            _package.Save(f);
            FilePath = f;
            MainForm.Instance?.RefreshProjectExplorer();
            RefreshResourceList();
            UpdateTitle();
        }

        private void exportResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            A2DPackageEntry entry = SelectedEntry;
            if (entry == null)
                return;

            string f = FileIO.SaveFile("All Files (*.*)|*.*", entry.Name, "Export A2D Resource");
            if (f == null)
                return;

            if (MainForm.Instance != null && !MainForm.Instance.ValidateProjectWritePath(f))
                return;

            File.WriteAllBytes(f, _package.GetEntryData(entry.Index));
        }

        private void replaceResourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            A2DPackageEntry entry = SelectedEntry;
            if (entry == null)
                return;

            string f = FileIO.OpenFile("All Files (*.*)|*.*", entry.Name);
            if (f == null)
                return;

            if (!_package.ReplaceEntry(entry.Index, f, out string error))
            {
                MessageBox.Show(error, "Replace A2D Resource", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            RefreshResourceList();
            UpdateTitle();
        }
    }
}
