using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories
{
    class GetHelper
    {
        public static object GetResource(string name)
        {
            if (name == "Cube")
            {
                var meshBuilder = new HelixToolkit.Wpf.MeshBuilder();
                meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                return new GeometryModel3D
                {
                    Geometry = meshBuilder.ToMesh(),
                    Material = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Blue),
                    BackMaterial = HelixToolkit.Wpf.MaterialHelper.CreateMaterial(Colors.Gray)
                };
            }
            if (name == "Sunlight")
            {
                var sunlight = new Model3DGroup();
                sunlight.Children.Add(new DirectionalLight { Direction = new Vector3D(-0.32139380484327, 0.383022221559489, -0.866025403784439), Color = Color.FromRgb(255, 153, 153) });
                sunlight.Children.Add(new AmbientLight { Color = Color.FromRgb(255, 102, 102) });
                return sunlight;
            }
            var stream = GetStream(name);
            if(stream != null) { return _GetResource(name); }
            return null;
        }

        public static Stream GetStream(string name)
        {
            var assembly = typeof(GlobalFixture).Assembly;
            foreach (var resource_name in assembly.GetManifestResourceNames())
            {
                if (resource_name.Contains(name)) { return assembly.GetManifestResourceStream(resource_name); }
            }
            return null;
        }

        public static object _GetResource(string name)
        {
            try
            {
                if (name == null) return null;
                if (name.Length < 1) return null;

                var assembly = typeof(GlobalFixture).Assembly;
                foreach (var resource_name in assembly.GetManifestResourceNames())
                {
                    if (resource_name.Contains(name))
                    {
                        var stream = assembly.GetManifestResourceStream(resource_name);
                        try
                        {
                            if (name.Contains(".json"))
                            {
                                var reader = new Node.Net.Readers.JsonReader();
                                return reader.Read(stream);
                            }
                            if (name.Contains(".xaml"))
                            {
                                return XamlReader.Load(stream);
                            }
                        }
                        catch { }
                    }
                
                }
                return null;
            }
            catch(Exception e)
            {
                throw new Exception($"Error loading {name}", e);
            }
        }
    }
}
