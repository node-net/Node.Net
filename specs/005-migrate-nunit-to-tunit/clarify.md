# Clarification Questions: Migrate Node.Net.Test from NUnit to TUnit

**Feature ID**: 005-migrate-nunit-to-tunit  
**Status**: Clarified  
**Created**: 2026-01-22  
**Clarified**: 2026-01-22

## Overview

This document identifies questions and decisions needed before proceeding with the migration from NUnit to TUnit for `tests/Node.Net.Test`.

## Questions

### Q1: TUnit Version
**Question**: Which TUnit version should we use?

**Options**:
- **A**: Use version 1.12.15 as specified in the migration guide (`docs/dotnet/migrate_nunit_to_tunit.md`)
- **B**: Check NuGet for the latest stable version compatible with .NET 8.0
- **C**: Use the same version as `Node.Net.Components.Test` (if different from 1.12.15)

**Recommendation**: Option A - Use 1.12.15 for consistency with the migration guide and previous migration.

---

### Q2: SetUp/TearDown Pattern
**Question**: How should we handle `[SetUp]` and `[TearDown]` methods?

**Context**: 
- 4 test files use `[SetUp]` and `[TearDown]` attributes:
  - `Security/UserSecretProvider.Tests.cs`
  - `Service/Logging/LogService.Tests.cs`
  - `Service/Logging/LogService.Integration.Tests.cs`
  - `Service/FileSystem/LiteDbFileSystem.Tests.cs`
- TUnit doesn't have equivalent attributes
- Previous migration (`Node.Net.Components.Test`) manually calls `SetUp()` at start of each test and `TearDown()` in a `finally` block

**Options**:
- **A**: Follow the same pattern as `Node.Net.Components.Test`: manually call `SetUp()` at the start of each test method and `TearDown()` in a `finally` block
- **B**: Create a helper method that wraps test execution with SetUp/TearDown
- **C**: Convert SetUp/TearDown to instance methods and call them manually in each test (same as A, but clarify the pattern)

**Recommendation**: Option A - Follow the proven pattern from the previous migration.

---

### Q3: Assert.ThrowsAsync Conversion
**Question**: How should `Assert.ThrowsAsync<Exception>(async () => ...)` be converted?

**Context**: 
- Found in `Security/UserSecretProvider.Tests.cs` (lines 96, 106, 262, 357)
- Migration guide mentions `Assert.Throws` works the same, but doesn't explicitly cover `Assert.ThrowsAsync`
- Previous migration used: `await Assert.That(async () => await ...).Throws<Exception>()`

**Options**:
- **A**: Convert to `await Assert.That(async () => await ...).Throws<Exception>()` (same pattern as previous migration)
- **B**: Convert to `await Assert.That(() => ...).Throws<Exception>()` (remove async wrapper)
- **C**: Keep as `Assert.ThrowsAsync<Exception>(async () => ...)` if TUnit supports it

**Recommendation**: Option A - Follow the pattern from the previous migration.

---

### Q4: Assertion Messages
**Question**: How should assertion messages be handled?

**Context**: 
- Many NUnit assertions have message parameters: `Assert.That(value, Is.EqualTo(expected), "message")`
- TUnit assertions use fluent syntax: `await Assert.That(value).IsEqualTo(expected)`
- Need to preserve test failure messages for debugging

**Options**:
- **A**: Add messages as comments above assertions (loses runtime message)
- **B**: Use TUnit's message parameter if available: `await Assert.That(value).IsEqualTo(expected, "message")`
- **C**: Use descriptive variable names and rely on TUnit's default error messages

**Recommendation**: Option B - Check if TUnit supports message parameters, otherwise Option C.

---

### Q5: Time-Based Comparisons (.Within())
**Question**: How should `Assert.That(value, Is.EqualTo(expected).Within(TimeSpan))` be converted?

**Context**: 
- Found in `Security/UserSecretProvider.Tests.cs` (line 70): `Assert.That(secret2.CreatedUtc, Is.EqualTo(secret1.CreatedUtc).Within(TimeSpan.FromSeconds(1)))`
- Tests time-based equality with tolerance

**Options**:
- **A**: Convert to manual comparison: `await Assert.That(Math.Abs((secret2.CreatedUtc - secret1.CreatedUtc).TotalSeconds) <= 1).IsTrue()`
- **B**: Use TUnit's built-in tolerance if available: `await Assert.That(secret2.CreatedUtc).IsEqualTo(secret1.CreatedUtc, TimeSpan.FromSeconds(1))`
- **C**: Check TUnit documentation for time comparison methods

**Recommendation**: Option C - Check TUnit documentation first, then Option A if no built-in support.

---

### Q6: Compound Assertions (.And)
**Question**: How should `Assert.That(value, Is.Not.Null.And.Not.Empty)` be converted?

**Context**: 
- Found in `Security/UserSecretProvider.Tests.cs` (line 44): `Assert.That(secret.Base64, Is.Not.Null.And.Not.Empty)`
- Compound assertions with `.And` operator

