using System;
using System.Collections.Generic;
using System.Text;

namespace Node.Net.Math
{
    public static class MatrixExtension
    {
        public static Matrix Multiply(this Matrix a,Matrix b)
        {
            var result = new Matrix(a.Rows, b.Columns);
            for(int arow = 0; arow < a.Rows; arow++)
            {
                for(int bcolumn = 0; bcolumn < b.Columns; bcolumn++)
                {
                    for (int acolumn = 0; acolumn < a.Columns; acolumn++)
                    {
                        result[arow, bcolumn] = a[arow, acolumn] * b[acolumn, bcolumn];
                    }
                }
            }
            return result;
        }
    }
}
