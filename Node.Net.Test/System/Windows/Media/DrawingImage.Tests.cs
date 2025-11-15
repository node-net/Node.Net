using System;
using NUnit.Framework;
using System.Windows.Media;

namespace Node.Net.Test
{
    [TestFixture]
    internal static class DrawingImageTests
    {
#if !IS_WINDOWS
        // Test concrete implementation for Geometry
        private class TestGeometry : Geometry
        {
            private Rect _bounds;

            public TestGeometry(Rect bounds)
            {
                _bounds = bounds;
            }

            public override Rect Bounds => _bounds;
        }

        [Test]
        public static void DrawingImage_Constructor_Default_InitializesCorrectly()
        {
            // Arrange & Act
            DrawingImage drawingImage = new DrawingImage();

            // Assert
            Assert.That(drawingImage, Is.Not.Null);
            Assert.That(drawingImage.Drawing, Is.Null);
        }

        [Test]
        public static void DrawingImage_Constructor_WithDrawing_InitializesCorrectly()
        {
            // Arrange
            GeometryDrawing drawing = new GeometryDrawing();

            // Act
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Assert
            Assert.That(drawingImage, Is.Not.Null);
            Assert.That(drawingImage.Drawing, Is.EqualTo(drawing));
        }

        [Test]
        public static void DrawingImage_Drawing_CanBeSet()
        {
            // Arrange
            DrawingImage drawingImage = new DrawingImage();
            GeometryDrawing drawing = new GeometryDrawing();

            // Act
            drawingImage.Drawing = drawing;

            // Assert
            Assert.That(drawingImage.Drawing, Is.EqualTo(drawing));
        }

        [Test]
        public static void DrawingImage_Width_WithNullDrawing_ReturnsZero()
        {
            // Arrange
            DrawingImage drawingImage = new DrawingImage();

            // Act & Assert
            Assert.That(drawingImage.Width, Is.EqualTo(0.0));
        }

        [Test]
        public static void DrawingImage_Height_WithNullDrawing_ReturnsZero()
        {
            // Arrange
            DrawingImage drawingImage = new DrawingImage();

            // Act & Assert
            Assert.That(drawingImage.Height, Is.EqualTo(0.0));
        }

        [Test]
        public static void DrawingImage_Width_WithGeometryDrawing_ReturnsBoundsWidth()
        {
            // Arrange
            Rect bounds = new Rect(0, 0, 100, 200);
            Geometry geometry = new TestGeometry(bounds);
            GeometryDrawing drawing = new GeometryDrawing { Geometry = geometry };
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            Assert.That(drawingImage.Width, Is.EqualTo(100.0));
        }

        [Test]
        public static void DrawingImage_Height_WithGeometryDrawing_ReturnsBoundsHeight()
        {
            // Arrange
            Rect bounds = new Rect(0, 0, 100, 200);
            Geometry geometry = new TestGeometry(bounds);
            GeometryDrawing drawing = new GeometryDrawing { Geometry = geometry };
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            Assert.That(drawingImage.Height, Is.EqualTo(200.0));
        }

        [Test]
        public static void DrawingImage_Width_WithNonGeometryDrawing_ReturnsZero()
        {
            // Arrange
            Drawing drawing = new TestDrawing();
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            Assert.That(drawingImage.Width, Is.EqualTo(0.0));
        }

        [Test]
        public static void DrawingImage_Height_WithNonGeometryDrawing_ReturnsZero()
        {
            // Arrange
            Drawing drawing = new TestDrawing();
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            Assert.That(drawingImage.Height, Is.EqualTo(0.0));
        }

        [Test]
        public static void GeometryDrawing_Geometry_CanBeSet()
        {
            // Arrange
            GeometryDrawing drawing = new GeometryDrawing();
            Rect bounds = new Rect(0, 0, 100, 200);
            Geometry geometry = new TestGeometry(bounds);

            // Act
            drawing.Geometry = geometry;

            // Assert
            Assert.That(drawing.Geometry, Is.EqualTo(geometry));
        }

        [Test]
        public static void GeometryDrawing_Brush_CanBeSet()
        {
            // Arrange
            GeometryDrawing drawing = new GeometryDrawing();
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);

            // Act
            drawing.Brush = brush;

            // Assert
            Assert.That(drawing.Brush, Is.EqualTo(brush));
        }

        [Test]
        public static void GeometryDrawing_Pen_CanBeSet()
        {
            // Arrange
            GeometryDrawing drawing = new GeometryDrawing();
            Pen pen = new Pen();

            // Act
            drawing.Pen = pen;

            // Assert
            Assert.That(drawing.Pen, Is.EqualTo(pen));
        }

        [Test]
        public static void Rect_Properties_CanBeSet()
        {
            // Arrange
            Rect rect = new Rect();

            // Act
            rect.X = 10.0;
            rect.Y = 20.0;
            rect.Width = 100.0;
            rect.Height = 200.0;

            // Assert
            Assert.That(rect.X, Is.EqualTo(10.0));
            Assert.That(rect.Y, Is.EqualTo(20.0));
            Assert.That(rect.Width, Is.EqualTo(100.0));
            Assert.That(rect.Height, Is.EqualTo(200.0));
        }

        [Test]
        public static void Rect_Constructor_WithParameters_SetsProperties()
        {
            // Arrange & Act
            Rect rect = new Rect(10.0, 20.0, 100.0, 200.0);

            // Assert
            Assert.That(rect.X, Is.EqualTo(10.0));
            Assert.That(rect.Y, Is.EqualTo(20.0));
            Assert.That(rect.Width, Is.EqualTo(100.0));
            Assert.That(rect.Height, Is.EqualTo(200.0));
        }

        [Test]
        public static void Pen_Brush_CanBeSet()
        {
            // Arrange
            Pen pen = new Pen();
            SolidColorBrush brush = new SolidColorBrush(Colors.Blue);

            // Act
            pen.Brush = brush;

            // Assert
            Assert.That(pen.Brush, Is.EqualTo(brush));
        }

        [Test]
        public static void Pen_Thickness_CanBeSet()
        {
            // Arrange
            Pen pen = new Pen();

            // Act
            pen.Thickness = 2.5;

            // Assert
            Assert.That(pen.Thickness, Is.EqualTo(2.5));
        }

        [Test]
        public static void Pen_Thickness_DefaultsToOne()
        {
            // Arrange & Act
            Pen pen = new Pen();

            // Assert
            Assert.That(pen.Thickness, Is.EqualTo(1.0));
        }

        private class TestDrawing : Drawing
        {
        }
#endif
    }
}

