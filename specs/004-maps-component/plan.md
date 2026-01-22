# Implementation Plan: Maps Component

**Branch**: `004-maps-component` | **Date**: 2025-01-14 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/004-maps-component/spec.md`

**Note**: This template is filled in by the `/speckit.plan` command. See `.specify/templates/commands/plan.md` for the execution workflow.

## Summary

Implement a reusable Maps Razor component that displays a map for a specific latitude and longitude. The component uses Leaflet with OpenStreetMap for map rendering, requires JavaScript interop for map initialization and updates, and supports optional zoom level and map type configuration. The implementation follows library-first design principles, supports multiple .NET target frameworks (net8.0, net8.0-windows), and integrates with both Blazor Server and WebAssembly hosting models. Default behavior: satellite map type, zoom level 13, 100% width/height dimensions, and uses (0, 0) as fallback for invalid coordinates.

## Technical Context

**Language/Version**: C# (Latest language version)  
**Primary Dependencies**: 
- Leaflet JavaScript library (loaded via script tag, no NuGet package)
- Microsoft.FluentUI.AspNetCore.Components 4.13.2 (for consistent styling if applicable - already in project, net8.0+ only)
- Microsoft.JSInterop (for map initialization and updates - built into Blazor)

**Storage**: N/A (no persistent storage required)  
**Testing**: NUnit (as per constitution)  
**Target Platform**: .NET multi-target (net8.0, net8.0-windows)  
**Project Type**: Library component (reusable across applications)  
**Performance Goals**: 
- Map initialization: <2 seconds (reasonable expectation for external tile loading)
- Coordinate updates: <500ms (map center update after parameter change)
- Responsive rendering: Component adapts to container size without performance degradation

**Constraints**: 
- Must work in both server-side (ASP.NET Host) and client-side (WebAssembly) environments
- Component must be reusable and self-contained
- Must follow TDD (tests before implementation)
- Razor components excluded from net48 builds (Fluent UI requires .NET 6+, Leaflet JSInterop requires modern Blazor)
- JavaScript interop required for map functionality
- Internet connectivity required for map tile loading (OpenStreetMap tiles)
- Must handle invalid coordinates gracefully (default to 0, 0)
- Must handle map service unavailability gracefully

**Scale/Scope**: 
- Single reusable component (`Node.Net/Components/Maps.razor`)
- Integration with two example applications
- No service layer required (component is self-contained)
- Coordinate validation logic embedded in component

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

Verify compliance with Node.Net Constitution principles:

- **I. Library-First Design**: ✅ Feature is designed as reusable library component (`Node.Net/Components/Maps.razor`) with clear purpose - displaying maps for geographic coordinates
- **II. Multi-Targeting Support**: ✅ Plan addresses net8.0 and net8.0-windows targets appropriately. Component excluded from net48 builds (Blazor Razor components and Fluent UI require .NET 6+)
- **III. Test-First Development**: ✅ Test strategy defined - unit tests for coordinate validation, component rendering tests with JSInterop setup, integration tests in example applications
- **IV. API Stability & Versioning**: ✅ Public API changes documented - new public component (`Maps.razor`) will be part of next MINOR version increment. Component parameters are public API.
- **V. Cross-Platform Compatibility**: ✅ Platform-specific code identified - JavaScript interop works cross-platform, Leaflet library is browser-based and platform-agnostic. No platform-specific code required.

**Status**: [x] All principles satisfied | [ ] Violations documented below

## Project Structure

### Documentation (this feature)

```text
specs/004-maps-component/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command) - COMPLETE
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
source/Node.Net/
├── Components/
│   └── Maps.razor        # Blazor component for displaying maps (net8.0+ only)
└── [existing structure...]

tests/Node.Net.Test/
└── Components/
    └── Maps.Tests.cs     # Unit tests for Maps component

examples/Node.Net.AspNet.Host/
└── Components/Pages/
    └── Maps.razor        # Integration page using Node.Net.Components.Maps

examples/Node.Net.Wasm/
└── Pages/
    └── Maps.razor        # Integration page using Node.Net.Components.Maps
