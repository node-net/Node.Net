# Plan to Fix Matrix3D Rotation Behavior

## Problem Statement
The Matrix3D.Rotate(Quaternion) implementation produces different results on `net8.0` (custom implementation) vs `net8.0-windows` (Windows implementation). This is unacceptable - the behavior must be identical regardless of platform.

## Root Cause Analysis

### Current Issues:
1. **Quaternion Constructor**: Currently negates the angle (`-angleInDegrees`), which may be incorrect
2. **Matrix3D.Rotate**: Uses conjugate quaternion (negates x, y, z components) and Prepend
3. **Rotation Matrix Formula**: May not match Windows WPF exactly
4. **Append vs Prepend**: Windows Matrix3D.Rotate uses Append, not Prepend

### Key Observations:
- Tests show rotation direction is reversed (negative values instead of positive)
- The issue affects both the rotation itself and the extraction methods (GetOrientation, GetTilt, GetSpin)

## Solution Plan

### Phase 1: Research Windows Behavior
1. **Test Windows Implementation Directly**
   - Create a simple test program that uses Windows Matrix3D
   - Test: `new Matrix3D().Rotate(new Quaternion(new Vector3D(0,0,1), 15))`
   - Capture the actual matrix values produced
   - Test with Append vs Prepend to determine which Windows uses

2. **Verify Quaternion Constructor**
   - Test Windows Quaternion constructor with same inputs
   - Verify if angle is negated or not
   - Check if axis normalization matches

3. **Verify Rotation Matrix Formula**
   - Test Windows Matrix3D.Rotate with known quaternion
   - Extract the rotation matrix values
   - Compare with standard quaternion-to-matrix formulas

### Phase 2: Fix Implementation

#### Step 1: Fix Quaternion Constructor
- Remove the angle negation if Windows doesn't use it
- Ensure axis normalization matches Windows exactly
- Test: `new Quaternion(new Vector3D(0,0,1), 15)` should produce same quaternion as Windows

#### Step 2: Fix Matrix3D.Rotate Method
- **Critical**: Windows Matrix3D.Rotate uses **Append**, not Prepend
- Remove conjugate quaternion usage if Windows doesn't use it
- Verify the rotation matrix formula matches Windows exactly
- Test with known inputs and compare matrix values byte-for-byte

#### Step 3: Verify Rotation Matrix Formula
The standard formula for quaternion to rotation matrix is:
```
[1-2(y²+z²)  2(xy-wz)    2(xz+wy)   ]
[2(xy+wz)    1-2(x²+z²)  2(yz-wx)   ]
[2(xz-wy)    2(yz+wx)    1-2(x²+y²) ]
```

But Windows may use:
- Transpose of this matrix
- Conjugate quaternion (negate x,y,z)
- Different sign conventions

### Phase 3: Testing and Validation

1. **Create Reference Tests**
   - Use Windows implementation to generate reference matrices
   - Store expected matrix values for key test cases
   - Compare our implementation output with Windows output

2. **Fix All Rotation Tests**
   - Remove all `#if IS_WINDOWS` conditionals from Matrix3D tests
   - All tests should pass on both platforms with identical behavior
   - Verify: Identity, Translate, Rotate, RotateOTS, GetRotationsOTS all match

3. **Verify Extension Methods**
   - Test that extension methods work identically on both platforms
   - Ensure GetOrientation, GetTilt, GetSpin produce same results

### Phase 4: Implementation Details

#### Option A: Match Windows Exactly (Recommended)
1. Test Windows behavior with a simple C# program
2. Capture exact matrix values for known inputs
3. Adjust our implementation until outputs match exactly
4. Remove all conditional test code

#### Option B: Reverse Engineer from Tests
1. Use the existing Windows test results as reference
2. Adjust our implementation until tests pass
3. Verify with additional test cases

### Critical Fixes Needed:

1. **Matrix3D.Rotate should use Append, not Prepend**
   ```csharp
   Append(rotationMatrix);  // Not Prepend
   ```

2. **Quaternion constructor may need angle sign fix**
   ```csharp
   // Remove negation if Windows doesn't use it
   double angleInRadians = angleInDegrees * (Math.PI / 180.0);
   ```

3. **Rotation matrix may need to use quaternion directly (not conjugate)**
   ```csharp
   // Use quaternion directly, not conjugate
   double x = q.X;  // Not -q.X
   double y = q.Y;  // Not -q.Y
   double z = q.Z;  // Not -q.Z
   double w = q.W;
   ```

## Implementation Steps

1. **Create diagnostic test** to compare Windows vs our implementation
2. **Fix Quaternion constructor** - remove angle negation
3. **Fix Matrix3D.Rotate** - use Append instead of Prepend, remove conjugate
4. **Test and verify** - ensure all tests pass on both platforms
5. **Remove all conditional test code** - no `#if IS_WINDOWS` in Matrix3D tests

## Success Criteria

- All Matrix3D tests pass on both `net8.0` and `net8.0-windows`
- No conditional compilation in test code (`#if IS_WINDOWS`)
- Matrix values match Windows exactly for identical inputs
- Rotation direction matches Windows (no sign flips)
- `rake test` passes completely

