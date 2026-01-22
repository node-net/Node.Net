# Detailed Tasks: Migrate Node.Net.Test from NUnit to TUnit

**Feature ID**: 005-migrate-nunit-to-tunit  
**Status**: Draft  
**Created**: 2026-01-22  
**Strategy**: Incremental file-by-file migration

## Overview

This document provides a detailed, actionable task breakdown for migrating all test files in `tests/Node.Net.Test` from NUnit to TUnit. Tasks are organized by phase and can be tracked individually.

## Task Organization

- **Phase 0**: Preparation and Setup (6 tasks)
- **Phase 1**: TestHarness Compatibility Verification (15 tasks)
- **Phase 2**: Migrate Remaining TestHarness Files (8 tasks)
- **Phase 3**: Migrate Files with SetUp/TearDown (8 tasks)
- **Phase 4**: Migrate Remaining Test Files (57 files × ~5 tasks each = ~285 tasks, organized by category)
- **Phase 5**: Final Verification and Cleanup (12 tasks)

**Total Estimated Tasks**: ~334 tasks

---

## Phase 0: Preparation and Setup

### T001: Document Baseline Metrics
**Priority**: High  
**Estimated Time**: 10 minutes  
**Dependencies**: None

**Description**: Record current test count and execution time before migration.

**Tasks**:
1. Run `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --list-tests` and record test count
2. Run `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj -c Release` and record execution time
3. Document baseline metrics in a file or notes
4. Verify all tests pass with NUnit (100% pass rate)

**Acceptance Criteria**:
- Baseline test count documented
- Baseline execution time documented
- All tests passing with NUnit

---

### T002: Research TUnit API Documentation
**Priority**: High  
**Estimated Time**: 20 minutes  
**Dependencies**: None

**Description**: Research TUnit API for time comparisons, boolean assertions, and message parameters.

**Tasks**:
1. Check TUnit documentation for time comparison methods (`.Within()` equivalent)
2. Check TUnit documentation for boolean assertion methods (`IsTrue()`/`IsFalse()`)
3. Check TUnit documentation for message parameters in assertions
4. Document findings for use during migration

**Acceptance Criteria**:
- Time comparison approach determined
- Boolean assertion approach determined
- Message parameter support verified
- Findings documented

---

### T003: Add TUnit Package to Project
**Priority**: Critical  
**Estimated Time**: 5 minutes  
**Dependencies**: T002

**Description**: Add TUnit package (latest version) to `tests/Node.Net.Test/Node.Net.Test.csproj`.

**Tasks**:
1. Open `tests/Node.Net.Test/Node.Net.Test.csproj`
2. Add `<PackageReference Include="TUnit" />` (no version constraint for latest)
3. Save file
4. Run `dotnet restore tests/Node.Net.Test/Node.Net.Test.csproj`
5. Verify package restored successfully

**Acceptance Criteria**:
- TUnit package added to project file
- Package restored successfully
- No package conflicts

---

### T004: Remove NUnit Packages from Project
**Priority**: Critical  
**Estimated Time**: 5 minutes  
**Dependencies**: T003

**Description**: Remove NUnit and NUnit3TestAdapter packages from project file.

**Tasks**:
1. Open `tests/Node.Net.Test/Node.Net.Test.csproj`
2. Remove `<PackageReference Include="NUnit" Version="4.1.0" />`
3. Remove `<PackageReference Include="NUnit3TestAdapter" Version="4.5.0">...</PackageReference>`
4. Save file
5. Run `dotnet restore tests/Node.Net.Test/Node.Net.Test.csproj`
6. Verify no errors

**Acceptance Criteria**:
- NUnit package removed
- NUnit3TestAdapter package removed
- Project restores successfully

---

### T005: Add TUnit Using Statements to GlobalUsings.cs
**Priority**: Critical  
**Estimated Time**: 5 minutes  
**Dependencies**: T003

**Description**: Add TUnit global using statements to `tests/Node.Net.Test/GlobalUsings.cs`.

