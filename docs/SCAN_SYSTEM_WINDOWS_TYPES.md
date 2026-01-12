# Scan Results: System.Windows Types for Potential net8.0 Implementation

## Summary
This document identifies System.Windows classes and structs found in the codebase that may need to be implemented for non-Windows targets (net8.0).

## Already Implemented ✓
1. **System.Windows.Point** - 2D point structure
2. **System.Windows.Vector** - 2D vector structure  
3. **System.Windows.Media.Matrix** - 2D transformation matrix
4. **System.Windows.Media.Media3D.Vector3D** - 3D vector structure
5. **System.Windows.Media.Media3D.Point3D** - 3D point structure
6. **System.Windows.Media.Media3D.Quaternion** - Rotation quaternion
7. **System.Windows.Media.Media3D.Matrix3D** - 3D transformation matrix

## High Priority Candidates (Used in Extension Methods)

### 1. System.Windows.Media.Media3D.Rect3D
**Location:** `Node.Net/Extension/Rect3D.Extension.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- `Rect3D.Scale()` extension method
- `Rect3D.GetCenter()` extension method
- `Rect3D.GetCorners()` extension method
- Used in `Matrix3D.Extension.cs` `Transform(Rect3D)` method

**Properties/Methods Used:**
- `IsEmpty` property
- `SizeX`, `SizeY`, `SizeZ` properties
- `X`, `Y`, `Z` properties
- `Location` property (Point3D)
- `Size` property (Size3D)
- Constructor: `new Rect3D(Point3D, Size3D)`

**Priority:** HIGH - Extension methods are wrapped in conditional compilation

### 2. System.Windows.Media.Media3D.Size3D
**Location:** Used with Rect3D
**Usage:**
- Created in `Rect3D.Extension.cs`: `new Size3D(x, y, z)`
- Used as `Rect3D.Size` property
- Used in `Matrix3D.Extension.cs` `Transform(Rect3D)` method

**Properties/Methods Used:**
- `X`, `Y`, `Z` properties
- Constructor: `new Size3D(double, double, double)`

**Priority:** HIGH - Required for Rect3D implementation

## Medium Priority Candidates (Used in Factories/Internal Code)

### 3. System.Windows.Media.Color
**Location:** `Node.Net/Internal/ColorFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- ColorFactory creates Color from strings
- Used in BrushFactory
- Used in MaterialFactory

**Properties/Methods Used:**
- `Color.FromRgb(byte, byte, byte)` static method
- `Color.FromArgb(byte, byte, byte, byte)` static method
- `Colors` class with static properties (e.g., `Colors.Red`)

**Priority:** MEDIUM - Used in factory pattern, but factories are conditionally compiled

### 4. System.Windows.Media.Brush
**Location:** `Node.Net/Internal/BrushFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- BrushFactory creates Brush from Color or ImageSource
- Used in MaterialFactory

**Subtypes Used:**
- `SolidColorBrush` - `new SolidColorBrush(Color)`
- `ImageBrush` - `new ImageBrush { ImageSource = ..., TileMode = TileMode.Tile }`

**Priority:** MEDIUM - Used in factory pattern

### 5. System.Windows.Media.ImageSource
**Location:** `Node.Net/Internal/ImageSourceReader.cs`, `Node.Net/Internal/BitmapSourceWriter.cs`
**Usage:**
- ImageSourceReader reads ImageSource
- BitmapSourceWriter writes BitmapSource
- Used in BrushFactory

**Priority:** MEDIUM - Used for image handling

### 6. System.Windows.Media.Imaging.BitmapSource
**Location:** `Node.Net/Internal/BitmapSourceWriter.cs`
**Usage:**
- BitmapSourceWriter writes BitmapSource to streams

**Priority:** MEDIUM - Used for image handling

## Lower Priority Candidates (Complex/UI-Related)

### 7. System.Windows.Media.Media3D.Material
**Location:** `Node.Net/Internal/MaterialFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- MaterialFactory creates Material from dictionaries
- Used in GeometryModel3D

**Priority:** LOW - Complex type, used in 3D rendering pipeline

### 8. System.Windows.Media.Media3D.Transform3D
**Location:** `Node.Net/Internal/Transform3DFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- Transform3DFactory creates Transform3D from dictionaries

**Priority:** LOW - Complex type, used in 3D rendering pipeline

### 9. System.Windows.Media.Media3D.MeshGeometry3D
**Location:** `Node.Net/Internal/MeshGeometry3DFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- MeshGeometry3DFactory creates MeshGeometry3D from dictionaries

