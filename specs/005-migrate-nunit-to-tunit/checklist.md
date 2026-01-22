# Quality Checklist: Migrate Node.Net.Test from NUnit to TUnit

**Feature ID**: 005-migrate-nunit-to-tunit  
**Status**: Draft  
**Created**: 2026-01-22

## Overview

This checklist ensures the migration from NUnit to TUnit meets all quality standards, requirements, and best practices. Use this checklist during and after migration to verify completeness and correctness.

## Requirements Quality Checklist

### Specification Completeness
- [ ] All functional requirements clearly defined
- [ ] All non-functional requirements specified
- [ ] Scope clearly defined (in-scope and out-of-scope)
- [ ] Success criteria measurable and testable
- [ ] Risks identified and mitigation strategies defined
- [ ] Dependencies documented
- [ ] Technical constraints identified

### Clarification Completeness
- [ ] All clarification questions answered
- [ ] Decisions documented with rationale
- [ ] Conversion patterns documented
- [ ] Edge cases identified and handled
- [ ] Special patterns (SetUp/TearDown, TestHarness) addressed

### Plan Completeness
- [ ] Phases clearly defined
- [ ] Tasks broken down to actionable items
- [ ] Dependencies between tasks identified
- [ ] Verification steps included in each phase
- [ ] Risk mitigation strategies included
- [ ] Timeline estimates provided

---

## Pre-Migration Checklist

### Baseline Documentation
- [ ] Current test count documented
- [ ] Current test execution time measured
- [ ] Test discovery verified (`dotnet test --list-tests`)
- [ ] All tests passing with NUnit (100% pass rate)
- [ ] Test file inventory completed (57 files)

### Project Setup
- [ ] TUnit package version researched (latest from NuGet)
- [ ] TUnit API documentation reviewed for:
  - [ ] Time comparison methods
  - [ ] Boolean assertion methods (`IsTrue()`/`IsFalse()`)
  - [ ] Message parameters in assertions
  - [ ] Exception assertion patterns
- [ ] GlobalUsings.cs prepared for TUnit using statements
- [ ] Migration patterns reference created

---

## Phase 0: Preparation Checklist

### Package Management
- [ ] TUnit package added to `Node.Net.Test.csproj` (latest version)
- [ ] NUnit package removed from `Node.Net.Test.csproj`
- [ ] NUnit3TestAdapter package removed from `Node.Net.Test.csproj`
- [ ] Project restores successfully (`dotnet restore`)
- [ ] No package conflicts or warnings

### GlobalUsings Configuration
- [ ] `global using TUnit.Core;` added to `GlobalUsings.cs`
- [ ] `global using TUnit.Assertions;` added to `GlobalUsings.cs`
- [ ] No duplicate using statements
- [ ] GlobalUsings.cs syntax correct

### Baseline Metrics
- [ ] Test count baseline recorded
- [ ] Test execution time baseline recorded
- [ ] Test discovery baseline verified
- [ ] Metrics stored for comparison

### TUnit API Research
- [ ] Time comparison methods researched
- [ ] Boolean assertion methods researched
- [ ] Message parameter support verified
- [ ] Research findings documented

---

## Phase 1: TestHarness Verification Checklist

### File Migration: `Security/UserSecretProvider.Tests.cs`
- [ ] `using NUnit.Framework;` removed
- [ ] `[TestFixture]` attribute removed
- [ ] All test methods converted to `async Task`
- [ ] `[Test]` attributes remain (compatible with TUnit)
- [ ] `[SetUp]` method converted to private instance method
- [ ] `[TearDown]` method converted to private instance method
- [ ] All test methods call `SetUp()` at start
- [ ] All test methods wrap body in `try/finally` block
- [ ] All test methods call `TearDown()` in `finally` block

