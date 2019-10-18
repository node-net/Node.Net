using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
	public class Matrix3D
	{
        public Matrix3D() { }

        private Matrix _matrix = new Matrix(4, 4);

        public double M11 { get { return _matrix[0,0]; } set { _matrix[0,0] = value; } }

		public double M12 { get { return _matrix[0,1]; } set { _matrix[0,1] = value; } }
        public double M13 { get { return _matrix[0,2]; } set { _matrix[0,2] = value; } }
        public double M14 { get { return _matrix[0,3]; } set { _matrix[0,3] = value; } }

        public double M21 { get { return _matrix[1,0]; } set { _matrix[1,0] = value; } }

        public double M22 { get { return _matrix[1,1]; } set { _matrix[1,1] = value; } }
        public double M23 { get { return _matrix[1,2]; } set { _matrix[1,2] = value; } }
        public double M24 { get { return _matrix[1,3]; } set { _matrix[1,3] = value; } }

        public double M31 { get { return _matrix[2,0]; } set { _matrix[2,0] = value; } }

        public double M32 { get { return _matrix[2,1]; } set { _matrix[2,1] = value; } }
        public double M33 { get { return _matrix[2,2]; } set { _matrix[2,2] = value; } }
        public double M34 { get { return _matrix[2,3]; } set { _matrix[2,3] = value; } }

        public double OffsetX { get { return _matrix[3,0]; } set { _matrix[3,0] = value; } }
        public double OffsetY { get { return _matrix[3,1]; } set { _matrix[3,1] = value; } }
        public double OffsetZ { get { return _matrix[3,2]; } set { _matrix[3,2] = value; } }
        public double M44 { get { return _matrix[3, 3]; } set { _matrix[3, 3] = value; } }

        public bool IsIdentity
        {
            get
            {
                if (M11 != 1) return false;
                if (M12 != 0) return false;
                if (M13 != 0) return false;
                if (M14 != 0) return false;
                if (M21 != 0) return false;
                if (M22 != 1) return false;
                if (M23 != 0) return false;
                if (M24 != 0) return false;
                if (M31 != 0) return false;
                if (M32 != 0) return false;
                if (M33 != 1) return false;
                if (M34 != 0) return false;
                if (OffsetX != 0) return false;
                if (OffsetY != 0) return false;
                if (OffsetZ != 0) return false;
                if (M44 != 1) return false;
                return true;
            }
        }

        public void Translate(Vector3D translate)
        {
            OffsetX += translate.X;
            OffsetY += translate.Y;
            OffsetZ += translate.Z;
        }

        public Point3D Transform(Point3D point)
        {
            var pointMatrix = new Matrix(1,new double[] { point.X, point.Y, point.Z, 1 });
            var result = pointMatrix.Multiply(_matrix);
            return new Point3D(
              result[0,0],result[0,1],result[0,2]
            );
        }
	}
}
