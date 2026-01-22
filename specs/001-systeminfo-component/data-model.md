# Data Model: SystemInfo Razor Component

**Feature**: SystemInfo Razor Component  
**Date**: 2025-01-12  
**Phase**: 1 - Design & Contracts

## Entities

### SystemInfo Component

A Razor component that displays system information retrieved from the .NET runtime environment.

**Type**: Blazor Razor Component  
**Location**: `source/Node.Net/Components/SystemInfo.razor`  
**Namespace**: `Node.Net.Components`

#### Properties

| Property | Type | Required | Description | Source |
|----------|------|----------|-------------|--------|
| `ProfilePictureUrl` | `string?` | No | Optional URL or path to user profile picture | Component parameter |
| `UserName` | `string` | Yes | Current logged-in user name | `Environment.UserName` |
| `MachineName` | `string` | Yes | Name of the local computer | `Environment.MachineName` |
| `OperatingSystem` | `string` | Yes | Operating system description | `RuntimeInformation.OSDescription` |
| `Domain` | `string` | Yes | Network domain name | `Environment.UserDomainName` |

#### Behavior

- All system information properties are computed at render time
- Properties retrieve values from .NET runtime environment APIs
- Empty or null values from APIs are handled with fallback text ("Not available")
- Profile picture is optional; if not provided or fails to load, displays default person icon

#### State Transitions

N/A - Component is stateless and displays current system information at render time.

#### Validation Rules

- `ProfilePictureUrl`: If provided, must be a valid URL or file path (validation handled by browser/image loading)
- System information values: No validation required (read-only from runtime environment)

#### Edge Cases

1. **WASM Environment**: System information APIs return empty strings due to browser security restrictions
   - **Handling**: Display "Not available" for restricted fields
   
2. **Profile Picture Load Failure**: Image URL provided but fails to load
   - **Handling**: Fallback to default person icon via `@onerror` handler
   
3. **Null/Empty System Values**: Environment APIs return null or empty
   - **Handling**: Display "Not available" placeholder text
   
4. **Different Operating Systems**: Component used on Windows, macOS, Linux
   - **Handling**: Display OS-specific information from `RuntimeInformation.OSDescription`

## Relationships

- **Component → Fluent UI Components**: Uses `FluentCard`, `FluentStack`, `FluentIcon` for layout and styling
- **Component → .NET Runtime**: Reads system information from `Environment` and `RuntimeInformation` classes
- **Component → Host Applications**: Referenced and used by Blazor applications (Node.Net.AspNet.Host, Node.Net.Wasm)

## Data Flow

1. Component renders
2. System information properties computed from .NET APIs
3. Values checked for null/empty, fallback text applied if needed
4. Profile picture URL checked, default icon used if not provided
5. Component displays all information in Fluent UI card layout

## Notes

- No data persistence required (display-only component)
- No database or external storage needed
- All data is retrieved from runtime environment at render time
- Component is stateless and can be used multiple times on the same page
