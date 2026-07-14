using HSDRawViewer.Tools;
using KARToolkit.Core;
using System;
using System.Collections.Generic;
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
            _treeView.ShowNodeToolTips = true;
            _treeView.NodeMouseDoubleClick += treeView_NodeMouseDoubleClick;
            _treeView.KeyDown += treeView_KeyDown;

            Controls.Add(_treeView);
            RefreshProject();
        }

        public void RefreshProject()
        {
            _treeView.BeginUpdate();
            _treeView.Nodes.Clear();

            HashSet<string> mapFiles = new(StringComparer.OrdinalIgnoreCase);
            TreeNode root = new(_project.Name)
            {
                ToolTipText = _project.ProjectRoot,
            };

            TreeNode mapsNode = BuildMapsNode(mapFiles);
            if (mapsNode.Nodes.Count > 0)
                root.Nodes.Add(mapsNode);

            AddSection(root, "Map Shared", mapFiles, KarFileKind.MapCommon, KarFileKind.StageTable);
            AddSection(root, "Ungrouped Map Files", mapFiles, KarFileKind.MapData, KarFileKind.MapModel, KarFileKind.MapEvent);
            AddSection(root, "A2D Packages", mapFiles, KarFileKind.A2dPackage);
            AddSection(root, "Vehicles", mapFiles, KarFileKind.VehicleData);
            AddSection(root, "Riders", mapFiles, KarFileKind.RiderData);
            AddSection(root, "Effects", mapFiles, KarFileKind.EffectData);
            AddSection(root, "Items", mapFiles, KarFileKind.ItemData);
            AddSection(root, "Enemies", mapFiles, KarFileKind.EnemyData);
            AddSection(root, "UI", mapFiles, KarFileKind.UiData);
            AddSection(root, "Versus", mapFiles, KarFileKind.VersusData);
            AddSection(root, "Audio", mapFiles, KarFileKind.Audio);
            AddSection(root, "Movies", mapFiles, KarFileKind.Movie);
            AddSection(root, "Config", mapFiles, KarFileKind.Config);
            AddSection(root, "Other", mapFiles, KarFileKind.HsdArchive, KarFileKind.Unknown);

            root.Expand();
            mapsNode.Expand();
            _treeView.Nodes.Add(root);

            _treeView.EndUpdate();
        }

        private TreeNode BuildMapsNode(HashSet<string> mapFiles)
        {
            TreeNode mapsNode = new($"Maps ({_project.Maps.Count})");

            foreach (KarMapBundle map in _project.Maps)
            {
                TreeNode mapNode = new(map.Name);
                AddMapFileNode(mapNode, map.DataFile, "Data", mapFiles);
                AddMapFileNode(mapNode, map.ModelFile, "Model", mapFiles);
                AddMapFileNode(mapNode, map.EventFile, "Event", mapFiles);

                if (mapNode.Nodes.Count > 0)
                    mapsNode.Nodes.Add(mapNode);
            }

            return mapsNode;
        }

        private void AddMapFileNode(TreeNode parent, KarProjectFile file, string label, HashSet<string> mapFiles)
        {
            if (file == null)
                return;

            mapFiles.Add(file.RelativePath);
            parent.Nodes.Add(BuildFileNode(file, label));
        }

        private void AddSection(TreeNode root, string title, HashSet<string> excludedFiles, params KarFileKind[] kinds)
        {
            HashSet<KarFileKind> kindSet = new(kinds);
            List<KarProjectFile> files = _project.Files
                .Where(file => kindSet.Contains(file.Kind))
                .Where(file => !excludedFiles.Contains(file.RelativePath))
                .OrderBy(file => file.RelativePath, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (files.Count == 0)
                return;

            TreeNode sectionNode = new($"{title} ({files.Count})");
            foreach (KarProjectFile file in files)
                sectionNode.Nodes.Add(BuildFileNode(file));

            root.Nodes.Add(sectionNode);
        }

        private TreeNode BuildFileNode(KarProjectFile file, string label = null)
        {
            string text = label == null ? file.RelativePath : $"{label}: {Path.GetFileName(file.RelativePath)}";
            if (_project.HasOutputOverride(file.RelativePath))
                text += " *";

            TreeNode node = new(text)
            {
                Tag = new ProjectFileEntry(file.RelativePath),
                ToolTipText = GetToolTip(file),
            };

            return node;
        }

        private string GetToolTip(KarProjectFile file)
        {
            string status = _project.HasOutputOverride(file.RelativePath) ? "Modified in output" : "Source";
            string openHint = _project.IsSupportedOpenFile(file.RelativePath) ? "Double-click to open" : "No viewer registered for this file";
            string roots = file.ArchiveDefinition.Roots.Count == 0
                ? null
                : "Expected roots: " + string.Join(", ", file.ArchiveDefinition.Roots.Select(root => root.Pattern));

            if (roots == null)
                return $"{file.DisplayName}\n{file.Kind}: {file.RelativePath}\n{status}\n{openHint}";

            return $"{file.DisplayName}\n{file.Kind}: {file.RelativePath}\n{roots}\n{status}\n{openHint}";
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
