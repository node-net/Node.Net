# TUnit API Research: Migrate Node.Net.Test from NUnit to TUnit

**Feature ID**: 005-migrate-nunit-to-tunit  
**Date**: 2026-01-22  
**Source**: Analysis of `tests/Node.Net.Components.Test` (already migrated) and codebase patterns

## Research Findings

### ✅ Boolean Assertions: IsTrue() and IsFalse()
**Status**: **SUPPORTED**

**Evidence**: Found in `tests/Node.Net.Components.Test/Maps.Tests.cs`:
```csharp
await Assert.That(MapsComponentHelper.ValidateLatitude(-90.0)).IsTrue();
await Assert.That(MapsComponentHelper.ValidateLatitude(-90.1)).IsFalse();
```

**Conclusion**: TUnit supports `IsTrue()` and `IsFalse()` methods directly. Use these instead of `IsEqualTo(true/false)`.

**Conversion Pattern**:
```csharp
// NUnit → TUnit
Assert.That(condition, Is.True) → await Assert.That(condition).IsTrue()
Assert.That(condition, Is.False) → await Assert.That(condition).IsFalse()
```

---

### ❓ Assert.Fail() Equivalent
**Status**: **NOT FOUND IN MIGRATED CODE**

**Evidence**: Searched `tests/Node.Net.Components.Test` - no usage of `Assert.Fail()` found.

**Found in**: `tests/Node.Net.Test/Service/User/SystemUser.Tests.cs` line 227:
```csharp
Assert.Fail($"JPEG file failed to load as System.Drawing.Image: {ex.Message}");
```

**Research Needed**: 
- Check TUnit documentation for `Assert.Fail()` equivalent
- Alternative: Use `throw new AssertionException(message)` or similar

**Recommendation**: 
- Research TUnit's failure mechanism
- If no direct equivalent, use exception throwing pattern
- Document pattern for migration

**Action**: Verify during Phase 1 migration

---

### ❓ [Explicit] Attribute Support
**Status**: **NOT FOUND IN MIGRATED CODE**

**Evidence**: Searched `tests/Node.Net.Components.Test` - no `[Explicit]` attributes found.

**Found in**: `tests/Node.Net.Test/Service/WebServer.Test.cs` line 10:
```csharp
[Test, Explicit]
public void Default_Usage()
```

**Research Needed**: 
- Check if TUnit supports `[Explicit]` attribute
- If not, document that explicit tests will run by default (or need alternative approach)

**Recommendation**: 
- Research TUnit's explicit test support
- If not supported, note in migration that explicit tests will run
- Consider using test filtering or conditional execution

**Action**: Verify during Phase 1 migration or research TUnit docs

---

### ❓ [Category] Attribute Support
**Status**: **NOT FOUND IN MIGRATED CODE**

**Evidence**: Searched `tests/Node.Net.Components.Test` - no `[Category]` attributes found.

**Found in**: `tests/Node.Net.Test/Service/WebServer.Test.cs` line 7:
```csharp
[TestFixture, Category(nameof(WebServer))]
```

**Research Needed**: 
- Check if TUnit supports `[Category]` or similar attribute
- Alternative: Use test filtering by name or trait

**Recommendation**: 
- Research TUnit's category/tag support
- If not supported, document that category filtering won't work
- Consider using test name filtering instead

**Action**: Verify during Phase 1 migration or research TUnit docs

---

### ✅ Static Test Methods
**Status**: **LIKELY SUPPORTED** (needs verification)

**Evidence**: Found static helper methods in migrated code, but no static test methods.

**Found in**: `tests/Node.Net.Test/Writer.Test.cs`:
```csharp
internal static class WriterTest
{
    [Test]
    public static void WriteJson()
```

**Research Needed**: 
- Verify TUnit supports static test methods
- If not, convert to instance methods

**Recommendation**: 
- Test with one static test method first
- If not supported, convert static classes to instance classes
- Convert static test methods to instance methods

**Action**: Verify during Phase 1 or Phase 4 migration

---

### ❓ Time-Based Comparisons (.Within())
**Status**: **NEEDS RESEARCH**

**Evidence**: No time-based comparisons found in migrated code.

