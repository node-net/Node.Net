using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net
{
	/// <summary>
	/// Extension methods for Rect3D
	/// </summary>
	public static class Rect3DExtension
	{
		/// <summary>
		/// Scale a Rect3D
		/// </summary>
		/// <param name="source"></param>
		/// <param name="factor"></param>
		/// <returns></returns>
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
		/// <summary>
		/// Get the center of a Rect3D
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>

		public static Point3D GetCenter(this Rect3D source)
		{
			var diagonal = new Vector3D(source.SizeX, source.SizeY, source.SizeZ);
			return source.Location + (diagonal * 0.5);
		}
	}
}
