# Research: Refactor to Nested Source Structure

**Feature**: Refactor to Nested Source Structure  
**Date**: 2025-01-12  
**Phase**: Phase 0 - Outline & Research

## Research Tasks

### 1. Git History Preservation for Nested Moves

**Decision**: Use `git mv` command to preserve file history for nested directory moves

**Rationale**: 
- `git mv` maintains full file history across moves, including nested directory structures
- Git automatically tracks renames when files are moved into subdirectories
- No data loss or history fragmentation
- Standard practice for structural refactorings
- Works correctly for moving files from `source/` to `source/Node.Net/` and `tests/` to `source/Node.Net.Test/`

**Alternatives Considered**:
- Manual copy + delete: Would lose git history
- Git filter-branch: Overly complex for simple directory moves
- Third-party tools: Unnecessary for standard git operations

### 2. Solution File Update Strategy for Nested Paths

**Decision**: Manually update solution file paths to nested structure after directory moves

**Rationale**:
- Visual Studio solution files are text-based and straightforward to update
- Path updates are simple string replacements: `source/Node.Net.csproj` → `source/Node.Net/Node.Net.csproj`
- Solution file format is well-documented
- No risk of breaking solution structure

**Alternatives Considered**:
- Automated tooling: Unnecessary complexity for one-time change
- Recreate solution: Would lose project configuration and build settings

### 3. Build Script Path Updates for Nested Structure

**Decision**: Update Rakefile paths to nested structure after directory moves

**Rationale**:
- Build script has explicit paths to `source/` and `tests/`
- Must update to `source/Node.Net/` and `source/Node.Net.Test/`
- Simple path string replacements required
- Script logic remains unchanged
- Cross-platform compatibility maintained

**Alternatives Considered**:
- Environment variables: Adds complexity without benefit
- Relative path detection: Current explicit paths are clearer

### 4. Project Reference Updates for Nested Structure

**Decision**: Update ProjectReference paths in test project after move

**Rationale**:
- Test project currently has `<ProjectReference Include="..\source\Node.Net.csproj" />`
- After move to `source/Node.Net.Test/`, must update to `..\Node.Net\Node.Net.csproj` (sibling directories under `source/`)
- Simple XML path update
- MSBuild will validate references during build

**Alternatives Considered**:
- Absolute paths: Would break portability
- Environment variables: Unnecessary complexity

### 5. CI/CD Configuration Updates

**Decision**: Check and update GitHub Actions workflows if they exist

**Rationale**:
- CI/CD pipelines may have hardcoded paths
- Must verify no workflows reference old directory structure
- Update any path references found to nested structure

**Alternatives Considered**:
- Ignore CI/CD: Would break automated builds
- Recreate workflows: Unnecessary if paths are only change needed

### 6. Documentation Updates

**Decision**: Search and update README and other docs that reference old paths

**Rationale**:
- README may contain examples with old paths
- Documentation should reflect current nested structure
- Prevents confusion for new contributors

**Alternatives Considered**:
- Leave documentation outdated: Would cause confusion
- Automated find/replace: Manual review ensures accuracy

### 7. Empty Directory Handling

**Decision**: Remove empty `tests/` directory after move, keep `source/` directory (contains subdirectories)

**Rationale**:
- `tests/` directory will be empty after moving all content to `source/Node.Net.Test/`
- `source/` directory will contain `Node.Net/` and `Node.Net.Test/` subdirectories
- Empty directories can be removed to clean up structure
- `source/` directory should remain as it contains the project subdirectories

**Alternatives Considered**:
- Keep empty directories: Adds clutter without value
- Remove both: `source/` should remain as parent container

## Technical Decisions Summary

| Decision Area | Decision | Impact |
|--------------|----------|--------|
| Git History | Use `git mv` for nested moves | Preserves full history |
| Solution File | Manual update to nested paths | Simple, low risk |
| Build Scripts | Update paths in Rakefile | Maintains build functionality |
| Project References | Update XML paths (sibling reference) | Maintains project relationships |
| CI/CD | Check and update if needed | Maintains automation |
| Documentation | Search and update | Maintains accuracy |
| Empty Directories | Remove `tests/`, keep `source/` | Clean structure |

## No Blocking Issues

All research tasks resolved. No technical blockers identified. Refactoring can proceed with standard git and build tool operations.
