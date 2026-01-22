# Quickstart: System.Windows Types Always Available

**Feature**: Enable transparent type access in tests for System.Windows types  
**Date**: 2025-01-27

## Validation Scenarios

This document provides validation scenarios to verify that transparent type access has been correctly implemented.

### Scenario 1: Verify Global Usings Configuration

**Objective**: Verify that global usings are configured correctly in the test project

**Steps**:
1. Open `tests/Node.Net.Test/GlobalUsings.cs`
2. Verify it contains global using statements for `System.Windows.*` namespaces:
   - `global using System.Windows;`
   - `global using System.Windows.Media;`
   - `global using System.Windows.Media.Imaging;`
   - `global using System.Windows.Media.Media3D;`

**Expected Result**: 
- `GlobalUsings.cs` file exists
- Contains all necessary global using statements
- No syntax errors

**Validation Command**:
```bash
cat tests/Node.Net.Test/GlobalUsings.cs
```

---

### Scenario 2: Verify Extern Alias Removal

**Objective**: Verify that `extern alias` has been removed from the project reference

**Steps**:
1. Open `tests/Node.Net.Test/Node.Net.Test.csproj`
2. Find the project reference to `source/Node.Net/Node.Net.csproj`
3. Verify that `<Aliases>NodeNet</Aliases>` is not present

**Expected Result**:
- Project reference exists
- No `<Aliases>` element in the project reference
- Project reference format: `<ProjectReference Include="..\..\source\Node.Net\Node.Net.csproj" />`

**Validation Command**:
```bash
grep -A 2 "ProjectReference.*Node.Net.csproj" tests/Node.Net.Test/Node.Net.Test.csproj
```

---

### Scenario 3: Verify Test File Refactoring

**Objective**: Verify that test files use standard namespace references

**Steps**:
1. Open `tests/Node.Net.Test/Vector3D.Test.cs`
2. Verify:
   - No `extern alias NodeNet;` declaration
   - No `NodeNet::` prefixes in type references
   - Uses standard `using System.Windows.Media.Media3D;` or relies on global usings
   - Type references use standard syntax: `Vector3D` instead of `NodeNet::System.Windows.Media.Media3D.Vector3D`

**Expected Result**:
- File uses standard namespace references
- No `extern alias` or `NodeNet::` prefixes
- Code is more readable

**Validation Commands**:
```bash
# Check for extern alias (should find none)
grep -r "extern alias NodeNet" tests/Node.Net.Test/

# Check for NodeNet:: prefixes (should find none)
grep -r "NodeNet::" tests/Node.Net.Test/
```

---

### Scenario 4: Build Verification (net8.0)

**Objective**: Verify the test project builds successfully for net8.0 target framework

**Steps**:
1. Open terminal in repository root
2. Run: `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0`
3. Verify build completes with zero errors

**Expected Result**: 
- Build succeeds
- No compilation errors
- No ambiguous type reference errors
- No missing type errors

**Validation Command**:
```bash
dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0
```

---

### Scenario 5: Build Verification (net8.0-windows)

**Objective**: Verify the test project builds successfully for net8.0-windows target framework (Windows only)

**Steps**:
1. Open terminal in repository root (on Windows machine)
2. Run: `dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows`
3. Verify build completes with zero errors

**Expected Result**:
- Build succeeds
- No compilation errors
- Platform types are used (from WPF/PresentationCore)
- No namespace conflicts

**Validation Command**:
```bash
dotnet build tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows
```

---

### Scenario 6: Test Execution (net8.0)

**Objective**: Verify all tests pass on net8.0 target framework

**Steps**:
1. Open terminal in repository root
2. Run: `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0`
3. Verify all tests pass

**Expected Result**:
- All tests pass
- No test failures related to type resolution
- Extension methods work correctly
- Custom implementations are used (non-Windows target)

**Validation Command**:
```bash
dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0
```

---

### Scenario 7: Test Execution (net8.0-windows)

**Objective**: Verify all tests pass on net8.0-windows target framework (Windows only)

**Steps**:
1. Open terminal in repository root (on Windows machine)
2. Run: `dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows`
3. Verify all tests pass

**Expected Result**:
- All tests pass
- No test failures related to type resolution
- Extension methods work correctly
- Platform types are used (Windows target)

**Validation Command**:
```bash
dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows
```

---

### Scenario 8: Extension Methods Verification

**Objective**: Verify extension methods work with both platform and custom types

**Steps**:
1. Run tests that use extension methods on `System.Windows.*` types
2. Verify extension methods work on both target frameworks
3. Check that extension methods are found and invoked correctly

**Expected Result**:
- Extension methods work on net8.0 (custom types)
- Extension methods work on net8.0-windows (platform types)
- No "method not found" errors
- Extension methods produce identical results

**Validation Commands**:
```bash
# Run extension method tests
dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0 --filter "FullyQualifiedName~Extension"

# On Windows, also test with platform types
dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0-windows --filter "FullyQualifiedName~Extension"
```

---

### Scenario 9: Type Resolution Verification

**Objective**: Verify that types resolve correctly on different target frameworks

**Steps**:
1. Create a simple test that uses `System.Windows.Media.Media3D.Vector3D`
2. Run the test on net8.0 (should use custom implementation)
3. Run the test on net8.0-windows (should use platform type)
4. Verify both produce identical results

**Expected Result**:
- Types resolve correctly on both frameworks
- No ambiguous type reference errors
- Behavior is identical regardless of implementation source

**Validation**: Use existing `Vector3D.Test.cs` as verification - it should work on both frameworks without modification.

---

## Troubleshooting

### Issue: Ambiguous Type Reference Errors

**Symptom**: Compiler error: "The type 'Vector3D' exists in both 'PresentationCore' and 'Node.Net'"

**Cause**: Both platform and custom types are being compiled for the same target framework

**Solution**: Verify that conditional compilation is working correctly:
- Check that `IS_WINDOWS` is defined for Windows targets
- Check that custom implementations use `#if !IS_WINDOWS`
- Verify only one set of types exists per target framework

### Issue: Extension Methods Not Found

**Symptom**: Compiler error: "Extension method 'X' not found"

**Cause**: Extension methods may not be resolving correctly

**Solution**: 
- Verify extension methods are in the `Node.Net` namespace
- Ensure `using Node.Net;` is present (or in global usings)
- Check that the type's public API matches the extension method's target type

### Issue: Tests Fail After Refactoring

**Symptom**: Tests that previously passed now fail

**Cause**: Type resolution may be using different types than expected

**Solution**:
- Verify that the correct types are being used (platform vs custom)
- Check that API contracts match exactly
- Ensure extension methods are working correctly
- Compare test results before and after refactoring

---

## Success Criteria

All scenarios should pass with:
- ✅ No compilation errors
- ✅ All tests pass on all target frameworks
- ✅ No ambiguous type references
- ✅ Extension methods work correctly
- ✅ Code is more readable (no `NodeNet::` prefixes)
