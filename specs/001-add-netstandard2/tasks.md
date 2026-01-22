# Tasks: Add .NET Standard 2.0 Target Framework

**Input**: Design documents from `/specs/001-add-netstandard2/`
**Prerequisites**: plan.md ✅, spec.md ✅, research.md ✅, quickstart.md ✅

**Tests**: TDD is REQUIRED per constitution - tests must be written before implementation.

**Organization**: Tasks are organized by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2)
- Include exact file paths in descriptions

## Path Conventions

- **Library project**: `source/Node.Net/` at repository root
- **Project file**: `source/Node.Net/Node.Net.csproj`
- **Test project**: `tests/Node.Net.Test/` at repository root

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and verification

- [x] T001 [P] Verify source/Node.Net/Node.Net.csproj exists and current target frameworks are documented
- [x] T002 [P] Verify existing conditional compilation patterns (IS_WINDOWS, IS_FRAMEWORK) are understood
- [x] T003 [P] Verify package references in source/Node.Net/Node.Net.csproj are documented

**Checkpoint**: Project structure verified, ready for implementation

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T004 [P] Research package compatibility for .NET Standard 2.0 (completed in research.md)
- [x] T005 [P] Document conditional compilation strategy for netstandard2.0 (completed in research.md)
- [x] T006 [P] Document package reference strategy for conditional versions (completed in research.md)

**Checkpoint**: Foundation ready - package compatibility and strategies documented

---

## Phase 3: User Story 1 - Add .NET Standard 2.0 Target Framework (Priority: P1) 🎯 MVP

**Goal**: Add netstandard2.0 as a target framework to Node.Net.csproj, enabling broader library compatibility with .NET Framework 4.6.1+, .NET Core 2.0+, and other platforms supporting .NET Standard 2.0.

**Independent Test**: Can be fully tested by building the Node.Net library with netstandard2.0 target framework and verifying that a test project targeting .NET Standard 2.0 can successfully reference and use the library without compilation errors.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T007 [P] [US1] Write build verification test: verify netstandard2.0 builds successfully in tests/Node.Net.Test/Components/BuildVerification.Tests.cs
- [x] T008 [P] [US1] Write package reference test: create test project targeting .NET Standard 2.0 that references Node.Net library (added to BuildVerification.Tests.cs)
- [x] T009 [P] [US1] Write backward compatibility test: verify existing target frameworks (net48, net8.0, net8.0-windows) still build successfully (added to BuildVerification.Tests.cs)

### Implementation for User Story 1

- [x] T010 [US1] Add netstandard2.0 to TargetFrameworks property for Windows builds in source/Node.Net/Node.Net.csproj (update line 6: add netstandard2.0 to net48;net8.0;net8.0-windows)
- [x] T011 [US1] Add netstandard2.0 to TargetFrameworks property for non-Windows builds in source/Node.Net/Node.Net.csproj (update line 7: add netstandard2.0 to net8.0)
- [x] T012 [US1] Add conditional package reference for System.Drawing.Common 7.0.0 for netstandard2.0 in source/Node.Net/Node.Net.csproj (use condition: '$(TargetFramework)' == 'netstandard2.0')
- [x] T013 [US1] Ensure System.Drawing.Common 8.0.2 is excluded from netstandard2.0 builds in source/Node.Net/Node.Net.csproj (add condition to existing ItemGroup)
- [x] T014 [US1] Exclude Razor components from netstandard2.0 builds in source/Node.Net/Node.Net.csproj (add ItemGroup with condition '$(TargetFramework)' == 'netstandard2.0', same pattern as net48)
- [x] T015 [US1] Exclude static web assets (JavaScript modules) from netstandard2.0 builds in source/Node.Net/Node.Net.csproj (update existing ItemGroup condition to exclude netstandard2.0)
- [x] T016 [US1] Ensure Microsoft.FluentUI.AspNetCore.Components is excluded from netstandard2.0 builds in source/Node.Net/Node.Net.csproj (verify existing condition excludes netstandard2.0)
- [x] T017 [US1] Ensure Microsoft.Windows.SDK.Contracts is excluded from netstandard2.0 builds in source/Node.Net/Node.Net.csproj (verify existing condition excludes netstandard2.0)
- [x] T018 [US1] Review source files in source/Node.Net/ for platform-specific code requiring conditional compilation directives. Focus on:
  - Files using WPF types (System.Windows.*, PresentationFramework, etc.)
  - Files using Windows-specific APIs (WinRT, Windows Runtime, etc.)
  - Files referencing IS_WINDOWS or IS_FRAMEWORK constants that may need NETSTANDARD2_0 exclusion
  - Service classes with platform-specific implementations (e.g., OsUserProfileService, UserInformation)
