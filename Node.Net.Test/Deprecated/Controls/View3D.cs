using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace Node.Net.Deprecated.Controls
{
    class View3D : Grid
    {
        public View3D()
        {
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private HelixToolkit.Wpf.HelixViewport3D _viewport;
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            _viewport = new HelixToolkit.Wpf.HelixViewport3D { ZoomExtentsWhenLoaded = true };
            Children.Add(_viewport);
        }

        private void Update()
        {
            if(!ReferenceEquals(null, _viewport))
            {
                _viewport.Children.Clear();

                var visual3D = Factory.Default.Transform(DataContext, typeof(Visual3D)) as Visual3D;
                if (!ReferenceEquals(null, visual3D))
                {
                    _viewport.Children.Add(visual3D);
                }
            }
        }
    }
}
