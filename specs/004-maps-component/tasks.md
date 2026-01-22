# Tasks: Maps Component

**Input**: Design documents from `/specs/004-maps-component/`
**Prerequisites**: plan.md ✅, spec.md ✅, research.md ✅, data-model.md ✅, quickstart.md ✅

**Tests**: TDD is REQUIRED per constitution - tests must be written before implementation.

**Organization**: Tasks are organized by user story to enable independent implementation and testing of each story.

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

**Purpose**: Project initialization and basic structure

- [x] T001 [P] Verify Components directory exists at source/Node.Net/Components/ (should already exist from previous components)
- [x] T002 [P] Verify Microsoft.FluentUI.AspNetCore.Components package reference exists in source/Node.Net/Node.Net.csproj (should already exist, net8.0+ only)
- [x] T003 [P] Create Components test directory structure in tests/Node.Net.Test/Components/ (should already exist)

**Checkpoint**: Directory structure ready, dependencies verified

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T004 [P] Create coordinate validation helper methods in source/Node.Net/Components/Maps.razor @code block (ValidateLatitude, ValidateLongitude, NormalizeCoordinates)
- [x] T005 [P] Create internal JSInterop service class MapsJSInterop in source/Node.Net/Components/Maps.razor @code block for map operations (InitializeMap, UpdateMapCenter, UpdateZoomLevel, UpdateMapType)
- [x] T006 [P] Write unit tests for coordinate validation in tests/Node.Net.Test/Components/Maps.Tests.cs (valid ranges, invalid coordinates default to 0, 0, null handling)

**Checkpoint**: Foundation ready - coordinate validation and JSInterop service structure in place

---

## Phase 3: User Story 1 - Display Map for Coordinates (Priority: P1) 🎯 MVP

**Goal**: Create a reusable Maps Razor component that displays a map centered on specific latitude and longitude coordinates with default zoom level 13, satellite map type, and 100% width/height dimensions.

**Independent Test**: Verify that the component can be added to a Blazor page with latitude and longitude parameters, renders correctly, displays a map centered on the specified coordinates, and handles invalid coordinates gracefully.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T007 [P] [US1] Write component test for Maps.razor rendering with valid coordinates in tests/Node.Net.Test/Components/Maps.Tests.cs (verify component renders, map element exists, coordinates passed correctly)
- [x] T008 [P] [US1] Write component test for invalid coordinate handling in tests/Node.Net.Test/Components/Maps.Tests.cs (verify invalid coordinates default to 0, 0)
- [x] T009 [P] [US1] Write component test for required parameter validation in tests/Node.Net.Test/Components/Maps.Tests.cs (verify error thrown if Latitude or Longitude not provided)
- [x] T010 [P] [US1] Write component test for coordinate parameter updates in tests/Node.Net.Test/Components/Maps.Tests.cs (verify map updates when coordinates change)
- [x] T011 [P] [US1] Write integration test for map display in example applications in tests/Node.Net.Test/Components/Maps.Integration.Tests.cs

### Implementation for User Story 1

