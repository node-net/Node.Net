<!--
Sync Impact Report:
Version: 1.0.0 (initial creation)
Ratified: 2025-01-12
Last Amended: 2025-01-12

Principles Created:
- I. Library-First Design
- II. Multi-Targeting Support
- III. Test-First Development (NON-NEGOTIABLE)
- IV. API Stability & Versioning
- V. Cross-Platform Compatibility

Templates Status:
✅ plan-template.md - Constitution Check section will reference these principles
✅ spec-template.md - No direct constitution references
✅ tasks-template.md - No direct constitution references
✅ checklist-template.md - No direct constitution references

Follow-up: None
-->

# Node.Net Constitution

## Core Principles

### I. Library-First Design

Every feature must be designed as a reusable library component. The library must be self-contained, independently testable, and well-documented. Each component must have a clear, single purpose. No organizational-only or utility-only components without clear value proposition. All public APIs must be designed for consumption by external developers.

### II. Multi-Targeting Support

The library MUST support multiple .NET target frameworks (net48, net8.0, net8.0-windows) with appropriate conditional compilation. Windows-specific features must be wrapped in `#if IS_WINDOWS` directives. All non-Windows targets must build and function correctly without Windows dependencies. Platform-specific code must be clearly documented and tested on appropriate platforms.

### III. Test-First Development (NON-NEGOTIABLE)

Test-Driven Development (TDD) is mandatory for all new features and bug fixes. The cycle is strictly enforced: Write tests → Get approval → Verify tests fail → Implement feature → Verify tests pass → Refactor. All public APIs must have corresponding unit tests. Integration tests are required for cross-component interactions and serialization/deserialization workflows.

### IV. API Stability & Versioning

Public APIs must follow semantic versioning (MAJOR.MINOR.PATCH). Breaking changes require MAJOR version increment and migration documentation. New features increment MINOR version. Bug fixes and non-breaking changes increment PATCH version. All public types, methods, and properties must be documented with XML comments. Deprecated APIs must be marked with `[Obsolete]` attributes and removed only in MAJOR versions.

### V. Cross-Platform Compatibility

The library must function correctly on Windows, Linux, and macOS where applicable. Platform-specific code must be isolated and clearly marked. All platform-agnostic features must work identically across platforms. Platform-specific features must gracefully degrade or provide clear error messages when unavailable.

## Technical Standards

### Code Quality

- **Nullable Reference Types**: Enabled (`<Nullable>enable</Nullable>`) - all code must handle nullability correctly
- **Language Version**: Latest C# language features are encouraged but must maintain compatibility with target frameworks
- **Code Style**: Follow .NET coding conventions and use consistent formatting
- **Documentation**: All public APIs require XML documentation comments

### Testing Requirements

- **Framework**: NUnit for all test projects
- **Coverage**: All public APIs must have unit tests
- **Integration Tests**: Required for serialization/deserialization, factory patterns, and cross-component interactions
- **Test Organization**: Tests mirror source structure in `Node.Net.Test` project

### Build & Packaging

- **NuGet Package**: Automatically generated on build (`GeneratePackageOnBuild=true`)
- **Package Metadata**: Must include version, license, icon, authors, and project URL
- **Multi-Targeting**: Build scripts must handle conditional compilation for Windows vs non-Windows targets
- **Version Management**: Version numbers must be updated in `.csproj` file

## Development Workflow

### Feature Development

1. Create feature branch using `/speckit.specify` command
2. Write specification following spec-template.md
3. Create implementation plan using `/speckit.plan`
4. Generate tasks using `/speckit.tasks`
5. Implement following TDD cycle
6. Ensure all tests pass on all target frameworks
7. Update documentation and XML comments
8. Submit for review

### Code Review Requirements

- All PRs must verify constitution compliance
- Tests must pass on all target frameworks
- Breaking changes must be documented and justified
- New dependencies must be approved and documented
- Platform-specific code must be reviewed for cross-platform impact

### Quality Gates

- All tests must pass before merge
- Code must build successfully on all target frameworks
- No breaking changes without MAJOR version increment
- XML documentation must be complete for public APIs
- NuGet package must build and validate successfully

## Governance

This constitution supersedes all other development practices and guidelines. Amendments to this constitution require:

1. Documentation of the proposed change
2. Justification for the amendment
3. Impact analysis on existing code and practices
4. Approval from project maintainers
5. Version increment following semantic versioning
6. Update to `LAST_AMENDED_DATE`

All PRs and code reviews must verify compliance with these principles. Complexity must be justified. When in doubt, prefer simplicity and clarity over clever solutions.

**Version**: 1.0.0 | **Ratified**: 2025-01-12 | **Last Amended**: 2025-01-12
