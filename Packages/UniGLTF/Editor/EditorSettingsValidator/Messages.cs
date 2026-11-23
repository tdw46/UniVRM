
using UniGLTF.M17N;

namespace UniGLTF.EditorSettingsValidator
{
    public enum Messages
    {
        [LangMsg(Languages.en, "Color space")]
        [LangMsg(Languages.ja, "色空間")]
        [LangMsg(Languages.ko, "색 공간")]
        [LangMsg(Languages.zh_HANS, "色彩空间")]
        [LangMsg(Languages.zh_HANT, "色彩空間")]
        [LangMsg(Languages.es, "Espacio de color")]
        [LangMsg(Languages.fr, "Espace colorimétrique")]
        [LangMsg(Languages.de, "Farbraum")]
        ColorSpace,
        
        [LangMsg(Languages.en, "Gamma")]
        [LangMsg(Languages.ja, "Gamma")]
        ColorSpaceGamma,
        
        [LangMsg(Languages.en, "Linear")]
        [LangMsg(Languages.ja, "Linear")]
        ColorSpaceLinear,
        
        [LangMsg(Languages.en, "You can close this window.")]
        [LangMsg(Languages.ja, "You can close this window.")]
        YouCanCloseThisWindow,
        
        [LangMsg(Languages.en, "Thank you!")]
        [LangMsg(Languages.ja, "Thank you!")]
        ThankYou,
        
        [LangMsg(Languages.en, "Close")]
        [LangMsg(Languages.ja, "閉じる")]
        [LangMsg(Languages.ko, "닫기")]
        [LangMsg(Languages.zh_HANS, "关闭")]
        [LangMsg(Languages.zh_HANT, "關閉")]
        [LangMsg(Languages.es, "Cerrar")]
        [LangMsg(Languages.fr, "Fermer")]
        [LangMsg(Languages.de, "Schließen")]
        CloseWindowButton,
        
        [LangMsg(Languages.en, "Recommended project settings for UniGLTF/UniVRM")]
        [LangMsg(Languages.ja, "Recommended project settings for UniGLTF/UniVRM")]
        RecommendedProjectSettingsForUniGltfAndUniVrm,
        
        [LangMsg(Languages.en, "current")]
        [LangMsg(Languages.ja, "current")]
        CurrentValue,
        
        [LangMsg(Languages.en, "Use recommended")]
        [LangMsg(Languages.ja, "Use recommended")]
        UseRecommended,
        
        [LangMsg(Languages.en, "Accept All")]
        [LangMsg(Languages.ja, "Accept All")]
        AcceptAllButton,

        [LangMsg(Languages.en, "VRMXT Material Setup")]
        [LangMsg(Languages.ja, "VRMXT マテリアル設定")]
        [LangMsg(Languages.ko, "VRMXT 머티리얼 설정")]
        [LangMsg(Languages.zh_HANS, "VRMXT 材质设置")]
        [LangMsg(Languages.zh_HANT, "VRMXT 材質設定")]
        [LangMsg(Languages.es, "Configuración de materiales VRMXT")]
        [LangMsg(Languages.fr, "Configuration des matériaux VRMXT")]
        [LangMsg(Languages.de, "VRMXT-Materialeinstellungen")]
        VrmxtSetup,

        [LangMsg(Languages.en, "VRM Material Setup")]
        [LangMsg(Languages.ja, "VRM マテリアル設定")]
        [LangMsg(Languages.ko, "VRM 머티리얼 설정")]
        [LangMsg(Languages.zh_HANS, "VRM 材质设置")]
        [LangMsg(Languages.zh_HANT, "VRM 材質設定")]
        [LangMsg(Languages.es, "Configuración de materiales VRM")]
        [LangMsg(Languages.fr, "Configuration des matériaux VRM")]
        [LangMsg(Languages.de, "VRM-Materialeinstellungen")]
        VrmSetup,

        [LangMsg(Languages.en, "Use Linear color space for character materials")]
        [LangMsg(Languages.ja, "キャラクターのマテリアルにリニア色空間を使用")]
        [LangMsg(Languages.ko, "캐릭터 머티리얼에 선형 색 공간 사용")]
        [LangMsg(Languages.zh_HANS, "为角色材质使用线性色彩空间")]
        [LangMsg(Languages.zh_HANT, "為角色材質使用線性色彩空間")]
        [LangMsg(Languages.es, "Usar el espacio de color lineal para el personaje")]
        [LangMsg(Languages.fr, "Utiliser un espace linéaire pour les matériaux du personnage")]
        [LangMsg(Languages.de, "Linearen Farbraum für Charaktermaterialien verwenden")]
        LinearHeading,

