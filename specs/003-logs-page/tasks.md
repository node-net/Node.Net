# Tasks: Logs Page

**Input**: Design documents from `/specs/003-logs-page/`
**Prerequisites**: plan.md ✅, spec.md ✅, research.md ✅, data-model.md ✅, contracts/ ✅

**Tests**: TDD is REQUIRED per constitution - tests must be written before implementation.

**Organization**: Tasks are organized by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- Source code: `source/Node.Net/`
- Tests: `tests/Node.Net.Test/`
- Examples: `examples/Node.Net.AspNet.Host/` and `examples/Node.Net.Wasm/`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [x] T001 Add Microsoft.Extensions.Logging package reference to source/Node.Net/Node.Net.csproj
- [x] T002 [P] Create Service/Logging directory structure in source/Node.Net/
- [x] T003 [P] Create Service/Logging test directory structure in tests/Node.Net.Test/

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [x] T004 [P] Create LogEntry model class in source/Node.Net/Service/Logging/LogEntry.cs with all properties (Id, Timestamp, Level, Message, MessageTemplate, Properties, Exception, SourceContext, IsManualEntry)
- [x] T005 [P] Create ILogService interface in source/Node.Net/Service/Logging/ILogService.cs with Create, GetById, Update, Delete, Search, GetCount methods
- [x] T006 [P] Create LogService implementation in source/Node.Net/Service/Logging/LogService.cs with LiteDB backing store
- [x] T007 [P] Create LogServiceLoggerProvider class in source/Node.Net/Service/Logging/LogServiceLoggerProvider.cs implementing ILoggerProvider and ILogger for Microsoft.Extensions.Logging integration
- [x] T008 [P] Create extension method AddNodeNetLogging in source/Node.Net/Service/Logging/LoggingExtensions.cs for service registration
- [x] T009 [P] Write unit tests for LogEntry in tests/Node.Net.Test/Service/Logging/LogEntry.Tests.cs (validation rules, property access)
- [x] T010 [P] Write unit tests for LogService in tests/Node.Net.Test/Service/Logging/LogService.Tests.cs (CRUD operations, search, pagination, read-only enforcement)
- [x] T011 [P] Write integration tests for LogServiceLoggerProvider in tests/Node.Net.Test/Service/Logging/LogService.Integration.Tests.cs (Microsoft.Extensions.Logging capture, structured data extraction)

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - View Application Logs (Priority: P1) 🎯 MVP

**Goal**: Display log entries in a user interface with chronological ordering (newest first by default), showing timestamp, log level, and message content with clear visual distinction between log levels.

**Independent Test**: Navigate to logs page and verify log entries are displayed in chronological order with newest first, showing timestamp, level, and message for each entry.

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T012 [P] [US1] Write component test for Logs.razor displaying log entries in tests/Node.Net.Test/Components/Logs.Tests.cs (verify entries render, chronological order, newest first default)
- [x] T013 [P] [US1] Write integration test for log entry display in example applications in tests/Node.Net.Test/Components/Logs.Integration.Tests.cs

### Implementation for User Story 1

- [x] T014 [US1] Create Logs.razor component in source/Node.Net/Components/Logs.razor with FluentDataGrid for log entry display
- [x] T015 [US1] Implement LoadLogs() method in Logs.razor to fetch entries from ILogService with newest-first ordering
- [x] T016 [US1] Implement log entry display with timestamp, level, and message columns in Logs.razor
- [x] T017 [US1] Add visual styling to distinguish log levels (color coding, icons) in Logs.razor
- [x] T018 [US1] Implement empty state handling (FR-012) in Logs.razor when no entries exist
- [x] T019 [US1] Add error handling and user feedback (FR-014) in Logs.razor for operation failures

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently - users can view log entries in chronological order

---

## Phase 4: User Story 2 - Search and Filter Logs (Priority: P2)

**Goal**: Enable users to search log entries by content (message text and structured data fields) and filter by log level, with results updating within 1 second.

**Independent Test**: Enter search criteria and verify only matching entries are displayed. Filter by log level and verify only entries with selected level are shown. Clear search/filter and verify all entries displayed again.

