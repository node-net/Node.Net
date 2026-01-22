# Feature Specification: System.Windows Types Always Available

**Feature Branch**: `002-system-windows-types-always-available`  
**Created**: 2025-01-27  
**Status**: Draft  
**Input**: User description: "make sure the code defined in source/Node.Net/System is always defined when not supported by the target framework, for example on net8.0-windows, these are defined by the platform, when targeting net8.0, they are not defined by the system and need to be defined, The tests should be able to always access these classes, no matter the target framework, for example the Vector3D.Test.cs should always be able to use System.Windows.Media.Media3D.Vector3D without any special notations"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Transparent Type Access in Tests (Priority: P1)

Developers writing tests for System.Windows types should be able to use standard namespace references (e.g., `System.Windows.Media.Media3D.Vector3D`) without requiring `extern alias NodeNet` or `NodeNet::` prefixes. The implementation should be transparent - tests should work identically whether the types come from the platform (on Windows targets) or from Node.Net's custom implementations (on non-Windows targets).

**Why this priority**: This improves developer experience by eliminating the need for special aliases and prefixes in test code, making tests more readable and maintainable. It also ensures consistent API availability across all target frameworks.

**Independent Test**: Can be fully tested by verifying that test files can use `System.Windows.Media.Media3D.Vector3D` directly without `extern alias` or `NodeNet::` prefixes, and that tests pass on both Windows and non-Windows target frameworks.

**Acceptance Scenarios**:

1. **Given** a test file like `Vector3D.Test.cs`, **When** it uses `System.Windows.Media.Media3D.Vector3D` directly (without `extern alias` or `NodeNet::`), **Then** the test compiles and runs successfully on all target frameworks
2. **Given** the library is built for `net8.0-windows`, **When** tests reference `System.Windows.Media.Media3D.Vector3D`, **Then** the platform-provided type is used
3. **Given** the library is built for `net8.0` (non-Windows), **When** tests reference `System.Windows.Media.Media3D.Vector3D`, **Then** Node.Net's custom implementation is used
4. **Given** tests are written using standard namespace references, **When** they run on different target frameworks, **Then** they produce identical results regardless of whether platform or custom types are used

---

### Edge Cases

- What happens when both platform types and custom implementations are available? (Solution: Use conditional compilation to exclude custom implementations when platform types are available)
- How do we handle type identity and equality across platform vs custom implementations? (Solution: Ensure custom implementations match platform API contracts exactly)
- What if a test project references both the platform types and Node.Net library? (Solution: Use proper namespace resolution and type forwarding if needed)
- How do we ensure extension methods work with both platform and custom types? (Solution: Extension methods should target the common interface/contract)

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: All types in `source/Node.Net/System` MUST be conditionally compiled to be available when the platform does not provide them. Specifically:
  - On Windows targets (`net8.0-windows`, `net48`), platform types are used, custom implementations are excluded
  - On non-Windows targets (`net8.0`, `netstandard2.0`), custom implementations are included
- **FR-002**: Tests MUST be able to reference `System.Windows.*` types using standard namespace syntax (e.g., `using System.Windows.Media.Media3D;`) without requiring `extern alias NodeNet` or `NodeNet::` prefixes
- **FR-003**: The test project MUST be configured to allow direct access to `System.Windows.*` types from Node.Net library without namespace conflicts
- **FR-004**: Custom implementations in `source/Node.Net/System` MUST match the platform API contracts exactly, including:
  - All public properties, methods, and constructors
  - Type names and namespaces
  - Behavior and edge cases (e.g., NaN handling, normalization of zero vectors)
- **FR-005**: Extension methods in Node.Net namespace MUST work with both platform-provided and custom-implemented types
- **FR-006**: All existing tests MUST continue to pass after refactoring to remove `extern alias` and `NodeNet::` prefixes
- **FR-007**: The solution MUST work for all target frameworks: `net48`, `net8.0`, `net8.0-windows`, `netstandard2.0`

### Non-Functional Requirements

- **NFR-001**: Test code readability MUST improve by eliminating special aliases and prefixes
- **NFR-002**: The solution MUST not introduce runtime performance overhead
- **NFR-003**: Build time MUST not increase significantly (target: <5% increase)
- **NFR-004**: The solution MUST maintain backward compatibility with existing code that uses `extern alias NodeNet` (applies to external consumers, not the test project - see CL-005)

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `Vector3D.Test.cs` can use `System.Windows.Media.Media3D.Vector3D` directly without `extern alias` or `NodeNet::` prefix
- **SC-002**: All tests in `tests/Node.Net.Test/System/Windows/**` can use standard namespace references
- **SC-003**: Tests pass on all target frameworks (`net8.0`, `net8.0-windows`)
- **SC-004**: No compilation errors related to ambiguous type references
- **SC-005**: Extension methods continue to work correctly with both platform and custom types

## Technical Context

### Current State

- Custom implementations in `source/Node.Net/System` use `#if !IS_WINDOWS` to conditionally compile
- Test project uses `extern alias NodeNet` and `<Aliases>NodeNet</Aliases>` in project reference
- Tests use `NodeNet::System.Windows.Media.Media3D.Vector3D` syntax which is verbose
- On Windows targets, `IS_WINDOWS` is defined, so custom implementations are excluded
- On non-Windows targets, `IS_WINDOWS` is not defined, so custom implementations are included

### Target State

- Tests use standard `System.Windows.Media.Media3D.Vector3D` syntax
- No `extern alias` required in test files
- Type resolution automatically uses platform types when available, custom types otherwise
- Implementation is transparent to test code

## Dependencies

- Node.Net library project (`source/Node.Net/Node.Net.csproj`)
- Test project (`tests/Node.Net.Test/Node.Net.Test.csproj`)
- All types in `source/Node.Net/System/Windows/**`

## Constraints

- Must not break existing functionality
- Must maintain API compatibility with platform types
- Must work across all target frameworks
- Must not require changes to consuming code outside the test project (if any)

## Clarifications

### CL-001: Type Resolution Mechanism
**Question**: How should we achieve transparent type access in tests?
**Answer**: Remove `extern alias` from project reference and add global usings in test project to map `System.Windows.*` namespaces. This allows standard namespace resolution while ensuring types are available across all target frameworks.

### CL-002: Handling Namespace Conflicts on Windows Targets
**Question**: How do we handle cases where both platform and custom types might be in scope?
**Answer**: Rely on conditional compilation - on Windows targets, custom implementations are excluded via `#if !IS_WINDOWS`, so platform types are used automatically. On non-Windows targets, only custom implementations are compiled. No explicit conflict resolution needed because only one set of types exists per target framework.

### CL-003: Extension Methods Compatibility
**Question**: How do we ensure extension methods work with both platform-provided and custom-implemented types?
**Answer**: Extension methods work automatically - they target the common API contract which is identical for both platform and custom types. Since custom implementations match platform API contracts exactly (per FR-004), the same extension methods will work with both types without modification.

### CL-004: Scope of Test File Refactoring
**Question**: Should we refactor all test files at once or incrementally?
**Answer**: Refactor all test files that use `System.Windows.*` types to remove `extern alias` and `NodeNet::` prefixes in one pass. This ensures consistency across the test suite and fully achieves the goal of transparent type access.

### CL-005: Backward Compatibility Scope
**Question**: How does backward compatibility requirement (NFR-004) apply given we're removing `extern alias`?
**Answer**: Backward compatibility applies only to external consumers outside the test project. Test project changes are acceptable and expected. The `extern alias` removal is internal to the test project and doesn't impact the library's public API or external consumers.

### Open Questions

All open questions have been clarified. The specification is ready for planning.
