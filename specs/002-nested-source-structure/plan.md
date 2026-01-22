# Implementation Plan: Refactor to Nested Source Structure

**Branch**: `002-nested-source-structure` | **Date**: 2025-01-12 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-nested-source-structure/spec.md`

## Summary

Refactor the Node.Net project structure to create a nested organization by relocating all source code from `source/` to `source/Node.Net/` and all test code from `tests/` to `source/Node.Net.Test/`. This creates a clearer project organization where both the library and test projects are explicitly named subdirectories under `source/`. This is a structural refactoring that requires updating all project references, solution files, build scripts, and configuration files to maintain project buildability and test executability.

## Technical Context

**Language/Version**: C# (.NET SDK, Latest language version)  
**Primary Dependencies**: .NET SDK, NUnit 4.1.0, Microsoft.NET.Test.Sdk 17.9.0  
**Storage**: N/A (structural refactoring, no data storage changes)  
**Testing**: NUnit test framework (existing)  
**Target Platform**: Windows, Linux, macOS (multi-platform .NET library)  
**Project Type**: Single project (library with test project)  
**Performance Goals**: No performance impact (structural change only)  
**Constraints**: Must preserve git history, maintain buildability on all target frameworks (net48, net8.0, net8.0-windows), zero breaking changes to functionality  
**Scale/Scope**: ~100+ source files, ~50+ test files, solution file, project files, build scripts, CI/CD configuration

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Verify compliance with Node.Net Constitution principles:

- **I. Library-First Design**: ✅ N/A - This is a structural refactoring, not a new feature. The library design remains unchanged.
- **II. Multi-Targeting Support**: ✅ Must maintain - All target frameworks (net48, net8.0, net8.0-windows) must continue to build after refactoring
- **III. Test-First Development**: ✅ Must maintain - All existing tests must continue to pass. No new tests required for structural change.
- **IV. API Stability & Versioning**: ✅ Must maintain - No API changes. This is a PATCH-level change (structural only, no functional impact)
- **V. Cross-Platform Compatibility**: ✅ Must maintain - Build scripts must work on Windows, Linux, and macOS after path updates

**Status**: ✅ All principles satisfied - No violations

## Project Structure

### Documentation (this feature)

```text
specs/002-nested-source-structure/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── spec.md              # Feature specification
└── checklists/          # Quality checklists
    └── requirements.md
```

**Note**: This refactoring does not require data-model.md, contracts/, or quickstart.md as it is a structural change with no functional impact.

### Source Code (repository root)

**Current Structure** (to be refactored):
```text
source/                  # Source code (to move contents to source/Node.Net/)
├── Collections/
├── Converters/
├── Extension/
├── Internal/
├── JsonRPC/
├── Service/
├── System/
├── View/
├── Node.Net.csproj
└── [other source files]

tests/                   # Test code (to move contents to source/Node.Net.Test/)
├── Collections/
├── Converters/
├── Extension/
├── JsonRPC/
├── Resources/
├── Service/
├── System/
├── View/
├── Node.Net.Test.csproj
└── [other test files]
```

**Target Structure** (after refactoring):
```text
source/                  # Source directory (contains subdirectories only)
├── Node.Net/            # Source code (nested)
│   ├── Collections/
│   ├── Converters/
│   ├── Extension/
│   ├── Internal/
│   ├── JsonRPC/
│   ├── Service/
│   ├── System/
│   ├── View/
│   ├── Node.Net.csproj  # Project file
│   └── [other source files]
└── Node.Net.Test/       # Test code (nested)
    ├── Collections/
    ├── Converters/
    ├── Extension/
    ├── JsonRPC/
    ├── Resources/
    ├── Service/
    ├── System/
    ├── View/
    ├── Node.Net.Test.csproj  # Project file
    └── [other test files]

Node.Net.sln             # Solution file (updated paths)
Rakefile                  # Build script (updated paths)
```

**Structure Decision**: Nested project structure with `source/Node.Net/` and `source/Node.Net.Test/` subdirectories. This creates explicit project naming within the source directory and groups both projects under a single parent directory. Project file names remain unchanged to maintain NuGet package identity and avoid breaking external references.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

No violations - this is a straightforward structural refactoring with no complexity additions.
