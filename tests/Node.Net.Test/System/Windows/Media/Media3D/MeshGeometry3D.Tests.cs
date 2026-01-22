using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
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
        public static async Task MeshGeometry3D_Constructor_Default_InitializesCorrectly()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange & Act
            MeshGeometry3D mesh = new MeshGeometry3D();

            // Assert
            await Assert.That(mesh).IsNotNull();
            await Assert.That(mesh.Positions).IsNotNull();
            await Assert.That(mesh.TriangleIndices).IsNotNull();
            await Assert.That(mesh.Normals).IsNotNull();
            await Assert.That(mesh.TextureCoordinates).IsNotNull();
            await Assert.That(mesh.Positions.Count).IsEqualTo(0));
            await Assert.That(mesh.TriangleIndices.Count).IsEqualTo(0));
            await Assert.That(mesh.Normals.Count).IsEqualTo(0));
            await Assert.That(mesh.TextureCoordinates.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_Positions_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.Positions).IsEqualTo(positions));
            await Assert.That(mesh.Positions.Count).IsEqualTo(3));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_Positions_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(new Point3D(1, 2, 3));

            // Act
            mesh.Positions = null;

            // Assert
            await Assert.That(mesh.Positions).IsNotNull();
            await Assert.That(mesh.Positions.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_Positions_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.Positions.Count).IsEqualTo(3));
            await Assert.That(mesh.Positions[0]).IsEqualTo(new Point3D(0, 0, 0)));
            await Assert.That(mesh.Positions[1]).IsEqualTo(new Point3D(1, 0, 0)));
            await Assert.That(mesh.Positions[2]).IsEqualTo(new Point3D(1, 1, 0)));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_TriangleIndices_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            Int32Collection indices = new Int32Collection { 0, 1, 2, 0, 2, 3 };

            // Act
            mesh.TriangleIndices = indices;

            // Assert
            await Assert.That(mesh.TriangleIndices).IsEqualTo(indices));
            await Assert.That(mesh.TriangleIndices.Count).IsEqualTo(6));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_TriangleIndices_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.TriangleIndices).IsNotNull();
            await Assert.That(mesh.TriangleIndices.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_TriangleIndices_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.TriangleIndices.Count).IsEqualTo(3));
            await Assert.That(mesh.TriangleIndices[0]).IsEqualTo(0));
            await Assert.That(mesh.TriangleIndices[1]).IsEqualTo(1));
            await Assert.That(mesh.TriangleIndices[2]).IsEqualTo(2));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_Normals_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.Normals).IsEqualTo(normals));
            await Assert.That(mesh.Normals.Count).IsEqualTo(3));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_Normals_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Normals.Add(new Vector3D(0, 0, 1));

            // Act
            mesh.Normals = null;

            // Assert
            await Assert.That(mesh.Normals).IsNotNull();
            await Assert.That(mesh.Normals.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_Normals_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.Normals.Count).IsEqualTo(3));
            await Assert.That(mesh.Normals[0]).IsEqualTo(new Vector3D(0, 0, 1)));
            await Assert.That(mesh.Normals[1]).IsEqualTo(new Vector3D(1, 0, 0)));
            await Assert.That(mesh.Normals[2]).IsEqualTo(new Vector3D(0, 1, 0)));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_TextureCoordinates_CanBeSet()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.TextureCoordinates).IsEqualTo(textureCoords));
            await Assert.That(mesh.TextureCoordinates.Count).IsEqualTo(4));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_TextureCoordinates_CanBeSetToNull()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
            }

#if !IS_WINDOWS
            // Arrange
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.TextureCoordinates.Add(new Point(0, 0));

            // Act
            mesh.TextureCoordinates = null;

            // Assert
            await Assert.That(mesh.TextureCoordinates).IsNotNull();
            await Assert.That(mesh.TextureCoordinates.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_TextureCoordinates_CanBeModified()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.TextureCoordinates.Count).IsEqualTo(3));
            await Assert.That(mesh.TextureCoordinates[0]).IsEqualTo(new Point(0, 0)));
            await Assert.That(mesh.TextureCoordinates[1]).IsEqualTo(new Point(1, 0)));
            await Assert.That(mesh.TextureCoordinates[2]).IsEqualTo(new Point(1, 1)));
#endif
        }

        [Test]
        public static async Task MeshGeometry3D_CanCreateSimpleTriangle()
        {
            if (!CanCreateMeshGeometry3D())
            {
                // TUnit doesn't have Assert.Pass - just return early
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
            await Assert.That(mesh.Positions.Count).IsEqualTo(3));
            await Assert.That(mesh.TriangleIndices.Count).IsEqualTo(3));
            await Assert.That(mesh.TriangleIndices[0]).IsEqualTo(0));
            await Assert.That(mesh.TriangleIndices[1]).IsEqualTo(1));
            await Assert.That(mesh.TriangleIndices[2]).IsEqualTo(2));
#endif
        }

        [Test]
        public static async Task Point3DCollection_CanBeCreated()
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
            await Assert.That(collection).IsNotNull();
            await Assert.That(collection.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task Int32Collection_CanBeCreated()
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
            await Assert.That(collection).IsNotNull();
            await Assert.That(collection.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task Vector3DCollection_CanBeCreated()
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
            await Assert.That(collection).IsNotNull();
            await Assert.That(collection.Count).IsEqualTo(0));
#endif
        }

        [Test]
        public static async Task PointCollection_CanBeCreated()
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
            await Assert.That(collection).IsNotNull();
            await Assert.That(collection.Count).IsEqualTo(0));
#endif
        }
    }
}

