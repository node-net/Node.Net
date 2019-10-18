using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Node.Net.Math
{
    [TestFixture]
    internal class MatrixTest
    {
        [Test]
        public void Usage()
        {
            var matrix = new Matrix(1, 4);
            Assert.AreEqual(1, matrix.Rows, "matrix.Rows");
            Assert.AreEqual(4, matrix.Columns, "matrix.Columns");
            Assert.AreEqual(0, matrix[0, 0], "matrix[0,0]");
            matrix[0, 3] = 1;
            Assert.AreEqual(1, matrix[0, 3], "matrix[0,3]");
        }
    }
}