### Tests for User Story 2 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T020 [P] [US2] Write component test for search functionality in tests/Node.Net.Test/Components/Logs.Tests.cs (search by message text, search by structured data, case-insensitive)
- [x] T021 [P] [US2] Write component test for log level filtering in tests/Node.Net.Test/Components/Logs.Tests.cs (filter by level, clear filter)
- [x] T022 [P] [US2] Write component test for empty search results state (FR-013) in tests/Node.Net.Test/Components/Logs.Tests.cs

### Implementation for User Story 2

- [x] T023 [US2] Add FluentSearch component to Logs.razor for search input
- [x] T024 [US2] Add FluentSelect component to Logs.razor for log level filter dropdown
- [x] T025 [US2] Implement Search() method in Logs.razor calling ILogService.Search() with search term and level filter
- [x] T026 [US2] Implement real-time search with debouncing (1 second response time per SC-002) in Logs.razor
- [x] T027 [US2] Implement clear search functionality in Logs.razor to reset filters and show all entries
- [x] T028 [US2] Add empty search results message (FR-013) in Logs.razor when search returns no matches

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently - users can view and search/filter log entries

---

## Phase 5: User Story 3 - Manage Log Entries (Priority: P3)

**Goal**: Enable users to create, update (manual entries only), and delete log entries through the user interface, with 100% operation success rate under normal conditions.

**Independent Test**: Create a new log entry and verify it appears in the list. Update a manually created entry and verify changes persist. Attempt to update an automatic entry and verify it's prevented. Delete an entry and verify it's removed.

### Tests for User Story 3 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [x] T029 [P] [US3] Write component test for creating log entry in tests/Node.Net.Test/Components/Logs.Tests.cs (create form, save, verify appears in list)
- [x] T030 [P] [US3] Write component test for updating manual log entry in tests/Node.Net.Test/Components/Logs.Tests.cs (edit form, save, verify changes)
- [x] T031 [P] [US3] Write component test for read-only enforcement in tests/Node.Net.Test/Components/Logs.Tests.cs (automatic entry edit button disabled, exception if attempted)
- [x] T032 [P] [US3] Write component test for deleting log entry in tests/Node.Net.Test/Components/Logs.Tests.cs (delete action, confirmation, verify removal)

### Implementation for User Story 3

- [x] T033 [US3] Add create log entry form/dialog to Logs.razor with FluentButton and input fields
- [x] T034 [US3] Implement CreateLogEntry() method in Logs.razor calling ILogService.Create() with IsManualEntry=true
- [x] T035 [US3] Add edit log entry form/dialog to Logs.razor for manually created entries only
- [x] T036 [US3] Implement UpdateLogEntry() method in Logs.razor calling ILogService.Update() with validation
- [x] T037 [US3] Add delete confirmation dialog to Logs.razor with FluentButton
- [x] T038 [US3] Implement DeleteLogEntry() method in Logs.razor calling ILogService.Delete()
- [x] T039 [US3] Add UI logic to disable edit/delete buttons for read-only entries (IsManualEntry=false) in Logs.razor
- [x] T040 [US3] Add error handling for update attempts on read-only entries in Logs.razor

**Checkpoint**: All user stories should now be independently functional - users can view, search, filter, create, update, and delete log entries

---

## Phase 6: Cross-Cutting Features

**Purpose**: Features that enhance all user stories (pagination, long message handling, sort order toggle)

- [x] T041 [P] Add pagination controls to Logs.razor with FluentDataGrid pagination support (FR-015)
- [x] T042 [P] Implement configurable page size selector (25, 50, 100) in Logs.razor
- [x] T043 [P] Implement sort order toggle (newest first / oldest first) in Logs.razor (FR-003)
- [x] T044 [P] Add long message truncation with expand/collapse functionality in Logs.razor (FR-016)
- [x] T045 [P] Write tests for pagination in tests/Node.Net.Test/Components/Logs.Tests.cs (page navigation, page size change)
- [x] T046 [P] Write tests for sort order toggle in tests/Node.Net.Test/Components/Logs.Tests.cs (newest first, oldest first)
- [x] T047 [P] Write tests for long message truncation in tests/Node.Net.Test/Components/Logs.Tests.cs (truncate, expand, collapse)

