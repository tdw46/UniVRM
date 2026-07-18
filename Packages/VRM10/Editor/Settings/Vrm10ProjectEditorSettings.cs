using UnityEditor;
using UnityEngine;

namespace VRM10.Settings
{
    [FilePath("ProjectSettings/Vrm10ProjectEditorSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class Vrm10ProjectEditorSettings : ScriptableSingleton<Vrm10ProjectEditorSettings>
    {
        [SerializeField] private MaterialDescriptorGeneratorFactory materialDescriptorGeneratorFactory;

        [SerializeField]
        [Tooltip(
            "When enabled, packages registered with Vrm10ImportExtensionRegistry may run " +
            "during .vrm ScriptedImporter. Disable to keep import free of third-party handlers.")]
        private bool enableImportExtensions = true;

        public MaterialDescriptorGeneratorFactory MaterialDescriptorGeneratorFactory =>
            materialDescriptorGeneratorFactory;

        public bool EnableImportExtensions => enableImportExtensions;

        public void Save()
        {
            Save(true);
        }
    }
}
