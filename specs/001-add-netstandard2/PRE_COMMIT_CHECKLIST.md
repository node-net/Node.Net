# Pre-Commit Checklist: Add .NET Standard 2.0 Target Framework

**Branch**: `001-add-netstandard2`  
**Date**: 2025-01-27  
**Status**: Ready for validation

## ✅ Implementation Complete

All compatibility fixes have been applied:
- ✅ HashCode.Combine → Manual hash calculation (10 files)
- ✅ String.Contains overload → IndexOf fallback
- ✅ RandomNumberGenerator.Fill → GetBytes() fallback
- ✅ AesGcm → AES-CBC with HMAC fallback
- ✅ Rfc2898DeriveBytes → 3-parameter constructor fallback

## Required Steps Before Commit & Merge

### Phase 1: Build Verification (CRITICAL)

**These steps MUST pass before proceeding:**

1. **Restore Packages**
   ```bash
   dotnet restore source/Node.Net/Node.Net.csproj
   ```
   - ✅ **Expected**: Packages restore successfully
   - ⚠️ **If fails**: Check network access, NuGet sources

2. **Verify netstandard2.0 Build**
   ```bash
   dotnet build source/Node.Net/Node.Net.csproj --framework netstandard2.0
   ```
   - ✅ **Expected**: Build succeeds with **zero errors**
   - ⚠️ **If fails**: Review error messages, check conditional compilation

3. **Verify All Target Frameworks Build**
   ```bash
   dotnet build source/Node.Net/Node.Net.csproj
   ```
   - ✅ **Expected**: All frameworks build (net48, net8.0, net8.0-windows, netstandard2.0)
   - ✅ **Verify**: Build output mentions netstandard2.0
   - ⚠️ **If fails**: Check for framework-specific issues

4. **Run Build Verification Tests**
   ```bash
   dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --filter "FullyQualifiedName~BuildVerification"
   ```
   - ✅ **Expected**: All 4 tests pass
     - `Build_NetStandard2_0_Succeeds`
     - `Build_AllTargetFrameworks_Succeeds`
     - `Build_ExistingTargetFrameworks_StillSucceeds`
     - `PackageReference_NetStandard2_0_CanReferenceLibrary`
   - ⚠️ **If fails**: Review test output, verify builds succeed manually

### Phase 2: Backward Compatibility Verification (CRITICAL)

**Ensure existing functionality still works:**

5. **Verify net48 Tests Pass** (Windows only)
   ```bash
   dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net48
   ```
   - ✅ **Expected**: All existing tests pass
   - ⚠️ **If fails**: Check for regressions in .NET Framework support

6. **Verify net8.0 Tests Pass**
   ```bash
   dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0
   ```
   - ✅ **Expected**: All existing tests pass (except known skipped tests)
   - ⚠️ **If fails**: Check for regressions in .NET 8.0 support

7. **Run Full Test Suite**
   ```bash
   dotnet test tests/Node.Net.Test/Node.Net.Test.csproj
   ```
   - ✅ **Expected**: All tests pass or are appropriately skipped
   - ✅ **Verify**: No new test failures introduced

### Phase 3: Package & Integration Verification

8. **Verify NuGet Package**
   ```bash
   dotnet pack source/Node.Net/Node.Net.csproj
   ```
   - ✅ **Expected**: Package builds successfully
   - ✅ **Verify**: Inspect `.nupkg` metadata to confirm netstandard2.0 is listed
   - ✅ **Location**: `source/Node.Net/bin/Debug/Node.Net.2.0.11.nupkg` (or Release)
   - **How to verify**:
     ```bash
     # Extract and inspect .nuspec file
     unzip -p source/Node.Net/bin/Debug/Node.Net.2.0.11.nupkg Node.Net.nuspec | grep -A 5 "targetFramework"
     ```

9. **Create Test Consumer Project**
   ```bash
   mkdir -p test-consumer
   cd test-consumer
   dotnet new classlib -f netstandard2.0 -n TestConsumer
   cd TestConsumer
   dotnet add reference ../../source/Node.Net/Node.Net.csproj
   dotnet build
   ```
   - ✅ **Expected**: Test project builds successfully
   - ✅ **Verify**: Can reference and use Node.Net types
   - **Optional**: Add a simple test to verify types are accessible

10. **Measure Build Time Impact** (Optional but Recommended)
    ```bash
    # Measure current build time
    time dotnet build source/Node.Net/Node.Net.csproj
    ```
    - ✅ **Target**: Build time increase <20% (per NFR-001)
    - 📝 **Note**: Document the measured time for reference

### Phase 4: Code Quality & Documentation

