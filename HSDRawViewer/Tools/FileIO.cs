using System.IO;
using System.Windows.Forms;

namespace HSDRawViewer.Tools
{
    public class FileIO
    {
        private static string PrevSaveLocation = null;

        private static string PrevOpenLocation = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// 

        public static string OpenFolder(string description = "", string selectedPath = "")
        {
            using FolderBrowserDialog fbd = new();
            fbd.Description = description;
            if (!string.IsNullOrEmpty(selectedPath))
                fbd.SelectedPath = selectedPath;

            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                return fbd.SelectedPath;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string OpenFile(string filter, string filename = "")
        {
            using OpenFileDialog d = new();
            d.Filter = filter;
            d.FileName = filename;

            if (PrevOpenLocation != null)
            {
                d.InitialDirectory = PrevOpenLocation;
            }

            if (d.ShowDialog() == DialogResult.OK)
            {
                PrevOpenLocation = Path.GetDirectoryName(d.FileName);
                return d.FileName;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string[] OpenFiles(string filter)
        {
            using OpenFileDialog d = new();
            d.Filter = filter;
            d.Multiselect = true;

            if (d.ShowDialog() == DialogResult.OK)
            {
                return d.FileNames;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string SaveFile(string filter)
        {
            return SaveFile(filter, "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string SaveFile(string filter, string defaultName, string caption = "Save File", string initialDirectory = null)
        {
            using SaveFileDialog d = new();
            d.Title = caption;
            d.Filter = filter;

            d.FileName = defaultName;

            if (initialDirectory != null)
            {
                d.InitialDirectory = initialDirectory;
            }
            else
            if (PrevSaveLocation != null)
            {
                d.InitialDirectory = PrevSaveLocation;
            }

            if (d.ShowDialog() == DialogResult.OK)
            {
                if (HSDRawViewer.MainForm.Instance != null &&
                    !HSDRawViewer.MainForm.Instance.ValidateProjectWritePath(d.FileName))
                {
                    return null;
                }

                PrevSaveLocation = Path.GetDirectoryName(d.FileName);
                return d.FileName;
            }
            return null;
        }
    }
}
