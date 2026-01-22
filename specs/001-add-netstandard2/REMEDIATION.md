# Remediation Suggestions

**Date**: 2025-01-27  
**Issues Addressed**: A1, A3, A5 from ANALYSIS.md

## Issue A1: Ambiguity in NFR-001

**Current Text** (spec.md line 46):
```markdown
- **NFR-001**: Build time for netstandard2.0 target MUST not significantly increase overall build time (target: <20% increase)
```

**Issue**: "Significantly increase" is redundant since the requirement already specifies "<20% increase"

**Suggested Remediation**:

```markdown
- **NFR-001**: Build time for netstandard2.0 target MUST not increase overall build time by more than 20% (target: <20% increase)
```

**Rationale**: Removes vague "significantly" qualifier while maintaining the quantified target. Makes the requirement clearer and more measurable.

---

## Issue A3: Platform-Specific Features Not Explicitly Enumerated

**Current Text** (spec.md line 38):
```markdown
- **FR-003**: Platform-specific features (e.g., WPF, Windows-specific APIs) MUST be conditionally excluded from netstandard2.0 builds
```

**Issue**: "Platform-specific features" is somewhat vague - should explicitly enumerate what needs to be excluded

**Suggested Remediation**:

```markdown
- **FR-003**: Platform-specific features MUST be conditionally excluded from netstandard2.0 builds. This includes:
  - WPF (Windows Presentation Foundation) types and APIs
  - Windows-specific APIs (e.g., WinRT, Windows Runtime)
  - Windows-specific packages (Microsoft.Windows.SDK.Contracts)
  - Any code that depends on Windows-only functionality
```

**Rationale**: Explicitly enumerates what "platform-specific features" means, making the requirement clearer and easier to verify. Aligns with tasks T016-T019.

---

## Issue A5: Vague Tasks for Source Code Review

**Current Text** (tasks.md lines 74-75):
```markdown
- [ ] T018 [US1] Review source files for platform-specific code requiring conditional compilation directives (#if !NETSTANDARD2_0) in source/Node.Net/
- [ ] T019 [US1] Add conditional compilation directives (#if !NETSTANDARD2_0) for WPF/Windows-specific APIs if needed in source/Node.Net/
```

**Issue**: "Review source files" and "if needed" are vague - should provide specific guidance on what to look for and when to add directives

**Suggested Remediation**:

```markdown
- [ ] T018 [US1] Review source files in source/Node.Net/ for platform-specific code requiring conditional compilation directives. Focus on:
  - Files using WPF types (System.Windows.*, PresentationFramework, etc.)
  - Files using Windows-specific APIs (WinRT, Windows Runtime, etc.)
  - Files referencing IS_WINDOWS or IS_FRAMEWORK constants that may need NETSTANDARD2_0 exclusion
  - Service classes with platform-specific implementations (e.g., OsUserProfileService, UserInformation)
- [ ] T019 [US1] Add conditional compilation directives (#if !NETSTANDARD2_0) for identified platform-specific code in source/Node.Net/. Wrap:
  - WPF/Windows-specific type references
  - Windows-only API calls
  - Platform-specific implementations that cannot work on .NET Standard 2.0
  - Ensure core library functionality remains available for netstandard2.0 builds
```

**Rationale**: Provides specific guidance on:
1. What files to review (focus areas)
2. What to look for (specific patterns)
3. When to add directives (what needs wrapping)
4. How to ensure core functionality remains (constraint)

This makes the tasks more actionable and reduces ambiguity.

---

## Summary of Changes

### Files to Modify:

1. **spec.md** (2 changes):
   - Line 46: Update NFR-001 to remove "significantly"
   - Line 38: Expand FR-003 to enumerate platform-specific features

2. **tasks.md** (2 changes):
   - Lines 74-75: Expand T018-T019 with specific review guidance

### Impact:

- **A1**: Improves clarity of NFR-001 (LOW → resolved)
- **A3**: Improves specificity of FR-003 (MEDIUM → LOW)
- **A5**: Improves actionability of T018-T019 (LOW → resolved)

All changes are non-breaking and improve clarity without changing the scope or intent of the requirements.

---

## Application Instructions

To apply these remediations:

1. **For spec.md**:
   - Replace line 46 with the updated NFR-001 text
   - Replace line 38 with the expanded FR-003 text

2. **For tasks.md**:
   - Replace lines 74-75 with the expanded T018-T019 text

These changes can be applied independently - each remediation addresses a separate issue.
