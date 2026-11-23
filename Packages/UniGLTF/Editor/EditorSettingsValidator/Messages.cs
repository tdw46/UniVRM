ï»¿
using UniGLTF.M17N;

namespace UniGLTF.EditorSettingsValidator
{
    public enum Messages
    {
        [LangMsg(Languages.en, "Color space")]
        [LangMsg(Languages.ja, "Color space")]
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
        [LangMsg(Languages.ja, "Close")]
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
        VrmxtSetup,

        [LangMsg(Languages.en, "VRM Material Setup")]
        [LangMsg(Languages.ja, "VRM マテリアル設定")]
        VrmSetup,

        [LangMsg(Languages.en, "Use Linear color space for character materials")]
        [LangMsg(Languages.ja, "キャラクターのマテリアルにリニア色空間を使用")]
        LinearHeading,

        [LangMsg(Languages.en, "Click Apply Linear color space to preview the imported character’s VRMXT materials with their intended lighting and shading.")]
        [LangMsg(Languages.ja, "インポートしたキャラクターの VRMXT マテリアルを意図した光と陰影で表示するには、「リニア色空間を適用」をクリックしてください。")]
        VrmxtExplanation,

        [LangMsg(Languages.en, "Apply Linear color space to preview imported VRM materials with the lighting recommended by UniVRM.")]
        [LangMsg(Languages.ja, "UniVRM が推奨するライティングでインポートした VRM マテリアルを表示するには、リニア色空間を適用してください。")]
        VrmExplanation,

        [LangMsg(Languages.en, "This change affects every scene in this Unity project.")]
        [LangMsg(Languages.ja, "この変更は、この Unity プロジェクト内のすべてのシーンに適用されます。")]
        ProjectScope,

        [LangMsg(Languages.en, "Later")]
        [LangMsg(Languages.ja, "後で")]
        Later,

        [LangMsg(Languages.en, "Apply Linear color space")]
        [LangMsg(Languages.ja, "リニア色空間を適用")]
        ApplyLinear,

        [LangMsg(Languages.en, "Material import settings are ready")]
        [LangMsg(Languages.ja, "マテリアルのインポート設定は完了しています")]
        SettingsReady,
    }
}