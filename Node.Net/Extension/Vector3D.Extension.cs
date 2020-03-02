using System;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Vector3DExtension
    {
        public static Vector3D GetPerpendicular(this Vector3D vector)
        {
            var other = new Vector3D(0, 0, 1);
            if (Vector3D.AngleBetween(vector, other) < 5)
            {
                other = new Vector3D(0, 1, 0);
            }

            return Vector3D.CrossProduct(other, vector);
        }

        public static double GetTheta(this Vector3D vector)
        {
            return Math.Atan2(vector.Y, vector.X) * 180 / Math.PI;
        }

        public static double GetPhi(this Vector3D vector)
        {
            return Math.Atan2(Math.Pow((vector.X * vector.X) + (vector.Y * vector.Y), 0.5), vector.Z) * 180 / Math.PI;
        }
    }
}