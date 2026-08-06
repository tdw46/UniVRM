using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace VRM10.Settings
{
    internal sealed class Vrm10ProjectSettingsProvider : SettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateProvider() => new Vrm10ProjectSettingsProvider();

        private Vrm10ProjectSettingsProvider() : base("Project/VRM10", SettingsScope.Project)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            var asset = Vrm10ProjectEditorSettings.instance;
            asset.hideFlags &= ~HideFlags.NotEditable;
            var assetObject = new SerializedObject(asset);

            var contentElement = new VisualElement
            {
                style =
                {
                    paddingLeft = 8,
                    paddingRight = 2,
                    paddingTop = 2,
                    paddingBottom = 2
                }
            };
            rootElement.Add(contentElement);
            var title = new Label
            {
                text = "VRM10",
                style =
                {
                    fontSize = 19,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            contentElement.Add(title);

            var materialField = new PropertyField(
                assetObject.FindProperty("materialDescriptorGeneratorFactory"));
            materialField.RegisterValueChangeCallback(_ => asset.Save());
            contentElement.Add(materialField);

            // Bind via EnableImportExtensions (storage is inverted disableImportExtensions).
            var importExtensionsToggle = new Toggle("Enable VRM Import Extensions")
            {
                value = asset.EnableImportExtensions
            };
            importExtensionsToggle.RegisterValueChangedCallback(evt =>
            {
                asset.EnableImportExtensions = evt.newValue;
                asset.Save();
            });
            contentElement.Add(importExtensionsToggle);

            contentElement.Add(new HelpBox(
                "When enabled, packages registered with Vrm10ImportExtensionRegistry may " +
                "run during .vrm ScriptedImporter (while AssetImportContext is live). " +
                "Disable to keep import free of third-party handlers. " +
                "Reimport .vrm assets after changing this setting.",
                HelpBoxMessageType.Info));

            var exportExtensionsToggle = new Toggle("Enable VRM Export Extensions")
            {
                value = asset.EnableExportExtensions
            };
            exportExtensionsToggle.RegisterValueChangedCallback(evt =>
            {
                asset.EnableExportExtensions = evt.newValue;
                asset.Save();
            });
            contentElement.Add(exportExtensionsToggle);

            contentElement.Add(new HelpBox(
                "When enabled, packages registered with Vrm10ExportExtensionRegistry may " +
                "run during VRM 1.0 export (pre-hierarchy strip, extra textures, root " +
                "extensions such as VRMXT_sprite_particle). Disable to keep export free of " +
                "third-party handlers.",
                HelpBoxMessageType.Info));

            var importGltfAnimationsToggle = new Toggle("Import glTF animations")
            {
                value = asset.ImportGltfAnimations
            };
            importGltfAnimationsToggle.RegisterValueChangedCallback(evt =>
            {
                asset.ImportGltfAnimations = evt.newValue;
                asset.Save();
            });
            contentElement.Add(importGltfAnimationsToggle);

            contentElement.Add(new HelpBox(
                "When enabled, .vrm ScriptedImporter loads embedded glTF animations " +
                "(node TRS and morph target weights) as AnimationClip sub-assets. " +
                "Default is off (upstream UniVRM behavior). " +
                "Reimport .vrm assets after changing this setting.",
                HelpBoxMessageType.Info));

            contentElement.Bind(assetObject);
        }
    }
}
