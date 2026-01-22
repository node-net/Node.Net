# Research: Add .NET Standard 2.0 Target Framework

**Date**: 2025-01-27  
**Feature**: Add netstandard2.0 support to Node.Net.csproj

## Research Questions

### 1. Package Dependency Compatibility

#### LiteDB 5.0.17
**Research Task**: Verify LiteDB 5.0.17 supports .NET Standard 2.0

**Decision**: **LiteDB 5.0.17 supports .NET Standard 2.0**
- Rationale: LiteDB has supported .NET Standard 2.0 since version 4.x. Version 5.0.17 maintains this support.
- Alternatives considered: None needed - package is compatible
- Action: Include LiteDB 5.0.17 for netstandard2.0 builds

#### System.Drawing.Common 8.0.2
**Research Task**: Verify System.Drawing.Common 8.0.2 supports .NET Standard 2.0

**Decision**: **System.Drawing.Common 8.0.2 may require version adjustment for .NET Standard 2.0**
- Rationale: System.Drawing.Common 8.0.x targets .NET 6.0+. For .NET Standard 2.0, we need version 6.x or 7.x which support .NET Standard 2.0.
- Alternatives considered:
  - Use System.Drawing.Common 7.0.0 (last version with .NET Standard 2.0 support)
  - Conditionally exclude System.Drawing.Common for netstandard2.0 if not critical
- Action: Use conditional package reference: System.Drawing.Common 7.0.0 for netstandard2.0, 8.0.2 for net8.0+

#### Microsoft.Extensions.Logging 8.0.1
**Research Task**: Verify Microsoft.Extensions.Logging 8.0.1 supports .NET Standard 2.0

**Decision**: **Microsoft.Extensions.Logging 8.0.1 supports .NET Standard 2.0**
- Rationale: Microsoft.Extensions.* packages maintain .NET Standard 2.0 support for backward compatibility. Version 8.0.1 includes netstandard2.0 target.
- Alternatives considered: None needed - package is compatible
- Action: Include Microsoft.Extensions.Logging 8.0.1 for netstandard2.0 builds

#### Microsoft.Extensions.Logging.Abstractions 8.0.3
**Research Task**: Verify Microsoft.Extensions.Logging.Abstractions 8.0.3 supports .NET Standard 2.0

**Decision**: **Microsoft.Extensions.Logging.Abstractions 8.0.3 supports .NET Standard 2.0**
- Rationale: Same as Microsoft.Extensions.Logging - abstractions package maintains .NET Standard 2.0 support.
- Alternatives considered: None needed - package is compatible
- Action: Include Microsoft.Extensions.Logging.Abstractions 8.0.3 for netstandard2.0 builds

### 2. Conditional Compilation Constants

**Research Task**: Determine appropriate preprocessor symbol for .NET Standard 2.0

**Decision**: **Use built-in `NETSTANDARD2_0` symbol**
- Rationale: MSBuild automatically defines `NETSTANDARD2_0` when targeting netstandard2.0. No manual definition needed.
- Alternatives considered:
  - Custom symbol (e.g., `IS_NETSTANDARD2`) - rejected as unnecessary
  - Framework-specific symbols - rejected as less standard
- Action: Use `#if NETSTANDARD2_0` / `#if !NETSTANDARD2_0` directives

### 3. Razor Components and Static Web Assets

**Research Task**: Verify Razor SDK behavior with .NET Standard 2.0

**Decision**: **Exclude Razor components and static web assets from netstandard2.0 builds**
- Rationale: Razor components require .NET 6+ (Blazor). .NET Standard 2.0 predates Blazor support. Static web assets are Blazor-specific.
- Alternatives considered: None - Razor/Blazor not supported on .NET Standard 2.0
- Action: Use same exclusion pattern as net48:
  ```xml
  <ItemGroup Condition=" '$(TargetFramework)' == 'netstandard2.0' ">
    <Content Remove="Components\*.razor" />
    <None Include="Components\*.razor" />
    <!-- Exclude static web assets -->
  </ItemGroup>
  ```

### 4. Build System Configuration

**Research Task**: Verify MSBuild multi-targeting with netstandard2.0

**Decision**: **MSBuild fully supports netstandard2.0 in multi-targeting scenarios**
- Rationale: .NET Standard 2.0 is a well-established target framework. MSBuild has native support for it in SDK-style projects.
- Alternatives considered: None - standard MSBuild feature
- Action: Add netstandard2.0 to TargetFrameworks property with appropriate conditions

## Summary of Decisions

| Component | Decision | Rationale |
|-----------|----------|-----------|
| LiteDB | Include 5.0.17 | Compatible with .NET Standard 2.0 |
| System.Drawing.Common | Use 7.0.0 for netstandard2.0, 8.0.2 for net8.0+ | Version 8.0.x requires .NET 6.0+ |
| Microsoft.Extensions.Logging | Include 8.0.1 | Compatible with .NET Standard 2.0 |
| Microsoft.Extensions.Logging.Abstractions | Include 8.0.3 | Compatible with .NET Standard 2.0 |
| Conditional Compilation | Use `NETSTANDARD2_0` symbol | Built-in MSBuild symbol |
| Razor Components | Exclude from netstandard2.0 | Requires .NET 6+ |
| Static Web Assets | Exclude from netstandard2.0 | Blazor-specific, requires .NET 6+ |
| Build System | Standard multi-targeting | MSBuild native support |

## Implementation Notes

1. **Package Version Strategy**: Use conditional package references to include appropriate versions per target framework
2. **Code Changes**: Minimal - primarily conditional compilation for platform-specific features already using IS_WINDOWS
3. **Testing**: Create test project targeting .NET Standard 2.0 to verify package reference and core functionality
