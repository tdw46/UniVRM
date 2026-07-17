# Cursor configuration

These rules define coding and agent guidance for Extended-UniVRM. UniVRM's existing code,
package layout, public API compatibility, and serialization requirements take precedence
over generic style guidance.

## Project assumptions

- Unity version: `2022.3.62f2`
- Authored packages: `Packages/UniGLTF`, `Packages/VRM`, and `Packages/VRM10`
- Main namespace families: `UniGLTF`, `VRM`, `UniVRM10`, `MToon`, and `VrmLib`
- Tests are package-local NUnit EditMode assemblies.
- C# and documentation use LF line endings.
- Recursive git submodules are part of the repository.

## Deliberately not copied

- Unrelated game namespaces, assemblies, issue links, and folder assumptions
- Game-specific UI architecture, story, backlog, scene, and sandbox policies
- CSharpier/Prettier and editor-agent workflows not installed in this repository
- Repository-wide naming normalization that could break serialized data or public APIs
