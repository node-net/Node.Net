using System;

namespace Node.Net.Beta.Internal.Factories
{
    class DoubleFactory
    {
        public object Create(Type targetType, object source)
        {
            if (targetType == null) return null;
            if (targetType != typeof(double)) return null;

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
