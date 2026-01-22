using System;
using System.Threading.Tasks;

namespace Node.Net.Test
{
    internal static class DrawingImageTests
    {
        private static bool CanCreateDrawingImage()
        {
#if !IS_WINDOWS
            try
            {
                DrawingImage drawingImage = new DrawingImage();
                return drawingImage != null;
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }

#if !IS_WINDOWS
        // Test concrete implementation for Geometry (only works on non-Windows)
        private class TestGeometry : Geometry
        {
            private Rect _bounds;

            public TestGeometry(Rect bounds)
            {
                _bounds = bounds;
            }

            public override Rect Bounds => _bounds;
        }

        private class TestDrawing : Drawing
        {
        }
#endif

        [Test]
        public static async Task DrawingImage_Constructor_Default_InitializesCorrectly()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange & Act
            DrawingImage drawingImage = new DrawingImage();

            // Assert
            await Assert.That(drawingImage, Is.Not.Null);
            await Assert.That(drawingImage.Drawing, Is.Null);
        }

        [Test]
        public static async Task DrawingImage_Constructor_WithDrawing_InitializesCorrectly()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
            GeometryDrawing drawing = new GeometryDrawing();

            // Act
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Assert
            await Assert.That(drawingImage, Is.Not.Null);
            await Assert.That(drawingImage.Drawing).IsEqualTo(drawing));
        }

        [Test]
        public static async Task DrawingImage_Drawing_CanBeSet()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
            DrawingImage drawingImage = new DrawingImage();
            GeometryDrawing drawing = new GeometryDrawing();

            // Act
            drawingImage.Drawing = drawing;

            // Assert
            await Assert.That(drawingImage.Drawing).IsEqualTo(drawing));
        }

        [Test]
        public static async Task DrawingImage_Width_WithNullDrawing_ReturnsZero()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
            DrawingImage drawingImage = new DrawingImage();

            // Act & Assert
            await Assert.That(drawingImage.Width).IsEqualTo(0.0));
        }

        [Test]
        public static async Task DrawingImage_Height_WithNullDrawing_ReturnsZero()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
            DrawingImage drawingImage = new DrawingImage();

            // Act & Assert
            await Assert.That(drawingImage.Height).IsEqualTo(0.0));
        }

        [Test]
        public static async Task DrawingImage_Width_WithGeometryDrawing_ReturnsBoundsWidth()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            Rect bounds = new Rect(0, 0, 100, 200);
            Geometry geometry = new TestGeometry(bounds);
            GeometryDrawing drawing = new GeometryDrawing { Geometry = geometry };
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            await Assert.That(drawingImage.Width).IsEqualTo(100.0));
#endif
        }

        [Test]
        public static async Task DrawingImage_Height_WithGeometryDrawing_ReturnsBoundsHeight()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            Rect bounds = new Rect(0, 0, 100, 200);
            Geometry geometry = new TestGeometry(bounds);
            GeometryDrawing drawing = new GeometryDrawing { Geometry = geometry };
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            await Assert.That(drawingImage.Height).IsEqualTo(200.0));
#endif
        }

        [Test]
        public static async Task DrawingImage_Width_WithNonGeometryDrawing_ReturnsZero()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            Drawing drawing = new TestDrawing();
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            await Assert.That(drawingImage.Width).IsEqualTo(0.0));
#endif
        }

        [Test]
        public static async Task DrawingImage_Height_WithNonGeometryDrawing_ReturnsZero()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("DrawingImage only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            Drawing drawing = new TestDrawing();
            DrawingImage drawingImage = new DrawingImage(drawing);

            // Act & Assert
            await Assert.That(drawingImage.Height).IsEqualTo(0.0));
#endif
        }

        [Test]
        public static async Task GeometryDrawing_Geometry_CanBeSet()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("GeometryDrawing only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            GeometryDrawing drawing = new GeometryDrawing();
            Rect bounds = new Rect(0, 0, 100, 200);
            Geometry geometry = new TestGeometry(bounds);

            // Act
            drawing.Geometry = geometry;

            // Assert
            await Assert.That(drawing.Geometry).IsEqualTo(geometry));
#endif
        }

        [Test]
        public static async Task GeometryDrawing_Brush_CanBeSet()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("GeometryDrawing only available on non-Windows platforms");
                return;
            }

            // Arrange
            GeometryDrawing drawing = new GeometryDrawing();
            SolidColorBrush brush = new SolidColorBrush(Colors.Red);

            // Act
            drawing.Brush = brush;

            // Assert
            await Assert.That(drawing.Brush).IsEqualTo(brush));
        }

        [Test]
        public static async Task GeometryDrawing_Pen_CanBeSet()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("GeometryDrawing only available on non-Windows platforms");
                return;
            }

            // Arrange
            GeometryDrawing drawing = new GeometryDrawing();
            Pen pen = new Pen();

            // Act
            drawing.Pen = pen;

            // Assert
            await Assert.That(drawing.Pen).IsEqualTo(pen));
        }

        [Test]
        public static async Task Rect_Properties_CanBeSet()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("Rect only available on non-Windows platforms");
                return;
            }

            // Arrange
#if !IS_WINDOWS
            Rect rect = new Rect();

            // Act
            rect.X = 10.0;
            rect.Y = 20.0;
            rect.Width = 100.0;
            rect.Height = 200.0;

            // Assert
            await Assert.That(rect.X).IsEqualTo(10.0));
            await Assert.That(rect.Y).IsEqualTo(20.0));
            await Assert.That(rect.Width).IsEqualTo(100.0));
            await Assert.That(rect.Height).IsEqualTo(200.0));
#endif
        }

        [Test]
        public static async Task Rect_Constructor_WithParameters_SetsProperties()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("Rect only available on non-Windows platforms");
                return;
            }

            // Arrange & Act
#if !IS_WINDOWS
            Rect rect = new Rect(10.0, 20.0, 100.0, 200.0);

            // Assert
            await Assert.That(rect.X).IsEqualTo(10.0));
            await Assert.That(rect.Y).IsEqualTo(20.0));
            await Assert.That(rect.Width).IsEqualTo(100.0));
            await Assert.That(rect.Height).IsEqualTo(200.0));
#endif
        }

        [Test]
        public static async Task Pen_Brush_CanBeSet()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("Pen only available on non-Windows platforms");
                return;
            }

            // Arrange
            Pen pen = new Pen();
            SolidColorBrush brush = new SolidColorBrush(Colors.Blue);

            // Act
            pen.Brush = brush;

            // Assert
            await Assert.That(pen.Brush).IsEqualTo(brush));
        }

        [Test]
        public static async Task Pen_Thickness_CanBeSet()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("Pen only available on non-Windows platforms");
                return;
            }

            // Arrange
            Pen pen = new Pen();

            // Act
            pen.Thickness = 2.5;

            // Assert
            await Assert.That(pen.Thickness).IsEqualTo(2.5));
        }

        [Test]
        public static async Task Pen_Thickness_DefaultsToOne()
        {
            if (!CanCreateDrawingImage())
            {
                // TUnit doesn't have Assert.Pass - just return early
                return;
                // Assert.Pass("Pen only available on non-Windows platforms");
                return;
            }

            // Arrange & Act
            Pen pen = new Pen();

            // Assert
            await Assert.That(pen.Thickness).IsEqualTo(1.0));
        }
    }
}

