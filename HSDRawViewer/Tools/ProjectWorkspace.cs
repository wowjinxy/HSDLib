using KARToolkit.Core;
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

        private readonly KarProject _project;

        public string SourceRoot => _project.SourceFilesRoot;

        public string ProjectRoot => _project.SourceRoot;

        public string OutputRoot => _project.OutputFilesRoot;

        public string ProjectOutputRoot => _project.OutputRoot;

        public string Name => Path.GetFileName(_project.SourceRoot);

        public IReadOnlyList<KarProjectFile> Files => _project.Files;

        public IReadOnlyList<KarMapBundle> Maps => _project.Maps;

        public ProjectWorkspace(string sourceRoot, string outputRoot)
        {
            _project = KarProject.Open(sourceRoot, outputRoot);
        }

        public static string NormalizeDirectory(string path)
        {
            return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static bool IsSamePath(string left, string right)
        {
            return string.Equals(
                NormalizeDirectory(left),
                NormalizeDirectory(right),
                StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsPathInDirectory(string path, string directory)
        {
            string fullPath = NormalizeDirectory(path);
            string fullDirectory = EnsureTrailingSeparator(NormalizeDirectory(directory));
            return fullPath.StartsWith(fullDirectory, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsSourcePath(string path)
        {
            return IsSamePath(path, _project.SourceRoot) ||
                IsPathInDirectory(path, _project.SourceRoot) ||
                IsSamePath(path, _project.SourceFilesRoot) ||
                IsPathInDirectory(path, _project.SourceFilesRoot);
        }

        public bool IsOutputPath(string path)
        {
            return IsSamePath(path, _project.OutputRoot) ||
                IsPathInDirectory(path, _project.OutputRoot) ||
                IsSamePath(path, _project.OutputFilesRoot) ||
                IsPathInDirectory(path, _project.OutputFilesRoot);
        }

        public bool TryGetRelativePath(string path, out string relativePath)
        {
            relativePath = GetRelativePath(path, _project.SourceFilesRoot);
            if (relativePath != null)
                return true;

            relativePath = GetRelativePath(path, _project.OutputFilesRoot);
            return relativePath != null;
        }

        public string GetSourcePath(string relativePath)
        {
            if (_project.TryGetFile(relativePath, out KarProjectFile file))
                return file.SourcePath;

            return ResolveUnderRoot(_project.SourceFilesRoot, relativePath);
        }

        public string GetOutputPath(string relativePath)
        {
            string outputPath = _project.GetOutputPath(relativePath);
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory))
                Directory.CreateDirectory(outputDirectory);
            return outputPath;
        }

        public string GetEffectiveReadPath(string relativePath)
        {
            if (_project.TryGetFile(relativePath, out KarProjectFile file))
                return file.ReadPath;

            string outputPath = _project.GetOutputPath(relativePath);
            return File.Exists(outputPath) ? outputPath : GetSourcePath(relativePath);
        }

        public bool HasOutputOverride(string relativePath)
        {
            return File.Exists(_project.GetOutputPath(relativePath));
        }

        public bool FileExists(string relativePath)
        {
            return _project.TryGetFile(relativePath, out _) || File.Exists(_project.GetOutputPath(relativePath));
        }

        public bool IsSupportedOpenFile(string relativePath)
        {
            return SupportedOpenExtensions.Contains(Path.GetExtension(relativePath));
        }

        public string FindFirstRelativeByFileName(string fileName)
        {
            return Files
                .Select(file => file.RelativePath)
                .FirstOrDefault(relativePath => string.Equals(Path.GetFileName(relativePath), fileName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<string> EnumerateSourceFiles()
        {
            return Files.Select(file => file.RelativePath);
        }

        private static string GetRelativePath(string path, string root)
        {
            string fullPath = NormalizeDirectory(path);
            string fullRoot = NormalizeDirectory(root);

            if (IsSamePath(fullPath, fullRoot))
                return string.Empty;

            string fullRootWithSeparator = EnsureTrailingSeparator(fullRoot);
            if (!fullPath.StartsWith(fullRootWithSeparator, StringComparison.OrdinalIgnoreCase))
                return null;

            return fullPath.Substring(fullRootWithSeparator.Length).Replace(Path.DirectorySeparatorChar, '/');
        }

        private static string ResolveUnderRoot(string root, string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Relative path cannot be empty.", nameof(relativePath));
            if (Path.IsPathRooted(relativePath))
                throw new ArgumentException("Project paths must be relative.", nameof(relativePath));

            string[] parts = relativePath
                .Replace('\\', '/')
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException("Relative path cannot be empty.", nameof(relativePath));
            if (parts.Any(part => part == "." || part == ".."))
                throw new ArgumentException("Project paths cannot contain traversal segments.", nameof(relativePath));

            string fullPath = Path.GetFullPath(Path.Combine(new[] { root }.Concat(parts).ToArray()));
            if (!IsSamePath(fullPath, root) && !IsPathInDirectory(fullPath, root))
                throw new InvalidOperationException("Resolved path escaped its project root.");

            return fullPath;
        }

        private static string EnsureTrailingSeparator(string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar) || path.EndsWith(Path.AltDirectorySeparatorChar))
                return path;

            return path + Path.DirectorySeparatorChar;
        }
    }
}