---

## Phase 7: Integration & Polish

**Purpose**: Integration with example applications and final polish

- [x] T048 [P] Create Logs.razor page in examples/Node.Net.AspNet.Host/Components/Pages/Logs.razor using Node.Net.Components.Log
- [x] T049 [P] Create Logs.razor page in examples/Node.Net.Wasm/Pages/Logs.razor using Node.Net.Components.Log
- [x] T050 [P] Add navigation links to Logs page in examples/Node.Net.AspNet.Host/Components/Layout/NavMenu.razor
- [x] T051 [P] Add navigation links to Logs page in examples/Node.Net.Wasm/Layout/NavMenu.razor
- [x] T052 [P] Configure Microsoft.Extensions.Logging integration in examples/Node.Net.AspNet.Host/Program.cs using AddNodeNetLogging()
- [x] T053 [P] Configure Microsoft.Extensions.Logging integration in examples/Node.Net.Wasm/Program.cs using AddNodeNetLogging()
- [x] T054 [P] Add XML documentation comments to all public APIs (ILogService, LogEntry, LogService, LogServiceLoggerProvider, Logs component)
- [ ] T055 [P] Verify all tests pass on all target frameworks (net48, net8.0, net8.0-windows)
- [ ] T056 [P] Run quickstart.md validation scenarios to verify integration works correctly
- [ ] T057 [P] Performance testing: Verify page load <2 seconds (SC-001), search <1 second (SC-002), 1000+ entries with pagination (SC-004)
- [ ] T058 [P] Verify Microsoft.Extensions.Logging capture rate >=95% (SC-005) in integration tests

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3-5)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3)
- **Cross-Cutting (Phase 6)**: Can start after User Story 1 (P1) is complete
- **Integration & Polish (Phase 7)**: Depends on all user stories and cross-cutting features being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Depends on US1 for UI structure but independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Depends on US1 for UI structure but independently testable

### Within Each User Story

- Tests (TDD) MUST be written and FAIL before implementation
- Models/Interfaces before implementations
- Service layer before component layer
- Core functionality before enhancements
- Story complete before moving to next priority

### Parallel Opportunities

- All Setup tasks (T001-T003) can run in parallel
- All Foundational tasks marked [P] (T004-T011) can run in parallel
- Once Foundational phase completes, all user story tests can start in parallel
- User Stories 1, 2, and 3 can be worked on in parallel by different developers (after foundational)
- All Cross-Cutting tasks (T041-T047) can run in parallel
- All Integration tasks (T048-T058) can run in parallel

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together:
Task T012: "Write component test for Logs.razor displaying log entries"
Task T013: "Write integration test for log entry display in example applications"

# After tests fail, launch implementation:
Task T014: "Create Logs.razor component"
Task T015: "Implement LoadLogs() method"
Task T016: "Implement log entry display with columns"
Task T017: "Add visual styling to distinguish log levels"
Task T018: "Implement empty state handling"
Task T019: "Add error handling and user feedback"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Add Cross-Cutting Features → Test independently → Deploy/Demo
6. Add Integration & Polish → Final validation → Deploy/Demo
7. Each increment adds value without breaking previous work

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1 (P1) - MVP
   - Developer B: User Story 2 (P2) - Search/Filter
   - Developer C: User Story 3 (P3) - CRUD Operations
3. Once User Story 1 complete:
   - Developer A: Cross-Cutting Features (pagination, sorting, truncation)
   - Developer B: Continue User Story 2
   - Developer C: Continue User Story 3
4. All stories complete → Integration & Polish phase together

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- TDD REQUIRED: Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
- All public APIs must have XML documentation comments
- Tests must pass on all target frameworks (net48, net8.0, net8.0-windows)
- Razor components (Logs.razor) excluded from net48 builds (Fluent UI requires .NET 6+)