**Tasks**:
1. Open `tests/Node.Net.Test/GlobalUsings.cs`
2. Add `global using TUnit.Core;`
3. Add `global using TUnit.Assertions;`
4. Save file
5. Verify syntax is correct

**Acceptance Criteria**:
- `global using TUnit.Core;` added
- `global using TUnit.Assertions;` added
- No syntax errors
- File saves successfully

---

### T006: Verify Project Builds (Expected to Fail)
**Priority**: Medium  
**Estimated Time**: 5 minutes  
**Dependencies**: T003, T004, T005

**Description**: Verify project builds (will fail until files are migrated, but confirms setup is correct).

**Tasks**:
1. Run `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj`
2. Verify build fails with expected errors (NUnit references)
3. Document that this is expected

**Acceptance Criteria**:
- Build fails as expected
- Errors are related to NUnit (not TUnit setup)
- Setup verified correct

---

## Phase 1: TestHarness Compatibility Verification

### T007: Remove NUnit Using from UserSecretProvider.Tests.cs
**Priority**: Critical  
**Estimated Time**: 2 minutes  
**Dependencies**: T005

**Description**: Remove `using NUnit.Framework;` from `Security/UserSecretProvider.Tests.cs`.

**Tasks**:
1. Open `tests/Node.Net.Test/Security/UserSecretProvider.Tests.cs`
2. Remove `using NUnit.Framework;` line
3. Save file

**Acceptance Criteria**:
- `using NUnit.Framework;` removed
- File saves successfully

---

### T008: Remove TestFixture Attribute from UserSecretProvider.Tests.cs
**Priority**: Critical  
**Estimated Time**: 1 minute  
**Dependencies**: T007

**Description**: Remove `[TestFixture]` attribute from test class.

**Tasks**:
1. Open `tests/Node.Net.Test/Security/UserSecretProvider.Tests.cs`
2. Remove `[TestFixture]` attribute from class declaration
3. Save file

**Acceptance Criteria**:
- `[TestFixture]` attribute removed
- Class declaration correct

---

### T009: Convert Test Methods to Async Task in UserSecretProvider.Tests.cs
**Priority**: Critical  
**Estimated Time**: 10 minutes  
**Dependencies**: T008

**Description**: Convert all test methods from `void` to `async Task`.

**Tasks**:
1. Find all `[Test]` methods in file
2. Change method signature from `public void MethodName()` to `public async Task MethodName()`
3. Ensure all test methods are converted
4. Save file

**Acceptance Criteria**:
- All test methods return `async Task`
- No `void` test methods remaining
- `[Test]` attributes remain

---

### T010: Convert SetUp/TearDown in UserSecretProvider.Tests.cs
**Priority**: High  
**Estimated Time**: 15 minutes  
**Dependencies**: T009

**Description**: Convert `[SetUp]` and `[TearDown]` methods to manual calls in try/finally blocks.

**Tasks**:
1. Remove `[SetUp]` attribute from SetUp method
2. Remove `[TearDown]` attribute from TearDown method
3. Make SetUp and TearDown private instance methods
4. For each test method:
   - Add `SetUp();` call at start
   - Wrap test body in `try { ... } finally { TearDown(); }`
5. Save file

**Acceptance Criteria**:
- `[SetUp]` and `[TearDown]` attributes removed
- SetUp and TearDown are private instance methods
- All test methods call SetUp() and TearDown() correctly

---

### T011: Convert Basic Assertions in UserSecretProvider.Tests.cs
**Priority**: Critical  
**Estimated Time**: 30 minutes  
**Dependencies**: T010

**Description**: Convert all basic NUnit assertions to TUnit syntax.

