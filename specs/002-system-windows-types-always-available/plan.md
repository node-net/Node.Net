# Implementation Plan: System.Windows Types Always Available

**Branch**: `002-system-windows-types-always-available` | **Date**: 2025-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-system-windows-types-always-available/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Enable transparent type access in tests by removing `extern alias NodeNet` requirements and allowing tests to use standard `System.Windows.*` namespace references directly. The solution uses global usings in the test project to map `System.Windows.*` namespaces, removing the need for verbose `NodeNet::` prefixes. Conditional compilation ensures only one set of types exists per target framework (platform types on Windows, custom implementations on non-Windows), eliminating namespace conflicts.

## Technical Context

**Language/Version**: C# (Latest language version, .NET 8.0 compatible)  
**Primary Dependencies**: 
- NUnit 4.1.0 (test framework)
- Microsoft.NET.Test.Sdk 17.9.0
- Node.Net library (source/Node.Net/Node.Net.csproj)

**Storage**: N/A (test infrastructure change)  
**Testing**: NUnit (existing test framework), verify all tests pass after refactoring  
**Target Platform**: Cross-platform (net8.0, net8.0-windows, net48, netstandard2.0)  
**Project Type**: Library test infrastructure (improving test developer experience)  
**Performance Goals**: No runtime performance impact (compile-time only changes)  
**Constraints**: 
- Must not break existing test functionality
- Must work across all target frameworks
- Must maintain API compatibility with platform types
- Must not require changes to external consumers

**Scale/Scope**: 
- Test project configuration changes (Node.Net.Test.csproj)
- Refactoring ~75 test files to remove `extern alias` and `NodeNet::` prefixes
- Adding global usings for `System.Windows.*` namespaces

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Verify compliance with Node.Net Constitution principles:

- **I. Library-First Design**: ✅ Feature improves library testability and developer experience without changing library API
- **II. Multi-Targeting Support**: ✅ Solution works across all target frameworks (net48, net8.0, net8.0-windows, netstandard2.0) using conditional compilation
- **III. Test-First Development**: ✅ Test strategy defined: refactor test files, verify all tests pass, ensure extension methods work correctly
- **IV. API Stability & Versioning**: ✅ No public API changes - test infrastructure improvement only, maintains backward compatibility for external consumers
- **V. Cross-Platform Compatibility**: ✅ Solution handles platform differences via conditional compilation, works identically across platforms

**Status**: [x] All principles satisfied | [ ] Violations documented below

## Project Structure

### Documentation (this feature)

```text
specs/002-system-windows-types-always-available/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # N/A - test infrastructure change, no data model
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # N/A - no API contracts for test infrastructure change
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
tests/Node.Net.Test/
├── Node.Net.Test.csproj  # PRIMARY CHANGE: Remove <Aliases>NodeNet</Aliases> from project reference
│                          # Add global usings for System.Windows.* namespaces
│
├── GlobalUsings.cs       # NEW FILE: Global using statements for System.Windows.* namespaces
│                          # Maps System.Windows, System.Windows.Media, System.Windows.Media.Media3D, etc.
│
└── [All existing test files]
    # Refactor to remove:
    # - `extern alias NodeNet;` declarations
    # - `NodeNet::` prefixes from type references
    # - `using NodeNet::System.Windows.*;` statements
    # Replace with standard namespace references:
    # - `using System.Windows.Media.Media3D;`
    # - Direct type usage: `Vector3D` instead of `NodeNet::System.Windows.Media.Media3D.Vector3D`
```

**Structure Decision**: Minimal changes to existing structure. Primary modifications are:
1. Test project file (`Node.Net.Test.csproj`) - remove extern alias, add global usings
2. New `GlobalUsings.cs` file - centralize global using statements
3. Refactor existing test files - remove extern alias declarations and NodeNet:: prefixes

## Implementation Approach

### Phase 1: Configuration Changes

1. **Remove extern alias from project reference**
   - Edit `tests/Node.Net.Test/Node.Net.Test.csproj`
   - Remove `<Aliases>NodeNet</Aliases>` from the project reference to `source/Node.Net/Node.Net.csproj`