        [LangMsg(Languages.en, "Click Apply Linear color space to preview the imported character’s VRMXT materials with their intended lighting and shading.")]
        [LangMsg(Languages.ja, "インポートしたキャラクターの VRMXT マテリアルを意図した光と陰影で表示するには、「リニア色空間を適用」をクリックしてください。")]
        [LangMsg(Languages.ko, "가져온 캐릭터의 VRMXT 머티리얼을 의도한 조명과 음영으로 보려면 ‘선형 색 공간 적용’을 클릭하세요.")]
        [LangMsg(Languages.zh_HANS, "点击“应用线性色彩空间”，以预期的光照和明暗效果预览导入角色的 VRMXT 材质。")]
        [LangMsg(Languages.zh_HANT, "按一下「套用線性色彩空間」，以預期的光照與陰影預覽匯入角色的 VRMXT 材質。")]
        [LangMsg(Languages.es, "Haz clic en «Aplicar espacio de color lineal» para ver los materiales VRMXT del personaje con la iluminación y el sombreado previstos.")]
        [LangMsg(Languages.fr, "Cliquez sur « Appliquer l’espace linéaire » pour afficher les matériaux VRMXT du personnage avec l’éclairage et les ombres prévus.")]
        [LangMsg(Languages.de, "Klicke auf „Linearen Farbraum anwenden“, um die VRMXT-Materialien der Figur mit der vorgesehenen Beleuchtung und Schattierung anzuzeigen.")]
        VrmxtExplanation,

        [LangMsg(Languages.en, "Apply Linear color space to preview imported VRM materials with the lighting recommended by UniVRM.")]
        [LangMsg(Languages.ja, "UniVRM が推奨するライティングでインポートした VRM マテリアルを表示するには、リニア色空間を適用してください。")]
        [LangMsg(Languages.ko, "UniVRM에서 권장하는 조명으로 VRM 머티리얼을 보려면 선형 색 공간을 적용하세요.")]
        [LangMsg(Languages.zh_HANS, "应用线性色彩空间，以 UniVRM 推荐的光照预览导入的 VRM 材质。")]
        [LangMsg(Languages.zh_HANT, "套用線性色彩空間，以 UniVRM 建議的光照預覽匯入的 VRM 材質。")]
        [LangMsg(Languages.es, "Aplica el espacio de color lineal para ver los materiales VRM con la iluminación recomendada por UniVRM.")]
        [LangMsg(Languages.fr, "Appliquez l’espace linéaire pour afficher les matériaux VRM avec l’éclairage recommandé par UniVRM.")]
        [LangMsg(Languages.de, "Wende den linearen Farbraum an, um VRM-Materialien mit der von UniVRM empfohlenen Beleuchtung anzuzeigen.")]
        VrmExplanation,

        [LangMsg(Languages.en, "This change affects every scene in this Unity project.")]
        [LangMsg(Languages.ja, "この変更は、この Unity プロジェクト内のすべてのシーンに適用されます。")]
        [LangMsg(Languages.ko, "이 변경 사항은 이 Unity 프로젝트의 모든 씬에 적용됩니다.")]
        [LangMsg(Languages.zh_HANS, "此更改会影响当前 Unity 项目中的所有场景。")]
        [LangMsg(Languages.zh_HANT, "此變更會影響這個 Unity 專案中的所有場景。")]
        [LangMsg(Languages.es, "Este cambio afecta a todas las escenas de este proyecto de Unity.")]
        [LangMsg(Languages.fr, "Ce changement s’applique à toutes les scènes de ce projet Unity.")]
        [LangMsg(Languages.de, "Diese Änderung gilt für alle Szenen in diesem Unity-Projekt.")]
        ProjectScope,

        [LangMsg(Languages.en, "Later")]
        [LangMsg(Languages.ja, "後で")]
        [LangMsg(Languages.ko, "나중에")]
        [LangMsg(Languages.zh_HANS, "稍后")]
        [LangMsg(Languages.zh_HANT, "稍後")]
        [LangMsg(Languages.es, "Más tarde")]
        [LangMsg(Languages.fr, "Plus tard")]
        [LangMsg(Languages.de, "Später")]
        Later,

        [LangMsg(Languages.en, "Apply Linear color space")]
        [LangMsg(Languages.ja, "リニア色空間を適用")]
        [LangMsg(Languages.ko, "선형 색 공간 적용")]
        [LangMsg(Languages.zh_HANS, "应用线性色彩空间")]
        [LangMsg(Languages.zh_HANT, "套用線性色彩空間")]
        [LangMsg(Languages.es, "Aplicar espacio de color lineal")]
        [LangMsg(Languages.fr, "Appliquer l’espace linéaire")]
        [LangMsg(Languages.de, "Linearen Farbraum anwenden")]
        ApplyLinear,

        [LangMsg(Languages.en, "Material import settings are ready")]
        [LangMsg(Languages.ja, "マテリアルのインポート設定は完了しています")]
        [LangMsg(Languages.ko, "머티리얼 임포트 설정 완료")]
        [LangMsg(Languages.zh_HANS, "材质导入设置已就绪")]
        [LangMsg(Languages.zh_HANT, "材質匯入設定已就緒")]
        [LangMsg(Languages.es, "La importación de materiales está configurada")]
        [LangMsg(Languages.fr, "Les paramètres d’importation des matériaux sont prêts")]
        [LangMsg(Languages.de, "Die Einstellungen für den Materialimport sind bereit")]
        SettingsReady,
    }
}