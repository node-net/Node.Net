#if IS_WINDOWS 
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
            // Early return if all rotations are zero (no rotation needed)
            if (rotationsXYZ.X == 0.0 && rotationsXYZ.Y == 0.0 && rotationsXYZ.Z == 0.0)
            {
                return matrix;
            }
            
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), rotationsXYZ.Z));

            Vector3D localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));

            Vector3D localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, rotationsXYZ.X));
            return matrix;
        }

        public static Matrix3D RotateOTS(this Matrix3D matrix, Vector3D rotationsOTS)
        {
            // Early return if all rotations are zero (no rotation needed)
            if (rotationsOTS.X == 0.0 && rotationsOTS.Y == 0.0 && rotationsOTS.Z == 0.0)
            {
                return matrix;
            }
            
            double orientation = rotationsOTS.X;
            double tilt = rotationsOTS.Y;
            double spin = rotationsOTS.Z;
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), orientation));

            Vector3D localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, tilt));

            Vector3D localZ = matrix.Transform(new Vector3D(0, 0, 1));
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
            double rotationZ = GetRotationZ(matrix);
            double rotationY = GetRotationY(matrix, rotationZ);
            return new Vector3D(GetRotationX(matrix, rotationZ, rotationY), rotationY, rotationZ);
        }

        public static Matrix3D RotateZXY(this Matrix3D matrix, Vector3D rotationsXYZ)
        {
            matrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), rotationsXYZ.Z));

            Vector3D localX = matrix.Transform(new Vector3D(1, 0, 0));
            matrix.Rotate(new Quaternion(localX, rotationsXYZ.X));

            Vector3D localY = matrix.Transform(new Vector3D(0, 1, 0));
            matrix.Rotate(new Quaternion(localY, rotationsXYZ.Y));
            return matrix;
        }
        /*
        public static Vector3D GetRotationsZXY(this Matrix3D matrix)
        {
            double rotationZ = GetRotationZ(matrix);
            double rotationX = GetRotationXZ(matrix, rotationZ);
            double rotationY = GetRotationYXZ(matrix, rotationX, rotationZ);
            return new Vector3D(rotationX, rotationY, rotationZ);
        }*/

        public static Point3D GetTranslation(this Matrix3D matrix)
        {
            return matrix.Transform(new Point3D(0, 0, 0));
        }

        public static double GetRotationZ(this Matrix3D matrix)
        {
            Vector3D localX = matrix.Transform(new Vector3D(1, 0, 0));
            Vector3D localZ = matrix.Transform(new Vector3D(0, 0, 1));

            // rotation about the Z axis
            localX.Z = 0.0;
            double angle = Vector3D.AngleBetween(localX, new Vector3D(1, 0, 0));
            double rotationZ = localX.Y < 0.0 ? angle * -1.0 : angle;
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
            Matrix3D matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));

            Vector3D localX = matrix2.Transform(new Vector3D(1, 0, 0));
            Vector3D localY = matrix2.Transform(new Vector3D(0, 1, 0));

            // rotation about the Y axis
            localX.Y = 0.0;
            double angle = Vector3D.AngleBetween(localX, new Vector3D(1, 0, 0));
            double rotationY = localX.Z > 0 ? angle * -1.0 : angle;
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
            Matrix3D matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));

            Vector3D localX = matrix2.Transform(new Vector3D(1, 0, 0));
            Vector3D localY = matrix2.Transform(new Vector3D(0, 1, 0));

            // rotation about the X axis
            localY.X = 0.0;
            double angle = Vector3D.AngleBetween(localY, new Vector3D(0, 1, 0));
            double rotationX = localX.Z > 0 ? angle * -1.0 : angle;
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
            double rotationZ = matrix.GetRotationZ();
            double rotationY = matrix.GetRotationY(rotationZ);
            return GetRotationX(matrix, rotationZ, rotationY);
        }

        public static double GetRotationX(this Matrix3D matrix, double rotationZ, double rotationY)
        {
            // back off z rotation and y rotation
            Matrix3D matrix2 = new Matrix3D();
            matrix2.Append(matrix);
            matrix2 = matrix2.RotateXYZ(new Vector3D(0.0, 0.0, rotationZ * -1.0));
            Matrix3D matrix3 = new Matrix3D();
            matrix3.Append(matrix2);
            matrix3 = matrix3.RotateXYZ(new Vector3D(0.0, rotationY * -1.0, 0.0));

            Vector3D localY = matrix3.Transform(new Vector3D(0, 1, 0));
            localY.X = 0.0;
            double angle = Vector3D.AngleBetween(localY, new Vector3D(0, 1, 0));
            double rotationX = localY.Z < 0 ? angle * -1.0 : angle;

            // rotation about the X axis
            return rotationX;
        }

        /*
        public static double GetRotationYXZ(this Matrix3D matrix, double rotationX, double rotationZ)
        {
            return 0.0;
        }*/

        public static Matrix3D SetDirectionVectors(this Matrix3D matrix, Vector3D xDirection, Vector3D yDirection, Vector3D zDirection)
        {
            // zRotation
            xDirection.Z = 0;
            xDirection.Normalize();
            double deltaZ = Abs(Vector3D.AngleBetween(new Vector3D(1, 0, 0), xDirection));
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
            Matrix3D inverse = Matrix3D.Multiply(new Matrix3D(), matrix);
            inverse.Invert();
            Vector3D zDir2 = inverse.Transform(zDirection);
            Vector3D localY = matrix.Transform(new Vector3D(0, 1, 0));
            double deltaY = Abs(Vector3D.AngleBetween(new Vector3D(0, 0, 1), zDir2));
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
            Vector3D yDir2 = inverse.Transform(yDirection);
            Vector3D localX = matrix.Transform(new Vector3D(1, 0, 0));
            double deltaX = Abs(Vector3D.AngleBetween(new Vector3D(0, 1, 0), yDir2));
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
            Vector3D zDirection = Vector3D.CrossProduct(xDirection, yDirection);

            // Align X axes
            Vector3D normal = Vector3D.CrossProduct(new Vector3D(1, 0, 0), xDirection);
            if (normal.Length > 0)
            {
                double deltaX = Vector3D.AngleBetween(new Vector3D(1, 0, 0), xDirection);
                matrix.Rotate(new Quaternion(normal, deltaX));
            }

            // Align Y axes
            Vector3D localY = matrix.Transform(new Vector3D(0, 1, 0));
            normal = Vector3D.CrossProduct(localY, yDirection);
            if (normal.Length > 0)
            {
                double deltaY = Vector3D.AngleBetween(localY, yDirection);
                matrix.Rotate(new Quaternion(normal, deltaY));
            }

            // Align Z axes
            Vector3D localZ = matrix.Transform(new Vector3D(0, 0, 1));
            normal = Vector3D.CrossProduct(localZ, zDirection);
            if (normal.Length > 0)
            {
                double deltaZ = Vector3D.AngleBetween(localZ, zDirection);
                matrix.Rotate(new Quaternion(normal, deltaZ));
            }

            return matrix;
        }

        public static Matrix3D AlignZDirectionVector(this Matrix3D matrix, Vector3D newZDirectionVector)
        {
            Vector3D negativeZDirection = new Vector3D(newZDirectionVector.X * -1.0, newZDirectionVector.Y * -1.0, newZDirectionVector.Z * -1.0);
            double orientation = negativeZDirection.GetOrientation();
            double polarAngle = newZDirectionVector.GetPolarAngle();

            return matrix.SetOrientation(orientation).SetTilt(polarAngle);
        }

        public static IDictionary GetDictionary(this Matrix3D matrix)
        {
            Dictionary<string, dynamic>? data = new Dictionary<string, dynamic>();
            //Vector3D rotationsZXY = Matrix3DExtension.GetRotationsZXY(matrix);
            Point3D translation = Matrix3DExtension.GetTranslation(matrix);
            data["X"] = $"{translation.X} m";
            data["Y"] = $"{translation.Y} m";
            data["Z"] = $"{translation.Z} m";
            Vector3D ots = Matrix3DExtension.GetRotationsOTS(matrix);
            if (Abs(ots.X) > 0.0001)
            {
                data["Orientation"] = $"{Round(ots.X, 4)} deg";
            }
            if (Abs(ots.Y) > 0.0001)
            {
                data["Tilt"] = $"{Round(ots.Y, 4)} deg";
            }
            if (Abs(ots.Z) > 0.0001)
            {
                data["Spin"] = $"{Round(ots.Z, 4)} deg";
            }
            //data["RotationX"] = $"{rotationsZXY.X} deg";
            //data["RotationY"] = $"{rotationsZXY.Y} deg";
            //data["RotationZ"] = $"{rotationsZXY.Z} deg";
            return data;
        }

        public static Rect3D Transform(this Matrix3D matrix, Rect3D bounds)
        {
            Point3D center = bounds.GetCenter();
            Point3D corner = new Point3D(
                bounds.Location.X + bounds.SizeX,
                bounds.Location.Y + bounds.SizeY,
                bounds.Location.Z + bounds.SizeZ);

            Point3D transformedCenter = matrix.Transform(center);
            Point3D transformedLocation = matrix.Transform(bounds.Location);
            Point3D transformedCorner = matrix.Transform(corner);

            Size3D transformedSize = new Size3D(
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
            Vector3D localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            Vector3D zcross = Vector3D.CrossProduct(new Vector3D(1, 0, 0), localX);
            double multiplier = 1.0;
            if (zcross.Z < 0)
            {
                multiplier = -1.0;
            }
            // rotation about the Z axis = angle between localX and parent X
            return System.Windows.Media.Media3D.Vector3D.AngleBetween(localX, new System.Windows.Media.Media3D.Vector3D(1, 0, 0)) * multiplier;
        }

        public static double GetTilt(this System.Windows.Media.Media3D.Matrix3D matrix)
        {
            // get the rotation about the X` local axis
            Vector3D localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            Vector3D localY = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(0, 1, 0));

            Matrix3D orientMatrix = new System.Windows.Media.Media3D.Matrix3D().SetOrientation(matrix.GetOrientation());
            Vector3D orientedY = orientMatrix.Transform(new System.Windows.Media.Media3D.Vector3D(0, 1, 0));

            double multiplier = 1.0;
            Vector3D xcross = Vector3D.CrossProduct(orientedY, localY);
            double xangle = Vector3D.AngleBetween(localX, xcross);
            if (xangle > 90.0) { multiplier = -1.0; }
            //if(xcross.X < 0.0) { multiplier = -1.0; }
            // rotation about the X' axis = angle between localY and orientedY
            double angle = System.Windows.Media.Media3D.Vector3D.AngleBetween(localY, orientedY) * multiplier;
            return angle;
        }

        public static double GetSpin(this System.Windows.Media.Media3D.Matrix3D matrix)
        {
            // get the rotatoin about the Z`` axis: angle between localX and tiltedX
            Vector3D localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
            Matrix3D tiltMatrix = new System.Windows.Media.Media3D.Matrix3D()
                .SetOrientation(matrix.GetOrientation())
                .SetTilt(matrix.GetTilt());
            Vector3D tiltedX = tiltMatrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));

            double multiplier = 1.0;
            Vector3D zcross = Vector3D.CrossProduct(tiltedX, localX);
            if (zcross.Z < 0) { multiplier = -1.0; }

            return System.Windows.Media.Media3D.Vector3D.AngleBetween(localX, tiltedX) * multiplier;
        }

        public static System.Windows.Media.Media3D.Matrix3D SetOrientation(this System.Windows.Media.Media3D.Matrix3D matrix, double orientationDegrees)
        {
            Vector3D translation = new System.Windows.Media.Media3D.Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
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
            Vector3D translation = new System.Windows.Media.Media3D.Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            /*
            var orientation = matrix.GetOrientation();
            var spin = matrix.GetSpin();

            var newMatrix = new Matrix3D();
            newMatrix = newMatrix.RotateOTS(new Vector3D(orientation, tilt,spin));
            newMatrix.Translate(translation);
            return newMatrix;*/

            // backoff translation
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(-translation.X, -translation.Y, -translation.Z));
            Vector3D localX = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(1, 0, 0));
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
            Vector3D translation = new System.Windows.Media.Media3D.Vector3D(matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ);
            // backoff translation
            matrix.Translate(new System.Windows.Media.Media3D.Vector3D(-translation.X, -translation.Y, -translation.Z));

            Vector3D localZ = matrix.Transform(new System.Windows.Media.Media3D.Vector3D(0, 0, 1));
            // backoff existing spin
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(localZ, matrix.GetSpin() * -1.0));
            matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(localZ, spin));
            // add back translation
            matrix.Translate(translation);
            return matrix;
        }

        public static bool AlmostEqual(this System.Windows.Media.Media3D.Matrix3D a,
            System.Windows.Media.Media3D.Matrix3D b, int decimals = 4)
        {
            if (Round(a.M11, decimals) != Round(b.M11, decimals)) return false;
            if (Round(a.M12, decimals) != Round(b.M12, decimals)) return false;
            if (Round(a.M13, decimals) != Round(b.M13, decimals)) return false;
            if (Round(a.M14, decimals) != Round(b.M14, decimals)) return false;
            if (Round(a.M21, decimals) != Round(b.M21, decimals)) return false;
            if (Round(a.M22, decimals) != Round(b.M22, decimals)) return false;
            if (Round(a.M23, decimals) != Round(b.M23, decimals)) return false;
            if (Round(a.M24, decimals) != Round(b.M24, decimals)) return false;
            if (Round(a.M31, decimals) != Round(b.M31, decimals)) return false;
            if (Round(a.M32, decimals) != Round(b.M32, decimals)) return false;
            if (Round(a.M33, decimals) != Round(b.M33, decimals)) return false;
            if (Round(a.M34, decimals) != Round(b.M34, decimals)) return false;
            if (Round(a.OffsetX, decimals) != Round(b.OffsetX, decimals)) return false;
            if (Round(a.OffsetY, decimals) != Round(b.OffsetY, decimals)) return false;
            if (Round(a.OffsetZ, decimals) != Round(b.OffsetZ, decimals)) return false;
            if (Round(a.M44, decimals) != Round(b.M44, decimals)) return false;
            return true;
        }

        public static List<double> GetValues(this System.Windows.Media.Media3D.Matrix3D matrix, int decimals)
        {
            List<double>? values = new List<double>();
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
    }
}
#endif