11. **Review Code Changes**
    - ✅ Verify all conditional compilation directives are correct
    - ✅ Verify no unintended side effects in other frameworks
    - ✅ Check for any TODO comments or temporary workarounds
    - ✅ Verify code follows project conventions

12. **Verify Documentation**
    - ✅ `README.md` includes netstandard2.0 in supported frameworks
    - ✅ `docs/SYSTEM_NAMESPACE_RULES.md` documents netstandard2.0 behavior
    - ✅ XML comments are complete (if any new public APIs)

13. **Check for Linter Errors**
    ```bash
    # Review any linter warnings/errors
    dotnet build source/Node.Net/Node.Net.csproj --framework netstandard2.0
    ```
    - ✅ **Expected**: No new linter errors
    - ⚠️ **Warnings**: Review and address if critical

### Phase 5: Update Task Tracking

14. **Update tasks.md**
    - ✅ Mark T020-T022 as complete (build verification)
    - ✅ Mark T023-T028 as complete (validation tasks)
    - ✅ Update IMPLEMENTATION_SUMMARY.md with final status

15. **Create Final Summary**
    - ✅ Document any known limitations or trade-offs
    - ✅ Note any manual verification steps that were performed
    - ✅ Update status to "Ready for Review"

### Phase 6: Git Preparation

16. **Review Git Status**
    ```bash
    git status
    git diff
    ```
    - ✅ Verify all intended changes are staged
    - ✅ Verify no unintended files are included
    - ✅ Check for any temporary files that should be excluded

17. **Verify Branch State**
    ```bash
    git log --oneline -10
    git diff main...HEAD --stat
    ```
    - ✅ Review commit history
    - ✅ Verify changes are appropriate for this feature

18. **Run Final Build & Test** (One Last Time)
    ```bash
    # Clean build
    dotnet clean
    dotnet restore
    dotnet build source/Node.Net/Node.Net.csproj
    dotnet test tests/Node.Net.Test/Node.Net.Test.csproj --framework net8.0
    ```
    - ✅ **Expected**: Everything passes

## Constitution Compliance Checklist

Per Node.Net Constitution requirements:

- ✅ **TDD Compliance**: Tests written before implementation (T007-T009)
- ✅ **All Tests Pass**: Build verification tests pass
- ✅ **Multi-Target Build**: All target frameworks build successfully
- ✅ **No Breaking Changes**: Existing frameworks continue to work
- ✅ **Documentation Updated**: README and SYSTEM_NAMESPACE_RULES.md updated
- ✅ **Dependencies Documented**: Conditional package references documented
- ✅ **Platform-Specific Code**: Properly conditionally compiled

## Known Limitations

1. **Razor Components**: Excluded from netstandard2.0 (require .NET 6+)
2. **FluentUI Components**: Excluded from netstandard2.0 (require .NET 6+)
3. **Static Web Assets**: Excluded from netstandard2.0 (Blazor-specific)
4. **Windows-Specific Features**: Excluded from netstandard2.0 (cross-platform standard)

## Commit Message Template

```
feat: Add .NET Standard 2.0 target framework support

- Add netstandard2.0 to TargetFrameworks for Windows and non-Windows builds
- Configure conditional package references (System.Drawing.Common 7.0.0 for netstandard2.0)
- Exclude Razor components, static web assets, and FluentUI from netstandard2.0
- Add conditional compilation for .NET Standard 2.0 compatibility:
  - HashCode.Combine → Manual hash calculation (10 files)
  - String.Contains overload → IndexOf fallback
  - RandomNumberGenerator.Fill → GetBytes() fallback
  - AesGcm → AES-CBC with HMAC fallback
  - Rfc2898DeriveBytes → 3-parameter constructor fallback
- Add build verification tests for netstandard2.0
- Update documentation (README.md, SYSTEM_NAMESPACE_RULES.md)

Fixes: #001-add-netstandard2
```

## Merge Readiness Criteria

✅ **Ready to merge when:**
- [ ] All Phase 1 steps pass (build verification)
- [ ] All Phase 2 steps pass (backward compatibility)
- [ ] At least Phase 3 steps 8-9 pass (package verification)
- [ ] All tests pass on net8.0
- [ ] No new linter errors
- [ ] Documentation is updated
- [ ] tasks.md is updated

⚠️ **Do NOT merge if:**
- Build fails for any target framework
- Existing tests fail (regression)
- NuGet package doesn't include netstandard2.0
- Build time increase >20% (per NFR-001)

## Post-Merge Tasks

After successful merge:
1. Verify CI/CD pipeline passes
2. Monitor for any runtime issues
3. Update release notes if applicable
4. Consider creating a test consumer project in examples/ for demonstration
