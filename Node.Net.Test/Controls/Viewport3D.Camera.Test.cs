using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Input;

namespace Node.Net.Controls
{
    [TestFixture,Apartment(ApartmentState.STA)]
    class Viewport3DCameraTest : Grid
    {
        [Test,Explicit]
        public void Viewport3D_CameraUsage_ShowDialog()
        {
            // given 1 Viewport with a Visual3D
            //       n ProjectionCameras
            new Window
            {
                Title = "Viewport Usage Test",
                WindowState = WindowState.Maximized,
                Content = new Viewport3DCameraTest()
            }.ShowDialog();
        }

        Viewport3D Viewport3D = new Viewport3D();
        Dictionary<string, ProjectionCamera> Cameras = new Dictionary<string, ProjectionCamera>
        {
            { "Plan Perspective",
                new PerspectiveCamera
                {
                    Position = new Point3D(0,0,20),
                    LookDirection = new Vector3D(0,0,-1),
                    UpDirection = new Vector3D(0,1,0)
                } },
            { "Front Perspective",
                new PerspectiveCamera
                {
                    Position = new Point3D(0,-20,0),
                    LookDirection = new Vector3D(0,1,0),
                    UpDirection = new Vector3D(0,0,1)
                }
            },
            { "Plan Orthographic",
                new OrthographicCamera
                {
                    Position = new Point3D(0,0,20),
                    LookDirection = new Vector3D(0,0,-1),
                    UpDirection = new Vector3D(0,1,0),
                    Width = 50,
                    NearPlaneDistance = -10,
                    FarPlaneDistance = double.PositiveInfinity
                }
            }
        };
        string CurrentCamera = "";
        Label CameraLabel = new Label();
        Button NextCameraButton = new Button { Content = "Next Camera" };
        Button ZoomExtentsButton = new Button { Content = "Zoom Extents" };
        Button ZoomInButton = new Button { Content = "Zoom In" };
        Button ZoomOutButton = new Button { Content = "Zoom Out" };
        //Label ShortCutsLabel = new System.Windows.Controls.Label { Content = "ZoomExtents (Z), Switch Camera (S)" };
        Label ScreenCoordinatesLabel = new Label();
        Label ModelCoordinatesLabel = new Label();
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Viewport3D.Children.Add(Factory.Default.Create<Visual3D>("Scene.Cubes.xaml"));

            RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition { Height =GridLength.Auto });

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.Children.Add(CameraLabel);

            var buttons = new StackPanel { Orientation = Orientation.Horizontal };
            buttons.Children.Add(NextCameraButton);
            buttons.Children.Add(ZoomExtentsButton);
            buttons.Children.Add(ZoomInButton);
            buttons.Children.Add(ZoomOutButton);

            grid.Children.Add(buttons);
            Grid.SetColumn(buttons, 1);
            NextCameraButton.Click += NextCameraButton_Click;
            ZoomExtentsButton.Click += ZoomExtentsButton_Click;
            ZoomInButton.Click += ZoomInButton_Click;
            ZoomOutButton.Click += ZoomOutButton_Click;
            grid.Children.Add(ScreenCoordinatesLabel);
            Grid.SetColumn(ScreenCoordinatesLabel, 3);
            grid.Children.Add(ModelCoordinatesLabel);
            Grid.SetColumn(ModelCoordinatesLabel, 4);
            Children.Add(grid);
            Children.Add(Viewport3D);
            Grid.SetRow(Viewport3D, 1);
            NextCamera();
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            Viewport3D.ZoomOut();
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            Viewport3D.ZoomIn();
        }

        private void ZoomExtentsButton_Click(object sender, RoutedEventArgs e)
        {
            Viewport3D.ZoomExtents();
        }

        private void NextCameraButton_Click(object sender, RoutedEventArgs e)
        {
            NextCamera();
        }

        private void NextCamera()
        {
            var lastCamera = CurrentCamera;
            var newCamera = Cameras.Keys.First();
            foreach(var key in Cameras.Keys)
            {
                if(lastCamera == CurrentCamera)
                {
                    newCamera = key;
                }
                lastCamera = key;
            }
            Viewport3D.Camera = Cameras[newCamera];
            CurrentCamera = newCamera;
        }
    }
}
