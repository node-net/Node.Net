using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
	public class Matrix3D
	{
       // public static double[] IdentityArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        private readonly double[] _data = new double[16] { 1, 0, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public double M11 { get { return _data[0]; } set { _data[0] = value; } }

		public double M12 { get { return _data[1]; } set { _data[1] = value; } }
        public double M13 { get { return _data[2]; } set { _data[2] = value; } }
        public double M14 { get; set; }

		public double M21 { get; set; }

		public double M22 { get; set; }
		public double M23 { get; set; }
		public double M24 { get; set; }

		public double M31 { get; set; }

		public double M32 { get; set; }
		public double M33 { get; set; }
		public double M34 { get; set; }
		public double M44 { get; set; }
		public double OffsetX { get; set; }
		public double OffsetY { get; set; }
		public double OffsetZ { get; set; }

        public bool IsIdentity { get { return true; } }
	}
}
