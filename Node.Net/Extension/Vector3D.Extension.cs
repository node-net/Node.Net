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

        public static Vector3D ComputeRayPlaneIntersection(Vector3D rayVector, Vector3D rayPoint, Vector3D planeNormal, Vector3D planePoint)
        {
            var diff = rayPoint - planePoint;
            var prod1 = Vector3D.DotProduct(diff, planeNormal);// diff.Dot(planeNormal);
            var prod2 = Vector3D.DotProduct(rayVector, planeNormal);// rayVector.Dot(planeNormal);
            var prod3 = prod1 / prod2;
            return rayPoint - Vector3D.Multiply(prod3, rayVector);// rayVector * prod3;
        }
    }
}