**Tasks**:
1. Convert `Assert.That(value, Is.True)` → `await Assert.That(value).IsTrue()` (or `IsEqualTo(true)`)
2. Convert `Assert.That(value, Is.False)` → `await Assert.That(value).IsFalse()` (or `IsEqualTo(false)`)
3. Convert `Assert.That(value, Is.EqualTo(expected))` → `await Assert.That(value).IsEqualTo(expected)`
4. Convert `Assert.That(value, Is.Not.Null)` → `await Assert.That(value).IsNotNull()`
5. Convert `Assert.That(value, Is.Not.Empty)` → `await Assert.That(value).IsNotEmpty()`
6. Convert `Assert.That(value, Does.Contain(expected))` → `await Assert.That(value).Contains(expected)`
7. Convert `Assert.That(value, Is.LessThan(expected))` → `await Assert.That(value).IsLessThan(expected)`
8. Convert `Assert.That(value, Is.GreaterThan(expected))` → `await Assert.That(value).IsGreaterThan(expected)`
9. Ensure all assertions are awaited
10. Save file

**Acceptance Criteria**:
- All basic assertions converted
- All assertions awaited
- No NUnit assertion syntax remaining

---

### T012: Convert Exception Assertions in UserSecretProvider.Tests.cs
**Priority**: High  
**Estimated Time**: 10 minutes  
**Dependencies**: T011

**Description**: Convert `Assert.ThrowsAsync` to TUnit syntax.

**Tasks**:
1. Find all `Assert.ThrowsAsync<Exception>(async () => await ...)` patterns
2. Convert to `await Assert.That(async () => await ...).Throws<Exception>()`
3. Find all `Assert.Throws<Exception>(() => ...)` patterns
4. Convert to `await Assert.That(() => ...).Throws<Exception>()`
5. Save file

**Acceptance Criteria**:
- All exception assertions converted
- All exception assertions awaited
- Exception tests will pass

---

### T013: Convert Compound Assertions in UserSecretProvider.Tests.cs
**Priority**: Medium  
**Estimated Time**: 5 minutes  
**Dependencies**: T011

**Description**: Split compound assertions (`.And`) into multiple assertions.

**Tasks**:
1. Find `Assert.That(value, Is.Not.Null.And.Not.Empty)` patterns
2. Split into:
   - `await Assert.That(value).IsNotNull();`
   - `await Assert.That(value).IsNotEmpty();`
3. Save file

**Acceptance Criteria**:
- All compound assertions split
- Multiple assertions used instead

---

### T014: Convert Time-Based Comparisons in UserSecretProvider.Tests.cs
**Priority**: Medium  
**Estimated Time**: 10 minutes  
**Dependencies**: T011, T002

**Description**: Convert `.Within(TimeSpan)` comparisons based on TUnit API research.

**Tasks**:
1. Find `Assert.That(value, Is.EqualTo(expected).Within(TimeSpan))` patterns
2. If TUnit has built-in support, use it
3. Otherwise, convert to: `await Assert.That(Math.Abs((value - expected).TotalSeconds) <= tolerance).IsTrue()`
4. Save file

**Acceptance Criteria**:
- All time-based comparisons converted
- Time tolerance preserved
- Tests will pass

---

### T015: Convert Type Checking in UserSecretProvider.Tests.cs
**Priority**: Medium  
**Estimated Time**: 5 minutes  
**Dependencies**: T011

**Description**: Convert `Is.InstanceOf` to `is` operator.

**Tasks**:
1. Find `Assert.That(value, Is.InstanceOf<Type>())` patterns
2. Convert to `await Assert.That(value is Type).IsTrue()`
3. Save file

**Acceptance Criteria**:
- All type checking converted
- `is` operator used correctly

---

### T016: Build and Verify UserSecretProvider.Tests.cs
**Priority**: Critical  
**Estimated Time**: 5 minutes  
**Dependencies**: T015

**Description**: Build project and verify no compilation errors.

**Tasks**:
1. Run `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj`
2. Verify no compilation errors
3. Verify no warnings (or document acceptable warnings)
4. Fix any errors found

**Acceptance Criteria**:
- Project builds successfully
- No compilation errors
- No critical warnings

---

### T017: Run Tests for UserSecretProvider.Tests.cs
**Priority**: Critical  
**Estimated Time**: 5 minutes  
**Dependencies**: T016

