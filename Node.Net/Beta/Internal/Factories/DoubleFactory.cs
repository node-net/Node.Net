using System;

namespace Node.Net.Beta.Internal.Factories
{
    class DoubleFactory
    {
        public object Create(Type target_type, object source)
        {
            if (target_type == null) return null;
            if (target_type != typeof(double)) return null;

            if (source != null && source.GetType() == typeof(string))
            {
                var str = source.ToString();
                if (str.Contains("deg") || str.Contains("rad")) return Units.Angle.GetDegrees(str);
                return Units.Length.GetMeters(str);
            }

            return null;
        }
    }
}
