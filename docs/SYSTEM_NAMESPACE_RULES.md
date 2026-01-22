# System Namespace Files and Conditional Compilation Rules

## Overview

The `source/Node.Net/System` directory contains implementations of `System.Windows.*` types that are **only available on Windows** when targeting Windows-specific frameworks. These files provide cross-platform compatibility by implementing these types for non-Windows targets (`net8.0` and `net8.0-wasm`).

## Key Principles

### 1. Conditional Compilation

**All files in `source/Node.Net/System` must be wrapped in `#if !IS_WINDOWS`:**

```csharp
#if !IS_WINDOWS
using System;

namespace System.Windows
{
    public struct Point
    {
        // Implementation...
    }
}
#endif
```

**Why?**
- When targeting `net8.0-windows` or `net48`, these types are **already provided by the framework** (WPF/PresentationCore)
- When targeting `net8.0`, `net8.0-wasm`, or `netstandard2.0` (non-Windows or cross-platform), these types are **not available** and must be provided by Node.Net
- The `IS_WINDOWS` constant is defined for Windows targets (`net8.0-windows`, `net48`)

### 2. Target Framework Behavior

| Target Framework | `IS_WINDOWS` Defined? | System.Windows Types Source |
|------------------|----------------------|----------------------------|
| `net8.0` | ❌ No | **Node.Net provides** (`source/Node.Net/System`) |
| `net8.0-wasm` | ❌ No | **Node.Net provides** (`source/Node.Net/System`) |
| `netstandard2.0` | ❌ No | **Node.Net provides** (`source/Node.Net/System`) |
| `net8.0-windows` | ✅ Yes | **Framework provides** (WPF/PresentationCore) |
| `net48` | ✅ Yes | **Framework provides** (WPF/PresentationCore) |

### 3. Namespace Structure

Files in `source/Node.Net/System` mirror the Windows namespace structure:

```
source/Node.Net/System/
├── Windows/
│   ├── Point.cs                    → namespace System.Windows
│   ├── Vector.cs                   → namespace System.Windows
│   ├── ResourceDictionary.cs       → namespace System.Windows
│   └── Media/
│       ├── Color.cs                → namespace System.Windows.Media
│       ├── Brushes.cs              → namespace System.Windows.Media
│       └── Media3D/
│           ├── Matrix3D.cs         → namespace System.Windows.Media.Media3D
│           ├── Vector3D.cs         → namespace System.Windows.Media.Media3D
│           └── ...
```

**Important:** These files define types in the **`System.` namespace**, not `Node.Net.System`. This allows code to use the same types regardless of target framework.

## Implementation Requirements

### File Structure

Every file in `source/Node.Net/System` must follow this pattern:

```csharp
#if !IS_WINDOWS
using System;
// ... other using statements ...

namespace System.Windows.Media.Media3D  // Match Windows namespace exactly
{
    /// <summary>
    /// Documentation matching Windows API
    /// </summary>
    public struct Vector3D
    {
        // Implementation matching Windows API exactly
    }
}
#endif
```

### API Compatibility

- **Must match Windows API exactly** - same properties, methods, operators, and behavior
- **No additional features** - these are compatibility shims, not extensions
- **Same behavior** - including edge cases (e.g., `Vector.Normalize()` on zero vector sets to NaN, not throws)

## Using System Types in Tests

### Transparent Type Access

Tests can now use `System.Windows.*` types with **standard namespace syntax** - no special prefixes or aliases required. The test project uses global usings to make these types available transparently.

### The Solution: Global Usings

The test project (`tests/Node.Net.Test/Node.Net.Test.csproj`) includes `GlobalUsings.cs` with:

```csharp
global using System.Windows;
global using System.Windows.Media;
global using System.Windows.Media.Imaging;
global using System.Windows.Media.Media3D;
```

This allows tests to use standard namespace syntax:

```csharp
using NUnit.Framework;

namespace Node.Net.Test
{
    [TestFixture]
    internal class Matrix3DTests
    {
        [Test]
        public void Test()
        {
            // Matrix3D is available via global usings - works on all target frameworks
            Matrix3D matrix = new Matrix3D();
            // ...
        }
    }
}
```

**How it works:**
- On `net8.0-windows`: Types resolve to WPF/PresentationCore (framework-provided)
- On `net8.0`: Types resolve to Node.Net's `source/Node.Net/System` files (custom implementation)
- Both use the same namespace (`System.Windows.*`), so code is identical

### Extension Methods

Extension methods in `Node.Net` namespace work with both:
- Framework-provided types (on Windows)
- Node.Net-provided types (on non-Windows)

```csharp
using NUnit.Framework;
using Node.Net;  // Extension methods are in Node.Net namespace

// Extension methods work regardless of where Matrix3D comes from
Matrix3D matrix = new Matrix3D();
matrix = matrix.RotateOTS(new Vector3D(45, 30, 0));  // Extension method
```