**Description**: Run tests for the migrated file and verify all pass.

**Tasks**:
1. Run `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --filter "FullyQualifiedName~UserSecretProvider"`
2. Verify all tests execute
3. Verify all tests pass (100% pass rate)
4. Document any failures and fix

**Acceptance Criteria**:
- All tests execute
- All tests pass
- TestHarness compatibility verified

---

### T018: Document Migration Patterns from UserSecretProvider.Tests.cs
**Priority**: Medium  
**Estimated Time**: 10 minutes  
**Dependencies**: T017

**Description**: Document patterns and issues discovered during migration.

**Tasks**:
1. Document conversion patterns used
2. Document any issues encountered
3. Document workarounds or solutions
4. Update conversion patterns reference if needed

**Acceptance Criteria**:
- Patterns documented
- Issues documented
- Reference updated

---

## Phase 2: Migrate Remaining TestHarness Files

### T019: Migrate SystemUser.Tests.cs
**Priority**: High  
**Estimated Time**: 45 minutes  
**Dependencies**: T018

**Description**: Migrate `Service/User/SystemUser.Tests.cs` following patterns from Phase 1.

**Tasks**:
1. Remove `using NUnit.Framework;`
2. Remove `[TestFixture]` attribute
3. Convert test methods to `async Task`
4. Convert all assertions to TUnit syntax
5. **Handle `Assert.Fail()`**: Convert to TUnit equivalent (research during migration, likely `throw new AssertionException(message)`)
6. **Handle file I/O**: Note that `File.ReadAllBytes`/`WriteAllBytes` don't have async versions - keep synchronous
7. Build and verify
8. Run tests and verify all pass

**Acceptance Criteria**:
- File migrated completely
- `Assert.Fail()` converted appropriately
- File I/O operations handled correctly
- All tests pass
- Build successful

---

### T020: Migrate LogService.Tests.cs
**Priority**: High  
**Estimated Time**: 45 minutes  
**Dependencies**: T018

**Description**: Migrate `Service/Logging/LogService.Tests.cs` (has SetUp/TearDown and TestHarness).

**Tasks**:
1. Remove `using NUnit.Framework;`
2. Remove `[TestFixture]` attribute
3. Convert SetUp/TearDown (if present)
4. Convert test methods to `async Task`
5. Convert all assertions to TUnit syntax
6. Build and verify
7. Run tests and verify all pass

**Acceptance Criteria**:
- File migrated completely
- SetUp/TearDown converted correctly
- All tests pass
- Build successful

---

## Phase 3: Migrate Files with SetUp/TearDown

### T021: Migrate LogService.Integration.Tests.cs
**Priority**: High  
**Estimated Time**: 45 minutes  
**Dependencies**: T020

**Description**: Migrate `Service/Logging/LogService.Integration.Tests.cs` with SetUp/TearDown.

**Tasks**:
1. Remove `using NUnit.Framework;`
2. Remove `[TestFixture]` attribute
3. Convert SetUp/TearDown to try/finally pattern
4. Convert test methods to `async Task`
5. Convert all assertions to TUnit syntax
6. Build and verify
7. Run tests and verify all pass

**Acceptance Criteria**:
- File migrated completely
- SetUp/TearDown converted correctly
- All tests pass
- Cleanup verified

---

### T022: Migrate LiteDbFileSystem.Tests.cs
**Priority**: High  
**Estimated Time**: 60 minutes  
**Dependencies**: T021

**Description**: Migrate `Service/FileSystem/LiteDbFileSystem.Tests.cs` with SetUp/TearDown.

**Tasks**:
1. Remove `using NUnit.Framework;`
2. Remove `[TestFixture]` attribute
3. Convert SetUp/TearDown to try/finally pattern
4. Convert test methods to `async Task`
5. Convert all assertions to TUnit syntax
6. Build and verify
7. Run tests and verify all pass
8. Verify database cleanup works

**Acceptance Criteria**:
- File migrated completely
- SetUp/TearDown converted correctly
- All tests pass
- Database cleanup verified