2. **Create GlobalUsings.cs file**
   - Create `tests/Node.Net.Test/GlobalUsings.cs`
   - Add global using statements for all `System.Windows.*` namespaces used in tests:
     - `global using System.Windows;`
     - `global using System.Windows.Media;`
     - `global using System.Windows.Media.Imaging;`
     - `global using System.Windows.Media.Media3D;`

### Phase 2: Test File Refactoring

1. **Identify all test files using System.Windows types**
   - Search for files containing `extern alias NodeNet`
   - Search for files containing `NodeNet::System.Windows`
   - Estimated: ~75 test files

2. **Refactor each test file**
   - Remove `extern alias NodeNet;` declaration
   - Replace `NodeNet::System.Windows.*` with standard namespace references
   - Replace `using NodeNet::System.Windows.*;` with `using System.Windows.*;`
   - Remove `NodeNet::` prefixes from type references
   - Update type aliases (e.g., `using NodeNetVector3D = NodeNet::System.Windows.Media.Media3D.Vector3D;` → `using Vector3D = System.Windows.Media.Media3D.Vector3D;`)

3. **Verify extension methods work**
   - Ensure extension methods in `Node.Net` namespace continue to work
   - Test with both platform types (Windows targets) and custom types (non-Windows targets)

### Phase 3: Validation

1. **Build verification**
   - Build test project for all target frameworks: `net8.0`, `net8.0-windows`
   - Verify no compilation errors related to ambiguous type references
   - Verify no compilation errors related to missing types

2. **Test execution**
   - Run all tests on `net8.0` target framework
   - Run all tests on `net8.0-windows` target framework (if on Windows)
   - Verify all tests pass with identical results

3. **Extension method verification**
   - Verify extension methods work correctly with platform types (Windows)
   - Verify extension methods work correctly with custom types (non-Windows)

## Technical Details

### Global Usings Implementation

The `GlobalUsings.cs` file will contain:

```csharp
global using System.Windows;
global using System.Windows.Media;
global using System.Windows.Media.Imaging;
global using System.Windows.Media.Media3D;
```

This allows all test files to use `System.Windows.*` types directly without explicit using statements.

### Type Resolution Behavior

- **On Windows targets (`net8.0-windows`, `net48`)**:
  - `IS_WINDOWS` is defined
  - Custom implementations in `source/Node.Net/System` are excluded via `#if !IS_WINDOWS`
  - Platform types from WPF/PresentationCore are used automatically
  - No namespace conflicts because custom types don't exist

- **On non-Windows targets (`net8.0`, `netstandard2.0`)**:
  - `IS_WINDOWS` is not defined
  - Custom implementations in `source/Node.Net/System` are compiled
  - Custom types in `System.Windows.*` namespaces are used
  - No namespace conflicts because platform types don't exist

### Extension Methods Compatibility

Extension methods in the `Node.Net` namespace target the public API of `System.Windows.*` types. Since custom implementations match platform API contracts exactly (per FR-004), the same extension methods work with both:

- Platform types (Windows targets): Extension methods bind to platform type's public API
- Custom types (non-Windows targets): Extension methods bind to custom type's public API (identical interface)

No changes to extension methods are needed.

## Dependencies

- **Node.Net library project** (`source/Node.Net/Node.Net.csproj`): No changes required
- **Test project** (`tests/Node.Net.Test/Node.Net.Test.csproj`): Configuration changes required
- **All test files** using `System.Windows.*` types: Refactoring required

## Risks & Mitigation

| Risk | Impact | Mitigation |
|------|--------|------------|
| Namespace conflicts on Windows targets | High | Conditional compilation ensures only one set of types exists per target - verified in CL-002 |
| Extension methods not working | Medium | API contracts are identical - verified in CL-003 |
| Test failures after refactoring | High | Comprehensive test execution on all target frameworks before and after refactoring |
| Ambiguous type references | High | Global usings provide clear namespace resolution - verified in CL-001 |

## Success Metrics

- All test files can use `System.Windows.*` types without `extern alias` or `NodeNet::` prefixes
- All tests pass on all target frameworks (`net8.0`, `net8.0-windows`)
- No compilation errors related to type resolution
- Extension methods work correctly with both platform and custom types
- Test code readability improved (measured by elimination of `NodeNet::` prefixes)

## Next Steps

1. Create `research.md` to document global usings syntax and best practices
2. Create `quickstart.md` with validation scenarios
3. Generate `tasks.md` with detailed implementation tasks