- [x] T012 [US1] Create Maps.razor component file at source/Node.Net/Components/Maps.razor
- [x] T013 [US1] Add @namespace Node.Net.Components directive to Maps.razor
- [x] T014 [US1] Add using statements for Microsoft.JSInterop, Microsoft.FluentUI.AspNetCore.Components (if using Fluent UI styling) in Maps.razor
- [x] T015 [US1] Add XML documentation comments to Maps.razor component (all parameters and component class summary) per Constitution Technical Standards
- [x] T016 [US1] Add component parameters to Maps.razor @code block: Latitude (double, required), Longitude (double, required), ZoomLevel (int, optional, default: 13), MapType (string, optional, default: "satellite")
- [x] T017 [US1] Implement parameter validation in Maps.razor (throw ArgumentNullException if Latitude or Longitude null, validate ranges, normalize to 0, 0 if invalid)
- [x] T018 [US1] Implement component markup in Maps.razor with container div (100% width, 100% height) and map element with unique ID
- [x] T019 [US1] Implement OnAfterRenderAsync in Maps.razor to initialize Leaflet map via JSInterop on first render
- [x] T020 [US1] Implement JSInterop method InitializeMap in Maps.razor @code block to call JavaScript and create Leaflet map instance
- [x] T021 [US1] Create JavaScript helper file or inline script for Leaflet map initialization (or use JSInterop to call Leaflet directly)
- [x] T022 [US1] Implement coordinate normalization logic in Maps.razor (use validated coordinates or default to 0, 0)
- [x] T023 [US1] Implement map center update logic in Maps.razor when Latitude or Longitude parameters change
- [x] T024 [US1] Add error handling for map initialization failures in Maps.razor (display error message or placeholder)
- [x] T025 [US1] Add loading state handling in Maps.razor (show loading indicator while map initializes)
- [x] T026 [US1] Implement responsive styling in Maps.razor (100% width, 100% height, adapts to container)
- [x] T027 [US1] Verify component builds successfully for net8.0 and net8.0-windows target frameworks
- [x] T028 [US1] Verify component is excluded from net48 builds (conditional compilation working)
- [x] T029 [US1] Verify integration tests pass (T011) - TDD: tests pass after implementation

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently - users can display maps for coordinates

---

## Phase 4: User Story 1 - Optional Configuration (Priority: P1, Extension)

**Goal**: Add optional zoom level and map type configuration to the Maps component.

**Independent Test**: Verify that zoom level and map type parameters work correctly, with defaults applied when not specified.

### Tests for Optional Configuration ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T030 [P] [US1] Write component test for zoom level parameter in tests/Node.Net.Test/Components/Maps.Tests.cs (verify default zoom 13, custom zoom level works)
- [x] T031 [P] [US1] Write component test for map type parameter in tests/Node.Net.Test/Components/Maps.Tests.cs (verify default satellite, custom map type works)
- [x] T032 [P] [US1] Write component test for zoom level updates in tests/Node.Net.Test/Components/Maps.Tests.cs (verify map updates when zoom level changes)
- [x] T033 [P] [US1] Write component test for map type updates in tests/Node.Net.Test/Components/Maps.Tests.cs (verify map updates when map type changes)

### Implementation for Optional Configuration

- [x] T034 [US1] Implement ZoomLevel parameter handling in Maps.razor (default to 13 if not provided)
- [x] T035 [US1] Implement MapType parameter handling in Maps.razor (default to "satellite" if not provided)
- [x] T036 [US1] Implement JSInterop method UpdateZoomLevel in Maps.razor to update map zoom when parameter changes
- [x] T037 [US1] Implement JSInterop method UpdateMapType in Maps.razor to update map tile layer when parameter changes
- [x] T038 [US1] Add parameter change detection in Maps.razor to trigger map updates when ZoomLevel or MapType change
- [x] T039 [US1] Verify zoom level and map type tests pass (T030-T033) - TDD: tests pass after implementation

**Checkpoint**: At this point, User Story 1 is complete with all optional configuration - users can display maps with custom zoom and map type

---

## Phase 5: User Story 2 - Integrate Component in Example Applications (Priority: P2)

**Goal**: Integrate the Maps component into both example applications (Node.Net.AspNet.Host and Node.Net.Wasm) to demonstrate usage.

**Independent Test**: Verify that the Maps component can be added to pages in both example applications, displays correctly, and works in both Blazor Server and WebAssembly hosting models.

### Tests for User Story 2 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T040 [P] [US2] Write integration test for Maps component in ASP.NET Host application in tests/Node.Net.Test/Components/Maps.Integration.Tests.cs
- [x] T041 [P] [US2] Write integration test for Maps component in WebAssembly application in tests/Node.Net.Test/Components/Maps.Integration.Tests.cs

### Implementation for User Story 2

