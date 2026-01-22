# Clarification Decisions: Migrate Node.Net.Test from NUnit to TUnit

**Feature ID**: 005-migrate-nunit-to-tunit  
**Date**: 2026-01-22

## Summary of Decisions

All 12 clarification questions have been answered. This document records the decisions made for the migration.

## Decisions

### Q1: TUnit Version
**Decision**: **B1** - Use the latest available TUnit version from NuGet (no version constraint, let NuGet resolve latest)

**Rationale**: Ensures we're using the most up-to-date version with latest features and bug fixes.

---

### Q2: SetUp/TearDown Pattern
**Decision**: **A** - Follow the same pattern as `Node.Net.Components.Test`: manually call `SetUp()` at the start of each test method and `TearDown()` in a `finally` block

**Rationale**: Proven pattern from previous migration, ensures proper initialization and cleanup.

**Pattern**:
```csharp
[Test]
public async Task MyTest()
{
    SetUp();
    try
    {
        // Test code here
    }
    finally
    {
        TearDown();
    }
}
```

---

### Q3: Assert.ThrowsAsync Conversion
**Decision**: **A** - Convert to `await Assert.That(async () => await ...).Throws<Exception>()`

**Rationale**: Follows the pattern from previous migration, maintains async behavior.

**Conversion**:
```csharp
// Before
Assert.ThrowsAsync<ArgumentException>(async () => await _provider.GetOrCreateAsync(invalidKey));

// After
await Assert.That(async () => await _provider.GetOrCreateAsync(invalidKey)).Throws<ArgumentException>();
```

---

### Q4: Assertion Messages
**Decision**: **B** - Use TUnit's message parameter if available: `await Assert.That(value).IsEqualTo(expected, "message")`, otherwise use descriptive variable names and rely on TUnit's default error messages

**Rationale**: Preserves test failure messages for debugging while maintaining code readability.

---

### Q5: Time-Based Comparisons (.Within())
**Decision**: **C** - Check TUnit documentation for time comparison methods first, then use Option A (manual comparison) if no built-in support

**Rationale**: Prefer built-in support if available, otherwise use explicit manual comparison for clarity.

**Fallback Pattern** (if no built-in support):
```csharp
// Before
Assert.That(secret2.CreatedUtc, Is.EqualTo(secret1.CreatedUtc).Within(TimeSpan.FromSeconds(1)));

// After
await Assert.That(Math.Abs((secret2.CreatedUtc - secret1.CreatedUtc).TotalSeconds) <= 1).IsTrue();
```

---

### Q6: Compound Assertions (.And)
**Decision**: **A** - Split into two assertions: `await Assert.That(secret.Base64).IsNotNull(); await Assert.That(secret.Base64).IsNotEmpty();`

**Rationale**: Better error messages and clearer test intent.

**Conversion**:
```csharp
// Before
Assert.That(secret.Base64, Is.Not.Null.And.Not.Empty);

// After
await Assert.That(secret.Base64).IsNotNull();
await Assert.That(secret.Base64).IsNotEmpty();
```

---

### Q7: Type Checking (Is.InstanceOf)
**Decision**: **A** - Use `is` operator: `await Assert.That(caughtException is OperationCanceledException).IsTrue()`

**Rationale**: Simple, clear, and works with any type.

**Conversion**:
```csharp
// Before
Assert.That(caughtException, Is.InstanceOf<OperationCanceledException>());

// After
await Assert.That(caughtException is OperationCanceledException).IsTrue();
```

---

### Q8: Migration Strategy
**Decision**: **B** - Migrate file-by-file, ensuring each file builds and tests pass before moving to the next

**Rationale**: Reduces risk, allows for easier rollback if issues arise, and provides incremental progress validation.

**Process**:
1. Migrate one file
2. Build and verify no compilation errors
3. Run tests for that file
4. Verify all tests pass
5. Move to next file

---

### Q9: Performance Tests
**Decision**: **A** - Convert timing assertions to TUnit syntax: `await Assert.That(percentile95.TotalMilliseconds).IsLessThan(200)`, keep performance test logic unchanged

**Rationale**: Maintains test behavior while using TUnit syntax consistently.

**Conversion**:
```csharp
// Before
Assert.That(percentile95.TotalMilliseconds, Is.LessThan(200));

// After
await Assert.That(percentile95.TotalMilliseconds).IsLessThan(200);
```

---

### Q10: TestHarness Base Class
**Decision**: **A** - Verify TestHarness works with TUnit by migrating one TestHarness-based test file first

**Rationale**: Early verification prevents issues later, especially since 3 test classes depend on TestHarness.

**Verification Order**:
1. Migrate one TestHarness-based file first (e.g., `Security/UserSecretProvider.Tests.cs`)
2. Verify it builds and tests pass
3. Proceed with remaining files

---

### Q11: GlobalUsings.cs
**Decision**: **A** - Add TUnit using statements to `GlobalUsings.cs`

**Rationale**: Reduces repetition and ensures consistency across all test files.

**Addition to `GlobalUsings.cs`**:
```csharp
global using TUnit.Core;
global using TUnit.Assertions;
```

---

### Q12: Assert.That with Is.True/Is.False
**Decision**: **C** - Check TUnit documentation for boolean assertion methods first, then use `IsTrue()`/`IsFalse()` if available, otherwise `IsEqualTo(true/false)`

**Rationale**: Prefer built-in methods if available for better readability.

**Preferred Pattern** (if available):
```csharp
// Before
Assert.That(condition, Is.True);

// After
await Assert.That(condition).IsTrue();
```

**Fallback Pattern** (if not available):
```csharp
await Assert.That(condition).IsEqualTo(true);
```

---

## Next Steps

With all clarifications complete, the next steps are:
1. Update the specification with these decisions
2. Create detailed migration plan (`/speckit.plan`)
3. Create quality checklist (`/speckit.checklist`)
4. Create detailed task breakdown (`/speckit.tasks`)
5. Analyze for improvements (`/speckit.analyze`)
6. Begin implementation (`/speckit.implement`)