---

## Phase 4: Migrate Remaining Test Files

### Service Tests

#### T023: Migrate ApplicationInfo.Tests.cs
**Priority**: Medium  
**Estimated Time**: 20 minutes  
**Dependencies**: T022

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T024: Migrate Application.Tests.cs
**Priority**: Medium  
**Estimated Time**: 30 minutes  
**Dependencies**: T023

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T025: Migrate LogEntry.Tests.cs
**Priority**: Medium  
**Estimated Time**: 20 minutes  
**Dependencies**: T024

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T026: Migrate WebServer.Test.cs
**Priority**: Medium  
**Estimated Time**: 25 minutes  
**Dependencies**: T025

**Description**: Migrate `Service/WebServer.Test.cs` - **NOTE**: This file uses `[Explicit]` and `[Category]` attributes.

**Tasks**:
1. Remove NUnit using/attributes
2. **Handle `[Explicit]` attribute**: Research TUnit support, if not supported, note that test will run by default
3. **Handle `[Category]` attribute**: Research TUnit support, if not supported, document that category filtering won't work
4. Convert test methods to async Task
5. Convert all assertions
6. Build and test

**Acceptance Criteria**:
- `[Explicit]` and `[Category]` handled appropriately
- All tests pass
- Behavior documented if attributes not supported

---

### System.Windows Tests (20 files)

#### T027-T046: Migrate System.Windows Test Files
**Priority**: Medium  
**Estimated Time**: 15-20 minutes per file  
**Dependencies**: T026

**Files**:
- `System/Windows/Vector.Tests.cs`
- `System/Windows/ResourceDictionary.Tests.cs`
- `System/Windows/Point.Tests.cs`
- `System/Windows/Media/Media3D/Vector3D.Tests.cs`
- `System/Windows/Media/Media3D/SpecularMaterial.Tests.cs`
- `System/Windows/Media/Media3D/Rect3D.Tests.cs`
- `System/Windows/Media/Media3D/Size3D.Tests.cs`
- `System/Windows/Media/Media3D/Point3D.Tests.cs`
- `System/Windows/Media/Media3D/MeshGeometry3D.Tests.cs`
- `System/Windows/Media/Media3D/Quaternion.Tests.cs`
- `System/Windows/Media/Media3D/Matrix3D.Tests.cs`
- `System/Windows/Media/Media3D/MaterialGroup.Tests.cs`
- `System/Windows/Media/Media3D/EmissiveMaterial.Tests.cs`
- `System/Windows/Media/Media3D/Material.Tests.cs`
- `System/Windows/Media/Media3D/DiffuseMaterial.Tests.cs`
- `System/Windows/Media/Imaging/BitmapSource.Tests.cs`
- `System/Windows/Media/ImageSource.Tests.cs`
- `System/Windows/Media/DrawingImage.Tests.cs`
- `System/Windows/Media/Color.Tests.cs`
- `System/Windows/Media/Brushes.Tests.cs`

**Tasks per file**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

### Extension Tests (9 files)

#### T047-T055: Migrate Extension Test Files
**Priority**: Medium  
**Estimated Time**: 15 minutes per file  
**Dependencies**: T046

**Files**:
- `Extension/Action.Extension.Test.cs`
- `Extension/Assembly.Extension.Test.cs`
- `Extension/IDictionary.Extension.Test.cs`
- `Extension/Matrix3D.Extension.Test.cs`
- `Extension/Object.Extension.Test.cs`
- `Extension/PerspectiveCamera.Extension.Test.cs`
- `Extension/Point.Extension.Test.cs`
- `Extension/Point3D.Extension.Test.cs`
- `Extension/Rect3D.Extension.Test.cs`
- `Extension/String.Extension.Test.cs`

**Tasks per file**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

### Collection Tests (4 files)

#### T056-T059: Migrate Collection Test Files
**Priority**: Medium  
**Estimated Time**: 15 minutes per file  
**Dependencies**: T055

