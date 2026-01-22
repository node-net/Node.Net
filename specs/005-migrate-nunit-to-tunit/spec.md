# Feature Specification: Migrate Node.Net.Test from NUnit to TUnit

**Feature ID**: 005-migrate-nunit-to-tunit  
**Status**: Draft  
**Created**: 2026-01-22  
**Target Project**: `tests/Node.Net.Test`

## Overview

Migrate all test files in `tests/Node.Net.Test` from NUnit to TUnit testing framework, following the migration guide documented in `docs/dotnet/migrate_nunit_to_tunit.md`.

## Background

The project has already successfully migrated `tests/Node.Net.Components.Test` from NUnit to TUnit. This migration will standardize the test framework across all test projects, providing:
- Improved test execution performance (~57% improvement observed)
- Modern async-first design
- Better parallel test execution
- Consistent testing framework across the codebase

## Scope

### In Scope

- Migrate all test files in `tests/Node.Net.Test` from NUnit to TUnit
- Update project file to replace NUnit packages with TUnit
- Convert all test methods to async Task pattern
- Update all assertions to TUnit syntax
- Handle SetUp/TearDown lifecycle methods
- Convert TestCase to Arguments attributes
- Update using statements
- Ensure all tests pass after migration

### Out of Scope

- Migrating other test projects (already done for Node.Net.Components.Test)
- Changing test logic or test coverage
- Modifying the code under test
- Performance optimization beyond framework benefits

## Functional Requirements

### FR-001: Package Migration
- **Priority**: Critical
- **Description**: Replace NUnit packages with TUnit in `tests/Node.Net.Test/Node.Net.Test.csproj`
- **Acceptance Criteria**:
  - Remove `NUnit` and `NUnit3TestAdapter` package references
  - Add `TUnit` package reference (latest version from NuGet, no version constraint)
  - **Note**: `Node.Net.Components.Test` uses TUnit 1.6.20, but we'll use latest for this migration
  - Project builds successfully with TUnit

### FR-002: Using Statement Updates
- **Priority**: Critical
- **Description**: Update all using statements in test files
- **Acceptance Criteria**:
  - Remove `using NUnit.Framework;` from all test files
  - Add `using TUnit.Core;` and `using TUnit.Assertions;` to all test files
  - No compilation errors related to missing namespaces

### FR-003: Test Class Attributes
- **Priority**: Critical
- **Description**: Remove `[TestFixture]` attributes from all test classes
- **Acceptance Criteria**:
  - All `[TestFixture]` attributes removed
  - Test classes still discoverable by TUnit
  - No test discovery failures

### FR-004: Test Method Conversion
- **Priority**: Critical
- **Description**: Convert all test methods to async Task pattern
- **Acceptance Criteria**:
  - All test methods return `async Task` instead of `void`
  - `[Test]` attributes remain (compatible with TUnit)
  - All tests still discoverable and executable

### FR-005: Assertion Syntax Migration
- **Priority**: Critical
- **Description**: Convert all NUnit assertions to TUnit syntax
- **Acceptance Criteria**:
  - All `Assert.That` statements use TUnit fluent syntax
  - All assertions are awaited
  - Common patterns converted:
    - `Assert.That(value, Is.True)` → `await Assert.That(value).IsEqualTo(true)`
    - `Assert.That(value, Is.EqualTo(expected))` → `await Assert.That(value).IsEqualTo(expected)`
    - `Assert.That(value, Is.Not.Null)` → `await Assert.That(value).IsNotNull()`
    - `Assert.That(value, Is.Not.Empty)` → `await Assert.That(value).IsNotEmpty()`
    - `Assert.That(value, Does.Contain(expected))` → `await Assert.That(value).Contains(expected)`
    - `Assert.That(value, Is.LessThan(expected))` → `await Assert.That(value).IsLessThan(expected)`
    - `Assert.That(value, Is.GreaterThan(expected))` → `await Assert.That(value).IsGreaterThan(expected)`

### FR-006: Exception Assertions
- **Priority**: High
- **Description**: Convert exception assertions to TUnit syntax
- **Acceptance Criteria**:
  - `Assert.Throws<Exception>(() => ...)` → `await Assert.That(() => ...).Throws<Exception>()`
  - `Assert.ThrowsAsync<Exception>(async () => ...)` → `await Assert.That(async () => ...).Throws<Exception>()`
  - All exception tests pass

