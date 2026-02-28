using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UniGLTF;

namespace VRM
{
    public static class VRMReimportSourceMenu
    {
        private const string MenuRoot = "Assets/VRM";
        private const string ReimportFromSource = MenuRoot + "/Reimport From Source";
        private const string ReimportFromFile = MenuRoot + "/Reimport From File...";
        private const string ClearSource = MenuRoot + "/Clear Source Override";
        private const string SourceKey = "VRM_SOURCE_PATH=";

        [MenuItem(ReimportFromSource, priority = 1000)]
        private static void ReimportFromSourceMenu()
        {
            if (!TryGetVrmAsset(out var assetPath, out var importer)) return;
            var sourcePath = GetSourcePath(importer);
            if (!string.IsNullOrEmpty(sourcePath) && TryCopySourceIntoAsset(sourcePath, assetPath))
            {
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                return;
            }
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        [MenuItem(ReimportFromSource, validate = true)]
        private static bool ReimportFromSourceValidate()
        {
            if (!TryGetVrmAsset(out var _, out var importer)) return false;
            var source = GetSourcePath(importer);
            var resolved = ResolveToFullPath(source);
            return !string.IsNullOrEmpty(resolved) && File.Exists(resolved);
        }

        [MenuItem(ReimportFromFile, priority = 1001)]
        private static void ReimportFromFileMenu()
        {
            if (!TryGetVrmAsset(out var assetPath, out var importer)) return;
            var picked = EditorUtility.OpenFilePanel("Select VRM", "", "vrm");
            if (string.IsNullOrEmpty(picked)) return;
            SetSourcePath(importer, picked);
            if (TryCopySourceIntoAsset(picked, assetPath))
            {
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                return;
            }
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        [MenuItem(ReimportFromFile, validate = true)]
        private static bool ReimportFromFileValidate()
        {
            return TryGetVrmAsset(out var _, out var _);
        }

        [MenuItem(ClearSource, priority = 1002)]
        private static void ClearSourceMenu()
        {
            if (!TryGetVrmAsset(out var assetPath, out var importer)) return;
            SetSourcePath(importer, string.Empty);
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        }

        [MenuItem(ClearSource, validate = true)]
        private static bool ClearSourceValidate()
        {
            if (!TryGetVrmAsset(out var _, out var importer)) return false;
            return !string.IsNullOrEmpty(GetSourcePath(importer));
        }

        private static bool TryGetVrmAsset(out string assetPath, out AssetImporter importer)
        {
            assetPath = null;
            importer = null;
            var obj = Selection.activeObject;
            if (obj == null) return false;
            assetPath = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(assetPath)) return false;
            if (Path.GetExtension(assetPath).ToLowerInvariant() != ".vrm") return false;
            importer = AssetImporter.GetAtPath(assetPath);
            return importer != null;
        }

        private static string GetSourcePath(AssetImporter importer)
        {
            if (importer == null) return null;
            var userData = importer.userData ?? string.Empty;
            foreach (var line in userData.Split('\n'))
            {
                if (line.StartsWith(SourceKey))
                {
                    return line.Substring(SourceKey.Length);
                }
            }
            return null;
        }

        private static void SetSourcePath(AssetImporter importer, string path)
        {
            if (importer == null) return;
            var userData = importer.userData ?? string.Empty;
            var lines = new List<string>();
            foreach (var line in userData.Split('\n'))
            {
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith(SourceKey)) continue;
                lines.Add(line);
            }
            if (!string.IsNullOrEmpty(path))
            {
                lines.Add(SourceKey + path);
            }
            importer.userData = string.Join("\n", lines);
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
