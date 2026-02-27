using UnityEditor;
using UnityEngine;
using System.IO;

namespace UniGLTF
{
    public static class ReimportSourceMenu
    {
        private const string MenuRoot = "Assets/UniGLTF";
        private const string ReimportFromSource = MenuRoot + "/Reimport From Source";
        private const string ReimportFromFile = MenuRoot + "/Reimport From File...";
        private const string ClearSource = MenuRoot + "/Clear Source Override";

        [MenuItem(ReimportFromSource, priority = 1000)]
        private static void ReimportFromSourceMenu()
        {
            if (!TryGetImporter(out var importer)) return;
            var assetPath = importer.assetPath;
            var sourcePath = importer.m_sourcePath;
            PersistImporterSettings(importer);
            if (!string.IsNullOrEmpty(sourcePath) && TryCopySourceIntoAsset(sourcePath, assetPath))
            {
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                return;
            }
            importer.SaveAndReimport();
        }

        [MenuItem(ReimportFromSource, validate = true)]
        private static bool ReimportFromSourceValidate()
        {
            if (!TryGetImporter(out var importer)) return false;
            var resolved = importer.GetResolvedSourcePath();
            return importer.m_useSourcePath && !string.IsNullOrEmpty(resolved) && File.Exists(resolved);
        }

        [MenuItem(ReimportFromFile, priority = 1001)]
        private static void ReimportFromFileMenu()
        {
            if (!TryGetImporter(out var importer)) return;
            var picked = EditorUtility.OpenFilePanel("Select glTF/GLB", "", "gltf,glb,zip");
            if (string.IsNullOrEmpty(picked)) return;
            importer.SetSourcePath(picked, true);
            PersistImporterSettings(importer);
            var assetPath = importer.assetPath;
            if (TryCopySourceIntoAsset(picked, assetPath))
            {
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                return;
            }
            importer.SaveAndReimport();
        }

        [MenuItem(ReimportFromFile, validate = true)]
        private static bool ReimportFromFileValidate()
        {
            return TryGetImporter(out _);
        }

        [MenuItem(ClearSource, priority = 1002)]
        private static void ClearSourceMenu()
        {
            if (!TryGetImporter(out var importer)) return;
            importer.SetSourcePath(string.Empty, false);
            PersistImporterSettings(importer);
            importer.SaveAndReimport();
        }

        [MenuItem(ClearSource, validate = true)]
        private static bool ClearSourceValidate()
        {
            if (!TryGetImporter(out var importer)) return false;
            return importer.m_useSourcePath || !string.IsNullOrEmpty(importer.m_sourcePath);
        }

        private static bool TryGetImporter(out GltfScriptedImporterBase importer)
        {
            importer = null;
            var obj = Selection.activeObject;
            if (obj == null) return false;
            var path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path)) return false;
            importer = AssetImporter.GetAtPath(path) as GltfScriptedImporterBase;
            return importer != null;
        }

        private static void PersistImporterSettings(GltfScriptedImporterBase importer)
        {
            if (importer == null) return;
            EditorUtility.SetDirty(importer);
            AssetDatabase.WriteImportSettingsIfDirty(importer.assetPath);
        }

        private static bool TryCopySourceIntoAsset(string sourcePath, string assetPath)
        {
            var srcFull = ResolveToFullPath(sourcePath);
            if (string.IsNullOrEmpty(srcFull) || !File.Exists(srcFull)) return false;

            var dstFull = UnityPath.FromUnityPath(assetPath).FullPath;
            if (string.IsNullOrEmpty(dstFull)) return false;

            try
            {
                var dstDir = Path.GetDirectoryName(dstFull);
                if (!string.IsNullOrEmpty(dstDir))
                {
                    Directory.CreateDirectory(dstDir);
                }

                File.Copy(srcFull, dstFull, true);

                var ext = Path.GetExtension(srcFull).ToLowerInvariant();
                if (ext == ".gltf")
                {
                    var bytes = File.ReadAllBytes(srcFull);
                    var data = new GltfFileWithResourceFilesParser(srcFull, bytes).Parse();
                    var srcDir = Path.GetDirectoryName(srcFull);
                    var dstDir2 = Path.GetDirectoryName(dstFull);

                    foreach (var buffer in data.GLTF.buffers)
                    {
                        if (string.IsNullOrEmpty(buffer.uri)) continue;
                        var srcPath = Path.Combine(srcDir, buffer.uri);
                        if (!File.Exists(srcPath)) continue;
                        var dstPath = Path.Combine(dstDir2, buffer.uri);
                        Directory.CreateDirectory(Path.GetDirectoryName(dstPath));
                        File.Copy(srcPath, dstPath, true);
                        UnityPath.FromFullpath(dstPath).ImportAsset();
                    }

                    foreach (var image in data.GLTF.images)
                    {
                        if (string.IsNullOrEmpty(image.uri)) continue;
                        var srcPath = Path.Combine(srcDir, image.uri);
                        if (!File.Exists(srcPath)) continue;
                        var dstPath = Path.Combine(dstDir2, image.uri);
                        Directory.CreateDirectory(Path.GetDirectoryName(dstPath));
                        File.Copy(srcPath, dstPath, true);
                        UnityPath.FromFullpath(dstPath).ImportAsset();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string ResolveToFullPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            if (Path.IsPathRooted(path)) return path;
            if (path.StartsWith("Assets/") || path.StartsWith("Packages/"))
            {
                return UnityPath.FromUnityPath(path).FullPath;
            }
            return Path.GetFullPath(path);
        }
    }
}
