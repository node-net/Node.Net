# Implementation Plan: Migrate Node.Net.Test from NUnit to TUnit

**Feature ID**: 005-migrate-nunit-to-tunit  
**Status**: Draft  
**Created**: 2026-01-22  
**Strategy**: Incremental file-by-file migration

## Overview

This plan outlines the step-by-step process to migrate all test files in `tests/Node.Net.Test` from NUnit to TUnit, following the decisions made during clarification.

## Migration Statistics

- **Total Test Files**: 57 files
- **Total Test Methods**: ~655 test methods (estimated from `[Test]` attribute count)
- **Files with SetUp/TearDown**: 4 files
- **Files with TestHarness**: 3 files
- **Target Frameworks**: net8.0, net8.0-windows

## Migration Strategy

**Approach**: Incremental file-by-file migration
- Migrate one file at a time
- Build and verify after each file
- Run tests for migrated file
- Ensure all tests pass before proceeding
- Verify TestHarness compatibility early

## Phases

### Phase 0: Preparation and Setup
**Goal**: Prepare the project for migration, verify compatibility, and establish baseline

**Tasks**:
1. Document current test count and execution time (baseline)
2. Add TUnit package to project (latest version from NuGet, no version constraint)
3. Remove NUnit packages
4. Add TUnit using statements to GlobalUsings.cs
5. Verify project builds (will fail until files are migrated)
6. Research TUnit documentation for:
   - Time comparisons (`.Within()` equivalent)
   - Boolean assertions (`IsTrue()`/`IsFalse()` - ✅ confirmed supported)
   - `Assert.Fail()` equivalent
   - `[Explicit]` attribute support
   - `[Category]` attribute support
   - Static test method support
   - Assertion message parameters
7. Note: `global.json` already has test runner configured

**Deliverables**:
- Project file updated with TUnit
- GlobalUsings.cs updated
- Baseline metrics documented
- TUnit API research completed

**Verification**:
- Project file has TUnit package reference
- GlobalUsings.cs has TUnit using statements
- Baseline test count and execution time recorded

---

### Phase 1: TestHarness Compatibility Verification
**Goal**: Verify TestHarness base class works with TUnit by migrating one TestHarness-based file

**Rationale**: Early verification prevents issues later, especially since 3 test classes depend on TestHarness.

**Selected File**: `Security/UserSecretProvider.Tests.cs`
- Uses TestHarness base class
- Has SetUp/TearDown methods
- Contains complex assertions (compound, time-based, type checking)
- Good representative sample

**Tasks**:
1. Migrate `Security/UserSecretProvider.Tests.cs`:
   - Remove `using NUnit.Framework;`
   - Remove `[TestFixture]` attribute
   - Convert test methods to `async Task`
   - Convert SetUp/TearDown to manual calls in try/finally
   - Convert all assertions to TUnit syntax
   - Handle special cases:
     - `Assert.ThrowsAsync` → `await Assert.That(async () => await ...).Throws<Exception>()`
     - Compound assertions (`.And`) → split into multiple assertions
     - Time-based comparisons (`.Within()`) → check TUnit docs, then manual comparison
     - Type checking (`Is.InstanceOf`) → use `is` operator
     - Boolean assertions → check TUnit docs for `IsTrue()`/`IsFalse()`
2. Build project
3. Run tests for this file
4. Verify all tests pass
5. Document any issues or patterns discovered

**Deliverables**:
- `Security/UserSecretProvider.Tests.cs` fully migrated
- TestHarness compatibility verified
- Migration patterns documented

**Verification**:
- File builds without errors
- All tests in file pass
- TestHarness methods work correctly

---

### Phase 2: Migrate Remaining TestHarness Files
**Goal**: Migrate the remaining 2 TestHarness-based files

**Files**:
1. `Service/User/SystemUser.Tests.cs`
2. `Service/Logging/LogService.Tests.cs` (also has SetUp/TearDown)

**Tasks** (for each file):
1. Migrate file following patterns from Phase 1
2. Build project
3. Run tests for migrated file
4. Verify all tests pass

**Deliverables**:
- All TestHarness-based files migrated
- All tests passing

**Verification**:
- All TestHarness files build and pass tests

---

### Phase 3: Migrate Files with SetUp/TearDown
**Goal**: Migrate files that use SetUp/TearDown lifecycle methods

**Files**:
1. `Service/Logging/LogService.Integration.Tests.cs` (already migrated in Phase 2 if it uses TestHarness)
2. `Service/FileSystem/LiteDbFileSystem.Tests.cs`

