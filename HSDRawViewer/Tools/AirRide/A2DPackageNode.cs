using HSDRaw;
using System;
using System.IO;
using System.Windows.Forms;

namespace HSDRawViewer.Tools.AirRide
{
    public class A2DPackageNode : TreeNode
    {
        public A2DPackage Package { get; }

        public string FilePath { get; set; }

        public A2DPackageNode(A2DPackage package, string filePath)
        {
            Package = package;
            FilePath = filePath;
            ImageKey = "folder";
            SelectedImageKey = "folder";

            RefreshEntries();
            UpdateText();
        }

        public void RefreshEntries()
        {
            Nodes.Clear();
            foreach (A2DPackageEntry entry in Package.Entries)
                Nodes.Add(new A2DPackageEntryNode(this, entry));
        }

        public void UpdateText()
        {
            Text = $"{Path.GetFileName(FilePath)} ({Package.Entries.Count} resources){(Package.Modified ? " *" : "")}";
        }
    }

    public class A2DPackageEntryNode : TreeNode
    {
        public A2DPackageNode PackageNode { get; }

        public A2DPackageEntry Entry { get; }

        public HSDRawFile HSDArchive { get; }

        public string HSDArchiveError { get; }

        public A2DPackageEntryNode(A2DPackageNode packageNode, A2DPackageEntry entry)
        {
            PackageNode = packageNode;
            Entry = entry;
            Text = entry.Name;
            ImageKey = "known";
            SelectedImageKey = "known";

            if (PackageNode.Package.TryGetEntryHSDArchiveData(entry.Index, out byte[] archiveData, out string error))
            {
                try
                {
                    HSDArchive = new HSDRawFile(archiveData);
                    ImageKey = "folder";
                    SelectedImageKey = "folder";

                    foreach (HSDRootNode root in HSDArchive.Roots)
                        Nodes.Add(new DataNode(root.Name, root.Data, root: root, readOnlyPreview: true));

                    foreach (HSDRootNode root in HSDArchive.References)
                        Nodes.Add(new DataNode(root.Name, root.Data, root: root, referenceNode: true, readOnlyPreview: true));
                }
                catch (Exception ex) when (ex is InvalidDataException || ex is EndOfStreamException || ex is ArgumentOutOfRangeException)
                {
                    HSDArchiveError = ex.Message;
                }
            }
            else
            {
                HSDArchiveError = error;
            }
        }
    }
}