- [x] T042 [US2] Add Leaflet CSS and JavaScript script tags to examples/Node.Net.AspNet.Host/Pages/_Host.cshtml or App.razor
- [x] T043 [US2] Add Leaflet CSS and JavaScript script tags to examples/Node.Net.Wasm/wwwroot/index.html
- [x] T044 [US2] Create Maps.razor page in examples/Node.Net.AspNet.Host/Components/Pages/Maps.razor using Node.Net.Components.Maps component
- [x] T045 [US2] Create Maps.razor page in examples/Node.Net.Wasm/Pages/Maps.razor using Node.Net.Components.Maps component
- [x] T046 [US2] Add navigation links to Maps page in examples/Node.Net.AspNet.Host/Components/Layout/NavMenu.razor
- [x] T047 [US2] Add navigation links to Maps page in examples/Node.Net.Wasm/Layout/NavMenu.razor
- [x] T048 [US2] Verify both example applications build successfully
- [ ] T049 [US2] Verify both example applications run and display maps correctly
- [x] T050 [US2] Verify integration tests pass (T040, T041) - TDD: tests pass after implementation

**Checkpoint**: At this point, User Story 2 should be complete - Maps component integrated and working in both example applications

---

## Phase 6: Cross-Cutting & Polish

**Purpose**: Final polish, error handling improvements, and documentation

- [x] T051 [P] Add comprehensive error handling for network failures in Maps.razor (map service unavailable, tile loading failures)
- [x] T052 [P] Add error handling for JavaScript interop failures in Maps.razor (Leaflet not loaded, JS errors)
- [x] T053 [P] Add user-friendly error messages or placeholders in Maps.razor for error states
- [x] T054 [P] Add XML documentation comments to all public APIs in Maps.razor (parameters, methods, component summary)
- [x] T055 [P] Verify all tests pass on all target frameworks (net8.0, net8.0-windows)
- [ ] T056 [P] Run quickstart.md validation scenarios to verify integration works correctly
- [ ] T057 [P] Performance testing: Verify map initialization <2 seconds, coordinate updates <500ms
- [x] T058 [P] Verify component works correctly in both Blazor Server and WebAssembly hosting models

---

## Dependencies & Execution Order

### User Story Dependencies

- **US1** (Display Map) → **US2** (Integration): US1 must be complete before US2 can begin (component must exist before integration)

### Task Dependencies Within User Stories

**US1 Dependencies**:
- T007-T011 (Tests) → T012-T029 (Implementation): Tests must be written first (TDD)
- T012-T029 (Core Implementation) → T030-T039 (Optional Configuration): Core map display must work before adding optional features
- T030-T033 (Optional Config Tests) → T034-T039 (Optional Config Implementation): Tests first

**US2 Dependencies**:
- T040-T041 (Tests) → T042-T050 (Implementation): Tests must be written first (TDD)
- T012-T029 (US1 Core) → T042-T050 (US2 Integration): Component must exist before integration

### Parallel Execution Opportunities

**Phase 2 (Foundational)**:
- T004, T005, T006 can run in parallel (different concerns: validation, JSInterop, tests)

**Phase 3 (US1 Tests)**:
- T007, T008, T009, T010, T011 can run in parallel (different test scenarios)

**Phase 4 (US1 Optional Config Tests)**:
- T030, T031, T032, T033 can run in parallel (different test scenarios)

**Phase 5 (US2 Tests)**:
- T040, T041 can run in parallel (different applications)

**Phase 6 (Cross-Cutting)**:
- T051-T058 can run in parallel (different polish tasks)

---

## Notes

- All tests must be written BEFORE implementation (TDD requirement per Constitution)
- JSInterop setup will be required for component tests (similar to Fluent UI components)
- Leaflet JavaScript library must be loaded in host applications (not via NuGet)
- Component is self-contained - no service layer required
- Coordinate validation happens in C# before JSInterop calls
- Invalid coordinates default to (0, 0) without error message (graceful degradation)
