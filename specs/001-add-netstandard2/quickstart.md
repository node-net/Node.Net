# Quickstart: Add .NET Standard 2.0 Target Framework

**Feature**: Add netstandard2.0 support to Node.Net library  
**Date**: 2025-01-27

## Validation Scenarios

This document provides validation scenarios to verify that netstandard2.0 support has been correctly implemented.

### Scenario 1: Build Verification

**Objective**: Verify the library builds successfully for netstandard2.0 target framework

**Steps**:
1. Open terminal in repository root
2. Run: `dotnet build source/Node.Net/Node.Net.csproj --framework netstandard2.0`
3. Verify build completes with zero errors

**Expected Result**: 
- Build succeeds
- No compilation errors
- No warnings related to missing APIs or incompatible packages

**Validation Command**:
```bash
dotnet build source/Node.Net/Node.Net.csproj --framework netstandard2.0
```

---

### Scenario 2: Multi-Target Build Verification

**Objective**: Verify all target frameworks build successfully together

**Steps**:
1. Open terminal in repository root
2. Run: `dotnet build source/Node.Net/Node.Net.csproj`
3. Verify all target frameworks build (net48, net8.0, net8.0-windows, netstandard2.0)

**Expected Result**:
- All target frameworks build successfully
- No regressions in existing target frameworks
- Build output shows successful builds for all targets

**Validation Command**:
```bash
dotnet build source/Node.Net/Node.Net.csproj
```

---

### Scenario 3: Package Reference Test

**Objective**: Verify a consumer project targeting .NET Standard 2.0 can reference and use the library

**Steps**:
1. Create a new test project targeting .NET Standard 2.0:
   ```bash
   mkdir test-consumer
   cd test-consumer
   dotnet new classlib -f netstandard2.0 -n TestConsumer
   ```
2. Add reference to Node.Net package (or project reference):
   ```bash
   dotnet add TestConsumer/TestConsumer.csproj reference ../source/Node.Net/Node.Net.csproj
   ```
3. Create a simple test file that uses Node.Net APIs:
   ```csharp
   using Node.Net;
   // Use some core Node.Net functionality
   ```
4. Build the test project:
   ```bash
   dotnet build TestConsumer/TestConsumer.csproj
   ```

**Expected Result**:
- Test project builds successfully
- Can reference and use Node.Net types
- No compilation errors

**Validation Commands**:
```bash
# Create test project
mkdir -p test-consumer
cd test-consumer
dotnet new classlib -f netstandard2.0 -n TestConsumer

# Add reference
dotnet add TestConsumer/TestConsumer.csproj reference ../source/Node.Net/Node.Net.csproj

# Build
dotnet build TestConsumer/TestConsumer.csproj
```

---

### Scenario 4: NuGet Package Metadata Verification

**Objective**: Verify NuGet package correctly identifies netstandard2.0 as supported framework

**Steps**:
1. Build the NuGet package:
   ```bash
   dotnet pack source/Node.Net/Node.Net.csproj
   ```
2. Inspect the generated .nupkg file (or use NuGet Package Explorer)
3. Verify netstandard2.0 appears in supported frameworks

**Expected Result**:
- Package builds successfully
- Package metadata includes netstandard2.0 in supported frameworks
- All existing target frameworks still listed

**Validation Command**:
```bash
dotnet pack source/Node.Net/Node.Net.csproj
# Then inspect the .nupkg file or use: nuget spec <package>.nupkg
```

---

### Scenario 5: Conditional Compilation Verification

**Objective**: Verify platform-specific features are excluded from netstandard2.0 builds

**Steps**:
1. Build for netstandard2.0:
   ```bash
   dotnet build source/Node.Net/Node.Net.csproj --framework netstandard2.0
   ```
2. Verify Razor components are excluded (check build output or obj folder)
3. Verify static web assets are excluded
4. Verify Windows-specific packages are excluded

**Expected Result**:
- Razor components not included in netstandard2.0 build
- Static web assets not included in netstandard2.0 build
- Windows-specific packages (Microsoft.Windows.SDK.Contracts, FluentUI) not referenced

**Validation Command**:
```bash
dotnet build source/Node.Net/Node.Net.csproj --framework netstandard2.0 -v detailed
# Check build output for excluded items
```

---

### Scenario 6: Existing Tests Verification

**Objective**: Verify all existing tests continue to pass for other target frameworks

**Steps**:
1. Run tests for net48:
   ```bash
   dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net48
   ```
2. Run tests for net8.0:
   ```bash
   dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0
   ```
3. Verify all tests pass

**Expected Result**:
- All existing tests pass for net48
- All existing tests pass for net8.0
- No test regressions introduced

**Validation Commands**:
```bash
dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net48
dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0
```

---

### Scenario 7: Build Time Impact

**Objective**: Verify build time increase is within acceptable limits (<20% per NFR-001)

**Steps**:
1. Measure baseline build time (without netstandard2.0):
   - Note: This requires building from a commit before netstandard2.0 addition
2. Measure build time with netstandard2.0:
   ```bash
   time dotnet build source/Node.Net/Node.Net.csproj
   ```
3. Calculate percentage increase

**Expected Result**:
- Build time increase is <20%
- Multi-targeting overhead is acceptable

**Validation Command**:
```bash
time dotnet build source/Node.Net/Node.Net.csproj
```

---

## Success Criteria Checklist

- [ ] Scenario 1: netstandard2.0 builds successfully
- [ ] Scenario 2: All target frameworks build together
- [ ] Scenario 3: Consumer project can reference library
- [ ] Scenario 4: NuGet package includes netstandard2.0
- [ ] Scenario 5: Platform-specific features excluded
- [ ] Scenario 6: Existing tests pass
- [ ] Scenario 7: Build time impact acceptable

## Troubleshooting

### Build Errors

**Issue**: Package not found for netstandard2.0  
**Solution**: Verify package supports .NET Standard 2.0, use conditional package references

**Issue**: API not available in .NET Standard 2.0  
**Solution**: Add conditional compilation directive `#if !NETSTANDARD2_0`

### Package Reference Issues

**Issue**: Consumer project cannot reference library  
**Solution**: Verify NuGet package was built with netstandard2.0, check package metadata

**Issue**: Missing dependencies in consumer project  
**Solution**: Verify transitive dependencies are compatible with .NET Standard 2.0
