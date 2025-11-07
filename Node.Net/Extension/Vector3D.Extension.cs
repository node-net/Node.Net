#if IS_WINDOWS
using System;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Vector3DExtension
    {
        public static Vector3D GetPerpendicular(this Vector3D vector)
        {
            Vector3D other = new Vector3D(0, 0, 1);
            if (Vector3D.AngleBetween(vector, other) < 5)
            {
                other = new Vector3D(0, 1, 0);
            }

            return Vector3D.CrossProduct(other, vector);
        }

        /// <summary>
        /// Rotation about Z axis, from the X axis
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double GetAzimuthalAngle(this Vector3D vector)
        {
            return Math.Atan2(vector.Y, vector.X) * 180 / Math.PI;
        }

        /// <summary>
        /// Rotation abou the Z axis, from the Y axis
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static double GetOrientation(this Vector3D vector)
        {
            double polar = vector.GetPolarAngle();
            if (System.Math.Round(polar, 3) == 0) return 0.0;
            if (System.Math.Round(polar, 3) == 180) return 0.0;
            double azimuthal = vector.GetAzimuthalAngle();
            double orientation = azimuthal - 90.0;
            if (orientation < -180.0) { orientation += 360.0; }
            return orientation;
        }

        public static double GetPolarAngle(this Vector3D vector)
        {
            return Math.Atan2(Math.Pow((vector.X * vector.X) + (vector.Y * vector.Y), 0.5), vector.Z) * 180 / Math.PI;
        }

        public static Vector3D ComputeRayPlaneIntersection(Vector3D rayVector, Vector3D rayPoint, Vector3D planeNormal, Vector3D planePoint)
        {
            Vector3D diff = rayPoint - planePoint;
            double prod1 = Vector3D.DotProduct(diff, planeNormal);// diff.Dot(planeNormal);
            double prod2 = Vector3D.DotProduct(rayVector, planeNormal);// rayVector.Dot(planeNormal);
            double prod3 = prod1 / prod2;
            return rayPoint - Vector3D.Multiply(prod3, rayVector);// rayVector * prod3;
        }
    }
}
#endif