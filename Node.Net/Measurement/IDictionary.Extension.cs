using System.Collections;

namespace Node.Net.Measurement
{
    public static class IDictionaryExtension
    {
        public static string GetLengthAsString(IDictionary dictionary, string name)
        {
            if (dictionary != null && name != null)
            {
                if (dictionary.Contains(name))
                {
                    return dictionary[name].ToString();
                }
            }
            return "0 m";
        }

        public static double GetLengthMeters(IDictionary dictionary, string name)
        {
            return Length.Parse(GetLengthAsString(dictionary, name))[LengthUnit.Meter];
        }

        public static void SetLength(IDictionary dictionary, string name, string value)
        {
            if (dictionary != null && name != null)
            {
                if (dictionary.Contains(name)) dictionary[name] = value;
                else dictionary.Add(name, value);
            }
        }
        public static void SetLength(IDictionary dictionary, string name, double lengthMeters)
        {
            SetLength(dictionary, name, new Length(lengthMeters, LengthUnit.Meter).ToString());
        }

        public static string GetAngleAsString(IDictionary dictionary, string name)
        {
            if (dictionary != null && name != null)
            {
                if (dictionary.Contains(name))
                {
                    return dictionary[name].ToString();
                }
            }
            return "0 deg";
        }
        public static double GetAngleDegrees(IDictionary dictionary, string name)
        {
            return Angle.Parse(GetAngleAsString(dictionary, name))[AngularUnit.Degrees];
        }
        public static void SetAngle(IDictionary dictionary, string name, string value)
        {
            if (dictionary != null && name != null)
            {
                if (dictionary.Contains(name)) dictionary[name] = value;
                else dictionary.Add(name, value);
            }
        }
        public static void SetAngle(IDictionary dictionary, string name, double angleDegrees)
        {
            SetAngle(dictionary, name, new Angle(angleDegrees, AngularUnit.Degrees).ToString());
        }
    }
}
