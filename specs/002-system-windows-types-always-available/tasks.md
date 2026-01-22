# Tasks: System.Windows Types Always Available

**Input**: Design documents from `/specs/002-system-windows-types-always-available/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, quickstart.md

**Tests**: This feature refactors existing tests. Baseline verification is required before changes, then validation after refactoring.

**Organization**: Tasks are organized by implementation phases to enable systematic refactoring and validation.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (US1)
- Include exact file paths in descriptions

## Phase 1: Setup & Baseline Verification

**Purpose**: Establish baseline and prepare for refactoring

- [ ] T001 [US1] Verify current tests pass on net8.0 target framework: `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0`
- [ ] T002 [US1] Verify current tests pass on net8.0-windows target framework (Windows only): `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows`
- [ ] T003 [US1] Document baseline test count and pass rate for comparison after refactoring
- [ ] T004 [US1] Identify all test files using `extern alias NodeNet`: `grep -r "extern alias NodeNet" tests/Node.Net.Test/`
- [ ] T005 [US1] Identify all test files using `NodeNet::` prefix: `grep -r "NodeNet::" tests/Node.Net.Test/`
- [ ] T006 [US1] Create inventory of test files requiring refactoring (estimated ~75 files)

**Checkpoint**: Baseline established - all current tests pass, inventory of files to refactor is complete

---

## Phase 2: Configuration Changes

**Purpose**: Update test project configuration to enable transparent type access

- [ ] T007 [US1] Remove `<Aliases>NodeNet</Aliases>` from project reference in `tests/Node.Net.Test/Node.Net.Test.csproj`
- [ ] T008 [US1] Create `tests/Node.Net.Test/GlobalUsings.cs` file with global using statements:
  - `global using System.Windows;`
  - `global using System.Windows.Media;`
  - `global using System.Windows.Media.Imaging;`
  - `global using System.Windows.Media.Media3D;`
- [ ] T009 [US1] Verify GlobalUsings.cs compiles without errors
- [ ] T010 [US1] Build test project for net8.0: `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0`
- [ ] T011 [US1] Build test project for net8.0-windows (Windows only): `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows`

**Checkpoint**: Configuration changes complete - project builds successfully (may have compilation errors in test files until refactored)

---

## Phase 3: Test File Refactoring - System/Windows Tests

**Purpose**: Refactor test files in System/Windows directory to use standard namespace references

- [ ] T012 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Point.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references
- [ ] T013 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Vector.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references
- [ ] T014 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/ResourceDictionary.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references
- [ ] T015 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Color.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.*` with standard references
- [ ] T016 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Brushes.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.*` with standard references
- [ ] T017 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/DrawingImage.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.*` with standard references
- [ ] T018 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/ImageSource.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.*` with standard references
- [ ] T019 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Matrix.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.*` with standard references
- [ ] T020 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Imaging/BitmapSource.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Imaging.*` with standard references
- [ ] T021 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/DiffuseMaterial.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T022 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/EmissiveMaterial.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T023 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/Material.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T024 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/MaterialGroup.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T025 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/Matrix3D.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T026 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/MeshGeometry3D.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T027 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/Point3D.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T028 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/Quaternion.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T029 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/Rect3D.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T030 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/Size3D.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T031 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/SpecularMaterial.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T032 [P] [US1] Refactor `tests/Node.Net.Test/System/Windows/Media/Media3D/Vector3D.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references

**Checkpoint**: All System/Windows test files refactored - verify compilation and tests pass

---

## Phase 4: Test File Refactoring - Extension Tests

**Purpose**: Refactor extension method tests to use standard namespace references

