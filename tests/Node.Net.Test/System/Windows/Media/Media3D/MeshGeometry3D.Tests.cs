extern alias NodeNet;
using System;
using NUnit.Framework;
using NodeNet::System.Windows; // For Point type
using NodeNet::System.Windows.Media.Media3D;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class MeshGeometry3DTests
    {
        private static bool CanCreateMeshGeometry3D()
        {
#if !IS_WINDOWS
            try
            {
                return new MeshGeometry3D() != null;
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

        [Test]
        public static void MeshGeometry3D_Constructor_Default_InitializesCorrectly()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Assert
            Assert.That(mesh, Is.Not.Null);
            Assert.That(mesh.Positions, Is.Not.Null);
            Assert.That(mesh.TriangleIndices, Is.Not.Null);
            Assert.That(mesh.Normals, Is.Not.Null);
            Assert.That(mesh.TextureCoordinates, Is.Not.Null);
            Assert.That(mesh.Positions.Count, Is.EqualTo(0));
            Assert.That(mesh.TriangleIndices.Count, Is.EqualTo(0));
            Assert.That(mesh.Normals.Count, Is.EqualTo(0));
            Assert.That(mesh.TextureCoordinates.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void MeshGeometry3D_Positions_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            Point3DCollection positions = new Point3DCollection
            {
                new Point3D(0, 0, 0),
                new Point3D(1, 0, 0),
                new Point3D(1, 1, 0)
            };

            // Act
            mesh.Positions = positions;

            // Assert
            Assert.That(mesh.Positions, Is.EqualTo(positions));
            Assert.That(mesh.Positions.Count, Is.EqualTo(3));
#endif
        }

        [Test]
        public static void MeshGeometry3D_Positions_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(1, 2, 3));

            // Act
            mesh.Positions = null;

            // Assert
            Assert.That(mesh.Positions, Is.Not.Null);
            Assert.That(mesh.Positions.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void MeshGeometry3D_Positions_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Act
            mesh.Positions.Add(new Point3D(0, 0, 0));
            mesh.Positions.Add(new Point3D(1, 0, 0));
            mesh.Positions.Add(new Point3D(1, 1, 0));

            // Assert
            Assert.That(mesh.Positions.Count, Is.EqualTo(3));
            Assert.That(mesh.Positions[0], Is.EqualTo(new Point3D(0, 0, 0)));
            Assert.That(mesh.Positions[1], Is.EqualTo(new Point3D(1, 0, 0)));
            Assert.That(mesh.Positions[2], Is.EqualTo(new Point3D(1, 1, 0)));
#endif
        }

        [Test]
        public static void MeshGeometry3D_TriangleIndices_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            Int32Collection indices = new Int32Collection { 0, 1, 2, 0, 2, 3 };

            // Act
            mesh.TriangleIndices = indices;

            // Assert
            Assert.That(mesh.TriangleIndices, Is.EqualTo(indices));
            Assert.That(mesh.TriangleIndices.Count, Is.EqualTo(6));
#endif
        }

        [Test]
        public static void MeshGeometry3D_TriangleIndices_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            // Act
            mesh.TriangleIndices = null;

            // Assert
            Assert.That(mesh.TriangleIndices, Is.Not.Null);
            Assert.That(mesh.TriangleIndices.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void MeshGeometry3D_TriangleIndices_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Act
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            // Assert
            Assert.That(mesh.TriangleIndices.Count, Is.EqualTo(3));
            Assert.That(mesh.TriangleIndices[0], Is.EqualTo(0));
            Assert.That(mesh.TriangleIndices[1], Is.EqualTo(1));
            Assert.That(mesh.TriangleIndices[2], Is.EqualTo(2));
#endif
        }

        [Test]
        public static void MeshGeometry3D_Normals_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            Vector3DCollection normals = new Vector3DCollection
            {
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1)
            };

            // Act
            mesh.Normals = normals;

            // Assert
            Assert.That(mesh.Normals, Is.EqualTo(normals));
            Assert.That(mesh.Normals.Count, Is.EqualTo(3));
