using System.Text;
using NUnit.Framework;
using UniGLTF;
using UnityEngine;
using VrmLib;

namespace UniVRM10.Test
{
    public sealed class Vrm10ExportExtensionTests
    {
        [Test]
        public void AddRootExtension_AndAddMaterialExtension_RegisterExtensionsUsedOnce()
        {
            var root = new GameObject("export-root");
            try
            {
                var storage = new ExportingGltfData();
                storage.Gltf.materials.Add(new glTFMaterial { name = "mat0" });
                storage.Gltf.extensionsUsed.Add("VRMC_vrm");
                storage.Gltf.extensionsRequired.Add("VRMC_vrm");

                var context = CreateBoundContext(root, storage);
                var payload = Encoding.UTF8.GetBytes(@"{""specVersion"":""1.0"",""emitters"":[]}");

                context.AddRootExtension("VRMXT_sprite_particle", payload);
                context.AddRootExtension("VRMXT_sprite_particle", payload);
                context.AddMaterialExtension(
                    0,
                    "VRMXT_materials_override",
                    Encoding.UTF8.GetBytes(@"{}"));
                context.AddMaterialExtension(
                    0,
                    "VRMXT_materials_override",
                    Encoding.UTF8.GetBytes(@"{}"));

                Assert.AreEqual(1, Count(storage.Gltf.extensionsUsed, "VRMXT_sprite_particle"));
                Assert.AreEqual(1, Count(storage.Gltf.extensionsUsed, "VRMXT_materials_override"));
                Assert.IsFalse(storage.Gltf.extensionsRequired.Contains("VRMXT_sprite_particle"));
                Assert.IsFalse(storage.Gltf.extensionsRequired.Contains("VRMXT_materials_override"));
                Assert.AreEqual(1, Count(storage.Gltf.extensionsRequired, "VRMC_vrm"));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        private static Vrm10ExportExtensionContext CreateBoundContext(
            GameObject root,
            ExportingGltfData storage)
        {
            var context = new Vrm10ExportExtensionContext(root);
            context.Bind(
                converter: new ModelExporter(),
                model: new Model(Coordinates.Vrm1),
                storage: storage,
                textureExporter: new TextureExporter(new EditorTextureSerializer()));
            return context;
        }

        private static int Count(System.Collections.Generic.IList<string> list, string value)
        {
            var count = 0;
            if (list == null)
            {
                return 0;
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] == value)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
