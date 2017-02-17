using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static System.Environment;
using System.Windows.Markup;

namespace Node.Net._Model3D
{
    class SpatialElement
    {
        public double X { get; set; } = 0;
        public double Y { get; set; } = 0;
        public double Z { get; set; } = 0;
        public double Orientation { get; set; } = 0;
        public double Tilt { get; set; } = 0;
        public double Spin { get; set; } = 0;
    }

    class TeapotOTSView : Grid
    {
        private readonly SpatialElement _spatialElement = new SpatialElement();
        //private Deprecated.Controls.PropertyControl _propertyControl;
        private HelixToolkit.Wpf.HelixViewport3D _viewport;
        private System.Windows.Media.Media3D.Model3D _teapotModel;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());

            _propertyControl = new Deprecated.Controls.PropertyControl();
            _propertyControl.ValueChanged += _propertyControl_ValueChanged;
            _propertyControl.DataContext = _spatialElement;
            Children.Add(_propertyControl);

            _viewport = new HelixToolkit.Wpf.HelixViewport3D();
            Children.Add(_viewport);
            Grid.SetColumn(_viewport, 1);

            _teapotModel = new HelixToolkit.Wpf.Teapot().Model;

            var filename = $"{GetFolderPath(SpecialFolder.MyDocuments)}\\Teapot.xaml";
            using (var stream = File.OpenWrite(filename))
            {

                XamlWriter.Save(new HelixToolkit.Wpf.Teapot(), stream);
            }
            Update();
        }

        private void _propertyControl_ValueChanged(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {
            var resources = new Resources.Resources();
            var teapotModel = new System.Windows.Media.Media3D.Model3DGroup
            {
                Transform = new System.Windows.Media.Media3D.RotateTransform3D(new System.Windows.Media.Media3D.AxisAngleRotation3D(new System.Windows.Media.Media3D.Vector3D(1, 0, 0), 90))
            };
            teapotModel.Children.Add(new HelixToolkit.Wpf.Teapot().Model);
            resources.Add("Teapot", teapotModel);
            var renderer = new Renderer
            {
                Resources = resources
            };

            var scene = new Dictionary<string, dynamic>();
            scene["Type"] = "Teapot";
            scene["ZAxisRotation"] = $"{_spatialElement.Orientation} deg";
            scene["YAxisRotation"] = $"{_spatialElement.Tilt} deg";
            scene["XAxisRotation"] = $"{_spatialElement.Spin} deg";
            scene["X"] = $"{_spatialElement.X} m";
            scene["Y"] = $"{_spatialElement.Y} m";
            scene["Z"] = $"{_spatialElement.Z} m";

            _viewport.Children.Clear();
            _viewport.Children.Add(new HelixToolkit.Wpf.SunLight());
            _viewport.Children.Add(renderer.GetVisual3D(scene));
            //_viewport.Children.Add(new HelixToolkit.Wpf.Teapot());
        }
    }
}