**Priority:** LOW - Complex type, used in 3D rendering pipeline

### 10. System.Windows.Media.Media3D.GeometryModel3D
**Location:** `Node.Net/Internal/GeometryModel3DFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- GeometryModel3DFactory creates GeometryModel3D from dictionaries

**Priority:** LOW - Complex type, used in 3D rendering pipeline

### 11. System.Windows.Media.Media3D.Model3D
**Location:** `Node.Net/Internal/Model3DFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- Model3DFactory creates Model3D from dictionaries

**Priority:** LOW - Complex type, used in 3D rendering pipeline

### 12. System.Windows.Media.Media3D.Visual3D
**Location:** `Node.Net/Internal/Visual3DFactory.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- Visual3DFactory creates Visual3D from dictionaries

**Priority:** LOW - Complex type, used in 3D rendering pipeline

### 13. System.Windows.Media.Media3D.ProjectionCamera
**Location:** `Node.Net/Extension/ProjectionCamera.Extension.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- Extension methods for camera operations

**Priority:** LOW - Complex type, used in 3D rendering pipeline

### 14. System.Windows.Media.Media3D.PerspectiveCamera
**Location:** `Node.Net/Extension/PerspectiveCamera.Extension.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- Extension methods for camera operations

**Priority:** LOW - Complex type, used in 3D rendering pipeline

## Very Low Priority (UI/Markup - Likely Not Needed)

### 15. System.Windows.DependencyObject
**Location:** `Node.Net/Writer.cs`, `Node.Net/Extension/DependencyObjectExtension.cs`
**Usage:**
- Used in Writer for XAML serialization
- Extension methods for DependencyObject

**Priority:** VERY LOW - UI framework type, likely not needed for non-Windows

### 16. System.Windows.Markup.XamlReader
**Location:** `Node.Net/Reader.cs`
**Usage:**
- `XamlReader.Load(Stream)` - loads XAML

**Priority:** VERY LOW - XAML parsing, likely not needed for non-Windows

### 17. System.Windows.Markup.XamlWriter
**Location:** `Node.Net/Writer.cs`
**Usage:**
- `XamlWriter.Save(object, XmlWriter)` - saves XAML

**Priority:** VERY LOW - XAML serialization, likely not needed for non-Windows

### 18. System.Windows.UIElement
**Location:** `Node.Net/Extension/UIElement.Extension.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- Extension methods for UIElement

**Priority:** VERY LOW - UI framework type

### 19. System.Windows.Input.ICommand
**Location:** `Node.Net/DelegateCommand.cs`
**Usage:**
- DelegateCommand implements ICommand

**Priority:** VERY LOW - UI framework type

### 20. System.Windows.Data.IValueConverter
**Location:** `Node.Net/Converters/*.cs` (wrapped in `#if IS_WINDOWS`)
**Usage:**
- Value converters for data binding

**Priority:** VERY LOW - UI framework type

### 21. System.Windows.ResourceDictionary
**Location:** `Node.Net/Internal/Factory.cs`, `Node.Net/Internal/AbstractFactory.cs`
**Usage:**
- Factory.Resources property
- Resource lookup in AbstractFactory

**Priority:** VERY LOW - UI framework type

## Recommendations

### Immediate Next Steps (High Priority)
1. **Rect3D** and **Size3D** - These are the most straightforward and are blocking extension methods from being available on non-Windows. They follow the same pattern as Point3D/Vector3D.

### Future Considerations (Medium Priority)
2. **Color** - If you want to support color operations on non-Windows, but factories are already conditionally compiled.
3. **Brush** - Depends on Color and ImageSource implementation.

### Not Recommended (Low/Very Low Priority)
- Complex 3D rendering types (Material, Transform3D, MeshGeometry3D, etc.) - These are part of the WPF 3D rendering pipeline and likely not needed for non-Windows scenarios.
- UI framework types (DependencyObject, UIElement, ICommand, etc.) - These are core WPF UI types and likely not applicable to non-Windows targets.
- XAML types (XamlReader, XamlWriter) - XAML is a Windows-specific markup language.

## Implementation Pattern
All implementations should follow the same pattern as Point/Vector/Point3D/Vector3D:
- Place in `Node.Net/System/Windows/...` directory structure matching Windows namespace
- Wrap in `#if !IS_WINDOWS`
- Match Windows API exactly (no IFormattable, same behavior)
- Create comprehensive tests in `Node.Net.Test/System/Windows/...`
- Remove conditional compilation from extension methods that use these types

