# Quickstart: SystemInfo Component

**Feature**: SystemInfo Razor Component  
**Date**: 2025-01-12

## Overview

The SystemInfo component displays system information (user name, machine name, operating system, domain) in a Fluent UI card layout. It's a reusable Blazor component that can be added to any Blazor page.

## Prerequisites

- .NET 8.0 SDK or later
- Blazor application (Server or WebAssembly)
- Microsoft.FluentUI.AspNetCore.Components package (already included in Node.Net library)

## Installation

The SystemInfo component is included in the Node.Net library. No additional installation required beyond referencing the Node.Net NuGet package.

## Basic Usage

### 1. Add Using Statement

In your Blazor page or `_Imports.razor`:

```razor
@using Node.Net.Components
```

### 2. Use the Component

Add the component to any Blazor page:

```razor
<SystemInfo />
```

### 3. With Profile Picture (Optional)

```razor
<SystemInfo ProfilePictureUrl="https://example.com/profile.jpg" />
```

## Example: Home Page Integration

### AspNet.Host Example

```razor
@page "/"
@using Node.Net.Components

<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>

<SystemInfo />

Welcome to your new Fluent Blazor app.
```

### Wasm Example

```razor
@page "/"
@using Node.Net.Components

<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>

<SystemInfo />

Welcome to your new Fluent Blazor app.
```

## Component Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `ProfilePictureUrl` | `string?` | `null` | Optional URL or path to user profile picture. If not provided or fails to load, displays default person icon. |

## Displayed Information

The component automatically displays:

- **User Name**: Current logged-in user (`Environment.UserName`)
- **Machine Name**: Local computer name (`Environment.MachineName`)
- **Operating System**: OS description (`RuntimeInformation.OSDescription`)
- **Domain**: Network domain name (`Environment.UserDomainName`)

## Browser Compatibility (WASM)

In Blazor WebAssembly environments, browser security restrictions may limit system information. The component gracefully handles this by displaying "Not available" for restricted fields.

## Styling

The component uses Fluent UI Blazor components for consistent styling:
- `FluentCard` for container
- `FluentStack` for responsive layout
- `FluentIcon` for profile picture placeholder

## Troubleshooting

### Component Not Rendering

- Ensure `@using Node.Net.Components` is included
- Verify Node.Net library is referenced in your project
- Check that Fluent UI services are registered: `builder.Services.AddFluentUIComponents();`

### System Information Shows "Not available"

- In WASM environments, this is expected due to browser security restrictions
- In server-side Blazor, verify the application has appropriate permissions

### Profile Picture Not Displaying

- Verify the URL is accessible
- Check browser console for CORS or loading errors
- Component will automatically fallback to default icon on error

## Next Steps

- See the example applications (`Node.Net.AspNet.Host` and `Node.Net.Wasm`) for complete integration examples
- Refer to [spec.md](./spec.md) for detailed requirements
- Check [plan.md](./plan.md) for implementation details