**Files**:
- `Collections/Dictionary.Test.cs`
- `Collections/Element.Test.cs`
- `Collections/Items.Test.cs`
- `Collections/Spatial.Test.cs`

**Tasks per file**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

### JsonRPC Tests (3 files)

#### T060-T062: Migrate JsonRPC Test Files
**Priority**: Medium  
**Estimated Time**: 20 minutes per file  
**Dependencies**: T059

**Files**:
- `JsonRPC/Request.Test.cs`
- `JsonRPC/Responder.Test.cs`
- `JsonRPC/Server.Test.cs`

**Tasks per file**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

### Root Level Tests

#### T063: Migrate Reader.Test.cs
**Priority**: Low  
**Estimated Time**: 20 minutes  
**Dependencies**: T062

**Description**: Migrate `Reader.Test.cs` - **NOTE**: This file uses `[TestCase]` attribute which needs conversion to `[Arguments]`.

**Tasks**:
1. Remove NUnit using/attributes
2. **Convert `[TestCase]` to `[Arguments]`**: `[TestCase("Object.Coverage.json")]` → `[Arguments("Object.Coverage.json")]`
3. Convert test methods to async Task
4. Convert all assertions
5. **Verify parameterized test**: Ensure test executes with parameter
6. Build and test

**Acceptance Criteria**:
- `[TestCase]` converted to `[Arguments]`
- Parameterized test works correctly
- All tests pass

---

#### T064: Migrate Log.Test.cs
**Priority**: Low  
**Estimated Time**: 10 minutes  
**Dependencies**: T063

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T065: Migrate Writer.Test.cs
**Priority**: Low  
**Estimated Time**: 20 minutes  
**Dependencies**: T064

**Description**: Migrate `Writer.Test.cs` - **NOTE**: This file uses `static class` and `static` test methods.

**Tasks**:
1. Remove NUnit using/attributes
2. **Handle static class**: Verify TUnit supports static test methods
3. **If static not supported**: Convert `static class` to instance class, convert `static` test methods to instance methods
4. Convert test methods to `async Task` (remove `static` if converting to instance)
5. Convert all assertions
6. Build and test

**Acceptance Criteria**:
- Static methods handled correctly (either supported or converted)
- All tests pass
- Build successful

---

#### T066: Migrate Formatter.Test.cs
**Priority**: Low  
**Estimated Time**: 15 minutes  
**Dependencies**: T065

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T067: Migrate Factory.Test.cs
**Priority**: Low  
**Estimated Time**: 15 minutes  
**Dependencies**: T066

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T068: Migrate Dictionary.Test.cs
**Priority**: Low  
**Estimated Time**: 20 minutes  
**Dependencies**: T067

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T069: Migrate ConvertTest.cs
**Priority**: Low  
**Estimated Time**: 20 minutes  
**Dependencies**: T068

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T070: Migrate ConvertRotationsXYZtoOTS.Comprehensive.Tests.cs
**Priority**: Low  
**Estimated Time**: 30 minutes  
**Dependencies**: T069

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T071: Migrate Matrix3DFactory.Tests.cs
**Priority**: Low  
**Estimated Time**: 15 minutes  
**Dependencies**: T070

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T072: Migrate DelegateCommand.Test.cs
**Priority**: Low  
**Estimated Time**: 15 minutes  
**Dependencies**: T071

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T073: Migrate HiddenWhenNull.Test.cs
**Priority**: Low  
**Estimated Time**: 15 minutes  
**Dependencies**: T072

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

#### T074: Migrate SDIView.Test.cs
**Priority**: Low  
**Estimated Time**: 15 minutes  
**Dependencies**: T073

**Tasks**:
1. Remove NUnit using/attributes
2. Convert test methods to async Task
3. Convert all assertions
4. Build and test

---

## Phase 5: Final Verification and Cleanup

### T075: Run Full Test Suite
**Priority**: Critical  
**Estimated Time**: 10 minutes  
**Dependencies**: T074

**Description**: Run complete test suite and verify all tests pass.

