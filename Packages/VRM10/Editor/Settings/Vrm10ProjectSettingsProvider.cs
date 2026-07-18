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

            var importExtensionsField = new PropertyField(
                assetObject.FindProperty("enableImportExtensions"),
                "Enable VRM Import Extensions");
            importExtensionsField.RegisterValueChangeCallback(_ => asset.Save());
            contentElement.Add(importExtensionsField);

            contentElement.Add(new HelpBox(
                "When enabled, packages registered with Vrm10ImportExtensionRegistry may " +
                "run during .vrm ScriptedImporter (while AssetImportContext is live). " +
                "Disable to keep import free of third-party handlers. " +
                "Reimport .vrm assets after changing this setting.",
                HelpBoxMessageType.Info));

            contentElement.Bind(assetObject);
        }
    }
}
