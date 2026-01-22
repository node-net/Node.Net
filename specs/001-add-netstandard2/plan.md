# Implementation Plan: Add .NET Standard 2.0 Target Framework

**Branch**: `001-add-netstandard2` | **Date**: 2025-01-27 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/001-add-netstandard2/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Add `netstandard2.0` as a target framework to Node.Net.csproj to enable broader library compatibility with .NET Framework 4.6.1+, .NET Core 2.0+, and other platforms supporting .NET Standard 2.0. This infrastructure change requires conditional compilation directives to exclude platform-specific features (WPF, Windows APIs, Razor components) and conditional package references for dependencies that may not support .NET Standard 2.0.

## Technical Context

**Language/Version**: C# (Latest language version, compatible with .NET Standard 2.0)  
**Primary Dependencies**: 
- LiteDB 5.0.17 (needs compatibility verification)
- System.Drawing.Common 8.0.2 (needs compatibility verification)
- Microsoft.Extensions.Logging 8.0.1 (needs compatibility verification)
- Microsoft.Extensions.Logging.Abstractions 8.0.3 (needs compatibility verification)
- Microsoft.FluentUI.AspNetCore.Components 4.13.2 (excluded from netstandard2.0 - requires .NET 6+)
- Microsoft.Windows.SDK.Contracts 10.0.22000.194 (Windows-only, excluded from netstandard2.0)

**Storage**: N/A (library infrastructure change)  
**Testing**: NUnit (existing test framework), verify builds and package compatibility  
**Target Platform**: Cross-platform (.NET Standard 2.0 compatible platforms: .NET Framework 4.6.1+, .NET Core 2.0+, Mono, Xamarin, Unity)  
**Project Type**: Library (infrastructure change to existing library)  
**Performance Goals**: Build time increase <20% (per NFR-001)  
**Constraints**: 
- Must maintain backward compatibility with existing target frameworks (net48, net8.0, net8.0-windows)
- Must exclude Razor components and static web assets from netstandard2.0 builds
- Must conditionally exclude platform-specific features (WPF, Windows APIs)
- Package dependencies must be compatible or conditionally excluded

**Scale/Scope**: Single project file modification with conditional compilation directives

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Verify compliance with Node.Net Constitution principles:

- **I. Library-First Design**: ✅ Feature maintains library-first design - adding target framework support enhances library reusability
- **II. Multi-Targeting Support**: ✅ Plan explicitly addresses multi-targeting by adding netstandard2.0 alongside existing targets (net48, net8.0, net8.0-windows) with appropriate conditional compilation
- **III. Test-First Development**: ✅ Test strategy defined: verify builds succeed, test project can reference library, existing tests continue to pass
- **IV. API Stability & Versioning**: ✅ No public API changes - infrastructure change only, maintains backward compatibility
- **V. Cross-Platform Compatibility**: ✅ Plan identifies platform-specific code exclusion requirements (WPF, Windows APIs, Razor components) and uses conditional compilation

**Status**: [x] All principles satisfied | [ ] Violations documented below

## Project Structure

### Documentation (this feature)

```text
specs/001-add-netstandard2/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # N/A - infrastructure change, no data model
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # N/A - no API contracts for infrastructure change
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
source/Node.Net/
├── Node.Net.csproj      # PRIMARY CHANGE: Add netstandard2.0 to TargetFrameworks
│                         # Add conditional compilation constants for netstandard2.0
│                         # Add conditional package references
│                         # Exclude Razor components and static web assets
│
└── [All existing source files]
    # May require conditional compilation directives (#if !NETSTANDARD2_0)
    # for platform-specific features (WPF, Windows APIs)
```

**Structure Decision**: Minimal changes to existing structure. Primary modification is to `source/Node.Net/Node.Net.csproj` to add netstandard2.0 target framework with appropriate conditional compilation. Source files may require minor conditional compilation directives for platform-specific features, but no structural reorganization needed.

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

No violations - all constitution principles satisfied.

## Phase 0: Research & Package Compatibility ✅ COMPLETE

**Output**: [research.md](./research.md)

### Research Findings Summary

