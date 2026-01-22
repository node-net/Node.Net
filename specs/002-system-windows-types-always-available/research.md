# Research: System.Windows Types Always Available

**Date**: 2025-01-27  
**Feature**: Enable transparent type access in tests for System.Windows types

## Research Questions

### 1. Global Usings Syntax and Best Practices

**Research Task**: Understand C# global usings syntax and how to use them with namespace resolution

**Decision**: **Use `global using` statements in a GlobalUsings.cs file**
- Rationale: C# 10+ supports `global using` statements that apply to all files in a project. Placing them in a `GlobalUsings.cs` file is the standard practice for organizing global usings.
- Syntax: `global using System.Windows.Media.Media3D;`
- Alternatives considered:
  - Per-file using statements: Too verbose, requires changes in every file
  - Implicit usings in .csproj: Less explicit, harder to see what's included
- Action: Create `tests/Node.Net.Test/GlobalUsings.cs` with global using statements for all `System.Windows.*` namespaces used in tests

**References**:
- [C# 10 Global Using Directives](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-directive#global-modifier)
- [Implicit Usings](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#implicitusings)

### 2. Extern Alias Removal Impact

**Research Task**: Understand the impact of removing `extern alias` from project references

**Decision**: **Removing `<Aliases>NodeNet</Aliases>` allows standard namespace resolution**
- Rationale: `extern alias` is used to disambiguate types when the same namespace exists in multiple assemblies. By removing it and relying on conditional compilation (only one set of types exists per target framework), we can use standard namespace resolution.
- Impact: 
  - Test files can no longer use `NodeNet::` prefix
  - Type resolution uses standard namespace lookup
  - On Windows: Platform types are found automatically
  - On non-Windows: Custom types are found automatically
- Alternatives considered:
  - Keep extern alias but make it optional: Adds complexity, doesn't achieve goal
  - Use type forwarding: More complex, requires assembly-level changes
- Action: Remove `<Aliases>NodeNet</Aliases>` from project reference in `Node.Net.Test.csproj`

**References**:
- [Extern Alias (C# Reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/extern-alias)
- [MSBuild Aliases Property](https://learn.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.projectreference.aliases)

### 3. Namespace Resolution Behavior

**Research Task**: Understand how C# resolves types when multiple assemblies define the same namespace

**Decision**: **Conditional compilation ensures only one set of types exists per target framework**
- Rationale: 
  - On Windows targets: `IS_WINDOWS` is defined, custom implementations are excluded via `#if !IS_WINDOWS`
  - On non-Windows targets: `IS_WINDOWS` is not defined, custom implementations are compiled
  - Result: Only one set of `System.Windows.*` types exists per target framework, so no ambiguity
- Behavior:
  - Compiler resolves types from available assemblies
  - If only one assembly defines the namespace, that assembly's types are used
  - No explicit disambiguation needed
- Alternatives considered:
  - Explicit type resolution logic: Unnecessary complexity
  - Conditional global usings: Not needed, standard resolution works
- Action: Rely on standard namespace resolution with conditional compilation

**References**:
- [C# Type Resolution](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/basic-concepts#type-resolution)

### 4. Extension Methods Compatibility

**Research Task**: Verify extension methods work with both platform and custom types

**Decision**: **Extension methods work automatically via common API contract**
- Rationale: Extension methods are resolved based on the type's public interface. Since custom implementations match platform API contracts exactly (per FR-004), the same extension methods work with both types.
- Mechanism:
  - Extension methods are defined in `Node.Net` namespace
  - They target the public API of `System.Windows.*` types
  - Platform types and custom types have identical public APIs
  - Compiler binds extension methods to the public interface, not the implementation
- Testing: Verify extension methods work on both Windows (platform types) and non-Windows (custom types) targets
- Alternatives considered:
  - Conditional extension methods: Unnecessary, API contracts are identical
  - Separate extension method overloads: Unnecessary complexity
- Action: No changes needed to extension methods

**References**:
- [Extension Methods (C# Programming Guide)](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods)

### 5. Test File Refactoring Strategy

**Research Task**: Determine the best approach for refactoring ~75 test files

**Decision**: **Refactor all files in one pass using search-and-replace patterns**
- Rationale: 
  - Consistency: All files use the same pattern after refactoring
  - Completeness: Ensures no files are missed
  - Efficiency: Single pass is faster than incremental approach
- Patterns to replace:
  - `extern alias NodeNet;` → Remove entirely
  - `NodeNet::System.Windows.*` → `System.Windows.*`
  - `using NodeNet::System.Windows.*;` → `using System.Windows.*;`
  - Type aliases: `using NodeNetVector3D = NodeNet::System.Windows.Media.Media3D.Vector3D;` → `using Vector3D = System.Windows.Media.Media3D.Vector3D;`
- Validation: Run all tests after refactoring to ensure correctness
- Alternatives considered:
  - Incremental refactoring: Risk of mixed patterns, more complex validation
  - Automated tooling: May miss edge cases, manual review still needed
- Action: Refactor all files in one pass, then validate with comprehensive test execution

## Summary

All research questions have been answered. The solution uses:
1. Global usings in `GlobalUsings.cs` for namespace mapping
2. Removal of `extern alias` from project reference
3. Standard namespace resolution (enabled by conditional compilation)
4. Extension methods work automatically (no changes needed)
5. Comprehensive refactoring in one pass

The approach is straightforward, leverages C# language features (global usings), and relies on existing conditional compilation infrastructure.
