namespace Node.Net.Model3D
{
    public class MeshGeometry3D
    {
        public static string GetCreateMeshMethod(string methodName,System.Windows.Media.Media3D.MeshGeometry3D mesh)
        {
            // Positions double[] groups of 3
            string positions = "";
            foreach (System.Windows.Media.Media3D.Point3D position in mesh.Positions)
            {
                if (positions.Length > 0) positions += ",";
                positions += position.X.ToString() + "," + position.Y.ToString() + "," + position.Z.ToString();
            }
            // Normals double[] groups of 3
            string normals = "";
            foreach (System.Windows.Media.Media3D.Vector3D normal in mesh.Normals)
            {
                if (normals.Length > 0) normals += ",";
                normals += normal.X.ToString() + "," + normal.Y.ToString() + "," + normal.Z.ToString();
            }
            // TextureCoordinates double[] groups of 2
            string coordinates = "";
            foreach (System.Windows.Point coordinate in mesh.TextureCoordinates)
            {
                if (coordinates.Length > 0) coordinates += ",";
                coordinates += coordinate.X.ToString() + "," + coordinate.Y.ToString();
            }
            // TriangleIndicies int[]
            string indicies = "";
            foreach (int index in mesh.TriangleIndices)
            {
                if (indicies.Length > 0) indicies += ",";
                indicies += index.ToString();
            }

            System.Collections.Generic.List<string> code = new System.Collections.Generic.List<string>();

            code.Add("public static System.Windows.Media.Media3D.MeshGeometry3D " + methodName + "()");
            code.Add("{");
            code.Add("  double[] positions = {" + positions + "};");
            code.Add("  double[] normals = {" + normals + "};");
            code.Add("  double[] coordinates = {" + coordinates + "};");
            code.Add("  int[] indicies = {" + indicies + "};");
            code.Add("  System.Windows.Media.Media3D.MeshGeometry3D mesh = new System.Windows.Media.Media3D.MeshGeometry3D();");
            code.Add("  for(int i = 0; i < positions.Length; i+=3)");
            code.Add("  {");
            code.Add("    mesh.Positions.Add(new System.Windows.Media.Media3D.Point3D(");
            code.Add("                       positions[i], positions[i + 1], positions[i + 2]));");
            code.Add("  }");
            code.Add("  for(int i = 0; i < normals.Length; i+=3)");
            code.Add("  {");
            code.Add("    mesh.Normals.Add(new System.Windows.Media.Media3D.Vector3D(");
            code.Add("                       normals[i], normals[i + 1], normals[i + 2]));");
            code.Add("  }");
            code.Add("  for(int i = 0; i < coordinates.Length; i+=2)");
            code.Add("  {");
            code.Add("    mesh.TextureCoordinates.Add(new System.Windows.Point(");
            code.Add("                       coordinates[i], coordinates[i + 1]));");
            code.Add("  }");
            code.Add("  for(int i = 0; i < indicies.Length; i+=2)");
            code.Add("  {mesh.TriangleIndices.Add(indicies[i]);}");
            code.Add("  return mesh;");
            code.Add("}");
            return System.String.Join(System.Environment.NewLine, code.ToArray());
        }

