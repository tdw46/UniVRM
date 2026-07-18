using UniVRM10;
using UnityEditor;

namespace VRM10.Settings
{
    /// <summary>
    /// Bridges Project Settings export-extension gate into Runtime
    /// <see cref="Vrm10ExportExtensionRegistry"/>.
    /// </summary>
    [InitializeOnLoad]
    internal static class Vrm10ExportExtensionSettingsBridge
    {
        static Vrm10ExportExtensionSettingsBridge()
        {
            Vrm10ExportExtensionRegistry.IsEnabledProvider = () =>
                Vrm10ProjectEditorSettings.instance.EnableExportExtensions;
        }
    }
}
