using HSDRaw;
using KARToolkit.Core.AirRide;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KARToolkit.Core
{
    public sealed class KarProject
    {
        private readonly Dictionary<string, KarProjectFile> _filesByPath;

        private KarProject(
            string sourceRoot,
            string sourceFilesRoot,
            string outputRoot,
            string outputFilesRoot,
            bool sourceHasFilesDirectory,
            IReadOnlyList<KarProjectFile> files,
            IReadOnlyList<KarMapBundle> maps)
        {
            SourceRoot = sourceRoot;
            SourceFilesRoot = sourceFilesRoot;
            OutputRoot = outputRoot;
            OutputFilesRoot = outputFilesRoot;
            SourceHasFilesDirectory = sourceHasFilesDirectory;
            Files = files;
            Maps = maps;
            _filesByPath = files.ToDictionary(f => f.RelativePath, StringComparer.OrdinalIgnoreCase);
        }

        public string SourceRoot { get; }

        public string SourceFilesRoot { get; }

        public string OutputRoot { get; }

        public string OutputFilesRoot { get; }

        public bool SourceHasFilesDirectory { get; }

        public IReadOnlyList<KarProjectFile> Files { get; }

        public IReadOnlyList<KarMapBundle> Maps { get; }

        public IReadOnlyDictionary<string, KarProjectFile> FilesByPath => _filesByPath;

        public static KarProject Open(string sourceRoot)
        {
            return Open(sourceRoot, null);
        }

        public static KarProject Open(string sourceRoot, string outputRoot)
        {
            if (string.IsNullOrWhiteSpace(sourceRoot))
                throw new ArgumentException("Source root cannot be empty.", nameof(sourceRoot));

            var fullSourceRoot = Path.GetFullPath(sourceRoot);
            if (!Directory.Exists(fullSourceRoot))
                throw new DirectoryNotFoundException(fullSourceRoot);

            bool sourceHasFilesDirectory;
            var sourceFilesRoot = ResolveSourceFilesRoot(fullSourceRoot, out sourceHasFilesDirectory);
            var fullOutputRoot = string.IsNullOrWhiteSpace(outputRoot)
                ? GetDefaultOutputRoot(fullSourceRoot, sourceHasFilesDirectory)
                : Path.GetFullPath(outputRoot);
            var outputFilesRoot = sourceHasFilesDirectory
                ? Path.Combine(fullOutputRoot, "files")
                : fullOutputRoot;

            EnsureSeparateRoot(sourceFilesRoot, outputFilesRoot, "Output files root cannot be inside the source files root.");
            EnsureSeparateRoot(fullSourceRoot, fullOutputRoot, "Output root cannot be inside the source root.");
            EnsureSeparateRoot(fullOutputRoot, fullSourceRoot, "Source root cannot be inside the output root.");

            var files = BuildFileIndex(sourceFilesRoot, outputFilesRoot);
            var maps = BuildMapIndex(files);

            return new KarProject(
                fullSourceRoot,
                sourceFilesRoot,
                fullOutputRoot,
                outputFilesRoot,
                sourceHasFilesDirectory,
                files,
                maps);
        }

        public KarProjectFile GetFile(string relativePath)
        {
            KarProjectFile file;
            var normalized = NormalizeRelativePath(relativePath);
            if (!_filesByPath.TryGetValue(normalized, out file))
                throw new FileNotFoundException("Project file was not found.", normalized);
            return file;
        }

        public bool TryGetFile(string relativePath, out KarProjectFile file)
        {
            var normalized = NormalizeRelativePath(relativePath);
            return _filesByPath.TryGetValue(normalized, out file);
        }

        public string GetReadPath(string relativePath)
        {
            return GetFile(relativePath).ReadPath;
        }

        public string GetSourcePath(string relativePath)
        {
            return GetFile(relativePath).SourcePath;
        }

        public string GetOutputPath(string relativePath)
        {
            return ResolveUnderRoot(OutputFilesRoot, NormalizeRelativePath(relativePath));
        }

        public string CopyToOutput(string relativePath, bool overwrite = false)
        {
            var file = GetFile(relativePath);
            var outputPath = PrepareOutputPath(file.RelativePath);

            if (overwrite || !File.Exists(outputPath))
                File.Copy(file.SourcePath, outputPath, overwrite);

            return outputPath;
        }

        public byte[] ReadBytes(string relativePath)
        {
            return File.ReadAllBytes(GetReadPath(relativePath));
        }

        public void WriteBytes(string relativePath, byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var normalized = NormalizeRelativePath(relativePath);
            var outputPath = PrepareOutputPath(normalized);
            var tempPath = outputPath + ".tmp";

            File.WriteAllBytes(tempPath, data);
            ReplaceFile(tempPath, outputPath);
        }

        public HSDRawFile OpenHsdFile(string relativePath)
        {
            return new HSDRawFile(GetReadPath(relativePath));
        }

        public bool TryOpenA2DPackage(string relativePath, out A2DPackage package, out string error)
        {
            return A2DPackage.TryOpen(GetReadPath(relativePath), out package, out error);
        }

        public A2DPackage OpenA2DPackage(string relativePath)
        {
            if (!TryOpenA2DPackage(relativePath, out A2DPackage package, out string error))
                throw new InvalidDataException(error);

            return package;
        }

        public string SaveHsdFile(string relativePath, HSDRawFile file, bool bufferAlign = true, bool optimize = true, bool trim = false)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var normalized = NormalizeRelativePath(relativePath);
            var outputPath = PrepareOutputPath(normalized);
            var tempPath = outputPath + ".tmp";

            file.Save(tempPath, bufferAlign, optimize, trim);
            ReplaceFile(tempPath, outputPath);
            return outputPath;
        }

        public string SaveA2DPackage(string relativePath, A2DPackage package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            var normalized = NormalizeRelativePath(relativePath);
            var outputPath = PrepareOutputPath(normalized);
            var tempPath = outputPath + ".tmp";

            package.Save(tempPath);
            ReplaceFile(tempPath, outputPath);
            return outputPath;
        }

        private static string ResolveSourceFilesRoot(string sourceRoot, out bool sourceHasFilesDirectory)
        {
            var filesRoot = Path.Combine(sourceRoot, "files");
            if (Directory.Exists(filesRoot))
            {
                sourceHasFilesDirectory = true;
                return filesRoot;
            }

            sourceHasFilesDirectory = false;
            return sourceRoot;
        }

        private static string GetDefaultOutputRoot(string sourceRoot, bool sourceHasFilesDirectory)
        {
            if (sourceHasFilesDirectory)
                return sourceRoot + "_mod";

            var directoryName = Path.GetFileName(sourceRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            var parent = Directory.GetParent(sourceRoot);
            if (string.Equals(directoryName, "files", StringComparison.OrdinalIgnoreCase) && parent != null && parent.Parent != null)
                return Path.Combine(parent.Parent.FullName, parent.Name + "_mod", "files");

            return sourceRoot + "_mod";
        }

        private static IReadOnlyList<KarProjectFile> BuildFileIndex(string sourceFilesRoot, string outputFilesRoot)
        {
            return Directory
                .EnumerateFiles(sourceFilesRoot, "*", SearchOption.AllDirectories)
                .Select(path =>
                {
                    var relativePath = GetRelativePath(sourceFilesRoot, path);
                    return new KarProjectFile(
                        relativePath,
                        path,
                        ResolveUnderRoot(outputFilesRoot, relativePath),
                        Classify(relativePath));
                })
                .OrderBy(file => file.RelativePath, StringComparer.OrdinalIgnoreCase)
                .ToList()
                .AsReadOnly();
        }

        private static IReadOnlyList<KarMapBundle> BuildMapIndex(IReadOnlyList<KarProjectFile> files)
        {
            var builders = new Dictionary<string, MapBundleBuilder>(StringComparer.OrdinalIgnoreCase);

            foreach (var file in files)
            {
                string mapName;
                if (!TryGetMapName(file, out mapName))
                    continue;

                MapBundleBuilder builder;
                if (!builders.TryGetValue(mapName, out builder))
                {
                    builder = new MapBundleBuilder(mapName);
                    builders.Add(mapName, builder);
                }

                builder.Add(file);
            }

            return builders
                .Values
                .Select(builder => builder.Build())
                .OrderBy(map => map.Name, StringComparer.OrdinalIgnoreCase)
                .ToList()
                .AsReadOnly();
        }

        private static bool TryGetMapName(KarProjectFile file, out string mapName)
        {
            mapName = null;

            if (file.Kind != KarFileKind.MapData &&
                file.Kind != KarFileKind.MapModel &&
                file.Kind != KarFileKind.MapEvent)
            {
                return false;
            }

            var name = Path.GetFileNameWithoutExtension(file.RelativePath);
            if (!name.StartsWith("Gr", StringComparison.OrdinalIgnoreCase))
                return false;

            mapName = name.Substring(2);

            if (file.Kind == KarFileKind.MapModel && mapName.EndsWith("Model", StringComparison.OrdinalIgnoreCase))
                mapName = mapName.Substring(0, mapName.Length - "Model".Length);
            else if (file.Kind == KarFileKind.MapEvent && mapName.EndsWith("Event", StringComparison.OrdinalIgnoreCase))
                mapName = mapName.Substring(0, mapName.Length - "Event".Length);

            return mapName.Length > 0;
        }

        private static KarFileKind Classify(string relativePath)
        {
            var fileName = Path.GetFileName(relativePath);
            var name = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);

            if (extension.Equals(".ini", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.Config;
            if (extension.Equals(".h4m", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.Movie;
            if (IsAudioExtension(extension))
                return KarFileKind.Audio;
            if (!extension.Equals(".dat", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.Unknown;

            if (fileName.Equals("Stage.dat", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.StageTable;
            if (name.StartsWith("A2", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.A2dPackage;
            if (name.Equals("GrCommon", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.MapCommon;
            if (name.StartsWith("Gr", StringComparison.OrdinalIgnoreCase))
            {
                if (name.EndsWith("Model", StringComparison.OrdinalIgnoreCase))
                    return KarFileKind.MapModel;
                if (name.EndsWith("Event", StringComparison.OrdinalIgnoreCase))
                    return KarFileKind.MapEvent;
                return KarFileKind.MapData;
            }
            if (name.StartsWith("Rd", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.RiderData;
            if (name.StartsWith("Vc", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.VehicleData;
            if (name.StartsWith("Ef", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.EffectData;
            if (name.StartsWith("It", StringComparison.OrdinalIgnoreCase) || name.Equals("Item", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.ItemData;
            if (name.StartsWith("Em", StringComparison.OrdinalIgnoreCase) || name.Equals("Enemy", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.EnemyData;
            if (name.StartsWith("Mn", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("If", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("Sis", StringComparison.OrdinalIgnoreCase) ||
                name.StartsWith("Ending", StringComparison.OrdinalIgnoreCase))
            {
                return KarFileKind.UiData;
            }
            if (name.StartsWith("Vs", StringComparison.OrdinalIgnoreCase))
                return KarFileKind.VersusData;

            return KarFileKind.HsdArchive;
        }

        private static bool IsAudioExtension(string extension)
        {
            return extension.Equals(".hps", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".ssm", StringComparison.OrdinalIgnoreCase) ||
                extension.Equals(".aw", StringComparison.OrdinalIgnoreCase);
        }

        private string PrepareOutputPath(string relativePath)
        {
            var outputPath = ResolveUnderRoot(OutputFilesRoot, relativePath);
            var parent = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(parent))
                Directory.CreateDirectory(parent);
            return outputPath;
        }

        private static void ReplaceFile(string tempPath, string outputPath)
        {
            if (File.Exists(outputPath))
                File.Delete(outputPath);
            File.Move(tempPath, outputPath);
        }

        private static string NormalizeRelativePath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Relative path cannot be empty.", nameof(relativePath));
            if (Path.IsPathRooted(relativePath))
                throw new ArgumentException("Project paths must be relative.", nameof(relativePath));

            var parts = relativePath
                .Replace('\\', '/')
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException("Relative path cannot be empty.", nameof(relativePath));

            foreach (var part in parts)
            {
                if (part == "." || part == "..")
                    throw new ArgumentException("Project paths cannot contain traversal segments.", nameof(relativePath));
            }

            return string.Join("/", parts);
        }

        private static string ResolveUnderRoot(string root, string relativePath)
        {
            var normalized = NormalizeRelativePath(relativePath);
            var platformPath = normalized.Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.GetFullPath(Path.Combine(root, platformPath));

            if (!IsSameOrChildPath(root, fullPath))
                throw new InvalidOperationException("Resolved path escaped its project root.");

            return fullPath;
        }

        private static string GetRelativePath(string root, string path)
        {
            var rootUri = new Uri(AppendDirectorySeparator(Path.GetFullPath(root)));
            var pathUri = new Uri(Path.GetFullPath(path));
            var relativeUri = rootUri.MakeRelativeUri(pathUri);
            return Uri.UnescapeDataString(relativeUri.ToString()).Replace('\\', '/');
        }

        private static string AppendDirectorySeparator(string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()) ||
                path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
            {
                return path;
            }

            return path + Path.DirectorySeparatorChar;
        }

        private static void EnsureSeparateRoot(string sourceRoot, string outputRoot, string message)
        {
            if (IsSameOrChildPath(sourceRoot, outputRoot))
                throw new InvalidOperationException(message);
        }

        private static bool IsSameOrChildPath(string root, string path)
        {
            var fullRoot = AppendDirectorySeparator(Path.GetFullPath(root));
            var fullPath = Path.GetFullPath(path);

            return fullPath.Equals(fullRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), StringComparison.OrdinalIgnoreCase) ||
                fullPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase);
        }

        private sealed class MapBundleBuilder
        {
            public MapBundleBuilder(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public KarProjectFile DataFile { get; private set; }

            public KarProjectFile ModelFile { get; private set; }

            public KarProjectFile EventFile { get; private set; }

            public void Add(KarProjectFile file)
            {
                switch (file.Kind)
                {
                    case KarFileKind.MapData:
                        DataFile = file;
                        break;
                    case KarFileKind.MapModel:
                        ModelFile = file;
                        break;
                    case KarFileKind.MapEvent:
                        EventFile = file;
                        break;
                }
            }

            public KarMapBundle Build()
            {
                return new KarMapBundle(Name, DataFile, ModelFile, EventFile);
            }
        }
    }
}