### FR-007: Lifecycle Methods
- **Priority**: High
- **Description**: Handle SetUp/TearDown methods (TUnit doesn't have equivalent attributes)
- **Acceptance Criteria**:
  - `[SetUp]` methods converted to manual calls at start of each test
  - `[TearDown]` methods converted to finally blocks in each test
  - **Note**: `[OneTimeSetUp]` and `[OneTimeTearDown]` are not present in codebase (N/A)
  - All test initialization and cleanup works correctly

### FR-008: Parameterized Tests
- **Priority**: Medium
- **Description**: Convert `[TestCase]` to `[Arguments]` attributes
- **Acceptance Criteria**:
  - All `[TestCase]` attributes converted to `[Arguments]` (found in `Reader.Test.cs`)
  - All parameterized tests still execute with all test cases
  - Test discovery includes all parameter combinations

### FR-009: Byte Array Comparisons
- **Priority**: Medium
- **Description**: Handle byte array comparisons (TUnit compares references, not content)
- **Acceptance Criteria**:
  - Byte array comparisons use `SequenceEqual` pattern
  - `Assert.That(bytes1, Is.EqualTo(bytes2))` → `await Assert.That(bytes1.SequenceEqual(bytes2)).IsEqualTo(true)`
  - All byte array tests pass

### FR-010: File I/O Operations
- **Priority**: Low
- **Description**: Convert synchronous file I/O to async where applicable
- **Acceptance Criteria**:
  - `File.ReadAllText` → `await File.ReadAllTextAsync` where possible
  - `File.WriteAllText` → `await File.WriteAllTextAsync` where possible
  - **Note**: `File.ReadAllBytes` and `File.WriteAllBytes` do NOT have async versions in .NET - keep as synchronous
  - Tests maintain same behavior

### FR-011: Test Discovery and Execution
- **Priority**: Critical
- **Description**: Ensure all tests are discoverable and executable
- **Acceptance Criteria**:
  - `dotnet test --list-tests` shows all test methods
  - All tests execute successfully
  - Test count matches pre-migration count
  - All tests pass

### FR-012: Base Class Compatibility
- **Priority**: High
- **Description**: Ensure TestHarness base classes work with TUnit
- **Acceptance Criteria**:
  - Test classes inheriting from TestHarness work correctly
  - Base class methods accessible and functional
  - No breaking changes to base class API

## Non-Functional Requirements

### NFR-001: Performance
- **Description**: Test execution should be faster or equivalent to NUnit
- **Target**: Maintain or improve test execution time
- **Measurement**: Compare test execution time before and after migration

### NFR-002: Compatibility
- **Description**: All existing tests must pass after migration
- **Target**: 100% test pass rate
- **Measurement**: Run full test suite and verify all tests pass

### NFR-003: Code Quality
- **Description**: Maintain code quality and readability
- **Target**: No degradation in code quality
- **Measurement**: Code reviews and static analysis

## Technical Constraints

- Must use TUnit latest version from NuGet (no version constraint)
- **Note**: `Node.Net.Components.Test` uses TUnit 1.6.20, but we'll use latest for consistency
- Must maintain .NET 8.0 target framework
- Must preserve all existing test logic
- Must maintain compatibility with TestHarness base classes
- Must work with existing test infrastructure (Rakefile, CI/CD)
- `global.json` already has test runner configured (`Microsoft.Testing.Platform`)

## Dependencies

- TUnit package (NuGet)
- Microsoft.NET.Test.Sdk (already present)
- Migration guide: `docs/dotnet/migrate_nunit_to_tunit.md`

## Risks and Mitigations

### Risk 1: Test Discovery Failures
- **Impact**: High
- **Probability**: Medium
- **Mitigation**: Follow migration guide exactly, verify test discovery after each phase

### Risk 2: Assertion Syntax Errors
- **Impact**: Medium
- **Probability**: High
- **Mitigation**: Use systematic conversion patterns, test incrementally

### Risk 3: Lifecycle Method Issues
- **Impact**: Medium
- **Probability**: Medium
- **Mitigation**: Test SetUp/TearDown conversion carefully, verify cleanup works

### Risk 4: Performance Regression
- **Impact**: Low
- **Probability**: Low
- **Mitigation**: Measure performance before/after, TUnit typically improves performance

## Success Criteria

1. All test files migrated from NUnit to TUnit
2. All tests pass (100% pass rate)
3. Test execution time maintained or improved
4. No compilation errors or warnings
5. Test discovery works correctly
6. Code quality maintained

## References

- Migration Guide: `docs/dotnet/migrate_nunit_to_tunit.md`
- TUnit GitHub: https://github.com/thomhurst/TUnit
- TUnit NuGet: https://www.nuget.org/packages/TUnit
- Previous Migration: `tests/Node.Net.Components.Test` (completed)
