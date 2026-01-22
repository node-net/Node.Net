# Migrating from NUnit to TUnit

**Last Updated**: 2026-01-22  
**TUnit Version**: 1.12.15  
**Target Framework**: .NET 8.0

## Overview

This guide documents the migration process from NUnit to TUnit for .NET 8.0 test projects. TUnit is a modern, fast, and flexible .NET testing framework designed with async test methods as the standard pattern.

**Key Benefits**:
- Improved test execution performance (observed ~57% improvement: 8.35s → 3.6s)
- Modern async-first design
- Better parallel test execution
- Active development and community support

## Prerequisites

- .NET 8.0 or later
- Microsoft.NET.Test.Sdk (compatible with TUnit)
- Existing NUnit test project

## Step-by-Step Migration Process

### Phase 1: Package Updates

#### Remove NUnit Packages

Remove the following packages from your `.csproj`:

```xml
<!-- Remove these -->
<PackageReference Include="nunit" Version="4.4.0" />
<PackageReference Include="NUnit3TestAdapter" Version="6.1.0">
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  <PrivateAssets>all</PrivateAssets>
</PackageReference>
```

#### Add TUnit Package

Add TUnit to your `.csproj`:

```xml
<PackageReference Include="TUnit" Version="1.12.15" />
```

**Note**: Check [NuGet](https://www.nuget.org/packages/TUnit) for the latest stable version compatible with your target framework.

### Phase 2: Update Using Statements

#### Remove NUnit Using Statements

Remove from all test files:
```csharp
using NUnit.Framework;
```

#### Add TUnit Using Statements

Add to all test files:
```csharp
using TUnit.Core;
using TUnit.Assertions;
```

**Complete Example**:
```csharp
// Before
using NUnit.Framework;

// After
using TUnit.Core;
using TUnit.Assertions;
```

### Phase 3: Update Test Class Attributes

#### Remove `[TestFixture]` Attributes

TUnit does not require `[TestFixture]` attributes. Simply remove them:

```csharp
// Before
[TestFixture]
internal class ProjectTests { }

// After
internal class ProjectTests { }
```

**Note**: TUnit identifies test classes by convention or explicit `[Test]` attributes on methods.

### Phase 4: Update Test Method Attributes

#### Keep `[Test]` Attribute

The `[Test]` attribute works the same in TUnit:

```csharp
// Both NUnit and TUnit
[Test]
public async Task MyTest() { }
```

#### Convert `[TestCase]` to `[Arguments]`

TUnit uses `[Arguments]` for parameterized tests instead of `[TestCase]`:

```csharp
// Before (NUnit)
[Test]
[TestCase("file1.txt")]
[TestCase("file2.txt")]
public void Open(string name) { }

// After (TUnit)
[Test]
[Arguments("file1.txt")]
[Arguments("file2.txt")]
public async Task Open(string name) { }
```

**Multiple Parameters**:
```csharp
// Before (NUnit)
[Test]
[TestCase("file.txt", "class Type", 5)]
public void TestMethod(string file, string type, int count) { }

// After (TUnit)
[Test]
[Arguments("file.txt", "class Type", 5)]
public async Task TestMethod(string file, string type, int count) { }
```

#### Handle `[Explicit]` Attribute

TUnit supports `[Explicit]` attribute with the same behavior:

```csharp
// Both NUnit and TUnit
[Test, Explicit]
public async Task ExplicitTest() { }
```

### Phase 5: Convert Test Methods to Async

TUnit uses async `Task`-returning methods as the standard pattern:

```csharp
// Before (NUnit)
[Test]
public void MyTest() { }

// After (TUnit)
[Test]
public async Task MyTest() { }
```

**Note**: Even if your test method doesn't use `await`, TUnit prefers async methods. The compiler won't warn about unused async if the method is marked `async Task`.

### Phase 6: Update Assertion Syntax

#### Basic Assertions

TUnit assertions must be awaited and use fluent syntax:

```csharp
// Before (NUnit)
Assert.That(actual, Is.True);
Assert.That(actual, Is.EqualTo(expected));
Assert.That(actual, Is.Not.Null);

// After (TUnit)
await Assert.That(actual).IsEqualTo(true);
await Assert.That(actual).IsEqualTo(expected);
await Assert.That(actual).IsNotNull();
```

#### Common Assertion Patterns

| NUnit | TUnit |
|-------|-------|
| `Assert.That(value, Is.True)` | `await Assert.That(value).IsEqualTo(true)` |
| `Assert.That(value, Is.False)` | `await Assert.That(value).IsEqualTo(false)` |
| `Assert.That(value, Is.EqualTo(expected))` | `await Assert.That(value).IsEqualTo(expected)` |
| `Assert.That(value, Is.Not.Null)` | `await Assert.That(value).IsNotNull()` |
| `Assert.That(value, Is.Not.Empty)` | `await Assert.That(value).IsNotEqualTo(string.Empty)` |
| `Assert.That(value, Is.Not.EqualTo(expected))` | `await Assert.That(value).IsNotEqualTo(expected)` |

#### Exception Assertions

```csharp
// Before (NUnit)
Assert.Throws<Exception>(() => MethodThatThrows());

// After (TUnit)
Assert.Throws<Exception>(() => MethodThatThrows());
```

**Note**: `Assert.Throws` works the same in TUnit and does not need to be awaited.

#### DoesNotThrow Pattern

TUnit does not have `Assert.DoesNotThrow`. Simply call the method - if it throws, the test will fail:

```csharp
// Before (NUnit)
Assert.DoesNotThrow(() => MethodThatShouldNotThrow());

// After (TUnit)
MethodThatShouldNotThrow(); // Test will fail if exception is thrown
```

#### Integer Comparisons

For integer comparisons, use boolean assertions:

```csharp
// Before (NUnit)
Assert.That(actualLength, Is.EqualTo(expectedLength));

// After (TUnit)
await Assert.That(actualLength == expectedLength).IsEqualTo(true);
```

**Alternative**: Use `IsEqualTo` directly (may require type-specific assertion):
```csharp
await Assert.That(actualLength).IsEqualTo(expectedLength);
```

#### Byte Array Comparisons

**⚠️ Important Gotcha**: TUnit's `IsEqualTo` compares references for byte arrays, not content. Use `SequenceEqual`:

```csharp
// Before (NUnit)
Assert.That(actualBytes, Is.EqualTo(expectedBytes));

// After (TUnit)
using System.Linq;
await Assert.That(actualBytes.SequenceEqual(expectedBytes)).IsEqualTo(true);
```

### Phase 7: Convert File I/O to Async

Where applicable, convert synchronous file I/O to async:

```csharp
// Before
var content = File.ReadAllText(filePath);
File.WriteAllText(filePath, content);
var bytes = File.ReadAllBytes(filePath);

// After
var content = await File.ReadAllTextAsync(filePath);
await File.WriteAllTextAsync(filePath, content);
var bytes = await File.ReadAllBytesAsync(filePath);
```

**Note**: Some APIs like `File.Exists()` don't have async versions - these remain synchronous.

### Phase 8: Base Class Compatibility

If your test classes inherit from custom base classes, they should work with TUnit without modification:

```csharp
// This pattern works with TUnit
internal class MyTests : CustomTestBase
{
    [Test]
    public async Task MyTest()
    {
        // Can use base class methods
        var result = this.BaseClassMethod();
    }
}
```

**Verification**: Test that base class methods work correctly after migration.

## Common Issues and Solutions

### Issue 1: Ambiguous Reference Between TUnit and NUnit

**Symptom**: `error CS0104: 'Test' is an ambiguous reference between 'TUnit.Core.TestAttribute' and 'NUnit.Framework.TestAttribute'`

**Solution**: Ensure all NUnit packages are removed and all `using NUnit.Framework;` statements are removed before adding TUnit.

### Issue 2: Byte Array Comparison Fails

**Symptom**: `AssertionException: Expected to be equal to System.Byte[] but found System.Byte[]`

**Solution**: Use `SequenceEqual` for byte array comparisons:
```csharp
await Assert.That(actualBytes.SequenceEqual(expectedBytes)).IsEqualTo(true);
```

### Issue 3: Assertion Must Be Awaited

**Symptom**: `error TUnitAssertions0002: Assert statements must be awaited - all TUnit assertions return Task`

**Solution**: All TUnit assertions must be awaited:
```csharp
// Wrong
Assert.That(value).IsEqualTo(expected);

// Correct
await Assert.That(value).IsEqualTo(expected);
```

### Issue 4: Integer Comparison Not Working

**Symptom**: `'ValueAssertion<int>' does not contain a definition for 'IsEqualTo'`

**Solution**: Use boolean comparison:
```csharp
await Assert.That(actual == expected).IsEqualTo(true);
```

### Issue 5: Test Discovery Fails

**Symptom**: Tests not discovered by test runner

**Solution**: 
1. Ensure `Microsoft.NET.Test.Sdk` is present
2. Verify TUnit package is correctly installed
3. Run `dotnet restore` and `dotnet build`
4. Check that test methods have `[Test]` attribute and return `async Task`

## Migration Checklist

- [ ] Remove NUnit packages from `.csproj`
- [ ] Add TUnit package to `.csproj`
- [ ] Remove `using NUnit.Framework;` from all test files
- [ ] Add `using TUnit.Core;` and `using TUnit.Assertions;` to all test files
- [ ] Remove `[TestFixture]` attributes from test classes
- [ ] Convert `[TestCase]` to `[Arguments]` for parameterized tests
- [ ] Convert test methods from `void` to `async Task`
- [ ] Update all assertions to use TUnit syntax with `await`
- [ ] Convert byte array comparisons to use `SequenceEqual`
- [ ] Convert file I/O operations to async where applicable
- [ ] Replace `Assert.DoesNotThrow` with direct method calls
- [ ] Verify base class compatibility (if applicable)
- [ ] Run tests and verify all pass
- [ ] Measure test execution time for comparison

## Performance Comparison

Typical performance improvements observed during migration:

| Metric | NUnit | TUnit | Improvement |
|--------|-------|-------|-------------|
| Test Execution Time | Baseline | ~40-60% faster | Significant improvement |
| Parallel Execution | Limited | Enhanced | Better resource utilization |
| Async Operations | Slower | Optimized | Better async/await support |

## Verification Steps

1. **Build Verification**:
   ```bash
   dotnet build --configuration Release
   ```
   Should show: `0 Warning(s), 0 Error(s)`

2. **Test Discovery**:
   ```bash
   dotnet test --list-tests
   ```
   Should list all test methods

3. **Test Execution**:
   ```bash
   dotnet test --configuration Release
   ```
   All tests should pass

4. **Performance Measurement**:
   ```bash
   dotnet test --configuration Release --verbosity minimal
   ```
   Compare execution time with baseline

## Additional Resources

- [TUnit GitHub Repository](https://github.com/thomhurst/TUnit)
- [TUnit NuGet Package](https://www.nuget.org/packages/TUnit)
- [TUnit Documentation](https://github.com/thomhurst/TUnit#readme)

## Example: Complete Migration

### Before (NUnit)

```csharp
using NUnit.Framework;
using System.IO;

[TestFixture]
internal class ProjectTests
{
    [Test]
    [TestCase("file1.txt")]
    [TestCase("file2.txt")]
    public void Open(string name)
    {
        var file = new FileInfo(name);
        Assert.That(file.Exists, Is.True);
    }

    [Test]
    public void VerifyBytes()
    {
        var bytes1 = File.ReadAllBytes("file1.txt");
        var bytes2 = File.ReadAllBytes("file2.txt");
        Assert.That(bytes1, Is.EqualTo(bytes2));
    }
}
```

### After (TUnit)

```csharp
using TUnit.Core;
using TUnit.Assertions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

internal class ProjectTests
{
    [Test]
    [Arguments("file1.txt")]
    [Arguments("file2.txt")]
    public async Task Open(string name)
    {
        var file = new FileInfo(name);
        await Assert.That(file.Exists).IsEqualTo(true);
    }

    [Test]
    public async Task VerifyBytes()
    {
        var bytes1 = await File.ReadAllBytesAsync("file1.txt");
        var bytes2 = await File.ReadAllBytesAsync("file2.txt");
        await Assert.That(bytes1.SequenceEqual(bytes2)).IsEqualTo(true);
    }
}
```

## Notes

- TUnit assertions are async and must be awaited
- TUnit prefers async test methods even if they don't use `await`
- Byte array comparisons require `SequenceEqual` - this is a common gotcha
- `Assert.DoesNotThrow` doesn't exist in TUnit - just call the method directly
- TestHarness base classes are compatible with TUnit
- Performance improvements are significant, especially for async operations

---

**Last Updated**: 2026-01-22  
**TUnit Version**: 1.12.15
