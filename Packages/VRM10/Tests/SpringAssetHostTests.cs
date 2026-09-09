using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace UniVRM10.Tests
{
    public class SpringAssetHostTests
    {
        [Test]
        public void HostsAreUniqueIdentityChildrenWithoutMovingExistingBones()
        {
            var root = new GameObject("parent");
            try
            {
                root.transform.position = new Vector3(1, 2, 3);
                root.transform.rotation = Quaternion.Euler(10, 30, 20);
                root.transform.localScale = new Vector3(2, 3, 4);
                var existing = new GameObject("__VRM10_Collider_0");
                existing.transform.SetParent(root.transform, false);
                var create = typeof(Vrm10Importer).GetMethod("CreateSpringAssetHost", BindingFlags.Static | BindingFlags.NonPublic);
                var first = (Transform)create.Invoke(null, new object[] { root.transform, "__VRM10_Collider_0" });
                var second = (Transform)create.Invoke(null, new object[] { root.transform, "__VRM10_Collider_0" });
                Assert.AreEqual("__VRM10_Collider_0", existing.name);
                Assert.AreEqual("__VRM10_Collider_0_1", first.name);
                Assert.AreEqual("__VRM10_Collider_0_2", second.name);
                foreach (var host in new[] { first, second })
                {
                    Assert.AreEqual(root.transform, host.parent);
                    Assert.AreEqual(Vector3.zero, host.localPosition);
                    Assert.AreEqual(Quaternion.identity, host.localRotation);
                    Assert.AreEqual(Vector3.one, host.localScale);
                    var offset = new Vector3(.2f, -.3f, .4f);
                    Assert.Less(Vector3.Distance(root.transform.TransformPoint(offset), host.TransformPoint(offset)), .00001f);
                }
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }
    }
}
