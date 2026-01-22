# Suggested Commit Message

```
refactor: Remove extern alias, enable transparent System.Windows type access in tests

Remove the need for `extern alias NodeNet` and `NodeNet::` prefixes in test files
by adding global usings for System.Windows namespaces. All tests can now use
standard namespace syntax to access System.Windows.* types transparently.

Changes:
- Remove <Aliases>NodeNet</Aliases> from test project reference
- Add GlobalUsings.cs with System.Windows namespace global usings
- Refactor all 68 test files to use standard namespace references
- Fix namespace conflict with LogLevel in LogService.Integration.Tests.cs
- Update docs/SYSTEM_NAMESPACE_RULES.md to reflect new usage pattern

Validation:
- Build succeeds on net8.0 (0 errors, 0 warnings)
- Extension method tests pass (52 tests)
- 0 extern alias declarations remaining (down from 68)
- 0 NodeNet:: references remaining (down from 163)
- All System/Windows test files verified to use standard namespaces

Benefits:
- Improved test code readability (NFR-001)
- No runtime performance overhead (compile-time only changes)
- Transparent type access across all target frameworks
- Maintains 100% backward compatibility with library API

Closes #[issue-number]
```

## Files Changed

### Configuration
- `tests/Node.Net.Test/Node.Net.Test.csproj` - Removed extern alias
- `tests/Node.Net.Test/GlobalUsings.cs` - Created with global usings

### Test Files (68 files refactored)
- All files in `tests/Node.Net.Test/System/Windows/` (20 files)
- All files in `tests/Node.Net.Test/Extension/` (9 files)
- All files in `tests/Node.Net.Test/Components/` (7 files)
- All files in `tests/Node.Net.Test/Service/` (9 files)
- All files in `tests/Node.Net.Test/Collections/` (4 files)
- All files in `tests/Node.Net.Test/JsonRPC/` (3 files)
- Root-level test files (16 files)

### Documentation
- `docs/SYSTEM_NAMESPACE_RULES.md` - Updated to reflect new usage pattern
- `specs/002-system-windows-types-always-available/IMPLEMENTATION_SUMMARY.md` - Created
- `specs/002-system-windows-types-always-available/tasks.md` - Updated with completion status

### Bug Fixes
- `tests/Node.Net.Test/Service/Logging/LogService.Integration.Tests.cs` - Fixed LogLevel namespace conflict

## Statistics

- **Files refactored:** 68
- **extern alias declarations removed:** 68
- **NodeNet:: references removed:** 163
- **Build status:** ✅ Success (0 errors, 0 warnings)
- **Extension tests:** ✅ 52 tests passed
- **Code quality:** ✅ All success criteria met
