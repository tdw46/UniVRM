using UnityEditor;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using UnityEngine;
using UniGLTF.M17N;

namespace UniGLTF.EditorSettingsValidator
{
    [InitializeOnLoad]
    public sealed class UnityEditorSettingsValidatorWindow : EditorWindow
    {
        private const string DeferredKey = "UniGLTF.LinearColorSpace.Deferred";
        private static readonly UnityColorSpaceSettingsValidator ColorSpace = new UnityColorSpaceSettingsValidator();
        private bool _usesVrmxt;

        static UnityEditorSettingsValidatorWindow()
        {
#if !UNITY_WEBGL
            EditorApplication.delayCall += ShowIfNeeded;
#endif
        }

        private static void ShowIfNeeded()
        {
            if (!Application.isBatchMode && !ColorSpace.IsValid && !SessionState.GetBool(DeferredKey, false))
            {
                Open();
            }
        }

        [MenuItem("UniGLTF/Material Import Settings")]
        private static void Open()
        {
            GetWindow<UnityEditorSettingsValidatorWindow>(utility: true);
        }

        private void OnEnable()
        {
            _usesVrmxt = PackageInfo.FindForAssetPath("Packages/com.vrmxt.univrmxt/package.json") != null;
            titleContent = new GUIContent((_usesVrmxt ? Messages.VrmxtSetup : Messages.VrmSetup).Msg());
            minSize = new Vector2(425, 222);
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(new GUIStyle { padding = new RectOffset(16, 16, 14, 14) }))
            {
                if (ColorSpace.IsValid)
                {
                    EditorGUILayout.LabelField(Messages.SettingsReady.Msg(), EditorStyles.boldLabel);
                    EditorGUILayout.LabelField(Messages.ColorSpace.Msg(), ColorSpace.CurrentValueDescription);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button(Messages.CloseWindowButton.Msg())) Close();
                    return;
                }

                EditorGUILayout.LabelField(Messages.LinearHeading.Msg(), EditorStyles.boldLabel);
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField((_usesVrmxt ? Messages.VrmxtExplanation : Messages.VrmExplanation).Msg(),
                    EditorStyles.wordWrappedLabel);
                EditorGUILayout.Space(10);
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField(Messages.ColorSpace.Msg(),
                        ColorSpace.CurrentValueDescription + "  →  " + ColorSpace.RecommendedValueDescription);
                    EditorGUILayout.LabelField(Messages.ProjectScope.Msg(), EditorStyles.wordWrappedMiniLabel);
                }

                GUILayout.FlexibleSpace();
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(Messages.Later.Msg(), GUILayout.Height(26)))
                    {
                        SessionState.SetBool(DeferredKey, true);
                        Close();
                    }
                    GUILayout.Space(4);
                    if (GUILayout.Button(Messages.ApplyLinear.Msg(), GUILayout.MinWidth(190), GUILayout.Height(26)))
                    {
                        ColorSpace.Validate();
                        Close();
                    }
                }
            }
        }
    }
}