**Tasks** (for each file):
1. Convert `[SetUp]` method to instance method
2. Convert `[TearDown]` method to instance method
3. Update each test method:
   - Add `SetUp();` call at start
   - Wrap test body in `try/finally` block
   - Add `TearDown();` call in `finally` block
4. Remove `[SetUp]` and `[TearDown]` attributes
5. Convert all other NUnit patterns to TUnit
6. Build and test

**Deliverables**:
- All SetUp/TearDown files migrated
- All tests passing

**Verification**:
- All SetUp/TearDown files build and pass tests
- Cleanup happens correctly in all tests

---

### Phase 4: Migrate Remaining Test Files
**Goal**: Migrate all remaining test files

**Strategy**: Migrate by category for better organization:
1. **Service Tests** (remaining):
   - `Service/Application/ApplicationInfo.Tests.cs`
   - `Service/Application/Application.Tests.cs`
   - `Service/Logging/LogEntry.Tests.cs`
   - `Service/WebServer.Test.cs`
2. **System.Windows Tests** (20 files):
   - All files in `System/Windows/` directory
3. **Extension Tests** (9 files):
   - All files in `Extension/` directory
4. **Collection Tests** (4 files):
   - All files in `Collections/` directory
5. **JsonRPC Tests** (3 files):
   - All files in `JsonRPC/` directory
6. **Root Level Tests** (remaining):
   - `Log.Test.cs`
   - `Reader.Test.cs`
   - `Writer.Test.cs`
   - `Formatter.Test.cs`
   - `Factory.Test.cs`
   - `Dictionary.Test.cs`
   - `ConvertTest.cs`
   - `ConvertRotationsXYZtoOTS.Comprehensive.Tests.cs`
   - `Matrix3DFactory.Tests.cs`
   - `DelegateCommand.Test.cs`
   - `Converters/HiddenWhenNull.Test.cs`
   - `View/SDIView.Test.cs`

**Tasks** (for each file):
1. Remove `using NUnit.Framework;` (if present, may be covered by GlobalUsings)
2. Remove `[TestFixture]` attribute
3. Convert test methods to `async Task`
4. Convert all assertions to TUnit syntax
5. Handle special cases as needed
6. Build and test

**Deliverables**:
- All test files migrated
- All tests passing

**Verification**:
- All files build without errors
- All tests pass
- Test count matches baseline

---

### Phase 5: Final Verification and Cleanup
**Goal**: Verify complete migration, measure performance, and clean up

**Tasks**:
1. Run full test suite
2. Verify test count matches baseline
3. Measure test execution time
4. Compare with baseline (should be faster or equivalent)
5. Remove any remaining NUnit references
6. Update documentation if needed
7. Verify CI/CD still works

**Deliverables**:
- All tests passing (100% pass rate)
- Performance metrics documented
- No NUnit references remaining
- Documentation updated

**Verification**:
- `dotnet test --list-tests` shows all tests
- `dotnet test` shows 100% pass rate
- Test execution time improved or maintained
- No compilation errors or warnings

---

## Conversion Patterns Reference

### Basic Assertions
```csharp
// NUnit → TUnit
Assert.That(value, Is.True) → await Assert.That(value).IsTrue() // ✅ Confirmed: TUnit supports IsTrue()
Assert.That(value, Is.False) → await Assert.That(value).IsFalse() // ✅ Confirmed: TUnit supports IsFalse()
Assert.That(value, Is.EqualTo(expected)) → await Assert.That(value).IsEqualTo(expected)
Assert.That(value, Is.Not.Null) → await Assert.That(value).IsNotNull()
Assert.That(value, Is.Not.Empty) → await Assert.That(value).IsNotEmpty()
Assert.That(value, Does.Contain(expected)) → await Assert.That(value).Contains(expected)
Assert.That(value, Is.LessThan(expected)) → await Assert.That(value).IsLessThan(expected)
Assert.That(value, Is.GreaterThan(expected)) → await Assert.That(value).IsGreaterThan(expected)
Assert.That(value, Is.LessThanOrEqualTo(expected)) → await Assert.That(value).IsLessThanOrEqualTo(expected)
Assert.That(value, Is.GreaterThanOrEqualTo(expected)) → await Assert.That(value).IsGreaterThanOrEqualTo(expected)
```

### Exception Assertions
```csharp
// NUnit → TUnit
Assert.Throws<Exception>(() => ...) → await Assert.That(() => ...).Throws<Exception>()
Assert.ThrowsAsync<Exception>(async () => await ...) → await Assert.That(async () => await ...).Throws<Exception>()
```

