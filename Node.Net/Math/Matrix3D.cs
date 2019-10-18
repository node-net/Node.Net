using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
	public class Matrix3D
	{
       // public static double[] IdentityArray = new double[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        private readonly double[] _data = new double[16] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0 };

        public double M11 { get { return _data[0]; } set { _data[0] = value; } }

		public double M12 { get { return _data[1]; } set { _data[1] = value; } }
        public double M13 { get { return _data[2]; } set { _data[2] = value; } }
        public double M14 { get { return _data[3]; } set { _data[3] = value; } }

        public double M21 { get { return _data[4]; } set { _data[4] = value; } }

        public double M22 { get { return _data[5]; } set { _data[5] = value; } }
        public double M23 { get { return _data[6]; } set { _data[6] = value; } }
        public double M24 { get { return _data[7]; } set { _data[7] = value; } }

        public double M31 { get { return _data[8]; } set { _data[8] = value; } }

        public double M32 { get { return _data[9]; } set { _data[9] = value; } }
        public double M33 { get { return _data[10]; } set { _data[10] = value; } }
        public double M34 { get { return _data[11]; } set { _data[11] = value; } }
        public double M44 { get { return _data[12]; } set { _data[12] = value; } }
        public double OffsetX { get { return _data[13]; } set { _data[13] = value; } }
        public double OffsetY { get { return _data[14]; } set { _data[14] = value; } }
        public double OffsetZ { get { return _data[15]; } set { _data[15] = value; } }

        public bool IsIdentity { get { return true; } }
	}
}
