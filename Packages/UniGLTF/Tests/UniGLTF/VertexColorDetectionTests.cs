using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace UniGLTF
{
    public class VertexColorDetectionTests
    {
        [TestCase(glComponentType.UNSIGNED_BYTE, 3)]
        [TestCase(glComponentType.UNSIGNED_BYTE, 4)]
        [TestCase(glComponentType.UNSIGNED_SHORT, 3)]
        [TestCase(glComponentType.UNSIGNED_SHORT, 4)]
        [TestCase(glComponentType.FLOAT, 3)]
        [TestCase(glComponentType.FLOAT, 4)]
        public void DetectsAllLegalColorFormats(glComponentType type, int components)
        {
            // Five vertices also catches byte lengths that cannot be aliased
            // as float RGBA. RGB black is opaque, whereas RGBA zero is unused.
            foreach (var nonzero in new[] { false, true })
            {
                using var stream = new MemoryStream();
                using var writer = new BinaryWriter(stream);
                for (var vertex = 0; vertex < 5; ++vertex)
                for (var component = 0; component < components; ++component)
                {
                    var on = nonzero && vertex == 4 && component == 0;
                    switch (type)
                    {
                        case glComponentType.UNSIGNED_BYTE: writer.Write((byte)(on ? 255 : 0)); break;
                        case glComponentType.UNSIGNED_SHORT: writer.Write((ushort)(on ? 65535 : 0)); break;
                        default: writer.Write(on ? 1f : 0f); break;
                    }
                }
                var bytes = stream.ToArray();
                var attributes = new glTFAttributes { COLOR_0 = 0 };
                var gltf = new glTF
                {
                    buffers = new List<glTFBuffer> { new glTFBuffer { byteLength = bytes.Length } },
                    bufferViews = new List<glTFBufferView> { new glTFBufferView { buffer = 0, byteLength = bytes.Length } },
                    accessors = new List<glTFAccessor> { new glTFAccessor
                    {
                        bufferView = 0, componentType = type, count = 5,
                        type = components == 3 ? "VEC3" : "VEC4", normalized = type != glComponentType.FLOAT,
                    } },
                    materials = new List<glTFMaterial> { new glTFMaterial() },
                    meshes = new List<glTFMesh> { new glTFMesh { primitives = new List<glTFPrimitives>
                    {
                        new glTFPrimitives { attributes = attributes, material = 0 },
                    } } },
                };
                using var data = GltfData.CreateFromGltfDataForTest(gltf, new ArraySegment<byte>(bytes));
                Assert.AreEqual(nonzero || components == 3, data.HasVertexColor(attributes));
                Assert.AreEqual(nonzero || components == 3, data.MaterialHasVertexColor(0));
                Assert.IsFalse(data.HasVertexColor(new glTFAttributes()));
            }
        }
    }
}
