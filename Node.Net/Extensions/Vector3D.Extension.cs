using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Node.Net.Extensions
{
    public class Vector3DExtension
    {
        public static Vector3D GetTranslationMeters(object value)
        {
            Vector3D translation = new Vector3D();
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary))
            {
                if (dictionary.Contains("X"))
                {
                    translation.X = Measurement.Length.GetLengthMeters(dictionary, "X");
                }
                if (dictionary.Contains("Y"))
                {
                    translation.Y = Measurement.Length.GetLengthMeters(dictionary, "Y"); ;
                }
                if (dictionary.Contains("Z"))
                {
                    translation.Z = Measurement.Length.GetLengthMeters(dictionary, "Z");
                }
            }
            return translation;
        }
        public static Vector3D GetScaleMeters(object value)
        {
            Vector3D scale = new Vector3D(1, 1, 1);
            IDictionary dictionary = value as IDictionary;
            if (!ReferenceEquals(null, dictionary))
            {
                if (dictionary.Contains("ScaleX"))
                {
                    scale.X = Measurement.Length.GetLengthMeters(dictionary, "ScaleX");
                }
                if (dictionary.Contains("Length"))
                {
                    scale.X = Measurement.Length.GetLengthMeters(dictionary, "Length");
                }
                if (dictionary.Contains("ScaleY"))
                {
                    scale.Y = Measurement.Length.GetLengthMeters(dictionary, "ScaleY");
                }
                if (dictionary.Contains("Width"))
                {
                    scale.Y = Measurement.Length.GetLengthMeters(dictionary, "Width");
                }
                if (dictionary.Contains("ScaleZ"))
                {
                    scale.Z = Measurement.Length.GetLengthMeters(dictionary, "ScaleZ");
                }
                if (dictionary.Contains("Height"))
                {
                    scale.Z = Measurement.Length.GetLengthMeters(dictionary, "Height");
                }
            }
            return scale;
        }
        
        
    }
}
