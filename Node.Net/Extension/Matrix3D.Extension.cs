using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using static System.Math;

namespace Node.Net
{
    public static class Matrix3DExtension
    {
        public static Matrix3D RotateXYZ(this Matrix3D matrix, Vector3D rotationsXYZ)
        {
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), rotationsXYZ.Z));

            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));

            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, rotationsXYZ.X));
            return matrix;
        }

        public static Matrix3D RotateOTS(this Matrix3D matrix, Vector3D rotationsOTS)
        {
            var orientation = rotationsOTS.X;
            var tilt = rotationsOTS.Y;
            var spin = rotationsOTS.Z;
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), orientation));

            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, tilt));

            var localZ = matrix.Transform(new Vector3D(0, 0, 1));
            matrix.Rotate(new Quaternion(localZ, spin));
            return matrix;
        }

        public static Vector3D GetRotationsOTS(this Matrix3D matrix)
        {
            return new Vector3D(matrix.GetOrientation(),
                matrix.GetTilt(),
                matrix.GetSpin());
            /*
            var xdirection = matrix.GetXDirectionVector();
            var ydirection = matrix.GetYDirectionVector();
            var zdirection = matrix.GetZDirectionVector();

            double orientation = 0.0;
            double tilt = 0.0;
            double spin = 0.0;
            if (Abs(1.0 - zdirection.Z) < 0.0001)
            {
                orientation = Math.Atan2(xdirection.Y, xdirection.X);
            }
            else if (Abs(zdirection.Z + 1.0) < 0.0001)
            {
                tilt = Math.PI;
                orientation = Math.Atan2(xdirection.Y, xdirection.X);
            }
            else
            {
                tilt = Math.Acos(zdirection.Z);
                if (Abs(1.0 - xdirection.X) > 0.0001)
                {
                    orientation = Math.Atan2(zdirection.Y * -1.0, zdirection.X * -1.0) - Math.PI * 0.5;
                }
            }
            double rad2Deg = 180 / Math.PI;
            return new Vector3D(orientation * rad2Deg, tilt * rad2Deg, spin * rad2Deg);
            */
        }

        public static Vector3D GetRotationsXYZ(this Matrix3D matrix)
        {
            var rotationZ = GetRotationZ(matrix);
            var rotationY = GetRotationY(matrix, rotationZ);
            return new Vector3D(GetRotationX(matrix, rotationZ, rotationY), rotationY, rotationZ);
        }

        public static Matrix3D RotateZXY(this Matrix3D matrix, Vector3D rotationsXYZ)
        {
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), rotationsXYZ.Z));

            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, rotationsXYZ.X));

            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));
            return matrix;
        }

        public static Vector3D GetRotationsZXY(this Matrix3D matrix)
        {
            var rotationZ = GetRotationZ(matrix);
            var rotationX = GetRotationXZ(matrix, rotationZ);
            var rotationY = GetRotationYXZ(matrix, rotationX, rotationZ);
            return new Vector3D(rotationX, rotationY, rotationZ);
        }

        public static Point3D GetTranslation(this Matrix3D matrix)
        {
            return matrix.Transform(new Point3D(0, 0, 0));
        }

        public static double GetRotationZ(this Matrix3D matrix)
        {
            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            var localZ = matrix.Transform(new Vector3D(0, 0, 1));

            // rotation about the Z axis
            localX.Z = 0.0;
            var angle = Vector3D.AngleBetween(localX, new Vector3D(1, 0, 0));
            var rotationZ = localX.Y < 0.0 ? angle * -1.0 : angle;
            if ((Abs(rotationZ) - 180.0) > 0.01 && Abs(localX.Y) < 0.0001)
            {
                rotationZ = 0.0;
            }

            if (Abs(localZ.Z + 1.0) < 0.01)
            {
                rotationZ = 0.0;
            }

            return rotationZ;
        }

        public static double GetRotationY(this Matrix3D matrix)
        {
            return GetRotationY(matrix, GetRotationZ(matrix));
        }

        public static double GetRotationY(this Matrix3D matrix, double rotationZ)
        {
            // back off z rotation
            var matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));

            var localX = matrix2.Transform(new Vector3D(1, 0, 0));
            var localY = matrix2.Transform(new Vector3D(0, 1, 0));

            // rotation about the Y axis
            localX.Y = 0.0;
            var angle = Vector3D.AngleBetween(localX, new Vector3D(1, 0, 0));
            var rotationY = localX.Z > 0 ? angle * -1.0 : angle;
            if (Abs(rotationY - 180.0) > 0.01 && Abs(localX.Z) < 0.0001)
            {
                rotationY = 0.0;
            }

            if (Abs(localY.Y + 1.0) < 0.01)
            {
                rotationY = 0.0;
            }

            return rotationY;
        }

        public static double GetRotationXZ(this Matrix3D matrix, double rotationZ)
        {
            // back off z rotation
            var matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));

            var localX = matrix2.Transform(new Vector3D(1, 0, 0));
            var localY = matrix2.Transform(new Vector3D(0, 1, 0));

            // rotation about the X axis
            localY.X = 0.0;
            var angle = Vector3D.AngleBetween(localY, new Vector3D(0, 1, 0));
            var rotationX = localX.Z > 0 ? angle * -1.0 : angle;
            if (Abs(rotationX - 180.0) > 0.01 && Abs(localY.Z) < 0.0001)
            {
                rotationX = 0.0;
            }

            if (Abs(localY.Y + 1.0) < 0.01)
            {
                rotationX = 0.0;
            }

            return rotationX;
        }

        public static double GetRotationX(this Matrix3D matrix)
        {
            var rotationZ = matrix.GetRotationZ();
            var rotationY = matrix.GetRotationY(rotationZ);
            return GetRotationX(matrix, rotationZ, rotationY);
        }

        public static double GetRotationX(this Matrix3D matrix, double rotationZ, double rotationY)
        {
            // back off z rotation and y rotation
            var matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));
            var matrix3 = new Matrix3D();
            matrix3.Append(matrix2);
            matrix3 = matrix3.RotateXYZ(new Vector3D(0.0, rotationY * -1.0, 0.0));

            var localY = matrix3.Transform(new Vector3D(0, 1, 0));
            localY.X = 0.0;
            var angle = Vector3D.AngleBetween(localY, new Vector3D(0, 1, 0));
            var rotationX = localY.Z < 0 ? angle * -1.0 : angle;

            // rotation about the X axis
            return rotationX;
        }

        public static double GetRotationYXZ(this Matrix3D matrix, double rotationX, double rotationZ)
        {
            return 0.0;
        }

        public static Matrix3D SetDirectionVectors(this Matrix3D matrix, Vector3D xDirection, Vector3D yDirection, Vector3D zDirection)
        {
            // zRotation
            xDirection.Z = 0;
            xDirection.Normalize();
            var deltaZ = Abs(Vector3D.AngleBetween(new Vector3D(1, 0, 0), xDirection));
            if (Abs(xDirection.Z - 1.0) < 0.001)
            {
                deltaZ = 0.0;
            }

            if (xDirection.Y < 0.0)
            {
                deltaZ *= -1.0;
            }

            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), deltaZ));

            // yRotation
            zDirection.Normalize();
            var inverse = Matrix3D.Multiply(new Matrix3D(), matrix);
            inverse.Invert();
            var zDir2 = inverse.Transform(zDirection);
            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            var deltaY = Abs(Vector3D.AngleBetween(new Vector3D(0, 0, 1), zDir2));
            if (zDir2.X < 0.0)
            {
                deltaY *= -1.0;
            }

            ////////////////////////
            if (Abs(Abs(deltaY) - 180.0) < 0.001)
            {
                matrix.SetIdentity();
            }
            ////////////////////////
            matrix.Rotate(new Quaternion(localY, deltaY));

            // xRotation
            yDirection.Normalize();
            inverse = Matrix3D.Multiply(new Matrix3D(), matrix);
            inverse.Invert();
            var yDir2 = inverse.Transform(yDirection);
            var localX = matrix.Transform(new Vector3D(1, 0, 0));
            var deltaX = Abs(Vector3D.AngleBetween(new Vector3D(0, 1, 0), yDir2));
            if (yDir2.Z < 0.0)
            {
                deltaX *= -1.0;
            }

            matrix.Rotate(new Quaternion(localX, deltaX));
            return matrix;
        }

        public static Matrix3D SetDirectionVectorsXY(this Matrix3D matrix, Vector3D xDirection, Vector3D yDirection)
        {
            xDirection.Normalize();
            yDirection.Normalize();
            var zDirection = Vector3D.CrossProduct(xDirection, yDirection);

            // Align X axes
            var normal = Vector3D.CrossProduct(new Vector3D(1, 0, 0), xDirection);
            if (normal.Length > 0)
            {
                var deltaX = Vector3D.AngleBetween(new Vector3D(1, 0, 0), xDirection);
                matrix.Rotate(new Quaternion(normal, deltaX));
            }

            // Align Y axes
            var localY = matrix.Transform(new Vector3D(0, 1, 0));
            normal = Vector3D.CrossProduct(localY, yDirection);
            if (normal.Length > 0)
            {
                var deltaY = Vector3D.AngleBetween(localY, yDirection);
                matrix.Rotate(new Quaternion(normal, deltaY));
            }

            // Align Z axes
            var localZ = matrix.Transform(new Vector3D(0, 0, 1));
            normal = Vector3D.CrossProduct(localZ, zDirection);
            if (normal.Length > 0)
            {
                var deltaZ = Vector3D.AngleBetween(localZ, zDirection);
                matrix.Rotate(new Quaternion(normal, deltaZ));
            }

            return matrix;
        }

        public static IDictionary GetDictionary(this Matrix3D matrix)
        {
            var data = new Dictionary<string, dynamic>();
            var rotationsZXY = Matrix3DExtension.GetRotationsZXY(matrix);
            var translation = Matrix3DExtension.GetTranslation(matrix);
            data["X"] = $"{translation.X} m";
            data["Y"] = $"{translation.Y} m";
            data["Z"] = $"{translation.Z} m";
            data["RotationX"] = $"{rotationsZXY.X} deg";
            data["RotationY"] = $"{rotationsZXY.Y} deg";
            data["RotationZ"] = $"{rotationsZXY.Z} deg";
            return data;
        }

        public static Rect3D Transform(this Matrix3D matrix, Rect3D bounds)
        {
            var center = bounds.GetCenter();
            var corner = new Point3D(
                bounds.Location.X + bounds.SizeX,
                bounds.Location.Y + bounds.SizeY,
                bounds.Location.Z + bounds.SizeZ);

            var transformedCenter = matrix.Transform(center);
            var transformedLocation = matrix.Transform(bounds.Location);
            var transformedCorner = matrix.Transform(corner);

            var transformedSize = new Size3D(
                Abs(transformedCenter.X - transformedLocation.X) * 2.0,
                Abs(transformedCenter.Y - transformedLocation.Y) * 2.0,
                Abs(transformedCenter.Z - transformedLocation.Z) * 2.0);

            transformedLocation = new Point3D(
                transformedCenter.X - (transformedSize.X / 2.0),
                transformedCenter.Y - (transformedSize.Y / 2.0),
                transformedCenter.Z - (transformedSize.Z / 2.0));
            return new Rect3D(transformedLocation, transformedSize);
        }

        public static Vector3D GetXDirectionVector(this Matrix3D matrix)
        {
            return matrix.Transform(new Vector3D(1, 0, 0));
        }

        public static Vector3D GetYDirectionVector(this Matrix3D matrix)
        {
            return matrix.Transform(new Vector3D(0, 1, 0));
        }

        public static Vector3D GetZDirectionVector(this Matrix3D matrix)
        {
            return matrix.Transform(new Vector3D(0, 0, 1));
        }

        public static double GetOrientation(this System.Windows.Media.Media3D.Matrix3D matrix)
        {
            var localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            // rotation about the Z axis = angle between localX and parent X
            return System.Windows.Media.Media3D.Vector3D.AngleBetween(localX, new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
        }

        public static double GetTilt(this System.Windows.Media.Media3D.Matrix3D matrix)
        {
            // get the rotation about the X` local axis
            var localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            var localY = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(0, 1, 0));

            var orientMatrix = new System.Windows.Media.Media3D.Matrix3D().SetOrientation(matrix.GetOrientation());
            var orientedY = orientMatrix.Transform(new System.Windows.Media.Media3D.Vector3D(0, 1, 0));

            // rotation about the X' axis = angle between localY and orientedY
            var angle = System.Windows.Media.Media3D.Vector3D.AngleBetween(localY, orientedY);
            return angle;
        }

        public static double GetSpin(this System.Windows.Media.Media3D.Matrix3D matrix)
        {
            // get the rotatoin about the Z`` axis: angle between localX and tiltedX
            var localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            var tiltMatrix = new System.Windows.Media.Media3D.Matrix3D()
                .SetOrientation(matrix.GetOrientation())
                .SetTilt(matrix.GetTilt());
            var tiltedX = tiltMatrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            return System.Windows.Media.Media3D.Vector3D.AngleBetween(localX, tiltedX);
        }

        public static System.Windows.Media.Media3D.Matrix3D SetOrientation(this System.Windows.Media.Media3D.Matrix3D matrix, double orientationDegrees)
        {
            var translation = new System.Windows.Media.Media3D.Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            // backoff translation
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(-translation.X, -translation.Y, -translation.Z));

            // back off any existing Z rotation
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), matrix.GetOrientation() * -1.0));
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(new System.Windows.Media.Media3D.Vector3D(0, 0, 1), orientationDegrees));
            // add back translation
            matrix.Translate(translation);
            return matrix;
        }

        public static System.Windows.Media.Media3D.Matrix3D SetTilt(
            this System.Windows.Media.Media3D.Matrix3D matrix, double tilt)
        {
            var translation = new System.Windows.Media.Media3D.Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            // backoff translation
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(-translation.X, -translation.Y, -translation.Z));
            var localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            // backoff exiting tilt
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(localX, matrix.GetTilt() * -1.0));
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(localX, tilt));
            // add back translation
            matrix.Translate(translation);
            return matrix;
        }

        public static System.Windows.Media.Media3D.Matrix3D SetSpin(
            this System.Windows.Media.Media3D.Matrix3D matrix, double spin)
        {
            var translation = new System.Windows.Media.Media3D.Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            // backoff translation
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(-translation.X, -translation.Y, -translation.Z));

            var localZ = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(0, 0, 1));
            // backoff existing spin
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(localZ, matrix.GetSpin() * -1.0));
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(localZ, spin));
            // add back translation
            matrix.Translate(translation);
            return matrix;
        }

        public static List<double> GetValues(this System.Windows.Media.Media3D.Matrix3D matrix, int decimals)
        {
            var values = new List<double>();
            values.Add(Round(matrix.M11, decimals));
            values.Add(Round(matrix.M12, decimals));
            values.Add(Round(matrix.M13, decimals));
            values.Add(Round(matrix.M14, decimals));
            values.Add(Round(matrix.M21, decimals));
            values.Add(Round(matrix.M22, decimals));
            values.Add(Round(matrix.M23, decimals));
            values.Add(Round(matrix.M24, decimals));
            values.Add(Round(matrix.M31, decimals));
            values.Add(Round(matrix.M32, decimals));
            values.Add(Round(matrix.M33, decimals));
            values.Add(Round(matrix.M34, decimals));
            values.Add(Round(matrix.OffsetX, decimals));
            values.Add(Round(matrix.OffsetY, decimals));
            values.Add(Round(matrix.OffsetZ, decimals));
            values.Add(Round(matrix.M44, decimals));
            return values;
        }

        public static bool AlmostEqual(this System.Windows.Media.Media3D.Matrix3D matrixA, System.Windows.Media.Media3D.Matrix3D matrixB, int decimals = 4)
        {
            var valuesA = matrixA.GetValues(8);
            var valuesB = matrixB.GetValues(8);
            for(int i = 0; i < valuesA.Count; ++i)
            {
                if (Round(valuesA[i], decimals) != Round(valuesB[i], decimals)) return false;
            }

            return true;
        }
    }
}