- [ ] T033 [P] [US1] Refactor `tests/Node.Net.Test/Extension/Point.Extension.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references
- [ ] T034 [P] [US1] Refactor `tests/Node.Net.Test/Extension/Point3D.Extension.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T035 [P] [US1] Refactor `tests/Node.Net.Test/Extension/Rect3D.Extension.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T036 [P] [US1] Refactor `tests/Node.Net.Test/Extension/Matrix3D.Extension.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T037 [P] [US1] Refactor `tests/Node.Net.Test/Extension/PerspectiveCamera.Extension.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references

**Checkpoint**: Extension method tests refactored - verify extension methods work correctly

---

## Phase 5: Test File Refactoring - Root Level Tests

**Purpose**: Refactor root-level test files that use System.Windows types

- [ ] T038 [US1] Refactor `tests/Node.Net.Test/Vector3D.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNetVector3D` alias and `NodeNet::System.Windows.Media.Media3D.Vector3D` with standard `Vector3D` references
- [ ] T039 [P] [US1] Refactor `tests/Node.Net.Test/Matrix3D.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T040 [P] [US1] Refactor `tests/Node.Net.Test/Matrix3D_Compare_Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T041 [P] [US1] Refactor `tests/Node.Net.Test/Matrix3D_Diagnostic_Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T042 [P] [US1] Refactor `tests/Node.Net.Test/Matrix3DFactory.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references
- [ ] T043 [P] [US1] Refactor `tests/Node.Net.Test/ConvertRotationsXYZtoOTS.Comprehensive.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.Media.Media3D.*` with standard references

**Checkpoint**: Root-level test files refactored - verify compilation

---

## Phase 6: Test File Refactoring - Component Tests

**Purpose**: Refactor component tests that use System.Windows types

- [ ] T044 [P] [US1] Refactor `tests/Node.Net.Test/Components/BuildVerification.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T045 [P] [US1] Refactor `tests/Node.Net.Test/Components/Maps.Integration.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T046 [P] [US1] Refactor `tests/Node.Net.Test/Components/Maps.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T047 [P] [US1] Refactor `tests/Node.Net.Test/Components/SystemInfo.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T048 [P] [US1] Refactor `tests/Node.Net.Test/Components/ApplicationInfo.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T049 [P] [US1] Refactor `tests/Node.Net.Test/Components/Logs.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T050 [P] [US1] Refactor `tests/Node.Net.Test/Components/Logs.Integration.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`

**Checkpoint**: Component tests refactored - verify compilation

---

## Phase 7: Test File Refactoring - Service Tests

**Purpose**: Refactor service tests that use System.Windows types

- [ ] T051 [P] [US1] Refactor `tests/Node.Net.Test/Service/Application/Application.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T052 [P] [US1] Refactor `tests/Node.Net.Test/Service/Application/ApplicationInfo.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T053 [P] [US1] Refactor `tests/Node.Net.Test/Service/FileSystem/LiteDbFileSystem.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T054 [P] [US1] Refactor `tests/Node.Net.Test/Service/Logging/LogEntry.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T055 [P] [US1] Refactor `tests/Node.Net.Test/Service/Logging/LogService.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T056 [P] [US1] Refactor `tests/Node.Net.Test/Service/Logging/LogService.Integration.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T057 [P] [US1] Refactor `tests/Node.Net.Test/Service/User/SystemUser.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T058 [P] [US1] Refactor `tests/Node.Net.Test/Service/WebServer.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`

**Checkpoint**: Service tests refactored - verify compilation

---

## Phase 8: Test File Refactoring - Remaining Tests

**Purpose**: Refactor remaining test files that use System.Windows types

