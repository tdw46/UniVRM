using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace UniVRM10
{
    /// <summary>
    /// Optional consumer hook invoked during <c>VrmScriptedImporter</c> while
    /// <see cref="AssetImportContext"/> is still live. Soft-detect via
    /// <see cref="Vrm10ImportExtensionRegistry"/>.
    /// </summary>
    public interface IVrm10ImportExtension
    {
        void OnVrmImported(Vrm10ImportExtensionContext context);
    }

    /// <summary>
    /// Import-time context for <see cref="IVrm10ImportExtension"/>.
    /// </summary>
    public sealed class Vrm10ImportExtensionContext
    {
        public Vrm10ImportExtensionContext(
            AssetImportContext assetContext,
            string assetPath,
            GameObject root,
            string json,
            IReadOnlyList<Transform> nodes)
        {
            AssetContext = assetContext ?? throw new ArgumentNullException(nameof(assetContext));
            AssetPath = assetPath;
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Json = json ?? string.Empty;
            Nodes = nodes ?? Array.Empty<Transform>();
        }

        public AssetImportContext AssetContext { get; }

        /// <summary>Project-relative path of the imported <c>.vrm</c>.</summary>
        public string AssetPath { get; }

        public GameObject Root { get; }

        /// <summary>Full glTF JSON text from the parse used for this import.</summary>
        public string Json { get; }

        /// <summary>
        /// glTF node index to Transform (same order as runtime <c>RuntimeGltfInstance.Nodes</c>).
        /// Valid for the duration of <see cref="IVrm10ImportExtension.OnVrmImported"/>.
        /// </summary>
        public IReadOnlyList<Transform> Nodes { get; }

        public void AddObjectToAsset(string identifier, UnityEngine.Object obj)
        {
            if (obj == null)
            {
                return;
            }

            AssetContext.AddObjectToAsset(identifier, obj);
        }
    }

    /// <summary>
    /// Registry for <see cref="IVrm10ImportExtension"/>. Presence of this type in
    /// <c>VRM10.Editor</c> is the soft signal that import hooks exist.
    /// Invocation is gated by <see cref="IsEnabled"/> (<see cref="Vrm10Preference.EnableImportExtensions"/>).
    /// </summary>
    public static class Vrm10ImportExtensionRegistry
    {
        private static readonly List<IVrm10ImportExtension> Typed = new List<IVrm10ImportExtension>();
        private static readonly List<Action<object>> Untyped = new List<Action<object>>();

        /// <summary>Always true when this assembly ships the import extension API.</summary>
        public static bool IsAvailable => true;

        /// <summary>
        /// User preference: when false, <see cref="InvokeAll"/> is a no-op and consumers
        /// should treat hooks as unavailable.
        /// </summary>
        public static bool IsEnabled => Vrm10Preference.EnableImportExtensions;

        public static void Register(IVrm10ImportExtension extension)
        {
            if (extension == null)
            {
                throw new ArgumentNullException(nameof(extension));
            }

            if (!Typed.Contains(extension))
            {
                Typed.Add(extension);
            }
        }

        public static void Unregister(IVrm10ImportExtension extension)
        {
            if (extension != null)
            {
                Typed.Remove(extension);
            }
        }

        /// <summary>
        /// Soft-binding entry for packages that must not hard-reference <c>VRM10.Editor</c>.
        /// Handler receives a <see cref="Vrm10ImportExtensionContext"/> boxed as
        /// <see cref="object"/>.
        /// </summary>
        public static void RegisterHandler(Action<object> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (!Untyped.Contains(handler))
            {
                Untyped.Add(handler);
            }
        }

        public static void UnregisterHandler(Action<object> handler)
        {
            if (handler != null)
            {
                Untyped.Remove(handler);
            }
        }

        internal static void InvokeAll(Vrm10ImportExtensionContext context)
        {
            if (context == null || !IsEnabled)
            {
                return;
            }

            for (var i = 0; i < Typed.Count; i++)
            {
                try
                {
                    Typed[i].OnVrmImported(context);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            for (var i = 0; i < Untyped.Count; i++)
            {
                try
                {
                    Untyped[i].Invoke(context);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}
