# Tasks: SystemInfo Razor Component

**Input**: Design documents from `/specs/001-systeminfo-component/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md

**Tests**: Per Node.Net Constitution (Test-First Development - Principle III), component validation follows TDD approach through integration testing in example applications. For this display-only component, integration tests in example applications serve as the test-first validation mechanism, verifying component behavior before and after implementation. Manual verification tasks are included to ensure tests pass.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2)
- Include exact file paths in descriptions

## Path Conventions

- **Library component**: `source/Node.Net/Components/` at repository root
- **Example applications**: `examples/Node.Net.AspNet.Host/`, `examples/Node.Net.Wasm/`
- **Project file**: `source/Node.Net/Node.Net.csproj`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and package dependencies

- [x] T001 Create Components directory structure at source/Node.Net/Components/
- [x] T002 Add Microsoft.FluentUI.AspNetCore.Components package reference (Version 4.13.2) to source/Node.Net/Node.Net.csproj with condition to exclude net48 target framework
- [x] T003 Add Microsoft.FluentUI.AspNetCore.Components.Icons package reference (Version 4.13.2) to source/Node.Net/Node.Net.csproj with condition to exclude net48 target framework

**Checkpoint**: Package dependencies configured, Components directory ready

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: No foundational tasks required - component is self-contained and independent

**Note**: This phase is empty as the component has no blocking dependencies on other infrastructure.

**Checkpoint**: Foundation ready - user story implementation can now begin

---

## Phase 3: User Story 1 - Display System Information (Priority: P1) 🎯 MVP

**Goal**: Create a reusable SystemInfo Razor component that displays system information (user name, machine name, operating system, domain) with optional profile picture support

**Independent Test**: Verify that the component can be added to a Blazor page, renders correctly, displays all four system information fields accurately, and handles profile picture display/fallback appropriately

### Tests for User Story 1 (Integration Testing Approach)

> **NOTE: Per Constitution III (Test-First Development), integration tests in example applications serve as test-first validation. Verify component behavior before and after implementation.**

- [x] T004 [P] [US1] Create test page in examples/Node.Net.AspNet.Host to verify SystemInfo component integration (before component creation)
- [x] T005 [P] [US1] Create test page in examples/Node.Net.Wasm to verify SystemInfo component integration (before component creation)
- [x] T006 [US1] Verify test pages fail appropriately when component doesn't exist (TDD: tests fail first)

### Implementation for User Story 1

- [x] T007 [US1] Create SystemInfo.razor component file at source/Node.Net/Components/SystemInfo.razor
- [x] T008 [US1] Add using statements for System.Runtime.InteropServices, Microsoft.FluentUI.AspNetCore.Components, and Icons namespace in SystemInfo.razor
- [x] T009 [US1] Add XML documentation comments to SystemInfo.razor component (ProfilePictureUrl parameter and component class summary) per Constitution Technical Standards
- [x] T010 [US1] Implement FluentCard container with padding styling in SystemInfo.razor
- [x] T011 [US1] Implement FluentStack vertical layout with title "System Information" in SystemInfo.razor
- [x] T012 [US1] Implement horizontal FluentStack with Wrap="true" for responsive layout in SystemInfo.razor
- [x] T013 [US1] Implement profile picture display logic with conditional rendering (image if URL provided, FluentIcon PersonCircle if not) in SystemInfo.razor
- [x] T014 [US1] Add @onerror handler for profile picture image to fallback to default icon in SystemInfo.razor
- [x] T015 [US1] Implement UserName property using Environment.UserName with null/empty fallback to "Not available" in SystemInfo.razor
- [x] T016 [US1] Implement MachineName property using Environment.MachineName with null/empty fallback to "Not available" in SystemInfo.razor
- [x] T017 [US1] Implement OperatingSystem property using RuntimeInformation.OSDescription in SystemInfo.razor
- [x] T018 [US1] Implement Domain property using Environment.UserDomainName with null/empty fallback to "Not available" in SystemInfo.razor
- [x] T019 [US1] Add ProfilePictureUrl parameter (string?, optional) to SystemInfo.razor @code block
- [x] T020 [US1] Implement FluentStack vertical layouts for each system information field (User Name, Machine, OS, Domain) with labels and values in SystemInfo.razor
- [x] T021 [US1] Verify component builds successfully for net8.0 and net8.0-windows target frameworks
- [x] T022 [US1] Verify component is excluded from net48 builds (conditional compilation working)
- [x] T023 [US1] Verify integration tests pass (T004, T005 test pages now work with component) - TDD: tests pass after implementation

**Checkpoint**: At this point, User Story 1 should be fully functional - SystemInfo component created and building successfully

---

## Phase 4: User Story 2 - Integrate Component in Example Applications (Priority: P1)

**Goal**: Integrate the SystemInfo component into both example applications (Node.Net.AspNet.Host and Node.Net.Wasm) on their respective Home pages for demonstration

**Independent Test**: Verify that both example applications build and run successfully, the SystemInfo component displays correctly on the Home page in both applications, and system information is shown appropriately for each runtime environment

### Implementation for User Story 2

- [x] T024 [US2] Add @using Node.Net.Components directive to examples/Node.Net.AspNet.Host/Components/Pages/Home.razor
- [x] T025 [US2] Add SystemInfo component to examples/Node.Net.AspNet.Host/Components/Pages/Home.razor page content
- [x] T026 [US2] Verify examples/Node.Net.AspNet.Host project builds successfully
- [ ] T027 [US2] Run examples/Node.Net.AspNet.Host application and verify SystemInfo component displays correctly on home page (MANUAL: requires running the app)
- [x] T028 [US2] Add @using Node.Net.Components directive to examples/Node.Net.Wasm/Pages/Home.razor
- [x] T029 [US2] Add SystemInfo component to examples/Node.Net.Wasm/Pages/Home.razor page content
- [x] T030 [US2] Verify examples/Node.Net.Wasm project builds successfully
- [ ] T031 [US2] Run examples/Node.Net.Wasm application and verify SystemInfo component displays correctly on home page (with WASM browser restrictions handled gracefully) (MANUAL: requires running the app)

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently - component created and integrated into both example applications

---

## Phase 5: Polish & Cross-Cutting Concerns

**Purpose**: Final validation and documentation

- [ ] T032 [P] Verify component renders in <100ms performance goal (manual timing check) - REQUIRES MANUAL TESTING
- [ ] T033 [P] Test component with profile picture URL provided (verify image displays) - REQUIRES MANUAL TESTING
- [ ] T034 [P] Test component without profile picture URL (verify default icon displays) - REQUIRES MANUAL TESTING
- [ ] T035 [P] Test component in WASM environment (verify graceful handling of restricted system information) - REQUIRES MANUAL TESTING
- [ ] T036 [P] Test component responsive behavior on screen widths <600px (verify wrapping) - REQUIRES MANUAL TESTING
- [x] T037 Verify all target frameworks build successfully (net8.0, net8.0-windows)
- [x] T038 Verify net48 build excludes Fluent UI packages and SystemInfo component appropriately (conditional package references verified)
- [x] T039 Verify component can be added to Blazor page with single line of markup (SC-001 validation) - Verified: `<SystemInfo />`
- [ ] T040 Update quickstart.md validation (verify examples match quickstart.md guidance) - SKIPPED: quickstart.md does not exist

**Checkpoint**: All validation complete, component ready for use

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Empty - no blocking prerequisites
- **User Story 1 (Phase 3)**: Depends on Setup completion (package references needed)
- **User Story 2 (Phase 4)**: Depends on User Story 1 completion (component must exist before integration)
- **Polish (Phase 5)**: Depends on all user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Setup (Phase 1) - No dependencies on other stories
- **User Story 2 (P1)**: **MUST** complete after User Story 1 - Requires component to exist before integration

### Within Each User Story

- Component structure before implementation details
- Properties before UI rendering
- Core functionality before edge case handling
- Story complete before moving to next priority

### Parallel Opportunities

- T002 and T003 (package references) can run in parallel [P]
- T004 and T005 (integration test setup) can run in parallel [P]
- T015, T016, T017, T018 (system information properties) can be implemented in parallel [P]
- T024 and T028 (adding using directives) can run in parallel [P]
- T025 and T029 (adding component to pages) can run in parallel [P]
- T026 and T030 (build verification) can run in parallel [P]
- T032-T036 (validation tasks) can run in parallel [P]

---

## Parallel Example: User Story 1

```bash
# Launch integration test setup together (TDD approach):
Task: "Create test page in examples/Node.Net.AspNet.Host to verify SystemInfo component integration (before component creation)"
Task: "Create test page in examples/Node.Net.Wasm to verify SystemInfo component integration (before component creation)"

