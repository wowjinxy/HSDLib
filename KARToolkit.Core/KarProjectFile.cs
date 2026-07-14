using System.IO;

namespace KARToolkit.Core
{
    public sealed class KarProjectFile
    {
        internal KarProjectFile(string relativePath, string sourcePath, string outputPath, KarFileKind kind)
        {
            RelativePath = relativePath;
            SourcePath = sourcePath;
            OutputPath = outputPath;
            Kind = kind;
        }

        public string RelativePath { get; }

        public string SourcePath { get; }

        public string OutputPath { get; }

        public KarFileKind Kind { get; }

        public bool HasOutputCopy => File.Exists(OutputPath);

        public string ReadPath => HasOutputCopy ? OutputPath : SourcePath;
    }
}
