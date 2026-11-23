using UniGLTF;
using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif


namespace UniVRM10
{
    [ScriptedImporter(2, "vrm")]
    public class VrmScriptedImporter : ScriptedImporter
    {
        [SerializeField]
        public bool MigrateToVrm1 = true;

        [SerializeField]
        public ImporterRenderPipelineTypes RenderPipeline = default;

        // Shader.Find may still be empty in a clean import worker. Request the
        // shader artifacts before Unity schedules any VRM that consumes them.
        internal const string Mtoon = "Packages/com.vrmc.vrm/MToon10/Shaders/vrmc_materials_mtoon.shader";
        internal const string MtoonUrp = "Packages/com.vrmc.vrm/MToon10/Shaders/vrmc_materials_mtoon_urp.shader";
        internal const string UniUnlit = "Packages/com.vrmc.gltf/UniUnlit/Shaders/UniUnlit.shader";

        static string[] GatherDependenciesFromSourceFile(string path)
            => new[] { Mtoon, MtoonUrp, UniUnlit };

        public override void OnImportAsset(AssetImportContext ctx)
        {
            VrmScriptedImporterImpl.Import(this, ctx, MigrateToVrm1, RenderPipeline);
        }
    }
}
