#if IS_WINDOWS 
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Rect3DExtension
    {
        public static Rect3D Scale(this Rect3D source, double factor)
        {
            if (source.IsEmpty)
            {
                return source;
            }

            Size3D newSize = new Size3D(source.SizeX * factor, source.SizeY * factor, source.SizeZ * factor);
            return new Rect3D
            {
                Size = newSize,
                Location = new Point3D
                (
                    source.X + ((source.SizeX - newSize.X) / 2.0),
                    source.Y + ((source.SizeY - newSize.Y) / 2.0),
                    source.Z + ((source.SizeZ - newSize.Z) / 2.0)
                )
            };
        }

        public static Point3D GetCenter(this Rect3D source)
        {
            Vector3D diagonal = new Vector3D(source.SizeX, source.SizeY, source.SizeZ);
            return source.Location + (diagonal * 0.5);
        }

        public static IEnumerable<Point3D> GetCorners(this Rect3D rect3d)
        {
            return new List<Point3D>
            {
                new Point3D(rect3d.X,rect3d.Y,rect3d.Z),
                new Point3D(rect3d.X + rect3d.SizeX,rect3d.Y,rect3d.Z),
                new Point3D(rect3d.X + rect3d.SizeX,rect3d.Y + rect3d.SizeY,rect3d.Z),
                new Point3D(rect3d.X ,rect3d.Y +rect3d.SizeY,rect3d.Z),
                new Point3D(rect3d.X,rect3d.Y,rect3d.Z + rect3d.SizeZ),
                new Point3D(rect3d.X + rect3d.SizeX,rect3d.Y,rect3d.Z+ rect3d.SizeZ),
                new Point3D(rect3d.X + rect3d.SizeX,rect3d.Y + rect3d.SizeY,rect3d.Z+ rect3d.SizeZ),
                new Point3D(rect3d.X ,rect3d.Y + rect3d.SizeY,rect3d.Z+ rect3d.SizeZ)
            };
        }
    }
}
#endif