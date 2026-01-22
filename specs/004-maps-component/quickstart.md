# Quickstart: Maps Component Integration

**Feature**: 004-maps-component  
**Date**: 2025-01-14

## Overview

This guide demonstrates how to integrate the Maps component into Blazor applications. The component displays a map centered on specific latitude and longitude coordinates using Leaflet and OpenStreetMap.

## Prerequisites

- .NET 8.0 or later (for Blazor components)
- Node.Net library reference
- Leaflet JavaScript library (loaded via script tag)

## Integration Steps

### 1. Add Leaflet Library

Add Leaflet CSS and JavaScript to your application's HTML file:

**For Blazor Server** (`Pages/_Host.cshtml` or `App.razor`):
```html
<link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
<script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
```

**For Blazor WebAssembly** (`wwwroot/index.html`):
```html
<link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
<script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
```

### 2. Add Component to Page

In your Razor page or component:

```razor
@page "/maps"
@using Node.Net.Components

<PageTitle>Maps</PageTitle>

<h1>Map Display</h1>

<Maps Latitude="37.7749" Longitude="-122.4194" />
```

### 3. Configure Optional Parameters

```razor
<Maps 
    Latitude="37.7749" 
    Longitude="-122.4194" 
    ZoomLevel="15" 
    MapType="roadmap" />
```

## Usage Scenarios

### Scenario 1: Display Map with Coordinates

**Given**: Application is running  
**When**: User navigates to page with Maps component  
**Then**: 
- Map displays centered on specified coordinates
- Map shows satellite imagery by default
- Map zoom level is 13 (neighborhood view) by default

**Test Steps**:
1. Add Maps component to page with coordinates (e.g., San Francisco: 37.7749, -122.4194)
2. Navigate to page
3. Verify map displays correctly
4. Verify map is centered on specified coordinates

---

### Scenario 2: Change Map Zoom Level

**Given**: Map is displayed  
**When**: User changes ZoomLevel parameter  
**Then**: 
- Map zoom level updates
- Map view changes accordingly

**Test Steps**:
1. Display map with default zoom (13)
2. Change ZoomLevel parameter to 10 (city view)
3. Verify map zooms out
4. Change ZoomLevel parameter to 16 (street view)
5. Verify map zooms in

---

### Scenario 3: Change Map Type

**Given**: Map is displayed  
**When**: User changes MapType parameter  
**Then**: 
- Map tile layer changes
- Map displays different imagery type

**Test Steps**:
1. Display map with default type (satellite)
2. Change MapType parameter to "roadmap"
3. Verify map shows street map instead of satellite
4. Change MapType parameter back to "satellite"
5. Verify map shows satellite imagery again

---

### Scenario 4: Update Coordinates

**Given**: Map is displayed  
**When**: User changes Latitude or Longitude parameters  
**Then**: 
- Map center updates to new coordinates
- Map displays new location

**Test Steps**:
1. Display map at coordinates (37.7749, -122.4194) - San Francisco
2. Change coordinates to (40.7128, -74.0060) - New York
3. Verify map center moves to New York
4. Verify map displays New York location

---

### Scenario 5: Invalid Coordinates Handling

**Given**: Map component is used  
**When**: User provides invalid coordinates (out of range)  
**Then**: 
- Component uses default coordinates (0, 0 - Null Island)
- Map displays without error

**Test Steps**:
1. Provide invalid latitude (e.g., 100)
2. Verify component uses default (0, 0)
3. Provide invalid longitude (e.g., 200)
4. Verify component uses default (0, 0)
5. Verify map displays at (0, 0) without error

---

### Scenario 6: Missing Required Parameters

**Given**: Maps component is used  
**When**: User does not provide Latitude or Longitude  
**Then**: 
- Component throws error
- Map does not display

**Test Steps**:
1. Attempt to use component without Latitude parameter
2. Verify error is thrown
3. Attempt to use component without Longitude parameter
4. Verify error is thrown

---

### Scenario 7: Responsive Layout

**Given**: Map is displayed  
**When**: User resizes browser window  
**Then**: 
- Map adapts to new container size
- Map remains functional

**Test Steps**:
1. Display map in container
2. Resize browser window to smaller size
3. Verify map adapts to new size
4. Resize browser window to larger size
5. Verify map adapts to new size

---

### Scenario 8: Map Loading Error Handling

**Given**: Map component is used  
**When**: Network error occurs or Leaflet fails to load  
**Then**: 
- Component handles error gracefully
- User sees appropriate error message or placeholder

**Test Steps**:
1. Disconnect from internet
2. Attempt to display map
3. Verify error handling (may show loading state or error message)
4. Reconnect to internet
5. Verify map loads successfully

---

### Scenario 9: Multiple Map Instances

**Given**: Application has multiple Maps components  
**When**: Multiple components are rendered on same page  
**Then**: 
- Each component displays independently
- Each component shows its specified coordinates

**Test Steps**:
1. Add two Maps components with different coordinates
2. Verify both maps display correctly
3. Verify each map shows its respective location
4. Update coordinates on one component
5. Verify only that component updates

---

### Scenario 10: Component in Different Hosting Models

**Given**: Maps component is integrated  
**When**: Component is used in Blazor Server and Blazor WebAssembly  
**Then**: 
- Component works in both hosting models
- Map displays correctly in both environments

**Test Steps**:
1. Add component to Blazor Server application
2. Verify map displays correctly
3. Add component to Blazor WebAssembly application
4. Verify map displays correctly
5. Compare behavior between both hosting models

---

## Configuration Options

### Default Behavior

- **Zoom Level**: 13 (neighborhood-level view)
- **Map Type**: "satellite" (satellite imagery)
- **Dimensions**: 100% width, 100% height (fills container)
- **Invalid Coordinates**: Defaults to (0, 0) - Null Island

### Custom Styling

The component fills its container by default. To control size:

```razor
<div style="width: 600px; height: 400px;">
    <Maps Latitude="37.7749" Longitude="-122.4194" />
</div>
```

## Troubleshooting

### Issue: Map doesn't display

**Solution**: 
1. Verify Leaflet CSS and JavaScript are loaded
2. Check browser console for JavaScript errors
3. Verify coordinates are valid
4. Check network connectivity (map tiles require internet)

### Issue: Map displays but shows wrong location

**Solution**:
1. Verify coordinates are in correct order (latitude, longitude)
2. Check coordinate values are within valid ranges
3. Verify coordinates are not reversed (common mistake)

### Issue: Map doesn't update when parameters change

**Solution**:
1. Ensure parameters are bound correctly (`@bind-Latitude` vs `Latitude`)
2. Check that parameter changes trigger component re-render
3. Verify JSInterop is working correctly

### Issue: Map tiles don't load

**Solution**:
1. Check internet connectivity
2. Verify OpenStreetMap tile servers are accessible
3. Check browser console for network errors
4. Consider using different tile provider if OSM is blocked

## Next Steps

- Review component API documentation
- Customize map styling and appearance
- Explore additional Leaflet features (if needed)
- Implement custom error handling (if needed)