### Assertion Conversions
- [ ] `Assert.That(value, Is.True)` → `await Assert.That(value).IsTrue()` (✅ TUnit supports IsTrue())
- [ ] `Assert.That(value, Is.False)` → `await Assert.That(value).IsFalse()` (✅ TUnit supports IsFalse())
- [ ] `Assert.That(value, Is.EqualTo(expected))` → `await Assert.That(value).IsEqualTo(expected)`
- [ ] `Assert.That(value, Is.Not.Null)` → `await Assert.That(value).IsNotNull()`
- [ ] `Assert.That(value, Is.Not.Empty)` → `await Assert.That(value).IsNotEmpty()`
- [ ] `Assert.That(value, Does.Contain(expected))` → `await Assert.That(value).Contains(expected)`
- [ ] `Assert.That(value, Is.LessThan(expected))` → `await Assert.That(value).IsLessThan(expected)`
- [ ] `Assert.That(value, Is.GreaterThan(expected))` → `await Assert.That(value).IsGreaterThan(expected)`
- [ ] `Assert.ThrowsAsync<Exception>(...)` → `await Assert.That(async () => await ...).Throws<Exception>()`
- [ ] Compound assertions (`.And`) split into multiple assertions
- [ ] Time-based comparisons (`.Within()`) converted appropriately
- [ ] Type checking (`Is.InstanceOf`) converted to `is` operator
- [ ] All assertions are awaited

### TestHarness Compatibility
- [ ] TestHarness base class methods accessible
- [ ] `GetArtifactsDirectoryInfo()` works correctly
- [ ] TestHarness constructor works with TUnit
- [ ] No compilation errors related to TestHarness

### Build and Test Verification
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Test discovery works (`dotnet test --list-tests`)
- [ ] All tests in file execute
- [ ] All tests in file pass (100% pass rate)
- [ ] Test execution time acceptable

### Documentation
- [ ] Migration patterns documented
- [ ] Any issues or workarounds documented
- [ ] TestHarness compatibility confirmed

---

## Phase 2: Remaining TestHarness Files Checklist

