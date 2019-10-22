namespace Node.Net.Model3D.Rendering
{
    class MatrixTransform3DRenderer : Base
    {
        public MatrixTransform3DRenderer(IRenderer renderer) : base(renderer)
        { }
        public System.Windows.Media.Media3D.MatrixTransform3D GetMatrixTransform3D_NoScale(System.Collections.IDictionary value,
                                                 Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            return new System.Windows.Media.Media3D.MatrixTransform3D(GetTransform3D_NoScale(value,units).LocalToParent);
        }
        public System.Windows.Media.Media3D.MatrixTransform3D GetMatrixTransform3D_ScaleOnly(System.Collections.IDictionary value,
                                                 Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            return new System.Windows.Media.Media3D.MatrixTransform3D(GetTransform3D_ScaleOnly(value, units).LocalToParent);
        }

        private Node.Net.Model3D.Transform3D GetTransform3D(System.Collections.IDictionary value,
                                                 Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {

            Node.Net.Model3D.Transform3D result = new Node.Net.Model3D.Transform3D();
            result.Translation = GetTranslation(value, units);
            result.RotationOTS = GetRotationOTS(value);
            result.Scale = GetScale(value, units);
            return result;
        }
        public Node.Net.Model3D.Transform3D GetTransform3D_NoScale(System.Collections.IDictionary value,
                                                 Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            Node.Net.Model3D.Transform3D result = new Node.Net.Model3D.Transform3D();
            result.Translation = GetTranslation(value, units);
            result.RotationOTS = GetRotationOTS(value);
            return result;
        }

        private Node.Net.Model3D.Transform3D GetTransform3D_ScaleOnly(System.Collections.IDictionary value,
                                                Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            Node.Net.Model3D.Transform3D result = new Node.Net.Model3D.Transform3D();
            result.Scale = GetScale(value, units);
            return result;
        }

        private System.Windows.Media.Media3D.Point3D GetTranslation(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            System.Windows.Media.Media3D.Point3D result = new System.Windows.Media.Media3D.Point3D();
            if (value.Contains("X"))
            {
                Node.Net.Measurement.Length x = Node.Net.Measurement.Length.Parse(value["X"].ToString());
                result.X = x[units];
            }
            if (value.Contains("Y"))
            {
                Node.Net.Measurement.Length y = Node.Net.Measurement.Length.Parse(value["Y"].ToString());
                result.Y = y[units];
            }
            if (value.Contains("Z"))
            {
                Node.Net.Measurement.Length z = Node.Net.Measurement.Length.Parse(value["Z"].ToString());
                result.Z = z[units];
            }
            return result;
        }

        private System.Windows.Media.Media3D.Point3D GetRotationOTS(System.Collections.IDictionary value)
        {
            System.Windows.Media.Media3D.Point3D result = new System.Windows.Media.Media3D.Point3D();
            if (value.Contains("Orientation"))
            {
                Node.Net.Measurement.Angle o = Node.Net.Measurement.Angle.Parse(value["Orientation"].ToString());
                result.X = o[Node.Net.Measurement.AngularUnit.Degrees];
            }
            if (value.Contains("Tilt"))
            {
                Node.Net.Measurement.Angle t = Node.Net.Measurement.Angle.Parse(value["Tilt"].ToString());
                result.Y = t[Node.Net.Measurement.AngularUnit.Degrees];
            }
            if (value.Contains("Spin"))
            {
                Node.Net.Measurement.Angle s = Node.Net.Measurement.Angle.Parse(value["Spin"].ToString());
                result.Z = s[Node.Net.Measurement.AngularUnit.Degrees];
            }
            return result;
        }

        private System.Windows.Media.Media3D.Point3D GetScale(System.Collections.IDictionary value,
                    Node.Net.Measurement.LengthUnit units = Node.Net.Measurement.LengthUnit.Meter)
        {
            System.Windows.Media.Media3D.Point3D result = new System.Windows.Media.Media3D.Point3D(1, 1, 1);
            if (value.Contains("Length"))
            {
                Node.Net.Measurement.Length length = Node.Net.Measurement.Length.Parse(value["Length"].ToString());
                result.X = length[units];
            }
            if (value.Contains("Width"))
            {
                Node.Net.Measurement.Length width = Node.Net.Measurement.Length.Parse(value["Width"].ToString());
                result.Y = width[units];
            }
            if (value.Contains("Height"))
            {
                Node.Net.Measurement.Length height = Node.Net.Measurement.Length.Parse(value["Height"].ToString());
                result.Z = height[units];
            }
            return result;
        }
    }
}
