# Extended UniVRM

Fork of [vrm-c/UniVRM](https://github.com/vrm-c/UniVRM) with generic VRM 1.0
import/export extension hooks (for [UniVRMXT](https://github.com/miramocha/UniVRMXT)
and other optional packages). Same UPM ids as upstream: `com.vrmc.gltf`,
`com.vrmc.vrm`, `com.vrmc.univrm`.

Install **this** repo’s git URLs when you need those hooks. Do not use
[vrm-c/UniVRM](https://github.com/vrm-c/UniVRM) Releases for that.

[![GitHub license](https://img.shields.io/github/license/miramocha/Extended-UniVRM)](LICENSE.txt)

Upstream UniVRM is the standard implementation of [VRM](https://vrm-consortium.org/en/)
for [Unity](https://unity.com/). VRM is an extension of
[glTF 2.0](https://www.khronos.org/gltf/).

## Features

UniVRM supports the [VRM 1.0 specification](https://github.com/vrm-c/vrm-specification) and the [glTF 2.0 specification](https://registry.khronos.org/glTF/).

UniVRM can import/export following supported file types at both runtime and editor.

### Supported file types

- VRM 1.0 (.vrm)
- VRM 0.x (.vrm)
- glTF 2.0 (.glb | .gltf | .zip)
- VRM-Animation (.vrma)

### Import features

- You can import supported file types at both runtime and editor.
- Support for async/await importing at runtime.
- Support for Migration VRM 0.x files into VRM 1.0 files.
- Support for ScriptedImporter for VRM 1.0 and glTF 2.0.
- You can import glTF's PBR materials into Unity Built-in RP's Standard materials.

### Export features

- You can export supported file types at both runtime and editor.
- You can export Unity Built-in RP's Standard materials into glTF's PBR materials.

## Supported Environments

The latest UniVRM supports Unity 2022.3 LTS or later from `v0.128.0`.

UniVRM supports scripting backends both .NET and IL2CPP.

UniVRM supports the following building target platforms:

- Standalone (Windows/Mac/Linux)
- iOS
- Android
- WebGL

The other platforms maybe work but they are not tested.

## Installation

Unity **2022.3 LTS**. Package versions match upstream UniVRM **0.131.2**.

**Window → Package Manager → + → Add package from git URL…**, in this order:

| UPM id | Git URL |
|--------|---------|
| `com.vrmc.gltf` | `https://github.com/miramocha/Extended-UniVRM.git?path=/Packages/UniGLTF` |
| `com.vrmc.vrm` (VRM 1.0) | `https://github.com/miramocha/Extended-UniVRM.git?path=/Packages/VRM10` |
| `com.vrmc.univrm` (VRM 0.x, optional) | `https://github.com/miramocha/Extended-UniVRM.git?path=/Packages/VRM` |

Or `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.vrmc.gltf": "https://github.com/miramocha/Extended-UniVRM.git?path=/Packages/UniGLTF",
    "com.vrmc.vrm": "https://github.com/miramocha/Extended-UniVRM.git?path=/Packages/VRM10"
  }
}
```

Then add [UniVRMXT](https://github.com/miramocha/UniVRMXT) (`com.vrmxt.univrmxt`). Install
notes: [UniVRMXT installation](https://github.com/miramocha/UniVRMXT/blob/main/docs/installation.md).

This repo is also a Unity project checkout. For VRMXT import onto the `.vrm` asset and
`VRMXT_*` export, enable **Project Settings → VRM10 → Enable VRM Import Extensions** and
**Enable VRM Export Extensions**.

Stock UniVRM without hooks: [vrm-c/UniVRM](https://github.com/vrm-c/UniVRM) (same `?path=/Packages/…` layout from `v0.131.0`). `.unitypackage` install is upstream-only; this fork is git UPM.

## Documentation

- https://vrm.dev/en/vrm/index.html

### For developers

- https://vrm.dev/en/api/index.html

## License

- [MIT License](./LICENSE.txt)