#endif
        }

        [Test]
        public static void MeshGeometry3D_Normals_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Normals.Add(new Vector3D(0, 0, 1));

            // Act
            mesh.Normals = null;

            // Assert
            Assert.That(mesh.Normals, Is.Not.Null);
            Assert.That(mesh.Normals.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void MeshGeometry3D_Normals_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Act
            mesh.Normals.Add(new Vector3D(0, 0, 1));
            mesh.Normals.Add(new Vector3D(1, 0, 0));
            mesh.Normals.Add(new Vector3D(0, 1, 0));

            // Assert
            Assert.That(mesh.Normals.Count, Is.EqualTo(3));
            Assert.That(mesh.Normals[0], Is.EqualTo(new Vector3D(0, 0, 1)));
            Assert.That(mesh.Normals[1], Is.EqualTo(new Vector3D(1, 0, 0)));
            Assert.That(mesh.Normals[2], Is.EqualTo(new Vector3D(0, 1, 0)));
#endif
        }

        [Test]
        public static void MeshGeometry3D_TextureCoordinates_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            PointCollection textureCoords = new PointCollection
            {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1)
            };

            // Act
            mesh.TextureCoordinates = textureCoords;

            // Assert
            Assert.That(mesh.TextureCoordinates, Is.EqualTo(textureCoords));
            Assert.That(mesh.TextureCoordinates.Count, Is.EqualTo(4));
#endif
        }

        [Test]
        public static void MeshGeometry3D_TextureCoordinates_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.TextureCoordinates.Add(new Point(0, 0));

            // Act
            mesh.TextureCoordinates = null;

            // Assert
            Assert.That(mesh.TextureCoordinates, Is.Not.Null);
            Assert.That(mesh.TextureCoordinates.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void MeshGeometry3D_TextureCoordinates_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Act
            mesh.TextureCoordinates.Add(new Point(0, 0));
            mesh.TextureCoordinates.Add(new Point(1, 0));
            mesh.TextureCoordinates.Add(new Point(1, 1));

            // Assert
            Assert.That(mesh.TextureCoordinates.Count, Is.EqualTo(3));
            Assert.That(mesh.TextureCoordinates[0], Is.EqualTo(new Point(0, 0)));
            Assert.That(mesh.TextureCoordinates[1], Is.EqualTo(new Point(1, 0)));
            Assert.That(mesh.TextureCoordinates[2], Is.EqualTo(new Point(1, 1)));
#endif
        }

        [Test]
        public static void MeshGeometry3D_CanCreateSimpleTriangle()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("MeshGeometry3D only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(0, 0, 0));
            mesh.Positions.Add(new Point3D(1, 0, 0));
            mesh.Positions.Add(new Point3D(0.5, 1, 0));
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);

            // Assert
            Assert.That(mesh.Positions.Count, Is.EqualTo(3));
            Assert.That(mesh.TriangleIndices.Count, Is.EqualTo(3));
            Assert.That(mesh.TriangleIndices[0], Is.EqualTo(0));
            Assert.That(mesh.TriangleIndices[1], Is.EqualTo(1));
            Assert.That(mesh.TriangleIndices[2], Is.EqualTo(2));
#endif
        }

        [Test]
        public static void Point3DCollection_CanBeCreated()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("Point3DCollection only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            Point3DCollection collection = new Point3DCollection();

            // Assert
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void Int32Collection_CanBeCreated()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("Int32Collection only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            Int32Collection collection = new Int32Collection();

            // Assert
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void Vector3DCollection_CanBeCreated()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("Vector3DCollection only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            Vector3DCollection collection = new Vector3DCollection();

            // Assert
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(0));
#endif
        }

        [Test]
        public static void PointCollection_CanBeCreated()
        {
            if (!CanCreateMeshGeometry3D())
            {
                Assert.Pass("PointCollection only available on non-Windows platforms");
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            PointCollection collection = new PointCollection();

            // Assert
            Assert.That(collection, Is.Not.Null);
            Assert.That(collection.Count, Is.EqualTo(0));
#endif
        }
    }
}

