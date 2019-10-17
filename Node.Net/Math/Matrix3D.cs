using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
	public struct Matrix3D
	{
		public double M11 { get; set; }

		public double M12 { get; set; }
		public double M13 { get; set; }
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
	}
}