# Launch system information property implementations together:
Task: "Implement UserName property using Environment.UserName with null/empty fallback to 'Not available' in SystemInfo.razor"
Task: "Implement MachineName property using Environment.MachineName with null/empty fallback to 'Not available' in SystemInfo.razor"
Task: "Implement OperatingSystem property using RuntimeInformation.OSDescription in SystemInfo.razor"
Task: "Implement Domain property using Environment.UserDomainName with null/empty fallback to 'Not available' in SystemInfo.razor"

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (package references, directory structure)
2. Complete Phase 3: User Story 1 (create SystemInfo component)
3. **STOP and VALIDATE**: Verify component builds and can be used independently
4. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup → Foundation ready
2. Add User Story 1 → Test independently → Component ready for use
3. Add User Story 2 → Test independently → Examples demonstrate usage
4. Each story adds value without breaking previous stories

### Sequential Approach (Recommended)

1. Complete Phase 1: Setup
2. Complete Phase 3: User Story 1 (create component)
3. Complete Phase 4: User Story 2 (integrate examples)
4. Complete Phase 5: Polish and validation

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Component must be conditionally excluded from net48 builds (Fluent UI requires .NET 6+)
- WASM browser security restrictions will limit some system information - component handles gracefully
- Profile picture is optional - component works without it
- All system information properties should handle null/empty values with "Not available" fallback
- Commit after each phase completion for safety
- Stop at any checkpoint to validate story independently
