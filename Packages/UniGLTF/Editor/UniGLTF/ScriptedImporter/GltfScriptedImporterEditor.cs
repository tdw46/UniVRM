using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif


namespace UniGLTF
{
    [CustomEditor(typeof(GltfScriptedImporter))]
    public class GltfScriptedImporterEditor : RemapScriptedImporterEditorBase
    {
        GltfData m_data;

        RemapEditorMaterial m_materialEditor;
        RemapEditorAnimation m_animationEditor;

        public override void OnEnable()
        {
            base.OnEnable();

            m_importer = target as GltfScriptedImporter;
            if (m_data != null)
            {
                m_data.Dispose();
            }
            m_data = new AutoGltfFileParser(m_importer.assetPath).Parse();

            var materialGenerator = new BuiltInGltfMaterialDescriptorGenerator();
            var materialKeys = m_data.GLTF.materials.Select((_, i) => materialGenerator.Get(m_data, i).SubAssetKey);
            var textureKeys = new GltfTextureDescriptorGenerator(m_data).Get().GetEnumerable().Select(x => x.SubAssetKey);
            m_materialEditor = new RemapEditorMaterial(materialKeys.Concat(textureKeys), GetEditorMap, SetEditorMap);
            m_animationEditor = new RemapEditorAnimation(AnimationImporterUtil.EnumerateSubAssetKeys(m_data.GLTF), GetEditorMap, SetEditorMap);
        }

        public override void OnDisable()
        {
            m_data.Dispose();
            m_data = null;

            base.OnDisable();
        }

        enum Tabs
        {
            Model,
            Animation,
            Materials,
        }
        static Tabs s_currentTab;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            s_currentTab = MeshUtility.TabBar.OnGUI(s_currentTab);
            GUILayout.Space(10);

            switch (s_currentTab)
            {
                case Tabs.Model:
                    DrawSourceOverrideGUI();
                    base.OnInspectorGUI();
                    break;

                case Tabs.Animation:
                    m_animationEditor.OnGUI(m_importer, m_data);
                    ApplyRevertGUI();
                    break;

                case Tabs.Materials:
                    m_materialEditor.OnGUI(m_importer, m_data,
                    new GltfTextureDescriptorGenerator(m_data),
                    assetPath => $"{Path.GetFileNameWithoutExtension(assetPath)}.Textures",
                    assetPath => $"{Path.GetFileNameWithoutExtension(assetPath)}.Materials");
                    ApplyRevertGUI();
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSourceOverrideGUI()
        {
            if (m_importer == null) return;

            var useProp = serializedObject.FindProperty("m_useSourcePath");
            var pathProp = serializedObject.FindProperty("m_sourcePath");

            EditorGUILayout.LabelField("Reimport Source", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                EditorGUILayout.PropertyField(useProp, new GUIContent("Use Source Override"));
                using (new EditorGUI.DisabledScope(!useProp.boolValue))
                {
                    EditorGUILayout.PropertyField(pathProp, new GUIContent("Source Path"));
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Pick..."))
                    {
                        var picked = EditorUtility.OpenFilePanel("Select glTF/GLB", "", "gltf,glb,zip");
                        if (!string.IsNullOrEmpty(picked))
                        {
                            pathProp.stringValue = picked;
                            useProp.boolValue = true;
                        }
                    }
                    if (GUILayout.Button("Clear"))
                    {
                        pathProp.stringValue = string.Empty;
                        useProp.boolValue = false;
                    }
                }

                if (useProp.boolValue)
                {
                    var resolved = ((GltfScriptedImporterBase)m_importer).GetResolvedSourcePath();
                    var exists = !string.IsNullOrEmpty(resolved) && File.Exists(resolved);
                    var message = exists
                        ? $"Reimporting from: {resolved}"
                        : "Source path does not exist.";
                    EditorGUILayout.HelpBox(message, exists ? MessageType.Info : MessageType.Warning);
                }
            }

            GUILayout.Space(6);
        }
    }
}