```

**Structure Decision**: Single reusable component in `source/Node.Net/Components/` following existing component structure (SystemInfo, Logs, ApplicationInfo). No service layer required as component is self-contained. Tests mirror source structure in `tests/Node.Net.Test/Components/`.

## Phase 0: Research & Planning

**Status**: ✅ COMPLETE

Research completed and documented in `research.md`. Key decisions:

- **Map Library Selected**: Leaflet with OpenStreetMap
- **Rationale**: No API key required, lightweight, free and open-source, good Blazor compatibility, sufficient for requirements
- **JavaScript Interop**: Required for map initialization, coordinate updates, zoom changes
- **Testing Strategy**: Component tests require JSInterop setup (similar to Fluent UI components)

## Phase 1: Design & Contracts

### Component API Design

**Component**: `Maps.razor`  
**Namespace**: `Node.Net.Components`

**Parameters**:
- `Latitude` (double, required): Latitude coordinate (-90 to 90). Component throws error if not provided.
- `Longitude` (double, required): Longitude coordinate (-180 to 180). Component throws error if not provided.
- `ZoomLevel` (int, optional): Map zoom level (default: 13 - neighborhood-level view)
- `MapType` (string, optional): Map type (default: "satellite"). Supported values depend on Leaflet tile provider.

**Behavior**:
- Validates coordinates on parameter change
- Invalid coordinates (out of range) default to (0, 0) - Null Island
- Initializes Leaflet map on component initialization
- Updates map center when coordinates change
- Updates zoom level when ZoomLevel parameter changes
- Updates map type when MapType parameter changes
- Handles map loading errors gracefully
- Component dimensions: 100% width, 100% height (fills container)

**JavaScript Interop Methods** (internal):
- `InitializeMap(elementId, latitude, longitude, zoomLevel, mapType)`: Initializes Leaflet map
- `UpdateMapCenter(elementId, latitude, longitude)`: Updates map center coordinates
- `UpdateZoomLevel(elementId, zoomLevel)`: Updates map zoom level
- `UpdateMapType(elementId, mapType)`: Updates map type/tile layer

### Coordinate Validation

**Validation Rules**:
- Latitude must be between -90 and 90 (inclusive)
- Longitude must be between -180 and 180 (inclusive)
- If validation fails, use default coordinates (0, 0)

**Implementation**:
- Validation performed in C# before JSInterop calls
- Invalid coordinates logged (optional) and replaced with defaults
- No error thrown to user - graceful degradation

### Error Handling Strategy

**Map Loading Failures**:
- Network errors: Display user-friendly message or placeholder
- JavaScript errors: Log error, display fallback content
- Tile loading failures: Leaflet handles automatically with retry logic

**Invalid Coordinates**:
- Validate in C# before map initialization
- Use default (0, 0) if invalid
- No user-visible error (graceful degradation)

**Missing Parameters**:
- Latitude/Longitude are required - component throws `ArgumentNullException` or similar

## Phase 2: Implementation Phases

### Phase 2.1: Foundational Setup
- Add Leaflet CSS and JavaScript references (via script tags in host applications or component)
- Create Maps.razor component structure
- Implement coordinate validation logic
- Set up JSInterop service for map operations

### Phase 2.2: Core Map Display
- Implement map initialization via JSInterop
- Implement coordinate parameter binding
- Implement default behavior (zoom 13, satellite, 100% dimensions)
- Add error handling for map loading

### Phase 2.3: Configuration Options
- Implement optional ZoomLevel parameter
- Implement optional MapType parameter
- Handle parameter changes (update map when parameters change)

### Phase 2.4: Testing
- Write unit tests for coordinate validation
- Write component rendering tests (with JSInterop setup)
- Write integration tests in example applications
- Test invalid coordinate handling
- Test parameter change handling

### Phase 2.5: Integration & Polish
- Integrate into example applications
- Add XML documentation
- Verify cross-platform compatibility
- Performance testing
- Final validation

## Technical Decisions

### Leaflet Integration Approach

**Decision**: Load Leaflet via script tags in host applications (not via NuGet package)

**Rationale**:
- Leaflet is a JavaScript library, not a .NET library
- Script tag loading is standard for JavaScript libraries in Blazor
- Allows host applications to control Leaflet version
- No NuGet package dependency required

**Implementation**:
- Host applications add Leaflet CSS and JS to `wwwroot/index.html` or `_Host.cshtml`
- Component assumes Leaflet is available globally
- JSInterop calls use `window.L` (Leaflet global object)

### JSInterop Service Pattern

**Decision**: Create internal JSInterop service class for map operations

**Rationale**:
- Encapsulates all JavaScript interop calls
- Makes testing easier (can mock JSInterop)
- Centralizes error handling for JSInterop calls
- Follows separation of concerns

**Implementation**:
- Internal class `MapsJSInterop` (or similar)
- Methods for each map operation
- Error handling and logging

### Coordinate Validation Location

**Decision**: Validate coordinates in C# before JSInterop calls

**Rationale**:
- Type-safe validation in C#
- Better error messages
- Avoids unnecessary JavaScript calls
- Easier to test

## Dependencies & Integration

### External Dependencies
- **Leaflet JavaScript Library**: Loaded via script tag (version to be determined, latest stable recommended)
- **OpenStreetMap Tiles**: No API key required, usage must respect OSM tile usage policy

### Internal Dependencies
- **Microsoft.JSInterop**: Built into Blazor, no additional package required
- **Microsoft.FluentUI.AspNetCore.Components**: Optional, for consistent styling if used

### Host Application Requirements
- Leaflet CSS and JavaScript must be loaded (via script tags)
- Fluent UI components must be registered (if using Fluent UI styling)

## Risk Assessment

### High Risk
- **JavaScript Interop Complexity**: Map initialization and updates require JSInterop, which can be complex to test
  - **Mitigation**: Use helper class for JSInterop operations, comprehensive test setup similar to Fluent UI components

### Medium Risk
- **Map Service Availability**: OpenStreetMap tiles may be unavailable or rate-limited
  - **Mitigation**: Leaflet handles retries automatically, graceful error handling in component

### Low Risk
- **Browser Compatibility**: Leaflet supports all modern browsers
  - **Mitigation**: Leaflet is well-tested across browsers

## Success Metrics

- Component renders map successfully in both example applications
- Coordinate validation works correctly (invalid coordinates default to 0, 0)
- Parameter changes update map correctly
- Component tests pass with JSInterop setup
- Integration tests verify map displays in example applications

## Next Steps

1. Create data-model.md documenting component structure
2. Create quickstart.md with integration examples
3. Generate tasks.md using `/speckit.tasks`
4. Begin test-first implementation
