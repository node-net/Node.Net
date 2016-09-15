using System;
using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Factories.Helpers
{
    public static class IScaleHelper
    {
        class ConcreteScale : IScale { public Vector3D Scale { get; set; } = new Vector3D(1, 1, 1); }
        public static IScale FromIDictionary(IDictionary source, IFactory factory)
        {
            var concreteScale = new ConcreteScale();
            if (source != null)
            {
                concreteScale = new ConcreteScale
                {
                    Scale = new Vector3D(
                        GetScaleMeters(source, "ScaleX,Length", factory),
                        GetScaleMeters(source, "ScaleY,Width", factory),
                        GetScaleMeters(source, "ScaleZ,Height", factory))
                };
            }
            return concreteScale;
        }

        private static double GetScaleMeters(IDictionary source, string key, IFactory factory)
        {
            var lengthMeters = IDictionaryHelper.GetLengthMeters(source, key, factory);
            if (source.Contains(key)) return lengthMeters;
            if (key.Contains(","))
            {
                var keys = key.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var k in keys)
                {
                    if (source.Contains(k)) return lengthMeters;
                }
            }
            return 1;
        }
    }
}
