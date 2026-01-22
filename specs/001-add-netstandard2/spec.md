# Feature Specification: Add .NET Standard 2.0 Target Framework

**Feature Branch**: `001-add-netstandard2`  
**Created**: 2025-01-27  
**Status**: Draft  
**Input**: User description: "add the targetframework netstandard2.0 to Node.Net.csproj"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Library Compatibility with .NET Standard 2.0 Projects (Priority: P1)

Developers working on projects that target .NET Standard 2.0 need to consume the Node.Net library without upgrading their entire project to .NET 8.0. By adding netstandard2.0 as a target framework, the library becomes compatible with a broader range of .NET implementations including .NET Framework 4.6.1+, .NET Core 2.0+, and other platforms that support .NET Standard 2.0.

**Why this priority**: This is the core value proposition - enabling broader library adoption and compatibility with existing projects that cannot immediately migrate to .NET 8.0.

**Independent Test**: Can be fully tested by building the Node.Net library with netstandard2.0 target framework and verifying that a test project targeting .NET Standard 2.0 can successfully reference and use the library without compilation errors.

**Acceptance Scenarios**:

1. **Given** the Node.Net.csproj file exists with current target frameworks, **When** netstandard2.0 is added to the TargetFrameworks property, **Then** the project builds successfully for netstandard2.0
2. **Given** a consumer project targeting .NET Standard 2.0, **When** it references the Node.Net NuGet package, **Then** it can successfully compile and use core library functionality
3. **Given** the library builds for netstandard2.0, **When** features incompatible with .NET Standard 2.0 are encountered, **Then** they are conditionally excluded using appropriate conditional compilation directives

---

### Edge Cases

- What happens when a feature requires APIs not available in .NET Standard 2.0? (Solution: Use conditional compilation to exclude platform-specific features)
- How does the build system handle multiple target frameworks including netstandard2.0? (Solution: Ensure proper conditional compilation and package references)
- What if some NuGet package dependencies don't support .NET Standard 2.0? (Solution: Use conditional package references with compatible versions for netstandard2.0, exclude incompatible packages, and conditionally compile dependent code to exclude features requiring those packages)

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Node.Net.csproj MUST include netstandard2.0 in the TargetFrameworks property for both Windows and non-Windows builds
- **FR-002**: The library MUST build successfully for netstandard2.0 target framework without errors
- **FR-003**: Platform-specific features MUST be conditionally excluded from netstandard2.0 builds. This includes:
  - WPF (Windows Presentation Foundation) types and APIs
  - Windows-specific APIs (e.g., WinRT, Windows Runtime)
  - Windows-specific packages (Microsoft.Windows.SDK.Contracts)
  - Any code that depends on Windows-only functionality
- **FR-004**: Package references MUST be compatible with .NET Standard 2.0 or conditionally included based on target framework. If a package doesn't support .NET Standard 2.0, use conditional package references with compatible versions for netstandard2.0, and conditionally compile dependent code to exclude features that require incompatible packages
- **FR-005**: The library MUST maintain backward compatibility with existing functionality for other target frameworks (net48, net8.0, net8.0-windows)
- **FR-006**: Razor components MUST be excluded from netstandard2.0 builds (as they require .NET 6+)
- **FR-007**: Static web assets (JavaScript modules) MUST be excluded from netstandard2.0 builds

### Non-Functional Requirements

- **NFR-001**: Build time for netstandard2.0 target MUST not increase overall build time by more than 20% (target: <20% increase)
- **NFR-002**: The NuGet package MUST correctly identify netstandard2.0 as a supported target framework
- **NFR-003**: All existing tests MUST continue to pass for other target frameworks after adding netstandard2.0

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Library builds successfully for netstandard2.0 target framework with zero compilation errors
- **SC-002**: A test project targeting .NET Standard 2.0 can reference and use the Node.Net library without errors
- **SC-003**: NuGet package metadata correctly lists netstandard2.0 as a supported target framework
- **SC-004**: All existing target frameworks (net48, net8.0, net8.0-windows) continue to build and function correctly
- **SC-005**: Platform-specific features are properly excluded from netstandard2.0 builds using conditional compilation

## Assumptions

- .NET Standard 2.0 provides sufficient API surface for core Node.Net functionality
- Package dependencies (LiteDB, System.Drawing.Common, etc.) have compatible versions for .NET Standard 2.0
- Conditional compilation directives can effectively exclude platform-specific code
- Razor components and Blazor-specific features will be excluded from netstandard2.0 builds (as they require .NET 6+)

## Dependencies

- Existing package references must be evaluated for .NET Standard 2.0 compatibility
- May require version adjustments for some NuGet packages to support .NET Standard 2.0
- Build system must support multi-targeting with .NET Standard 2.0

## Clarifications

### Session 2025-01-27

- Q: Should netstandard2.0 be included in both Windows and non-Windows builds, or only on specific platforms? → A: Include netstandard2.0 in both Windows and non-Windows builds (add to both conditional TargetFrameworks)
- Q: If a package dependency doesn't support .NET Standard 2.0, what should be the resolution strategy? → A: Use conditional package references - include compatible versions for netstandard2.0, exclude incompatible packages and conditionally compile dependent code

## Out of Scope

- Migrating existing code to use .NET Standard 2.0 APIs exclusively (maintaining multi-targeting approach)
- Adding .NET Standard 2.1 or other .NET Standard versions (only 2.0 is requested)
- Removing support for existing target frameworks
