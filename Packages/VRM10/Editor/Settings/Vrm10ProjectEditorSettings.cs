using UnityEditor;
using UnityEngine;

namespace VRM10.Settings
{
    [FilePath("ProjectSettings/Vrm10ProjectEditorSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class Vrm10ProjectEditorSettings : ScriptableSingleton<Vrm10ProjectEditorSettings>
    {
        [SerializeField] private MaterialDescriptorGeneratorFactory materialDescriptorGeneratorFactory;

        // Inverted so a missing field on older ProjectSettings assets deserializes as
        // false → extensions enabled (intended default). A positive bool would become false.
        [SerializeField]
        [Tooltip(
            "When set, Vrm10ImportExtensionRegistry handlers are not invoked during .vrm " +
            "ScriptedImporter.")]
        private bool disableImportExtensions;

        public MaterialDescriptorGeneratorFactory MaterialDescriptorGeneratorFactory =>
            materialDescriptorGeneratorFactory;

        public bool EnableImportExtensions
        {
            get => !disableImportExtensions;
            set => disableImportExtensions = !value;
        }

        public void Save()
        {
            Save(true);
        }
    }
}
