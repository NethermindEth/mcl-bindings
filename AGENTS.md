# AGENTS instructions

C# bindings for MCL. See [global.json](./global.json) and [src](./src/) directory for the project requirements and configuration.

## Project structure

- [src](./src/): The main codebase. The P/Invoke declarations mirror MCL's [bn.h](https://github.com/herumi/mcl/blob/master/include/mcl/bn.h) C API.
- [build-mcl.yml](./.github/workflows/build-mcl.yml): Builds MCL for the specified version and opens a pull request with the resulting binaries.
- [test-publish.yml](./.github/workflows/test-publish.yml): Runs the tests and optionally publishes on NuGet.

## Coding guidelines

- Follow [.editorconfig](./.editorconfig).
- Do not assume; measure, research, ask if unsure.
- Keep comments short and to the point.
- Add tests for new code and bug fixes.
- Use conventional commits; keep scoped and imperative.
- Keep the native binaries under `src/Nethermind.MclBindings/runtimes/` in sync with a single MCL version; they are Git LFS objects updated only by [build-mcl.yml](./.github/workflows/build-mcl.yml), so do not edit or rebuild them locally.
- Version the package as `<MCL version>.<run number>`: `VersionPrefix` in [Directory.Build.props](./src/Directory.Build.props) matches the shipped MCL version (e.g., `4.20.0`) and is updated by [build-mcl.yml](./.github/workflows/build-mcl.yml); [test-publish.yml](./.github/workflows/test-publish.yml) appends the run number.
- Keep the P/Invoke signatures and struct layouts in sync with the MCL headers of the shipped binaries.
- Map header pointers strictly: `void*` to `void*`, `char*` and `uint8_t*` to `byte*`, `uint64_t*` to `ulong*`, arrays of MCL types to typed pointers, and single MCL values to `ref` (`in` when `const`). Mark members with pointer parameters `unsafe` and document caller obligations in a `/// <safety>` block, following the [C# memory safety model](https://devblogs.microsoft.com/dotnet/improving-csharp-memory-safety/).
- Prefer the latest versions of GitHub Actions and runners.
- Update [THIRD-PARTY-NOTICES](./THIRD-PARTY-NOTICES) when introducing a dependency if needed.
- Keep [AGENTS.md](./AGENTS.md) in sync with the ongoing development.