### Assert.Fail() Pattern
```csharp
// NUnit → TUnit
Assert.Fail("message") → throw new AssertionException("message") // or TUnit equivalent (research needed)
// Alternative: Use Assert.That with descriptive message if TUnit supports it
```

### Compound Assertions
```csharp
// NUnit → TUnit
Assert.That(value, Is.Not.Null.And.Not.Empty) → 
    await Assert.That(value).IsNotNull();
    await Assert.That(value).IsNotEmpty();
```

### Type Checking
```csharp
// NUnit → TUnit
Assert.That(value, Is.InstanceOf<Type>()) → await Assert.That(value is Type).IsTrue()
```

### Time-Based Comparisons
```csharp
// NUnit → TUnit (if no built-in support)
Assert.That(value, Is.EqualTo(expected).Within(TimeSpan)) → 
    await Assert.That(Math.Abs((value - expected).TotalSeconds) <= tolerance).IsTrue()
```

### [TestCase] to [Arguments] Conversion
```csharp
// NUnit → TUnit
[TestCase("value1")] → [Arguments("value1")]
[TestCase("value1", "value2", 3)] → [Arguments("value1", "value2", 3)]
```

### [Explicit] and [Category] Attributes
```csharp
// NUnit → TUnit
[Test, Explicit] → Research TUnit support, if not supported, test will run by default
[TestFixture, Category("name")] → Research TUnit category/tag support, if not supported, use test filtering
```

### Static Test Methods
```csharp
// NUnit → TUnit
internal static class TestClass {
    [Test]
    public static void TestMethod() { }
}

// If TUnit doesn't support static:
internal class TestClass {  // Remove static
    [Test]
    public async Task TestMethod() { }  // Remove static, add async Task
}
```

### SetUp/TearDown Pattern
```csharp
// Before (NUnit)
[SetUp]
public void SetUp() { /* initialization */ }

[TearDown]
public void TearDown() { /* cleanup */ }

[Test]
public void MyTest() { /* test code */ }

// After (TUnit)
private void SetUp() { /* initialization */ }

private void TearDown() { /* cleanup */ }

[Test]
public async Task MyTest()
{
    SetUp();
    try
    {
        /* test code */
    }
    finally
    {
        TearDown();
    }
}
```

---

## Risk Mitigation

### Risk 1: Test Discovery Failures
**Mitigation**: 
- Verify test discovery after each phase
- Use `dotnet test --list-tests` to verify all tests are discoverable
- Check that test count matches baseline

### Risk 2: Assertion Syntax Errors
**Mitigation**: 
- Use systematic conversion patterns
- Test incrementally (file-by-file)
- Reference previous migration (`Node.Net.Components.Test`) for patterns

### Risk 3: Lifecycle Method Issues
**Mitigation**: 
- Test SetUp/TearDown conversion carefully in Phase 1
- Verify cleanup works correctly
- Test with files that have resource cleanup (databases, files)

### Risk 4: Performance Regression
**Mitigation**: 
- Measure baseline before migration
- Measure after migration
- TUnit typically improves performance, but verify

### Risk 5: TestHarness Compatibility
**Mitigation**: 
- Verify TestHarness compatibility early (Phase 1)
- Test with one file first before migrating others
- Document any issues or workarounds

---

## Success Criteria

1. ✅ All 57 test files migrated from NUnit to TUnit
2. ✅ All tests pass (100% pass rate)
3. ✅ Test execution time maintained or improved
4. ✅ No compilation errors or warnings
5. ✅ Test discovery works correctly (all tests discoverable)
6. ✅ Code quality maintained
7. ✅ No NUnit references remaining
8. ✅ TestHarness compatibility verified

---

## Dependencies

- TUnit package (NuGet, latest version)
- Microsoft.NET.Test.Sdk (already present)
- Migration guide: `docs/dotnet/migrate_nunit_to_tunit.md`
- Previous migration reference: `tests/Node.Net.Components.Test`

---

## Timeline Estimate

- **Phase 0**: 30 minutes (setup and research)
- **Phase 1**: 1-2 hours (TestHarness verification with complex file)
- **Phase 2**: 1 hour (2 remaining TestHarness files)
- **Phase 3**: 1 hour (2 SetUp/TearDown files)
- **Phase 4**: 4-6 hours (remaining ~50 files, ~10-15 minutes per file average)
- **Phase 5**: 30 minutes (final verification)

**Total Estimate**: 8-11 hours

---

## Notes

- Migration will be done incrementally to reduce risk
- Each file will be verified before moving to the next
- Patterns discovered in early phases will be documented and reused
- Performance will be measured before and after migration
- TestHarness compatibility is critical and will be verified early