### File: `Service/User/SystemUser.Tests.cs`
- [ ] All NUnit patterns converted to TUnit
- [ ] `Assert.Fail()` converted to TUnit equivalent
- [ ] File I/O operations handled (note: `File.ReadAllBytes`/`WriteAllBytes` don't have async versions)
- [ ] TestHarness methods work correctly
- [ ] All tests pass
- [ ] Build successful

### File: `Service/Logging/LogService.Tests.cs`
- [ ] All NUnit patterns converted to TUnit
- [ ] SetUp/TearDown converted (if present)
- [ ] TestHarness methods work correctly
- [ ] All tests pass
- [ ] Build successful

### Verification
- [ ] All TestHarness-based files migrated
- [ ] All TestHarness-based tests pass
- [ ] No TestHarness-related issues

---

## Phase 3: SetUp/TearDown Files Checklist

### File: `Service/Logging/LogService.Integration.Tests.cs`
- [ ] `[SetUp]` attribute removed
- [ ] `[TearDown]` attribute removed
- [ ] SetUp/TearDown converted to private instance methods
- [ ] All test methods updated with try/finally pattern
- [ ] All assertions converted
- [ ] All tests pass
- [ ] Cleanup verified (resources disposed correctly)

### File: `Service/FileSystem/LiteDbFileSystem.Tests.cs`
- [ ] `[SetUp]` attribute removed
- [ ] `[TearDown]` attribute removed
- [ ] SetUp/TearDown converted to private instance methods
- [ ] All test methods updated with try/finally pattern
- [ ] All assertions converted
- [ ] All tests pass
- [ ] Cleanup verified (database files deleted correctly)

### Verification
- [ ] All SetUp/TearDown files migrated
- [ ] All SetUp/TearDown tests pass
- [ ] Resource cleanup works correctly
- [ ] No resource leaks

---

## Phase 4: Remaining Files Checklist

### Service Tests
- [ ] `Service/Application/ApplicationInfo.Tests.cs` migrated
- [ ] `Service/Application/Application.Tests.cs` migrated
- [ ] `Service/Logging/LogEntry.Tests.cs` migrated
- [ ] `Service/WebServer.Test.cs` migrated
  - [ ] `[Explicit]` attribute handled (research TUnit support)
  - [ ] `[Category]` attribute handled (research TUnit support)
- [ ] All Service tests pass

### System.Windows Tests (20 files)
- [ ] All files in `System/Windows/` migrated
- [ ] All assertions converted
- [ ] All tests pass
- [ ] Framework-specific tests work correctly

### Extension Tests (9 files)
- [ ] All files in `Extension/` migrated
- [ ] All assertions converted
- [ ] All tests pass

### Collection Tests (4 files)
- [ ] All files in `Collections/` migrated
- [ ] All assertions converted
- [ ] All tests pass

### JsonRPC Tests (3 files)
- [ ] All files in `JsonRPC/` migrated
- [ ] All assertions converted
- [ ] All tests pass

### Root Level Tests
- [ ] `Log.Test.cs` migrated
- [ ] `Reader.Test.cs` migrated
  - [ ] `[TestCase]` converted to `[Arguments]`
  - [ ] Parameterized test verified
- [ ] `Writer.Test.cs` migrated
  - [ ] Static class/methods handled (verify TUnit support or convert to instance)
- [ ] `Formatter.Test.cs` migrated
- [ ] `Factory.Test.cs` migrated
- [ ] `Dictionary.Test.cs` migrated
- [ ] `ConvertTest.cs` migrated
- [ ] `ConvertRotationsXYZtoOTS.Comprehensive.Tests.cs` migrated
- [ ] `Matrix3DFactory.Tests.cs` migrated
- [ ] `DelegateCommand.Test.cs` migrated
- [ ] `Converters/HiddenWhenNull.Test.cs` migrated
- [ ] `View/SDIView.Test.cs` migrated
- [ ] All root level tests pass

### Per-File Verification (for each file)
- [ ] `using NUnit.Framework;` removed (if present)
- [ ] `[TestFixture]` attribute removed
- [ ] All test methods are `async Task`
- [ ] All assertions converted and awaited
- [ ] Assertion messages preserved (if TUnit supports)
- [ ] Special patterns handled:
  - [ ] `[TestCase]` → `[Arguments]` (if present)
  - [ ] `[Explicit]` handled (if present)
  - [ ] `[Category]` handled (if present)
  - [ ] Static methods handled (if present)
  - [ ] `Assert.Fail()` converted (if present)
- [ ] File builds without errors
- [ ] File builds without warnings
- [ ] All tests in file pass

---

## Phase 5: Final Verification Checklist

### Test Discovery
- [ ] `dotnet test --list-tests` shows all tests
- [ ] Test count matches baseline (or documented difference explained)
- [ ] No test discovery errors
- [ ] All test classes discoverable

### Test Execution
- [ ] `dotnet test` executes all tests
- [ ] All tests pass (100% pass rate)
- [ ] No test failures
- [ ] No test timeouts
- [ ] Test execution completes successfully

### Performance Verification
- [ ] Test execution time measured
- [ ] Performance compared to baseline
- [ ] Performance maintained or improved
- [ ] Performance metrics documented

### Code Quality
- [ ] No compilation errors
- [ ] No compilation warnings
- [ ] Code follows conversion patterns consistently
- [ ] No NUnit references remaining in code
- [ ] All using statements correct
- [ ] Code readability maintained

### Cleanup
- [ ] All NUnit packages removed
- [ ] All NUnit using statements removed
- [ ] All `[TestFixture]` attributes removed
- [ ] All NUnit-specific code removed
- [ ] No NUnit references in project file
- [ ] No NUnit references in code files

### Documentation
- [ ] Migration patterns documented
- [ ] Any issues or workarounds documented
- [ ] Performance metrics documented
- [ ] Test count changes documented (if any)

### CI/CD Verification
- [ ] CI/CD pipeline still works
- [ ] Test execution in CI/CD passes
- [ ] No CI/CD configuration changes needed

---

## Assertion Conversion Checklist

### Basic Assertions
- [ ] `Assert.That(value, Is.True)` converted
- [ ] `Assert.That(value, Is.False)` converted
- [ ] `Assert.That(value, Is.EqualTo(expected))` converted
- [ ] `Assert.That(value, Is.Not.EqualTo(expected))` converted
- [ ] `Assert.That(value, Is.Not.Null)` converted
- [ ] `Assert.That(value, Is.Null)` converted
- [ ] `Assert.That(value, Is.Not.Empty)` converted
- [ ] `Assert.That(value, Is.Empty)` converted
- [ ] `Assert.That(value, Does.Contain(expected))` converted
- [ ] `Assert.That(value, Does.Not.Contain(expected))` converted
- [ ] `Assert.That(value, Is.LessThan(expected))` converted
- [ ] `Assert.That(value, Is.GreaterThan(expected))` converted
- [ ] `Assert.That(value, Is.LessThanOrEqualTo(expected))` converted
- [ ] `Assert.That(value, Is.GreaterThanOrEqualTo(expected))` converted

### Exception Assertions
- [ ] `Assert.Throws<Exception>(() => ...)` converted
- [ ] `Assert.ThrowsAsync<Exception>(async () => ...)` converted
- [ ] Exception type checking works correctly

### Special Patterns
- [ ] Compound assertions (`.And`) split into multiple assertions
- [ ] Time-based comparisons (`.Within()`) converted (research TUnit support first)
- [ ] Type checking (`Is.InstanceOf`) converted
- [ ] Byte array comparisons use `SequenceEqual` (if any)
- [ ] Assertion messages preserved (research TUnit message parameter syntax)
- [ ] `Assert.Fail()` converted to TUnit equivalent (research needed)
- [ ] `[TestCase]` converted to `[Arguments]` (Reader.Test.cs)
- [ ] `[Explicit]` attribute handled (WebServer.Test.cs - research TUnit support)
- [ ] `[Category]` attribute handled (WebServer.Test.cs - research TUnit support)
- [ ] Static test methods handled (Writer.Test.cs - verify TUnit support)

### Async Patterns
- [ ] All assertions are awaited
- [ ] No missing `await` keywords
- [ ] Async test methods work correctly

---

## SetUp/TearDown Conversion Checklist

### Pattern Conversion
- [ ] `[SetUp]` attributes removed
- [ ] `[TearDown]` attributes removed
- [ ] SetUp methods converted to private instance methods
- [ ] TearDown methods converted to private instance methods
- [ ] All test methods call `SetUp()` at start
- [ ] All test methods wrap body in `try/finally`
- [ ] All test methods call `TearDown()` in `finally` block

### Functionality Verification
- [ ] Initialization works correctly
- [ ] Cleanup works correctly
- [ ] Resources disposed properly
- [ ] No resource leaks
- [ ] Test isolation maintained

---

## TestHarness Compatibility Checklist

### Base Class Functionality
- [ ] TestHarness constructor works
- [ ] `GetArtifactsDirectoryInfo()` works
- [ ] All TestHarness methods accessible
- [ ] No compilation errors with TestHarness
- [ ] TestHarness works with TUnit test discovery

### Test Execution
- [ ] Tests using TestHarness execute correctly
- [ ] TestHarness initialization works
- [ ] TestHarness cleanup works (if any)
- [ ] All TestHarness-based tests pass

---

## Code Quality Checklist

### Consistency
- [ ] Conversion patterns applied consistently
- [ ] Naming conventions maintained
- [ ] Code style consistent
- [ ] No mixed patterns (NUnit and TUnit)

### Readability
- [ ] Code remains readable
- [ ] Test intent clear
- [ ] Assertions clear and understandable
- [ ] Comments updated if needed

### Maintainability
- [ ] Code structure maintained
- [ ] Test organization maintained
- [ ] No unnecessary complexity introduced
- [ ] Patterns documented for future reference

---

## Success Criteria Verification

- [ ] ✅ All 57 test files migrated from NUnit to TUnit
- [ ] ✅ All tests pass (100% pass rate)
- [ ] ✅ Test execution time maintained or improved
- [ ] ✅ No compilation errors or warnings
- [ ] ✅ Test discovery works correctly
- [ ] ✅ Code quality maintained
- [ ] ✅ No NUnit references remaining
- [ ] ✅ TestHarness compatibility verified

---

## Final Sign-Off

### Migration Complete
- [ ] All phases completed
- [ ] All checklists verified
- [ ] All success criteria met
- [ ] Documentation complete
- [ ] Ready for code review

### Code Review
- [ ] Code reviewed by team
- [ ] Review feedback addressed
- [ ] Approval received

### Merge Ready
- [ ] All tests passing
- [ ] CI/CD passing
- [ ] Documentation updated
- [ ] Ready to merge

---

## Notes

- Use this checklist during migration to track progress
- Check off items as they are completed
- Document any deviations or issues
- Update checklist if patterns or requirements change
- Keep checklist updated with actual findings