**Found in**: `tests/Node.Net.Test/Security/UserSecretProvider.Tests.cs` line 70:
```csharp
Assert.That(secret2.CreatedUtc, Is.EqualTo(secret1.CreatedUtc).Within(TimeSpan.FromSeconds(1)))
```

**Research Needed**: 
- Check TUnit documentation for time comparison methods
- Check if TUnit has `.Within()` or tolerance support

**Recommendation**: 
- Research TUnit time comparison API
- If no built-in support, use manual comparison pattern:
  ```csharp
  await Assert.That(Math.Abs((value - expected).TotalSeconds) <= tolerance).IsTrue()
  ```

**Action**: Research during Phase 0, document findings

---

### ❓ Assertion Messages
**Status**: **NEEDS VERIFICATION**

**Evidence**: Many assertions in codebase have message parameters:
```csharp
Assert.That(value, Is.EqualTo(expected), "message")
```

**Research Needed**: 
- Check if TUnit assertions support message parameters
- Check syntax: `await Assert.That(value).IsEqualTo(expected, "message")` or similar

**Recommendation**: 
- Research TUnit message parameter syntax
- If supported, preserve messages
- If not, use descriptive variable names and comments

**Action**: Research during Phase 0, verify during Phase 1

---

### ✅ File I/O Async Conversion
**Status**: **LIMITED - NO ASYNC VERSIONS AVAILABLE**

**Evidence**: Found `File.ReadAllBytes` and `File.WriteAllBytes` in `SystemUser.Tests.cs`.

**Finding**: 
- `File.ReadAllBytes()` and `File.WriteAllBytes()` do NOT have async versions in .NET
- Only `File.ReadAllTextAsync()` and `File.WriteAllTextAsync()` exist
- `File.ReadAllBytesAsync()` and `File.WriteAllBytesAsync()` do not exist

**Conclusion**: 
- Cannot convert `File.ReadAllBytes`/`WriteAllBytes` to async
- Keep as synchronous calls
- Only convert `File.ReadAllText`/`WriteAllText` where applicable

**Action**: Update FR-010 to clarify this limitation

---

### ✅ [TestCase] to [Arguments] Conversion
**Status**: **CONFIRMED NEEDED**

**Evidence**: Found `[TestCase]` in `Reader.Test.cs` line 13:
```csharp
[TestCase("Object.Coverage.json")]
public void Read(string name)
```

**Conversion Pattern**:
```csharp
// NUnit → TUnit
[TestCase("value1")] → [Arguments("value1")]
[TestCase("value1", "value2", 3)] → [Arguments("value1", "value2", 3)]
```

**Action**: Add explicit conversion to T073 (migrate Reader.Test.cs)

---

## Summary of TUnit API Support

| Feature | Status | Action |
|---------|--------|--------|
| `IsTrue()`/`IsFalse()` | ✅ Supported | Use directly |
| `Assert.Fail()` | ❓ Unknown | Research/use exception pattern |
| `[Explicit]` | ❓ Unknown | Research TUnit docs |
| `[Category]` | ❓ Unknown | Research TUnit docs |
| Static test methods | ❓ Unknown | Test during migration |
| Time comparisons (`.Within()`) | ❓ Unknown | Research, use manual if needed |
| Assertion messages | ❓ Unknown | Research syntax |
| `[TestCase]` → `[Arguments]` | ✅ Confirmed | Convert directly |
| File I/O async | ⚠️ Limited | Only text files have async versions |

---

## Next Steps

1. **During Phase 0**: Research TUnit documentation for:
   - `Assert.Fail()` equivalent
   - `[Explicit]` attribute support
   - `[Category]` attribute support
   - Time comparison methods
   - Assertion message parameters

2. **During Phase 1**: Test with `UserSecretProvider.Tests.cs`:
   - Static methods (if any)
   - Time comparisons
   - Assertion messages
   - Document findings

3. **Update Documentation**: Add findings to conversion patterns reference

---

## Notes

- Most patterns can be verified during actual migration
- Some features may not have direct equivalents - document workarounds
- Prioritize critical patterns (assertions, test methods) over nice-to-haves (categories, explicit)
