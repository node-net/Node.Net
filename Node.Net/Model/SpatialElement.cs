using System.Windows.Media.Media3D;

namespace Node.Net.Model
{
    public class SpatialElement : Element, IModel3D, ITranslation3D,IRotation3D
    {
        public Matrix3D LocalToParent
        {
            get
            {
                var localToParent = new Matrix3D();
                localToParent.Rotate(Rotation3D);
                localToParent.Translate(Translation3D);
                return localToParent;
            }
        }

        public Vector3D Translation3D
        {
            get
            {
                return new Vector3D(
                    Measurement.Length.Parse(X)[Measurement.LengthUnit.Meter],
                    Measurement.Length.Parse(Y)[Measurement.LengthUnit.Meter],
                    Measurement.Length.Parse(Z)[Measurement.LengthUnit.Meter]);
            }
        }
        public Quaternion Rotation3D
        {
            get
            {
                var rotationZ = new Quaternion(new Vector3D(0, 0, 1),
                    Measurement.Angle.Parse(ZAxisRotation)[Measurement.AngularUnit.Degrees]);
                var rotationY = new Quaternion( new Vector3D(0,1,0),
                    Measurement.Angle.Parse(YAxisRotation)[Measurement.AngularUnit.Degrees]);
                var rotationX = new Quaternion(new Vector3D(1, 0, 0),
                    Measurement.Angle.Parse(XAxisRotation)[Measurement.AngularUnit.Degrees]);
                return Quaternion.Multiply(rotationX, Quaternion.Multiply(rotationY, rotationZ));
            }
        }

        public string X
        {
            get
            {
                if (ContainsKey(nameof(X))) return this[nameof(X)].ToString();
                return string.Empty;
            }
            set
            {
                this[nameof(X)] = value;
                //this.Update();
            }
        }
        public string Y
        {
            get {
                if (ContainsKey(nameof(Y))) return this[nameof(Y)].ToString();
                return string.Empty; }
            set
            {
                this[nameof(Y)] = value;
                //this.Update();
            }
        }
        public string Z
        {
            get {
                if (ContainsKey(nameof(Z))) return this[nameof(Z)].ToString();
                return string.Empty; }
            set
            {
                this[nameof(Z)] = value;
                //this.Update();
            }
        }

        public string XAxisRotation
        {
            get
            {
                if (ContainsKey(nameof(XAxisRotation))) return this[nameof(XAxisRotation)].ToString();
                if (ContainsKey("Spin")) return this["Spin"].ToString();
                return string.Empty;
            }
            set
            {
                this[nameof(XAxisRotation)] = value;
                //this.Update();
            }
        }
        public string YAxisRotation
        {
            get
            {
                if (ContainsKey(nameof(YAxisRotation))) return this[nameof(YAxisRotation)].ToString();
                if (ContainsKey("Tilt")) return this["Tilt"].ToString();
                return string.Empty;
            }
            set
            {
                this[nameof(YAxisRotation)] = value;
                //this.Update();
            }
        }
        public string ZAxisRotation
        {
            get
            {
                if (ContainsKey(nameof(ZAxisRotation))) return this[nameof(ZAxisRotation)].ToString();
                if (ContainsKey("Orientation")) return this["Orientation"].ToString();
                return string.Empty;
            }
            set
            {
                this[nameof(ZAxisRotation)] = value;
                //this.Update();
            }
        }
    }
}
