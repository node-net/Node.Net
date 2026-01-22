# Tasks: Refactor to Nested Source Structure

**Input**: Design documents from `/specs/002-nested-source-structure/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md

**Tests**: Tests are NOT requested for this structural refactoring. Validation will be done through build and test execution verification.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Nested structure**: `source/Node.Net/`, `source/Node.Net.Test/` at repository root (after refactoring)
- Current paths: `source/` (flat), `tests/` (flat) (to be nested)

---

## Phase 1: User Story 1 - Reorganize Source Code into Nested Directory (Priority: P1) 🎯 MVP

**Goal**: Relocate all source code files from `source/` directory to `source/Node.Net/` directory using git mv to preserve history

**Independent Test**: Verify that all source code files are accessible in the new `source/Node.Net/` location and the project builds successfully from the new location

### Implementation for User Story 1

- [ ] T001 [US1] Create source/Node.Net/ directory
- [ ] T002 [US1] Use git mv to relocate all files from source/ to source/Node.Net/ preserving git history (excluding Node.Net subdirectory itself)
- [ ] T003 [US1] Verify all source files are in source/Node.Net/ directory (Node.Net.csproj, Collections/, Converters/, Extension/, Internal/, JsonRPC/, Service/, System/, View/, etc.)
- [ ] T004 [US1] Verify no source files remain directly in source/ directory (only Node.Net subdirectory should exist)
- [ ] T005 [US1] Verify git history is preserved for all moved files (check git log)

**Checkpoint**: At this point, all source code should be in `source/Node.Net/` directory with preserved git history. Project may not build yet until references are updated.

---

## Phase 2: User Story 2 - Reorganize Test Code into Nested Directory (Priority: P1)

**Goal**: Relocate all test code files from `tests/` directory to `source/Node.Net.Test/` directory using git mv to preserve history

**Independent Test**: Verify that all test files are accessible in the new `source/Node.Net.Test/` location and all tests execute successfully from the new location

### Implementation for User Story 2

- [ ] T006 [US2] Create source/Node.Net.Test/ directory
- [ ] T007 [US2] Use git mv to relocate all files from tests/ to source/Node.Net.Test/ preserving git history
- [ ] T008 [US2] Verify all test files are in source/Node.Net.Test/ directory (Node.Net.Test.csproj, Collections/, Converters/, Extension/, JsonRPC/, Resources/, Service/, System/, View/, etc.)
- [ ] T009 [US2] Verify no test files remain in old tests/ directory
- [ ] T010 [US2] Verify git history is preserved for all moved test files (check git log)

**Checkpoint**: At this point, all test code should be in `source/Node.Net.Test/` directory with preserved git history. Tests may not execute yet until references are updated.

---

## Phase 3: User Story 3 - Update Project References and Configuration (Priority: P2)

**Goal**: Update all project references, solution files, build scripts, and configuration files to point to the new nested directory structure

**Independent Test**: Verify that the solution file opens correctly, all project references resolve, and the build system can locate all files without errors

### Implementation for User Story 3

- [ ] T011 [US3] Update solution file Node.Net.sln to reference source/Node.Net/Node.Net.csproj instead of source/Node.Net.csproj
- [ ] T012 [US3] Update solution file Node.Net.sln to reference source/Node.Net.Test/Node.Net.Test.csproj instead of tests/Node.Net.Test.csproj
- [ ] T013 [US3] Update ProjectReference in source/Node.Net.Test/Node.Net.Test.csproj from `..\source\Node.Net.csproj` to `..\Node.Net\Node.Net.csproj` (sibling directories)
- [ ] T014 [US3] Update build script Rakefile to replace `source/` paths with `source/Node.Net/` paths
- [ ] T015 [US3] Update build script Rakefile to replace `tests/` paths with `source/Node.Net.Test/` paths
- [ ] T016 [US3] Check for and update any GitHub Actions workflow files (.github/workflows/*.yml) that reference old paths
- [ ] T017 [US3] Search and update README.md for any references to old directory structure
- [ ] T018 [US3] Search and update any other documentation files that reference old paths (docs/, *.md files in root)

**Checkpoint**: At this point, all references should point to the new nested directory structure. Project should build and tests should execute.

---

## Phase 4: Validation & Polish

**Purpose**: Final validation and cleanup

- [ ] T019 Verify project builds successfully with zero build errors: `dotnet build Node.Net.sln`
- [ ] T020 Verify all tests execute successfully with zero test failures: `dotnet test source/Node.Net.Test/Node.Net.Test.csproj`
- [ ] T021 Verify solution file opens and loads all projects without errors
- [ ] T022 Verify all project references resolve correctly (check for broken references)
- [ ] T023 Verify no files remain directly in source/ directory (only Node.Net and Node.Net.Test subdirectories)
- [ ] T024 Verify no files remain in old tests/ directory (should be empty or removed)
- [ ] T025 [P] Remove empty tests/ directory if it exists
- [ ] T026 Verify git status shows all changes are staged/committed appropriately

---

## Dependencies & Execution Order

### Phase Dependencies

- **User Story 1 (Phase 1)**: No dependencies - can start immediately
- **User Story 2 (Phase 2)**: Can start after User Story 1 completes (independent but follows same pattern)
- **User Story 3 (Phase 3)**: **MUST** complete after User Stories 1 and 2 - depends on directories being moved first
- **Validation (Phase 4)**: Depends on all user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: No dependencies - foundation of nested refactoring
- **User Story 2 (P1)**: Independent of US1 but should follow same pattern. Can be done in parallel with US1 if desired, but sequential is safer.
- **User Story 3 (P2)**: **CRITICAL DEPENDENCY** - Must complete after US1 and US2. All reference updates depend on nested directories being created first.

### Within Each User Story

- Directory creation must complete before file moves
- File moves must complete before verification tasks
- Reference updates must complete before validation tasks
- Build verification must pass before test execution

### Parallel Opportunities

- T025 (removing empty tests/ directory) can run independently [P]
- Documentation search tasks (T017, T018) could be done in parallel if multiple files exist [P]
- CI/CD check (T016) can be done independently [P]

---

## Implementation Strategy

### Sequential Approach (Recommended)

1. Complete Phase 1: Move source code to nested directory
2. Complete Phase 2: Move test code to nested directory  
3. Complete Phase 3: Update all references
4. Complete Phase 4: Validate and polish
5. **STOP and VALIDATE**: Verify build and tests pass

### Parallel Approach (If Multiple Developers)

1. Developer A: User Story 1 (move source to nested)
2. Developer B: User Story 2 (move tests to nested) - can start after US1 pattern is established
3. Developer C: Prepare reference updates (can research while moves happen)
4. Once US1 and US2 complete: Developer C updates all references
5. Team validates together

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Use `git mv` to preserve file history (per research.md)
- Commit after each phase completion for safety
- Stop at any checkpoint to validate before proceeding
- Avoid: updating references before nested directories are created, breaking build during transition
- Note: Test project reference changes from `..\source\Node.Net.csproj` to `..\Node.Net\Node.Net.csproj` (sibling directories under source/)