        public static System.Windows.Media.Media3D.MeshGeometry3D LoadMeshResource(string name)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(MeshGeometry3D));
            System.Windows.Media.Media3D.MeshGeometry3D result
                = System.Windows.Markup.XamlReader.Load(assembly.GetManifestResourceStream(name))
                    as System.Windows.Media.Media3D.MeshGeometry3D;
            return result;
        }
        public static System.Windows.Media.Media3D.MeshGeometry3D CreateUnitCube()
        {
            return LoadMeshResource("Node.Net.Resources.UnitCube.MeshGeometry.xaml");
        }

        public static System.Windows.Media.Media3D.MeshGeometry3D CreateUnitPyramid()
        {
            return LoadMeshResource("Node.Net.Resources.UnitPyramid.MeshGeometry.xaml");
        }
        public static System.Windows.Media.Media3D.MeshGeometry3D CreateUnitCylinder()
        {
            return LoadMeshResource("Node.Net.Resources.UnitCylinder.MeshGeometry.xaml");
        }
        public static System.Windows.Media.Media3D.MeshGeometry3D CreateUnitCone()
        {
            return LoadMeshResource("Node.Net.Resources.UnitCone.MeshGeometry.xaml");
        }
        public static System.Windows.Media.Media3D.MeshGeometry3D CreateUnitSphere()
        {
            return LoadMeshResource("Node.Net.Resources.UnitSphere.MeshGeometry.xaml");
        }
        public static System.Windows.Media.Media3D.MeshGeometry3D CreateUnitHemisphere()
        {
            return LoadMeshResource("Node.Net.Resources.UnitHemisphere.MeshGeometry.xaml");
        }

        public static System.Windows.Media.Media3D.MeshGeometry3D TransformMesh(System.Windows.Media.Media3D.MeshGeometry3D mesh,System.Windows.Media.Media3D.Matrix3D transformation)
        {
            System.Windows.Media.Media3D.MeshGeometry3D mesh2 = new System.Windows.Media.Media3D.MeshGeometry3D();
            mesh2.TextureCoordinates = mesh.TextureCoordinates;
            mesh2.TriangleIndices = mesh.TriangleIndices;
            foreach(System.Windows.Media.Media3D.Point3D point in mesh.Positions)
            {
                mesh2.Positions.Add(Transform3D.ApplyTransform(point,transformation));
            }
            foreach (System.Windows.Media.Media3D.Vector3D normal in mesh.Normals)
            {
                mesh2.Normals.Add(Transform3D.ApplyTransform(normal, transformation));
            }
            return mesh2;
        }

        public static System.Windows.Media.Media3D.MeshGeometry3D GetTranslatedMesh(System.Windows.Media.Media3D.MeshGeometry3D mesh,
                                 double x_translation,double y_translation,double z_translation)
        {
            Transform3D transform = new Transform3D();
            transform.Translation = new System.Windows.Media.Media3D.Point3D(x_translation, y_translation, z_translation);
            return TransformMesh(mesh,transform.LocalToParent);
        }

        /*
        public static System.Windows.Media.Media3D.MeshGeometry3D GetUnitCube()
        {
            System.Windows.Media.Media3D.MeshGeometry3D unitCube
               = new System.Windows.Media.Media3D.MeshGeometry3D();
            int[] positions = { 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0 };
            int[] indices = { 2, 1, 0, 0, 3, 2, 6, 5, 4, 4, 7, 6, 10, 9, 8, 8, 11, 10, 14, 13, 12, 12, 15, 14, 18, 17, 16, 16, 19, 18, 22, 21, 20, 20, 23, 22 };

            System.Windows.Media.Media3D.Point3DCollection pointCollection
                = new System.Windows.Media.Media3D.Point3DCollection();

            for (int i = 0; i < positions.Length; i += 3)
            {
                pointCollection.Add(new System.Windows.Media.Media3D.Point3D(
                    positions[i], positions[i + 1], positions[i + 2]));
            }

            
            unitCube.Positions = pointCollection;
            unitCube.TriangleIndices = new System.Windows.Media.Int32Collection(indices);
            return unitCube;
        }
        public static System.Windows.Media.Media3D.MeshGeometry3D GetUnitPyramid()
        {
            System.Windows.Media.Media3D.MeshGeometry3D unitCube
               = new System.Windows.Media.Media3D.MeshGeometry3D();
            double[] positions = { -0.5,0.5,0 ,-0.5,-0.5,0 ,0,0,1 ,-0.5,-0.5,0 ,0.5,-0.5,0 ,0,0,1 ,0.5,-0.5,0 ,0.5,0.5,0 ,0,0,1 ,0.5,0.5,0 ,-0.5,0.5,0 ,0,0,1 };
            int[] indices = { 0 ,1 ,2 ,3 ,4 ,5 ,6 ,7 ,8 ,9 ,10 ,11 };

            System.Windows.Media.Media3D.Point3DCollection pointCollection
                = new System.Windows.Media.Media3D.Point3DCollection();

            for (int i = 0; i < positions.Length; i += 3)
            {
                pointCollection.Add(new System.Windows.Media.Media3D.Point3D(
                    positions[i], positions[i + 1], positions[i + 2]));
            }

            unitCube.Positions = pointCollection;
            unitCube.TriangleIndices = new System.Windows.Media.Int32Collection(indices);
            return unitCube;
        }*/
    }
}
