# Remediation: System.Windows Types Always Available

**Date**: 2025-01-27  
**Analysis Issues**: A3, A8, A9

## Issue A3: Glob Patterns in Tasks T060-T062

**Problem**: Tasks use glob patterns (`*.Test.cs`) which may not match actual file structure.

**Remediation**: Replace glob patterns with explicit file lists based on actual directory structure.

**Files Found**:
- Collections: Dictionary.Test.cs, Element.Test.cs, Items.Test.cs, Spatial.Test.cs (4 files)
- Converters: HiddenWhenNull.Test.cs (1 file)
- JsonRPC: Request.Test.cs, Responder.Test.cs, Server.Test.cs (3 files)

**Note**: These files use `extern alias NodeNet` but for `NodeNet::Node.Net.*` types, not `NodeNet::System.Windows.*`. They should only be refactored if they use System.Windows types. Otherwise, they can keep extern alias for Node.Net namespace types.

---

## Issue A8: NFR-002 Runtime Overhead Validation

**Problem**: "No runtime performance overhead" requirement has no explicit validation task.

**Remediation**: Add explicit note to validation phase that compile-time-only changes are confirmed by the build process. Since all changes are to test project configuration and test file syntax (not runtime code), overhead is inherently zero.

---

## Issue A9: netstandard2.0 Target Framework

**Problem**: Spec mentions netstandard2.0 in FR-007, but validation tasks only cover net8.0 and net8.0-windows.

**Remediation**: Verified that test project (`Node.Net.Test.csproj`) only targets `net8.0` and `net8.0-windows`, NOT `netstandard2.0`. The spec's FR-007 refers to the library project's target frameworks, not the test project. This is correct - test project doesn't need netstandard2.0 validation.

**Clarification**: FR-007 applies to the library project's ability to work across frameworks. Test project validation on net8.0 and net8.0-windows is sufficient to verify the solution works.

---

## Concrete Edits Applied

### Edit 1: Replace Glob Patterns with Explicit File Lists (A3) ✅ APPLIED

**Location**: tasks.md, Phase 8, Tasks T060-T062

**Changes Applied**:
- T060: Replaced `Collections/*.Test.cs` glob with explicit list: Dictionary.Test.cs, Element.Test.cs, Items.Test.cs, Spatial.Test.cs
- T061: Replaced `Converters/*.Test.cs` glob with explicit file: HiddenWhenNull.Test.cs
- T062: Replaced `JsonRPC/*.Test.cs` glob with explicit list: Request.Test.cs, Responder.Test.cs, Server.Test.cs
- Added guidance: Check each file for System.Windows type usage; if none, only refactor Node.Net namespace references

### Edit 2: Add Runtime Overhead Validation Note (A8) ✅ APPLIED

**Location**: tasks.md, Phase 9, after T074

**Changes Applied**:
- Added T074a: "Verify no runtime performance overhead (NFR-002): Confirm all changes are compile-time only (configuration and syntax changes, no runtime code modifications)"
- This explicitly validates NFR-002 requirement

### Edit 3: Clarify Target Framework Scope (A9) ✅ APPLIED

**Location**: tasks.md, Phase 9, after T073

**Changes Applied**:
- Added T073a: "Note: Test project targets net8.0 and net8.0-windows only (not netstandard2.0). Library project's netstandard2.0 support (FR-007) is validated through library build, not test project execution."
- Clarifies that FR-007 applies to library project, not test project

### Additional Improvements Applied

**Clarified Refactoring Instructions**:
- Updated all refactoring tasks to explicitly state: "replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`"
- Updated Notes section to clarify that ALL files using `extern alias NodeNet` must be refactored (since extern alias is removed from project reference)
- Added explicit refactoring steps including replacement of `NodeNet::Node.Net.*` references

**Updated Verification Tasks**:
- T070-T071: Added expected results ("should return no results") for clarity
