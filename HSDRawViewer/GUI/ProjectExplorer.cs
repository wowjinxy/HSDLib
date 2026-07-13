using HSDRawViewer.Tools;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace HSDRawViewer.GUI
{
    public class ProjectExplorer : DockContent
    {
        private readonly ProjectWorkspace _project;
        private readonly TreeView _treeView = new();

        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                TabText = value;
            }
        }

        public ProjectExplorer(ProjectWorkspace project)
        {
            _project = project;

            Text = "Project";
            DockAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.Float;

            _treeView.Dock = DockStyle.Fill;
            _treeView.HideSelection = false;
            _treeView.NodeMouseDoubleClick += treeView_NodeMouseDoubleClick;
            _treeView.KeyDown += treeView_KeyDown;

            Controls.Add(_treeView);
            RefreshProject();
        }

        public void RefreshProject()
        {
            _treeView.BeginUpdate();
            _treeView.Nodes.Clear();

            TreeNode root = BuildDirectoryNode(new DirectoryInfo(_project.SourceRoot), _project.Name);
            root.Expand();
            _treeView.Nodes.Add(root);

            _treeView.EndUpdate();
        }

        private TreeNode BuildDirectoryNode(DirectoryInfo directory, string text = null)
        {
            TreeNode node = new(text ?? directory.Name);

            try
            {
                foreach (DirectoryInfo childDirectory in directory.GetDirectories().OrderBy(e => e.Name))
                {
                    if ((childDirectory.Attributes & FileAttributes.Hidden) != 0)
                        continue;

                    node.Nodes.Add(BuildDirectoryNode(childDirectory));
                }

                foreach (FileInfo file in directory.GetFiles().OrderBy(e => e.Name))
                {
                    if ((file.Attributes & FileAttributes.Hidden) != 0)
                        continue;

                    node.Nodes.Add(BuildFileNode(file));
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }

            return node;
        }

        private TreeNode BuildFileNode(FileInfo file)
        {
            _project.TryGetRelativePath(file.FullName, out string relativePath);

            string text = file.Name;
            if (_project.HasOutputOverride(relativePath))
                text += " *";

            TreeNode node = new(text)
            {
                Tag = new ProjectFileEntry(relativePath),
            };

            if (_project.IsSupportedOpenFile(relativePath))
                node.ToolTipText = "Double-click to open";
            else
                node.ToolTipText = "No viewer registered for this file";

            return node;
        }

        private void OpenSelectedFile()
        {
            if (_treeView.SelectedNode?.Tag is not ProjectFileEntry entry)
                return;

            MainForm.Instance.OpenProjectFile(entry.RelativePath);
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _treeView.SelectedNode = e.Node;
            OpenSelectedFile();
        }

        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OpenSelectedFile();
        }

        private class ProjectFileEntry
        {
            public string RelativePath { get; }

            public ProjectFileEntry(string relativePath)
            {
                RelativePath = relativePath;
            }
        }
    }
}
