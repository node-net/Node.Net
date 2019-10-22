using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Media3D;

namespace Node.Net
{
    public class SpatialElement : Dictionary<string, dynamic>
    {
        public string Type
        {
            get { return this.Get<string>("Type"); }
        }
        [Category("Translation")]
        public string X
        {
            get { return this.Get<string>("X", "0 m"); }
            set { this.Set("X", value); }
        }
        [Category("Translation")]
        public string Y
        {
            get { return this.Get<string>("Y", "0 m"); }
            set { this.Set("Y", value); }
        }
        [Category("Translation")]
        public string Z
        {
            get { return this.Get<string>("Z", "0 m"); }
            set { this.Set("Z", value); }
        }

        [Category("Translation")]
        public string WorldOrigin
        {
            get
            {
                var worldOrigin = this.GetLocalToWorld().Transform(new Point3D(0, 0, 0));
                return worldOrigin.ToString();
            }
        }
        [Category("Rotations")]
        public string RotationZ
        {
            get { return this.Get<string>("RotationZ", "0"); }
            set { this.Set("RotationZ", value); }
        }
        [Category("Rotations")]
        public string RotationY
        {
            get { return this.Get<string>("RotationY", "0"); }
            set { this.Set("RotationY", value); }
        }
        [Category("Rotations")]
        public string RotationX
        {
            get { return this.Get<string>("RotationX", "0"); }
            set { this.Set("RotationX", value); }
        }
        [Category("Rotations")]
        public string WorldRotations
        {
            get { return this.GetLocalToWorld().GetRotationsXYZ().ToString(); }
        }
        [Browsable(false)]
        public new int Count { get { return base.Count; } }
        [Browsable(false)]
        public new IEqualityComparer<string> Comparer { get { return base.Comparer; } }
        [Browsable(false)]
        public new Dictionary<string, dynamic>.KeyCollection Keys { get { return base.Keys; } }
        [Browsable(false)]
        public new Dictionary<string, dynamic>.ValueCollection Values { get { return base.Values; } }
    }
}
