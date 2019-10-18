using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
    public struct Matrix
    {
        public Matrix(int rows,int columns)
        {
            Rows = rows;
            var data = new List<double>(Rows * columns);
            for(int i = 0; i < Rows * columns; ++i) { data.Add(0); }
            _data = data.ToArray();

            if(rows == columns)
            {
                for(int i = 0; i < rows; i++)
                {
                    int index = ((i * columns) + i);
                    _data[index] = 1;
                }
            }
        }

        public Matrix(int rows,double[] values)
        {
            Rows = rows;
            int columns = values.Length / rows;
            var data = new List<double>(Rows * columns);
            for (int i = 0; i < Rows * columns; ++i) { data.Add(0); }
            _data = data.ToArray();

            for(int i = 0; i < values.Length; i++)
            {
                _data[i] = values[i];
            }
        }

        public int Rows { get; }
        public int Columns { get { return _data.Length / Rows; } }
        public bool IsSquare { get { return Rows==Columns; } }

        public double this[int rowindex,int columnindex]
        {
            get {
                int index = ((rowindex * Columns) + columnindex);
                return _data[index];
            }
            set {
                int index = ((rowindex * Columns) + columnindex);
                _data[index] = value;
            }
        }

        private readonly double[] _data;

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            var hash = 0;
            for(int i = 0; i < _data.Length; i++)
            {
                hash ^= _data[i].GetHashCode();
            }
            return hash;
        }

        public int CompareTo(object value)
        {
            if (object.ReferenceEquals(this, value)) return 0;
            if (value is null) return 1;
            return GetHashCode().CompareTo(value.GetHashCode());
        }

        public static bool operator ==(Matrix left, Matrix right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Matrix left, Matrix right)
        {
            return !(left == right);
        }
    }
}
