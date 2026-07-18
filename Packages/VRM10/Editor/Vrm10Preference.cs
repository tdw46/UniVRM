using UnityEditor;

namespace UniVRM10
{
    /// <summary>
    /// User preferences for VRM 1.0 Editor behavior.
    /// </summary>
    public static class Vrm10Preference
    {
        const string EnableImportExtensionsKey = "VRM10_ENABLE_IMPORT_EXTENSIONS";

        /// <summary>
        /// When true, <see cref="Vrm10ImportExtensionRegistry"/> handlers run during
        /// <c>.vrm</c> ScriptedImporter. When false, import does not invoke registered
        /// extension handlers.
        /// Default: enabled.
        /// </summary>
        public static bool EnableImportExtensions
        {
            get => EditorPrefs.GetBool(EnableImportExtensionsKey, defaultValue: true);
            set => EditorPrefs.SetBool(EnableImportExtensionsKey, value);
        }

#if UNITY_2019_1_OR_NEWER
        [SettingsProvider]
        static SettingsProvider CreatePreferenceProvider()
        {
            var provider = new SettingsProvider("Preferences/VRM10", SettingsScope.User);
            provider.guiHandler = _ => OnPreferenceGUI();
            return provider;
        }
#else
        [PreferenceItem("VRM10")]
#endif
        private static void OnPreferenceGUI()
        {
            EditorGUI.BeginChangeCheck();
            var enabled = EditorGUILayout.Toggle(
                "Enable VRM import extensions",
                EnableImportExtensions);
            if (EditorGUI.EndChangeCheck())
            {
                EnableImportExtensions = enabled;
            }

            EditorGUILayout.HelpBox(
                "When enabled, packages registered with Vrm10ImportExtensionRegistry may " +
                "run during .vrm ScriptedImporter (while AssetImportContext is live). " +
                "Disable to keep import free of third-party handlers.\n" +
                "Reimport .vrm assets after changing this setting.",
                MessageType.Info,
                true);
        }
    }
}
