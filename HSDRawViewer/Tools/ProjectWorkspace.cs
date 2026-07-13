using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HSDRawViewer.Tools
{
    public class ProjectWorkspace
    {
        private static readonly HashSet<string> SupportedOpenExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".dat",
            ".usd",
            ".ssm",
            ".sdi",
            ".sem",
        };

        public string SourceRoot { get; }

        public string OutputRoot { get; }

        public string Name => Path.GetFileName(SourceRoot);

        public ProjectWorkspace(string sourceRoot, string outputRoot)
        {
            SourceRoot = NormalizeDirectory(sourceRoot);
            OutputRoot = NormalizeDirectory(outputRoot);
        }

        public static string NormalizeDirectory(string path)
        {
            return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static bool IsSamePath(string left, string right)
        {
            return string.Equals(
                Path.GetFullPath(left).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                Path.GetFullPath(right).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPathInDirectory(string path, string directory)
        {
            string fullPath = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string fullDirectory = EnsureTrailingSeparator(NormalizeDirectory(directory));
            return fullPath.StartsWith(fullDirectory, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsSourcePath(string path)
        {
            return IsSamePath(path, SourceRoot) || IsPathInDirectory(path, SourceRoot);
        }

        public bool IsOutputPath(string path)
        {
            return IsSamePath(path, OutputRoot) || IsPathInDirectory(path, OutputRoot);
        }

        public bool TryGetRelativePath(string path, out string relativePath)
        {
            relativePath = GetRelativePath(path, SourceRoot);
            if (relativePath != null)
                return true;

            relativePath = GetRelativePath(path, OutputRoot);
            return relativePath != null;
        }

        public string GetSourcePath(string relativePath)
        {
            return Path.Combine(SourceRoot, relativePath);
        }

        public string GetOutputPath(string relativePath)
        {
            string outputPath = Path.Combine(OutputRoot, relativePath);
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory))
                Directory.CreateDirectory(outputDirectory);
            return outputPath;
        }

        public string GetEffectiveReadPath(string relativePath)
        {
            string outputPath = Path.Combine(OutputRoot, relativePath);
            if (File.Exists(outputPath))
                return outputPath;

            return Path.Combine(SourceRoot, relativePath);
        }

        public bool HasOutputOverride(string relativePath)
        {
            return File.Exists(Path.Combine(OutputRoot, relativePath));
        }

        public bool FileExists(string relativePath)
        {
            return File.Exists(Path.Combine(OutputRoot, relativePath)) ||
                File.Exists(Path.Combine(SourceRoot, relativePath));
        }

        public bool IsSupportedOpenFile(string relativePath)
        {
            return SupportedOpenExtensions.Contains(Path.GetExtension(relativePath));
        }

        public string FindFirstRelativeByFileName(string fileName)
        {
            return EnumerateSourceFiles()
                .FirstOrDefault(relativePath => string.Equals(Path.GetFileName(relativePath), fileName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<string> EnumerateSourceFiles()
        {
            foreach (string file in Directory.EnumerateFiles(SourceRoot, "*", SearchOption.AllDirectories))
                if (TryGetRelativePath(file, out string relativePath))
                    yield return relativePath;
        }

        private static string GetRelativePath(string path, string root)
        {
            string fullPath = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string fullRoot = NormalizeDirectory(root);

            if (IsSamePath(fullPath, fullRoot))
                return string.Empty;

            string fullRootWithSeparator = EnsureTrailingSeparator(fullRoot);
            if (!fullPath.StartsWith(fullRootWithSeparator, StringComparison.OrdinalIgnoreCase))
                return null;

            return fullPath.Substring(fullRootWithSeparator.Length);
        }

        private static string EnsureTrailingSeparator(string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar) || path.EndsWith(Path.AltDirectorySeparatorChar))
                return path;

            return path + Path.DirectorySeparatorChar;
        }
    }
}
