using System.Windows.Media.Media3D;

namespace Node.Net
{
    public static class Rect3DExtension
    {
        public static Rect3D Scale(this Rect3D source, double factor)
        {
            if (source.IsEmpty) return source;
            var newSize = new Size3D(source.SizeX * factor, source.SizeY * factor, source.SizeZ * factor);
            return new Rect3D
            {
                Size = newSize,
                Location = new Point3D
                (
                    source.X + (source.SizeX - newSize.X) / 2.0,
                    source.Y + (source.SizeY - newSize.Y) / 2.0,
                    source.Z + (source.SizeZ - newSize.Z) / 2.0
                )
            };
        }
    }
}