- [ ] T059 [P] [US1] Refactor `tests/Node.Net.Test/Security/UserSecretProvider.Tests.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T060 [P] [US1] Refactor Collections test files: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references, replace `NodeNet::Node.Net.*` with `Node.Net.*`:
  - `tests/Node.Net.Test/Collections/Dictionary.Test.cs`
  - `tests/Node.Net.Test/Collections/Element.Test.cs`
  - `tests/Node.Net.Test/Collections/Items.Test.cs`
  - `tests/Node.Net.Test/Collections/Spatial.Test.cs`
  - Note: Check each file for System.Windows type usage; if none, only refactor Node.Net namespace references
- [ ] T061 [P] [US1] Refactor `tests/Node.Net.Test/Converters/HiddenWhenNull.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T062 [P] [US1] Refactor JsonRPC test files: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`:
  - `tests/Node.Net.Test/JsonRPC/Request.Test.cs`
  - `tests/Node.Net.Test/JsonRPC/Responder.Test.cs`
  - `tests/Node.Net.Test/JsonRPC/Server.Test.cs`
- [ ] T063 [P] [US1] Refactor `tests/Node.Net.Test/Formatter.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T064 [P] [US1] Refactor `tests/Node.Net.Test/Factory.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T065 [P] [US1] Refactor `tests/Node.Net.Test/Log.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T066 [P] [US1] Refactor `tests/Node.Net.Test/Reader.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T067 [P] [US1] Refactor `tests/Node.Net.Test/Writer.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T068 [P] [US1] Refactor `tests/Node.Net.Test/DelegateCommand.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [ ] T069 [P] [US1] Refactor `tests/Node.Net.Test/Dictionary.Test.cs`: Remove `extern alias NodeNet`, replace `NodeNet::System.Windows.*` with standard references (if any), replace `NodeNet::Node.Net.*` with `Node.Net.*`
- [x] T070 [US1] Verify no remaining `extern alias NodeNet` declarations: `grep -r "extern alias NodeNet" tests/Node.Net.Test/` - should return no results
- [x] T071 [US1] Verify no remaining `NodeNet::` prefixes: `grep -r "NodeNet::" tests/Node.Net.Test/` - should return no results (all NodeNet:: references should be replaced with standard namespace references)

**Checkpoint**: All test files refactored - no extern alias or NodeNet:: prefixes remain

---

## Phase 9: Validation & Verification

**Purpose**: Verify refactoring success and validate all requirements

- [x] T072 [US1] Build test project for net8.0: `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0` - verify no compilation errors
- [ ] T073 [US1] Build test project for net8.0-windows (Windows only): `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows` - verify no compilation errors
- [ ] T073a [US1] Note: Test project targets net8.0 and net8.0-windows only (not netstandard2.0). Library project's netstandard2.0 support (FR-007) is validated through library build, not test project execution.
- [x] T074 [US1] Verify no ambiguous type reference errors in build output
- [x] T074a [US1] Verify no runtime performance overhead (NFR-002): Confirm all changes are compile-time only (configuration and syntax changes, no runtime code modifications)
- [ ] T075 [US1] Run all tests on net8.0: `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0` - verify all tests pass
- [ ] T076 [US1] Run all tests on net8.0-windows (Windows only): `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows` - verify all tests pass
- [ ] T077 [US1] Compare test results between net8.0 and net8.0-windows - verify identical results (per Acceptance Scenario 4)
- [x] T078 [US1] Verify Vector3D.Test.cs uses standard namespace (SC-001): Check file contains no `extern alias` or `NodeNet::` prefixes
- [x] T079 [US1] Verify all System/Windows/** tests use standard namespaces (SC-002): Check files contain no `extern alias` or `NodeNet::` prefixes
- [x] T080 [US1] Run extension method tests on net8.0: `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0 --filter "FullyQualifiedName~Extension"` - verify extension methods work with custom types (52 tests passed)
- [ ] T081 [US1] Run extension method tests on net8.0-windows (Windows only): `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows --filter "FullyQualifiedName~Extension"` - verify extension methods work with platform types
- [ ] T082 [US1] Verify extension methods produce identical results on both target frameworks (SC-005)
- [x] T083 [US1] Verify test code readability improved: Compare before/after - confirm elimination of `NodeNet::` prefixes (NFR-001) - All 68 files refactored, 0 `NodeNet::` references remaining
- [ ] T084 [US1] Measure build time before and after changes - verify <5% increase (NFR-003)
- [ ] T085 [US1] Run quickstart.md validation scenarios to verify integration works correctly

**Checkpoint**: All validation complete - all success criteria met, all tests pass, requirements satisfied

---

## Phase 10: Documentation & Cleanup

**Purpose**: Update documentation and finalize implementation

- [x] T086 [US1] Update `docs/SYSTEM_NAMESPACE_RULES.md` to reflect new test usage pattern (remove extern alias guidance, add global usings guidance)
- [x] T087 [US1] Verify README.md or other documentation doesn't reference old extern alias pattern
- [ ] T088 [US1] Commit all changes with descriptive commit message
- [x] T089 [US1] Create summary of changes: number of files refactored, test results, validation outcomes (see IMPLEMENTATION_SUMMARY.md)

**Checkpoint**: Documentation updated, changes committed, implementation complete

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup & Baseline)**: No dependencies - can start immediately
- **Phase 2 (Configuration)**: Depends on Phase 1 completion (need baseline before changes)
- **Phase 3-8 (Refactoring)**: All depend on Phase 2 completion (need configuration before refactoring)
  - Phases 3-8 can proceed in parallel for different file groups
  - Within each phase, tasks marked [P] can run in parallel
- **Phase 9 (Validation)**: Depends on all refactoring phases (3-8) completion
- **Phase 10 (Documentation)**: Depends on Phase 9 completion (validate before documenting)

### Parallel Opportunities

- **Phase 1**: T004, T005 can run in parallel (different grep commands)
- **Phase 2**: T010, T011 can run in parallel (different target frameworks, if on Windows)
- **Phase 3-8**: All tasks marked [P] can run in parallel (different files)
- **Phase 9**: T080, T081 can run in parallel (different target frameworks, if on Windows)
- **Phase 10**: T086, T087 can run in parallel (different documentation files)

### Critical Path

1. Phase 1: Setup & Baseline (T001-T006)
2. Phase 2: Configuration Changes (T007-T011)
3. Phase 3-8: Test File Refactoring (T012-T071) - can parallelize heavily
4. Phase 9: Validation (T072-T085)
5. Phase 10: Documentation (T086-T089)

---

## Implementation Strategy

### Sequential Approach (Recommended for First Pass)

1. Complete Phase 1: Establish baseline
2. Complete Phase 2: Configuration changes
3. Complete Phase 3: System/Windows tests (highest priority - most System.Windows usage)
4. Validate: Build and test Phase 3 changes
5. Complete Phase 4: Extension tests (verify extension methods work)
6. Validate: Build and test Phase 4 changes
7. Complete remaining phases incrementally
8. Final validation in Phase 9

### Parallel Approach (For Team)

With multiple developers:
1. Team completes Phase 1-2 together
2. Once Phase 2 is done:
   - Developer A: Phase 3 (System/Windows tests)
   - Developer B: Phase 4 (Extension tests)
   - Developer C: Phase 5-8 (Other test files)
3. All meet at Phase 9 for validation
4. Phase 10 documentation

---

## Notes

- [P] tasks = different files, no dependencies - can run in parallel
- [US1] label maps task to User Story 1 for traceability
- Each refactoring task should:
  1. Remove `extern alias NodeNet;` declaration (required - extern alias is removed from project reference)
  2. Replace `NodeNet::System.Windows.*` with `System.Windows.*` (or rely on global usings)
  3. Replace `using NodeNet::System.Windows.*;` with `using System.Windows.*;` (or rely on global usings)
  4. Replace `NodeNet::Node.Net.*` with `Node.Net.*` (standard namespace reference)
  5. Replace `using NodeNet::Node.Net.*;` with `using Node.Net.*;` (standard namespace reference)
  6. Update type aliases to use standard namespaces
  7. Verify file compiles after changes
- Commit after each phase or logical group of files
- Stop at checkpoints to validate before proceeding
- **Important**: Since extern alias is removed from project reference, ALL files using `extern alias NodeNet` must be refactored, even if they only use `NodeNet::Node.Net.*` types (not System.Windows types)
- Verify extension methods work after each extension test refactoring phase