**Tasks**:
1. Run `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj -c Release`
2. Verify all tests execute
3. Verify all tests pass (100% pass rate)
4. Document any failures

**Acceptance Criteria**:
- All tests execute
- 100% pass rate
- No test failures

---

### T076: Verify Test Discovery
**Priority**: Critical  
**Estimated Time**: 5 minutes  
**Dependencies**: T075

**Description**: Verify all tests are discoverable.

**Tasks**:
1. Run `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --list-tests`
2. Verify test count matches baseline (or document difference)
3. Verify all test classes discoverable

**Acceptance Criteria**:
- All tests discoverable
- Test count matches baseline (or documented)

---

### T077: Measure Test Execution Time
**Priority**: High  
**Estimated Time**: 5 minutes  
**Dependencies**: T075

**Description**: Measure test execution time and compare to baseline.

**Tasks**:
1. Run `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj -c Release`
2. Record execution time
3. Compare to baseline
4. Document performance change

**Acceptance Criteria**:
- Execution time measured
- Performance maintained or improved
- Metrics documented

---

### T078: Verify No NUnit References
**Priority**: Critical  
**Estimated Time**: 10 minutes  
**Dependencies**: T074

**Description**: Verify no NUnit references remain in code or project files.

**Tasks**:
1. Search codebase for "NUnit" references
2. Verify no `using NUnit.Framework;` statements
3. Verify no `[TestFixture]` attributes
4. Verify no NUnit packages in project file
5. Document any remaining references (if any)

**Acceptance Criteria**:
- No NUnit references in code
- No NUnit packages in project file
- Clean migration

---

### T079: Verify Code Quality
**Priority**: High  
**Estimated Time**: 10 minutes  
**Dependencies**: T074

**Description**: Verify code quality maintained.

**Tasks**:
1. Run `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj`
2. Verify no warnings (or document acceptable warnings)
3. Review code for consistency
4. Verify conversion patterns applied consistently

**Acceptance Criteria**:
- No critical warnings
- Code quality maintained
- Patterns consistent

---

### T080: Update Documentation
**Priority**: Medium  
**Estimated Time**: 15 minutes  
**Dependencies**: T077, T078

**Description**: Update migration documentation with final results.

**Tasks**:
1. Document final test count
2. Document performance metrics
3. Document any issues encountered
4. Document patterns used
5. Update migration guide if needed

**Acceptance Criteria**:
- Documentation updated
- Metrics documented
- Patterns documented

---

### T081: Verify CI/CD
**Priority**: High  
**Estimated Time**: 5 minutes  
**Dependencies**: T075

**Description**: Verify CI/CD pipeline still works.

**Tasks**:
1. Check if CI/CD configuration needs updates
2. Verify test execution in CI/CD context
3. Document any changes needed

**Acceptance Criteria**:
- CI/CD works correctly
- No configuration changes needed (or documented)

---

## Task Summary

### By Phase
- **Phase 0**: 6 tasks (T001-T006)
- **Phase 1**: 12 tasks (T007-T018)
- **Phase 2**: 2 tasks (T019-T020)
- **Phase 3**: 2 tasks (T021-T022)
- **Phase 4**: 52 tasks (T023-T074) - 57 files total
- **Phase 5**: 7 tasks (T075-T081)

**Total**: 81 detailed tasks

### By Priority
- **Critical**: 25 tasks
- **High**: 20 tasks
- **Medium**: 30 tasks
- **Low**: 6 tasks

### Estimated Time
- **Phase 0**: 50 minutes
- **Phase 1**: 2 hours
- **Phase 2**: 1.25 hours
- **Phase 3**: 1.75 hours
- **Phase 4**: 10-12 hours (57 files)
- **Phase 5**: 1 hour

**Total Estimated Time**: 16-18 hours

---

## Notes

- Tasks are designed to be completed sequentially within each phase
- Some tasks can be parallelized (e.g., migrating different files in Phase 4)
- Time estimates are approximate and may vary
- Document any deviations or issues encountered
- Update task list if patterns or requirements change