- [x] T019 [US1] Add conditional compilation directives (#if !NETSTANDARD2_0) for identified platform-specific code in source/Node.Net/. Wrap:
  - WPF/Windows-specific type references
  - Windows-only API calls
  - Platform-specific implementations that cannot work on .NET Standard 2.0
  - Ensure core library functionality remains available for netstandard2.0 builds
- [ ] T020 [US1] Verify build succeeds for netstandard2.0 target framework: `dotnet build source/Node.Net/Node.Net.csproj --framework netstandard2.0` (requires package restore - see IMPLEMENTATION_SUMMARY.md)
- [ ] T021 [US1] Verify all target frameworks build successfully: `dotnet build source/Node.Net/Node.Net.csproj` (requires package restore - see IMPLEMENTATION_SUMMARY.md)
- [ ] T022 [US1] Verify integration tests pass (T007-T009) - TDD: tests pass after implementation (requires successful builds - see IMPLEMENTATION_SUMMARY.md)

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently - netstandard2.0 target framework added and library builds successfully

---

## Phase 4: Validation & Polish

**Purpose**: Final validation, package verification, and documentation

- [ ] T023 [P] Verify NuGet package includes netstandard2.0 in supported frameworks: `dotnet pack source/Node.Net/Node.Net.csproj` and inspect .nupkg metadata
- [ ] T024 [P] Create test consumer project targeting .NET Standard 2.0 and verify it can reference and use Node.Net library
- [ ] T025 [P] Verify all existing tests pass for net48: `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net48`
- [ ] T026 [P] Verify all existing tests pass for net8.0: `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0`
- [ ] T027 [P] Measure build time impact: compare build times before/after adding netstandard2.0 (target: <20% increase per NFR-001)
- [ ] T028 [P] Run quickstart.md validation scenarios to verify integration works correctly
- [x] T029 [P] Update documentation if needed to reflect netstandard2.0 support (README.md and SYSTEM_NAMESPACE_RULES.md updated)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories (already complete via research.md)
- **User Story 1 (Phase 3)**: Depends on Foundational phase completion
- **Validation & Polish (Phase 4)**: Depends on User Story 1 completion

### Task Dependencies Within User Story 1

**US1 Dependencies**:
- T007-T009 (Tests) → T010-T021 (Implementation): Tests must be written first (TDD)
- T010-T011 (TargetFrameworks) → T012-T019 (Package references and conditional compilation): Must add target framework before configuring packages
- T012-T017 (Package configuration) → T018-T019 (Source code review): Package exclusions must be configured before reviewing source code
- T010-T021 (Core Implementation) → T022 (Test verification): Implementation must complete before tests can pass

### Parallel Execution Opportunities

**Phase 1 (Setup)**:
- T001, T002, T003 can run in parallel (different verification tasks)

**Phase 3 (US1 Tests)**:
- T007, T008, T009 can run in parallel (different test scenarios)

**Phase 3 (US1 Implementation)**:
- T012, T013, T014, T015, T016, T017 can run in parallel (different package/asset exclusions, all modifying .csproj but different ItemGroups)
- T018, T019 can run in parallel (reviewing different source files)

**Phase 4 (Validation)**:
- T023, T024, T025, T026, T027, T028, T029 can run in parallel (different validation tasks)

---

## Parallel Example: User Story 1 Implementation

```bash
# Launch package configuration tasks together (all modify different ItemGroups in .csproj):
Task: "Add conditional package reference for System.Drawing.Common 7.0.0 for netstandard2.0"
Task: "Ensure System.Drawing.Common 8.0.2 is excluded from netstandard2.0 builds"
Task: "Exclude Razor components from netstandard2.0 builds"
Task: "Exclude static web assets from netstandard2.0 builds"
Task: "Ensure Microsoft.FluentUI.AspNetCore.Components is excluded from netstandard2.0 builds"
Task: "Ensure Microsoft.Windows.SDK.Contracts is excluded from netstandard2.0 builds"

# Launch validation tasks together:
Task: "Verify build succeeds for netstandard2.0 target framework"
Task: "Verify all target frameworks build successfully"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup ✅ (already complete)
2. Complete Phase 2: Foundational ✅ (already complete via research)
3. Complete Phase 3: User Story 1
   - Write tests (T007-T009)
   - Implement core changes (T010-T021)
   - Verify tests pass (T022)
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Complete Phase 4: Validation & Polish
6. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready ✅
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Each validation task adds confidence without breaking previous work

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together ✅
2. Once Foundational is done:
   - Developer A: Write tests (T007-T009)
   - Developer B: Add target frameworks (T010-T011)
   - Developer C: Configure package references (T012-T017)
3. Once tests written:
   - Developer A: Review source code for conditional compilation (T018-T019)
   - Developer B: Verify builds (T020-T021)
4. Validation tasks can run in parallel (T023-T029)

---

## Notes

- [P] tasks = different files or different ItemGroups, no dependencies
- [US1] label maps task to User Story 1 for traceability
- User Story 1 should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
- Primary file to modify: `source/Node.Net/Node.Net.csproj`
- Conditional compilation uses built-in `NETSTANDARD2_0` symbol (no manual definition needed)
