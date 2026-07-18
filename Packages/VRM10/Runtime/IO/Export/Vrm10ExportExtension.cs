using System;
using System.Collections.Generic;
using UniGLTF;
using UnityEngine;
using VrmLib;

namespace UniVRM10
{
    /// <summary>
    /// Optional consumer hook during VRM 1.0 export. Soft-detect via
    /// <see cref="Vrm10ExportExtensionRegistry"/>.
    /// </summary>
    public interface IVrm10ExportExtension
    {
        void OnVrmExport(Vrm10ExportExtensionContext context);
    }

    public enum Vrm10ExportExtensionPhase
    {
        /// <summary>
        /// Before <see cref="ModelExporter"/> walks the hierarchy. Strip ephemeral
        /// preview objects (e.g. VFX <c>ParticleSystem</c> children) here.
        /// </summary>
        PreHierarchy,

        /// <summary>
        /// After materials/meshes/nodes and VRM core registration, before
        /// <see cref="ITextureExporter"/> flush. Register extension-only textures.
        /// </summary>
        PrepareTextures,

        /// <summary>
        /// After VRMC root extensions are written. Add optional root extensions
        /// (e.g. <c>VRMXT_vfx</c>).
        /// </summary>
        WriteExtensions,
    }

    /// <summary>
    /// Export-time context for <see cref="IVrm10ExportExtension"/>. One instance is
    /// reused across <see cref="Vrm10ExportExtensionPhase"/> values for a single export.
    /// </summary>
    public sealed class Vrm10ExportExtensionContext
    {
        public Vrm10ExportExtensionContext(GameObject root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
        }

        public Vrm10ExportExtensionPhase Phase { get; internal set; }

        public GameObject Root { get; }

        public ModelExporter Converter { get; private set; }

        public Model Model { get; private set; }

        public ExportingGltfData Storage { get; private set; }

        public ITextureExporter TextureExporter { get; private set; }

        /// <summary>
        /// Cross-phase bag for soft-bound handlers (same context object for PreHierarchy →
        /// WriteExtensions).
        /// </summary>
        public Dictionary<string, object> UserData { get; } = new Dictionary<string, object>();

        internal void Bind(
            ModelExporter converter,
            Model model,
            ExportingGltfData storage,
            ITextureExporter textureExporter)
        {
            Converter = converter ?? throw new ArgumentNullException(nameof(converter));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
            TextureExporter = textureExporter
                ?? throw new ArgumentNullException(nameof(textureExporter));
        }

        /// <summary>
        /// glTF node index for <paramref name="transform"/>, or null if not in the export model.
        /// </summary>
        public int? TryGetNodeIndex(Transform transform)
        {
            if (transform == null || Converter == null || Model == null)
            {
                return null;
            }

            if (!Converter.Nodes.TryGetValue(transform.gameObject, out var node))
            {
                return null;
            }

            var index = Model.Nodes.IndexOf(node);
            return index >= 0 ? index : (int?)null;
        }

        /// <summary>
        /// Register an sRGB texture for export. Valid in
        /// <see cref="Vrm10ExportExtensionPhase.PrepareTextures"/>.
        /// </summary>
        public int RegisterSRgbTexture(Texture texture, bool needsAlpha)
        {
            if (TextureExporter == null)
            {
                throw new InvalidOperationException(
                    "TextureExporter is not bound until PrepareTextures / WriteExtensions.");
            }

            if (texture == null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            return TextureExporter.RegisterExportingAsSRgb(texture, needsAlpha);
        }

        /// <summary>
        /// Write a root glTF extension object (UTF-8 JSON body, no wrapping key).
        /// Valid in <see cref="Vrm10ExportExtensionPhase.WriteExtensions"/>.
        /// </summary>
        public void AddRootExtension(string extensionName, byte[] utf8Json)
        {
            if (string.IsNullOrEmpty(extensionName))
            {
                throw new ArgumentException("extensionName is required.", nameof(extensionName));
            }

            if (utf8Json == null)
            {
                throw new ArgumentNullException(nameof(utf8Json));
            }

            if (Storage == null)
            {
                throw new InvalidOperationException(
                    "Storage is not bound until PrepareTextures / WriteExtensions.");
            }

            var exported = glTFExtensionExport.GetOrCreate(ref Storage.Gltf.extensions);
            exported.Add(extensionName, new ArraySegment<byte>(utf8Json));
        }
    }

    /// <summary>
    /// Registry for <see cref="IVrm10ExportExtension"/>. Presence of this type in
    /// <c>VRM10</c> (Runtime) is the soft signal that export hooks exist.
    /// Invocation is gated by <see cref="IsEnabled"/>
    /// (<c>Project Settings / VRM10 / Enable VRM Export Extensions</c> when the Editor
    /// bridge is loaded; otherwise defaults to enabled).
    /// </summary>
    public static class Vrm10ExportExtensionRegistry
    {
        private static readonly List<IVrm10ExportExtension> Typed = new List<IVrm10ExportExtension>();
        private static readonly List<Action<object>> Untyped = new List<Action<object>>();

        /// <summary>
        /// Optional Editor bridge: returns the Project Settings gate. Null → enabled.
        /// </summary>
        public static Func<bool> IsEnabledProvider { get; set; }

        /// <summary>Always true when this assembly ships the export extension API.</summary>
        public static bool IsAvailable => true;

        public static bool IsEnabled => IsEnabledProvider?.Invoke() ?? true;

        /// <summary>True when at least one typed or untyped handler is registered.</summary>
        public static bool HasHandlers => Typed.Count > 0 || Untyped.Count > 0;

        public static void Register(IVrm10ExportExtension extension)
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

        public static void Unregister(IVrm10ExportExtension extension)
        {
            if (extension != null)
            {
                Typed.Remove(extension);
            }
        }

        /// <summary>
        /// Soft-binding entry for packages that must not hard-reference <c>VRM10</c>.
        /// Handler receives a <see cref="Vrm10ExportExtensionContext"/> boxed as
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

        public static void InvokePreHierarchy(Vrm10ExportExtensionContext context)
        {
            InvokePhase(context, Vrm10ExportExtensionPhase.PreHierarchy);
        }

        public static void InvokePrepareTextures(Vrm10ExportExtensionContext context)
        {
            InvokePhase(context, Vrm10ExportExtensionPhase.PrepareTextures);
        }

        public static void InvokeWriteExtensions(Vrm10ExportExtensionContext context)
        {
            InvokePhase(context, Vrm10ExportExtensionPhase.WriteExtensions);
        }

        private static void InvokePhase(
            Vrm10ExportExtensionContext context,
            Vrm10ExportExtensionPhase phase)
        {
            if (context == null || !IsEnabled)
            {
                return;
            }

            context.Phase = phase;

            for (var i = 0; i < Typed.Count; i++)
            {
                try
                {
                    Typed[i].OnVrmExport(context);
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
