using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Test.Controls
{
    public class View3D : Grid
    {
        public View3D(IFactory renderer, bool is_dynamic = false)
        {
            DataContextChanged += _DataContextChanged;
            dynamic = is_dynamic;
            renderers.Add(renderer);
        }
        public View3D(IFactory[] renderer_array, bool is_dynamic = false)
        {
            DataContextChanged += _DataContextChanged;
            dynamic = is_dynamic;
            renderers = new List<IFactory>(renderer_array);
        }

        public ProjectionCamera Camera
        {
            get { return camera; }
            set
            {
                camera = value;
                helixViewport3D.Camera = camera;
                viewport3D.Camera = camera;
                foreach (var viewport in backgroundViewports) { viewport.Camera = camera; }
            }
        }
        public event RoutedEventHandler CameraChanged;

        private readonly bool dynamic;
        private readonly List<IFactory> renderers = new List<IFactory>();
        private ProjectionCamera camera = new PerspectiveCamera { LookDirection = new Vector3D(0, 0, -1), UpDirection = new Vector3D(0, 1, 0) };
        private readonly HelixToolkit.Wpf.HelixViewport3D helixViewport3D = new HelixToolkit.Wpf.HelixViewport3D();
        private readonly System.Windows.Controls.Viewport3D viewport3D = new Viewport3D();
        private readonly List<Viewport3D> backgroundViewports = new List<Viewport3D>();

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) => Update();

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            helixViewport3D.CameraChanged += HelixViewport3D_CameraChanged;
            for (int i = 0; i < renderers.Count - 1; ++i)
            {
                var backgroundViewport = new Viewport3D();
                backgroundViewports.Add(backgroundViewport);
                Children.Add(backgroundViewport);
            }
            if (dynamic) Children.Add(helixViewport3D);
            else Children.Add(viewport3D);

            Update();
        }

        private void HelixViewport3D_CameraChanged(object sender, RoutedEventArgs e)
        {
            camera = helixViewport3D.Camera;
            foreach (var viewport in backgroundViewports) { viewport.Camera = camera; }
            viewport3D.Camera = camera;
            if (CameraChanged != null) CameraChanged(this, e);
        }

        private void Update()
        {
            helixViewport3D.Children.Clear();
            viewport3D.Children.Clear();
            for (int i = 0; i < renderers.Count; ++i)
            {
                var v3d = renderers[i].Create<Visual3D>(DataContext,null);
                if (i == renderers.Count - 1)
                {
                    if (dynamic)
                    {
                        if (v3d != null) helixViewport3D.Children.Add(v3d);
                    }
                    else
                    {
                        if (v3d != null) viewport3D.Children.Add(v3d);
                    }
                }
                else
                {
                    if (backgroundViewports.Count > i)
                    {
                        backgroundViewports[i].Children.Clear();
                        if (v3d != null) backgroundViewports[i].Children.Add(v3d);

                    }
                }
            }
            ZoomExtents();
        }

        public void ZoomExtents()
        {
            var new_camera = Camera.Clone();
            if (dynamic) new_camera.ZoomExtents(helixViewport3D.Viewport);
            else new_camera.ZoomExtents(viewport3D);
            Camera = new_camera;
        }

        public void ZoomExtents(Rect3D bounds)
        {
            var new_camera = Camera.Clone();
            if (dynamic) new_camera.ZoomExtents(helixViewport3D.Viewport, bounds);
            else new_camera.ZoomExtents(viewport3D, bounds);
            Camera = new_camera;
        }

        public static Rect3D Scale(Rect3D original, double factor)
        {
            var newsize = new Size3D(original.SizeX * factor,
                                     original.SizeY * factor,
                                     original.SizeZ * factor);
            var deltaSize = new Vector3D(newsize.X - original.SizeX,
                                       newsize.Y - original.SizeY,
                                       newsize.Z - original.SizeZ);
            return new Rect3D
            {
                Location = new Point3D(original.X - deltaSize.X / 2,
                                       original.Y - deltaSize.Y / 2,
                                       original.Z - deltaSize.Z / 2),
                Size = newsize
            };
        }

        public void ZoomExtents(double scale)
        {
            Visual3D v3d = null;
            if (renderers.Count > 0) v3d = renderers[renderers.Count - 1].Create<Visual3D>(DataContext, null);
            if (v3d != null)
            {
                var bounds = v3d.FindBounds(null);// Visual3DHelper.FindBounds(v3d, null);
                if (!bounds.IsEmpty)
                {
                    ZoomExtents(Scale(bounds, scale));
                }
            }
        }
    }
}