**Note:** The test project no longer uses `extern alias NodeNet`. All types are accessed using standard namespace references.

## Current Implementation Status

### ✅ Fully Implemented

- `System.Windows.Point` - 2D point structure
- `System.Windows.Vector` - 2D vector structure
- `System.Windows.ResourceDictionary` - Resource dictionary
- `System.Windows.Media.Matrix` - 2D transformation matrix
- `System.Windows.Media.Color` - Color structure
- `System.Windows.Media.Brushes` - Brush collection
- `System.Windows.Media.Media3D.Vector3D` - 3D vector structure
- `System.Windows.Media.Media3D.Point3D` - 3D point structure
- `System.Windows.Media.Media3D.Matrix3D` - 3D transformation matrix
- `System.Windows.Media.Media3D.Quaternion` - Rotation quaternion
- `System.Windows.Media.Media3D.Rect3D` - 3D rectangle
- `System.Windows.Media.Media3D.Size3D` - 3D size
- `System.Windows.Media.Media3D.MeshGeometry3D` - 3D mesh geometry
- `System.Windows.Media.Media3D.Material` and derived types
- `System.Windows.Media.Imaging.BitmapSource` and related types

### 📝 Implementation Pattern

When adding new types to `source/Node.Net/System`:

1. **Place file in matching directory structure:**
   ```
   source/Node.Net/System/Windows/Media/Media3D/NewType.cs
   ```

2. **Wrap entire file in `#if !IS_WINDOWS`:**
   ```csharp
   #if !IS_WINDOWS
   // ... implementation ...
   #endif
   ```

3. **Use exact Windows namespace:**
   ```csharp
   namespace System.Windows.Media.Media3D
   ```

4. **Match Windows API exactly:**
   - Same properties, methods, operators
   - Same behavior (including edge cases)
   - Same XML documentation

5. **Create tests:**
   ```
   tests/Node.Net.Test/System/Windows/Media/Media3D/NewType.Tests.cs
   ```

6. **Use standard namespace syntax in tests:**
   ```csharp
   using NUnit.Framework;
   // System.Windows.Media.Media3D types are available via global usings
   
   namespace Node.Net.Test
   {
       [TestFixture]
       internal class NewTypeTests
       {
           [Test]
           public void Test()
           {
               NewType value = new NewType();  // Standard namespace syntax
               // ...
           }
       }
   }
   ```

## Common Pitfalls

### ❌ Don't: Remove `#if !IS_WINDOWS`

```csharp
// WRONG - This will cause conflicts on Windows targets
namespace System.Windows
{
    public struct Point { }
}
```

### ❌ Don't: Use `Node.Net.System` namespace

```csharp
// WRONG - Breaks compatibility
namespace Node.Net.System.Windows
{
    public struct Point { }
}
```

### ❌ Don't: Add features not in Windows API

```csharp
// WRONG - Adds IFormattable which Windows Point doesn't have
public struct Point : IFormattable
{
    public string ToString(string format, IFormatProvider provider) { }
}
```

### ✅ Do: Match Windows API exactly

```csharp
#if !IS_WINDOWS
namespace System.Windows
{
    // Matches Windows API exactly - no IFormattable, only ToString()
    public struct Point
    {
        public override string ToString() { }
    }
}
#endif
```

## Build Configuration

The conditional compilation is controlled by MSBuild properties in `source/Node.Net/Node.Net.csproj`:

```xml
<!-- Windows targets define IS_WINDOWS -->
<PropertyGroup Condition="'$(TargetFramework)' == 'net8.0-windows'">
  <DefineConstants>IS_WINDOWS</DefineConstants>
</PropertyGroup>

<PropertyGroup Condition="'$(TargetFramework)' == 'net48'">
  <DefineConstants>IS_WINDOWS;IS_FRAMEWORK</DefineConstants>
</PropertyGroup>
```

## Related Documentation

- [SCAN_SYSTEM_WINDOWS_TYPES.md](./SCAN_SYSTEM_WINDOWS_TYPES.md) - Detailed scan of types that may need implementation
- [PLAN_FIX_MATRIX3D_ROTATION.md](./PLAN_FIX_MATRIX3D_ROTATION.md) - Matrix3D rotation implementation details

## Summary

- **Purpose:** Provide `System.Windows.*` types for non-Windows targets
- **Compilation:** Only compiled when `!IS_WINDOWS` (i.e., `net8.0`, `net8.0-wasm`, `netstandard2.0`)
- **Namespace:** Must match Windows exactly (`System.Windows.*`)
- **API:** Must match Windows API exactly
- **Tests:** Use standard namespace syntax - `System.Windows.*` types are available via global usings in the test project
