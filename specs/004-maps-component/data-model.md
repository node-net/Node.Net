# Data Model: Maps Component

**Feature**: 004-maps-component  
**Date**: 2025-01-14  
**Phase**: 1 - Design & Contracts

## Entities

### Maps Component

A Razor component that displays a map centered on specific geographic coordinates using Leaflet and OpenStreetMap.

**Type**: Blazor Razor Component  
**Location**: `source/Node.Net/Components/Maps.razor`  
**Namespace**: `Node.Net.Components`

#### Properties

| Property | Type | Required | Default | Description | Validation |
|----------|------|----------|---------|-------------|------------|
| `Latitude` | `double` | Yes | N/A | Latitude coordinate (-90 to 90) | Must be between -90 and 90. Component throws error if not provided. |
| `Longitude` | `double` | Yes | N/A | Longitude coordinate (-180 to 180) | Must be between -180 and 180. Component throws error if not provided. |
| `ZoomLevel` | `int` | No | 13 | Map zoom level (neighborhood-level view) | Typically 1-18 for most map libraries. Default: 13. |
| `MapType` | `string` | No | "satellite" | Map type/tile layer | Default: "satellite". Supported values depend on Leaflet tile provider. |

#### Behavior

- **Initialization**: Component initializes Leaflet map on first render using JSInterop
- **Coordinate Validation**: Coordinates are validated in C# before map operations. Invalid coordinates (out of range) default to (0, 0) - Null Island
- **Parameter Changes**: When Latitude, Longitude, ZoomLevel, or MapType parameters change, the map updates accordingly via JSInterop
- **Error Handling**: Map loading failures are handled gracefully with user-friendly messages or placeholders
- **Dimensions**: Component defaults to 100% width and 100% height (fills container)
- **Responsive**: Component adapts to container size automatically

#### State Transitions

1. **Uninitialized** → **Initializing**: Component created, JSInterop call to initialize map
2. **Initializing** → **Loaded**: Map successfully initialized and displayed
3. **Initializing** → **Error**: Map initialization failed (network error, JS error, etc.)
4. **Loaded** → **Updating**: Parameter changed, JSInterop call to update map
5. **Updating** → **Loaded**: Map successfully updated
6. **Updating** → **Error**: Map update failed

#### Relationships

- **Depends on**: Leaflet JavaScript library (loaded via script tag)
- **Uses**: Microsoft.JSInterop for map operations
- **Optional**: Microsoft.FluentUI.AspNetCore.Components for styling

#### Storage

- No persistent storage required
- Component state is managed in memory (Blazor component state)

#### Data Flow

1. **Component Creation**:
   - User provides Latitude and Longitude parameters (required)
   - Optional ZoomLevel and MapType parameters
   - Component validates coordinates
   - If invalid, uses default (0, 0)

2. **Map Initialization**:
   - Component calls JSInterop to initialize Leaflet map
   - JSInterop creates Leaflet map instance with specified coordinates
   - Map tiles load from OpenStreetMap
   - Component state transitions to "Loaded"

3. **Parameter Updates**:
   - User changes Latitude, Longitude, ZoomLevel, or MapType
   - Component validates new coordinates (if changed)
   - Component calls JSInterop to update map
   - Map updates center, zoom, or tile layer accordingly

4. **Error Handling**:
   - Network errors: Leaflet retries automatically, component may show loading state
   - JavaScript errors: Component catches and displays error message
   - Invalid coordinates: Component uses default (0, 0) without error

## Coordinate Validation Rules

### Latitude
- **Valid Range**: -90.0 to 90.0 (inclusive)
- **Validation**: `latitude >= -90.0 && latitude <= 90.0`
- **Invalid Behavior**: Use default value 0.0

### Longitude
- **Valid Range**: -180.0 to 180.0 (inclusive)
- **Validation**: `longitude >= -180.0 && longitude <= 180.0`
- **Invalid Behavior**: Use default value 0.0

### Null/Undefined Values
- **Latitude/Longitude**: Required parameters - component throws `ArgumentNullException` or similar if null
- **ZoomLevel**: Optional - uses default 13 if not provided
- **MapType**: Optional - uses default "satellite" if not provided

## Map Configuration

### Zoom Levels
- **Range**: Typically 1-18 for Leaflet/OpenStreetMap
- **Default**: 13 (neighborhood-level view)
- **Level Meanings**:
  - 1-3: World/continent view
  - 4-6: Country view
  - 7-9: Region/state view
  - 10-12: City view
  - 13-15: Neighborhood view (default)
  - 16-18: Street/building view

### Map Types
- **Default**: "satellite" (satellite imagery)
- **Supported Types** (depends on Leaflet tile provider):
  - "satellite": Satellite imagery
  - "roadmap": Standard street map
  - "terrain": Terrain/elevation map
  - "hybrid": Satellite with road labels

## Component Dimensions

- **Default Width**: 100% (fills container width)
- **Default Height**: 100% (fills container height)
- **Responsive**: Component adapts to container size automatically
- **Minimum Size**: No explicit minimum, but Leaflet requires minimum dimensions to render

## Error States

### Map Loading Failure
- **Cause**: Network error, JavaScript error, Leaflet not loaded
- **Behavior**: Display error message or placeholder
- **Recovery**: User can retry by re-rendering component or refreshing page

### Invalid Coordinates
- **Cause**: Coordinates out of valid range
- **Behavior**: Use default coordinates (0, 0) without error message
- **Recovery**: User provides valid coordinates

### Missing Required Parameters
- **Cause**: Latitude or Longitude not provided
- **Behavior**: Component throws exception (ArgumentNullException or similar)
- **Recovery**: User must provide required parameters