**Options**:
- **A**: Split into two assertions: `await Assert.That(secret.Base64).IsNotNull(); await Assert.That(secret.Base64).IsNotEmpty();`
- **B**: Use boolean logic: `await Assert.That(secret.Base64 != null && secret.Base64 != string.Empty).IsTrue()`
- **C**: Check if TUnit supports compound assertions

**Recommendation**: Option A - Split into multiple assertions for clarity and better error messages.

---

### Q7: Type Checking (Is.InstanceOf)
**Question**: How should `Assert.That(value, Is.InstanceOf<Type>())` be converted?

**Context**: 
- Found in `Security/UserSecretProvider.Tests.cs` (lines 192, 439, 472): `Assert.That(caughtException, Is.InstanceOf<OperationCanceledException>())`
- Tests exception type checking

**Options**:
- **A**: Use `is` operator: `await Assert.That(caughtException is OperationCanceledException).IsTrue()`
- **B**: Use TUnit's type checking if available: `await Assert.That(caughtException).IsInstanceOf<OperationCanceledException>()`
- **C**: Check TUnit documentation for type assertion methods

**Recommendation**: Option C - Check TUnit documentation first, then Option A if no built-in support.

---

### Q8: Migration Strategy
**Question**: Should we migrate all files at once or incrementally?

**Options**:
- **A**: Migrate all files at once (big bang approach)
- **B**: Migrate file-by-file, ensuring each file builds and tests pass before moving to the next
- **C**: Migrate by category (e.g., all Service tests, then all System tests, etc.)

**Recommendation**: Option B - Incremental migration reduces risk and allows for easier rollback if issues arise.

---

### Q9: Performance Tests
**Question**: How should performance tests with timing assertions be handled?

**Context**: 
- Found in `Security/UserSecretProvider.Tests.cs` (lines 487-549)
- Tests use `Stopwatch` and assert timing thresholds: `Assert.That(percentile95.TotalMilliseconds, Is.LessThan(200))`
- These are already async tests

**Options**:
- **A**: Convert timing assertions to TUnit syntax: `await Assert.That(percentile95.TotalMilliseconds).IsLessThan(200)`
- **B**: Keep performance test logic unchanged, only convert assertions
- **C**: Review if performance thresholds need adjustment for TUnit

**Recommendation**: Option A - Convert assertions to TUnit syntax, keep test logic unchanged.

---

### Q10: TestHarness Base Class
**Question**: Should we verify TestHarness compatibility with TUnit before starting migration?

**Context**: 
- 3 test classes inherit from `TestHarness`:
  - `Security/UserSecretProvider.Tests.cs`
  - `Service/User/SystemUser.Tests.cs`
  - `Service/Logging/LogService.Tests.cs`
- Previous migration (`Node.Net.Components.Test`) successfully used TestHarness with TUnit

**Options**:
- **A**: Verify TestHarness works with TUnit by migrating one TestHarness-based test file first
- **B**: Assume compatibility based on previous migration and proceed with all files
- **C**: Check TestHarness source code to understand its dependencies

**Recommendation**: Option A - Verify compatibility early to avoid issues later.

---

### Q11: GlobalUsings.cs
**Question**: Should we add TUnit using statements to `GlobalUsings.cs`?

**Context**: 
- `tests/Node.Net.Test/GlobalUsings.cs` contains global using statements for WPF types
- Could add `global using TUnit.Core;` and `global using TUnit.Assertions;` to avoid repeating in each file

**Options**:
- **A**: Add TUnit using statements to `GlobalUsings.cs`
- **B**: Add using statements to each test file individually
- **C**: Check if other test projects use GlobalUsings for test framework

**Recommendation**: Option A - Use GlobalUsings for consistency and to reduce repetition.

---

### Q12: Assert.That with Is.True/Is.False
**Question**: How should `Assert.That(condition, Is.True)` and `Assert.That(condition, Is.False)` be converted?

**Context**: 
- Common pattern throughout test files
- Migration guide shows: `Assert.That(value, Is.True)` → `await Assert.That(value).IsEqualTo(true)`
- But TUnit may have `IsTrue()` and `IsFalse()` methods

**Options**:
- **A**: Use `await Assert.That(condition).IsTrue()` and `await Assert.That(condition).IsFalse()` if available
- **B**: Use `await Assert.That(condition).IsEqualTo(true)` and `await Assert.That(condition).IsEqualTo(false)`
- **C**: Check TUnit documentation for boolean assertion methods

**Recommendation**: Option C - Check TUnit documentation first, then Option A if available, otherwise Option B.

---

## Summary

These clarifications will ensure:
1. Consistent migration approach across all test files
2. Proper handling of NUnit-specific features (SetUp/TearDown, compound assertions, etc.)
3. Preservation of test behavior and error messages
4. Efficient migration process with minimal risk

## Next Steps

After receiving answers to these questions:
1. Update the specification with decisions
2. Create detailed migration plan
3. Begin incremental migration following the clarified patterns
