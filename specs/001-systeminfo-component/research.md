# Research: SystemInfo Razor Component

**Feature**: SystemInfo Razor Component  
**Date**: 2025-01-12  
**Phase**: 0 - Outline & Research

## Research Questions

### Q1: Fluent UI Blazor Components Compatibility with net48

**Question**: Is Microsoft.FluentUI.AspNetCore.Components compatible with .NET Framework 4.8 (net48)?

**Research Findings**:
- Microsoft.FluentUI.AspNetCore.Components requires .NET 6.0 or later
- .NET Framework 4.8 is not supported by Fluent UI Blazor components
- Blazor components require .NET 6+ runtime

**Decision**: Fluent UI package will be conditionally included only for net8.0 and net8.0-windows targets. The net48 target will not include the SystemInfo component, or the component will be excluded from net48 builds using conditional compilation.

**Rationale**: Fluent UI Blazor components are modern .NET components that require .NET 6+ runtime. .NET Framework 4.8 does not support Blazor components.

**Alternatives Considered**:
1. Exclude SystemInfo component from net48 builds - **CHOSEN** (maintains library compatibility)
2. Create separate WPF component for net48 - **REJECTED** (out of scope, adds complexity)
3. Remove net48 support entirely - **REJECTED** (breaks existing compatibility)

**Implementation Approach**: Use conditional compilation directives to exclude the component from net48 builds:
```xml
<ItemGroup Condition="'$(TargetFramework)' != 'net48'">
  <PackageReference Include="Microsoft.FluentUI.AspNetCore.Components" Version="4.13.2" />
</ItemGroup>
```

### Q2: System Information APIs in Blazor WebAssembly

**Question**: How do .NET Environment APIs behave in Blazor WebAssembly environments?

**Research Findings**:
- `Environment.UserName` returns empty string in WASM (browser security restriction)
- `Environment.MachineName` returns empty string in WASM
- `Environment.UserDomainName` returns empty string in WASM
- `RuntimeInformation.OSDescription` returns browser user agent information in WASM
- Browser security model prevents access to local system information

**Decision**: Component will gracefully handle empty/null values from Environment APIs. In WASM environments, display "Not available" or similar placeholder text for restricted information. OS description will show browser information when available.

**Rationale**: Browser security restrictions are expected and documented. Component must degrade gracefully without errors.

**Alternatives Considered**:
1. Hide fields that are unavailable - **REJECTED** (inconsistent UI)
2. Show placeholder text - **CHOSEN** (clear user communication)
3. Use JavaScript interop to attempt retrieval - **REJECTED** (still restricted by browser security)

**Implementation Approach**: Check for empty strings and display appropriate fallback text:
```csharp
private string UserName => string.IsNullOrEmpty(Environment.UserName) 
    ? "Not available" 
    : Environment.UserName;
```

### Q3: Razor Component Structure in Class Library

**Question**: How should Razor components be structured in a .NET class library?

**Research Findings**:
- Razor components can be included in regular class libraries using `Microsoft.NET.Sdk`
- Components must be in `.razor` files
- Components are compiled to C# classes
- Namespace follows directory structure
- Components can be referenced by host applications via project reference

**Decision**: Create `source/Node.Net/Components/` directory and place `SystemInfo.razor` there. Component namespace will be `Node.Net.Components`.

**Rationale**: Standard Razor component organization pattern. Keeps components organized and discoverable.

**Alternatives Considered**:
1. Flat structure in source root - **REJECTED** (poor organization)
2. Separate Razor Class Library project - **REJECTED** (unnecessary complexity for single component)

### Q4: Profile Picture Image Loading Error Handling

**Question**: How should the component handle profile picture image load failures?

**Research Findings**:
- HTML `<img>` tag supports `onerror` attribute for fallback
- Blazor components can use `@onerror` event handler
- Fluent UI components may have built-in error handling

**Decision**: Use `@onerror` event handler to fall back to default person icon when image fails to load.

**Rationale**: Provides seamless user experience without broken image placeholders.

**Alternatives Considered**:
1. Show broken image icon - **REJECTED** (poor UX)
2. Hide image entirely - **REJECTED** (inconsistent layout)
3. Fallback to icon - **CHOSEN** (consistent UX)

### Q5: Fluent UI Package Version Selection

**Question**: What version of Microsoft.FluentUI.AspNetCore.Components should be used?

**Research Findings**:
- Example applications use version 4.13.2
- Latest stable version should be verified
- Version consistency across projects is important

**Decision**: Use version 4.13.2 to match existing example applications.

**Rationale**: Maintains consistency with existing codebase and reduces potential compatibility issues.

**Alternatives Considered**:
1. Use latest version - **REJECTED** (may introduce breaking changes)
2. Use specific version matching examples - **CHOSEN** (consistency)

## Summary

All research questions resolved. Key decisions:
1. Fluent UI components excluded from net48 builds (not supported)
2. WASM environment restrictions handled with graceful fallbacks
3. Component structure follows standard Razor organization
4. Image error handling uses fallback to default icon
5. Package version matches existing examples (4.13.2)
