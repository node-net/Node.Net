using System.Windows.Media.Media3D;

namespace Node.Net.Model
{
    public class SpatialElement : Element, IModel3D, ITranslation3D
    {
        public Matrix3D LocalToParent { get; set; } = new Matrix3D();

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
                this.Update();
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
                this.Update();
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
                this.Update();
            }
        }

    }
}
