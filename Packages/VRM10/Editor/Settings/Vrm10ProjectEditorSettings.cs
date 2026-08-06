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

        // Inverted so a missing field on older ProjectSettings assets deserializes as
        // false → export extensions enabled (intended default).
        [SerializeField]
        [Tooltip(
            "When set, Vrm10ExportExtensionRegistry handlers are not invoked during VRM " +
            "export.")]
        private bool disableExportExtensions;

        // Positive bool: missing field on older assets deserializes as false → match
        // upstream Vrm10Importer default (LoadAnimation off).
        [SerializeField]
        [Tooltip(
            "When set, .vrm ScriptedImporter loads embedded glTF animations (including " +
            "morph target weight channels) as AnimationClip sub-assets.")]
        private bool importGltfAnimations;

        public MaterialDescriptorGeneratorFactory MaterialDescriptorGeneratorFactory =>
            materialDescriptorGeneratorFactory;

        public bool EnableImportExtensions
        {
            get => !disableImportExtensions;
            set => disableImportExtensions = !value;
        }

        public bool EnableExportExtensions
        {
            get => !disableExportExtensions;
            set => disableExportExtensions = !value;
        }

        public bool ImportGltfAnimations
        {
            get => importGltfAnimations;
            set => importGltfAnimations = value;
        }

        public void Save()
        {
            Save(true);
        }
    }
}
