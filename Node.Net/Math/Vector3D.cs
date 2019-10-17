using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
	public struct Vector3D
	{
		//public Vector3D() { X = Y = Z = 0; }
		public Vector3D(double x,double y,double z) { X = x;Y = y;Z = z; }

		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
	}
}
