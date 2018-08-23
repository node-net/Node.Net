using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Node.Net.Internal
{
	internal sealed class Matrix3DFactory : IFactory
	{
		public object Create(Type targetType, object source)
		{
			if (source != null)
			{
				if (source is IDictionary)
				{
					var matrix = CreateFromIDictionary(source as IDictionary);
					if (matrix.HasValue) return matrix.Value;
				}
			}

			return null;
		}

		public static IDictionary GetDictionary(Matrix3D matrix)
		{
			var data = new Dictionary<string, dynamic>();
			var rotationsZXY = Matrix3DExtension.GetRotationsZXY(matrix);
			var translation = Matrix3DExtension.GetTranslation(matrix);
			data["X"] = $"{translation.X} m";
			data["Y"] = $"{translation.Y} m";
			data["Z"] = $"{translation.Z} m";
			data["RotationX"] = $"{rotationsZXY.X} deg";
			data["RotationY"] = $"{rotationsZXY.Y} deg";
			data["RotationZ"] = $"{rotationsZXY.Z} deg";
			return data;
		}

		private static Matrix3D? CreateFromIDictionary(IDictionary dictionary)
		{
			var log = new StringBuilder();
			var matrix3D = new Matrix3D();
			var xDirection = new Vector3D(1, 0, 0);
			var yDirection = new Vector3D(0, 1, 0);
			if (dictionary.Contains("XDirection"))
			{
				try
				{
					if (dictionary.Contains("XDirection"))
					{
						var xDirectionValue = dictionary.Get<string>("XDirection", "1,0,0");
						log.AppendLine($" XDirection = {xDirectionValue}");
						xDirection = Vector3D.Parse(xDirectionValue);
					}
					if (dictionary.Contains("YDirection"))
					{
						var yDirectionValue = dictionary.Get<string>("YDirection", "0,1,0");
						log.AppendLine($" YDirection = {yDirectionValue}");
						yDirection = Vector3D.Parse(yDirectionValue);
					}
				}
				catch (Exception e)
				{
					log.AppendLine($" IDictionary FullName {dictionary.GetFullName()}");
					throw new InvalidOperationException($"Matrix3DFactory.CreateFromIDictionary{Environment.NewLine}{log}", e);
				}
				//var xDirection = Vector3D.Parse(dictionary.Get<string>("XDirection", "1,0,0"));
				//var yDirection = Vector3D.Parse(dictionary.Get<string>("YDirection", "0,1,0"));
				matrix3D = matrix3D.SetDirectionVectorsXY(xDirection, yDirection);
				matrix3D.Translate(GetTranslation(dictionary));
				return matrix3D;
			}
			matrix3D = RotateXYZ(matrix3D, GetRotationsXYZ(dictionary));
			matrix3D.Translate(GetTranslation(dictionary));
			if (!matrix3D.IsIdentity) return matrix3D;
			return null;
		}

		private static string RotationXKey = "Spin,RotationX,Roll";
		public static string RotationYKey = "Tilt,RotationY,Pitch";
		public static string RotationZKey = "Orientation,RotationZ,Yaw";

		public static Vector3D GetRotationsXYZ(IDictionary source)
		{
			return new Vector3D(
				Internal.Angle.GetDegrees(source.Get<string>(RotationXKey)),
				Internal.Angle.GetDegrees(source.Get<string>(RotationYKey)),
				Internal.Angle.GetDegrees(source.Get<string>(RotationZKey)));
		}

		public static Vector3D GetTranslation(IDictionary source)
		{
			return new Vector3D(
				Internal.Length.GetMeters(source.Get<string>("X")),
				Internal.Length.GetMeters(source.Get<string>("Y")),
				Internal.Length.GetMeters(source.Get<string>("Z")));
		}

		public static Matrix3D RotateXYZ(Matrix3D matrix, Vector3D rotationsXYZ) => matrix.RotateXYZ(rotationsXYZ);
	}
}