1. **Package Compatibility** ✅
   - LiteDB 5.0.17: ✅ Compatible with .NET Standard 2.0
   - System.Drawing.Common: ⚠️ Requires version 7.0.0 for .NET Standard 2.0 (8.0.2 requires .NET 6.0+)
   - Microsoft.Extensions.Logging 8.0.1: ✅ Compatible with .NET Standard 2.0
   - Microsoft.Extensions.Logging.Abstractions 8.0.3: ✅ Compatible with .NET Standard 2.0

2. **Conditional Compilation** ✅
   - Use built-in `NETSTANDARD2_0` symbol (MSBuild automatically defines)
   - Pattern: `#if NETSTANDARD2_0` / `#if !NETSTANDARD2_0`

3. **Build System Configuration** ✅
   - MSBuild fully supports netstandard2.0 in multi-targeting
   - Razor components must be excluded (same pattern as net48)
   - Static web assets must be excluded (Blazor-specific)

## Phase 1: Design & Implementation Strategy

### Implementation Approach

1. **Update Node.Net.csproj**
   - Add `netstandard2.0` to both Windows and non-Windows TargetFrameworks conditions
   - Define conditional compilation constant for netstandard2.0 (e.g., `NETSTANDARD2_0`)
   - Add conditional package references for netstandard2.0 (compatible versions)
   - Exclude Razor components from netstandard2.0 builds (same pattern as net48)
   - Exclude static web assets from netstandard2.0 builds
   - Exclude Windows-specific packages (Microsoft.Windows.SDK.Contracts, FluentUI) from netstandard2.0

2. **Source Code Conditional Compilation**
   - Review source files for platform-specific code (WPF, Windows APIs)
   - Add `#if !NETSTANDARD2_0` directives where needed
   - Ensure core library functionality works without platform-specific features

3. **Testing Strategy**
   - Verify build succeeds for netstandard2.0
   - Create test project targeting .NET Standard 2.0 to verify package reference works
   - Verify existing tests continue to pass for other target frameworks
   - Verify NuGet package metadata includes netstandard2.0

### Conditional Compilation Constants

Current constants:
- `IS_WINDOWS` - Windows-specific targets
- `IS_FRAMEWORK` - .NET Framework targets (net48)

New constant needed:
- `NETSTANDARD2_0` or use built-in `NETSTANDARD2_0` symbol (if available)

### Package Reference Strategy

Based on research findings (see [research.md](./research.md)):
- **LiteDB 5.0.17**: Include for all targets (compatible)
- **System.Drawing.Common**: Use conditional reference - 7.0.0 for netstandard2.0, 8.0.2 for net8.0+
- **Microsoft.Extensions.Logging 8.0.1**: Include for all targets (compatible)
- **Microsoft.Extensions.Logging.Abstractions 8.0.3**: Include for all targets (compatible)
- **Microsoft.FluentUI.AspNetCore.Components**: Exclude from netstandard2.0 (requires .NET 6+)
- **Microsoft.Windows.SDK.Contracts**: Exclude from netstandard2.0 (Windows-only)

## Phase 2: Validation & Testing

### Build Validation
- [ ] Project builds successfully for netstandard2.0 on Windows
- [ ] Project builds successfully for netstandard2.0 on non-Windows platforms
- [ ] All existing target frameworks (net48, net8.0, net8.0-windows) continue to build
- [ ] No compilation errors or warnings introduced

### Package Validation
- [ ] NuGet package includes netstandard2.0 in supported frameworks
- [ ] Test project targeting .NET Standard 2.0 can reference and use the library
- [ ] Core library functionality works in .NET Standard 2.0 context

### Test Validation
- [ ] All existing tests pass for net48, net8.0, net8.0-windows
- [ ] Build time increase is <20% (per NFR-001)

## Risk Assessment

### Low Risk
- Adding target framework is well-understood MSBuild operation
- Conditional compilation patterns already established in codebase

### Medium Risk
- Package compatibility may require version adjustments
- Some features may need conditional exclusion if APIs unavailable in .NET Standard 2.0

### Mitigation
- Research phase will identify compatibility issues early
- Conditional compilation allows graceful feature exclusion
- Existing target frameworks remain unchanged, reducing regression risk
