using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
	public struct Point3D
	{
		public Point3D(double x,double y,double z) { X = x;Y = y;Z = z; }
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public int CompareTo(object value)
        {
            if (object.ReferenceEquals(this, value)) return 0;
            if (value is null) return 1;
            return GetHashCode().CompareTo(value.GetHashCode());
        }

        public static bool operator ==(Point3D left, Point3D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point3D left, Point3D right)
        {
            return !(left == right);
        }
    }
}
