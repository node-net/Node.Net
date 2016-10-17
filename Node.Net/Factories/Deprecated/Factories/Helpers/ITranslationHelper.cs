using System.Collections;
using System.Windows.Media.Media3D;

namespace Node.Net.Factories.Deprecated.Factories.Helpers
{
    public static class ITranslationHelper
    {
        class ConcreteTranslation : ITranslation { public Vector3D Translation { get; set; } = new Vector3D(0, 0, 0); }

        public static ITranslation FromIDictionary(IDictionary source, IFactory factory)
        {
            var concreteTranslation = new ConcreteTranslation
            {
                Translation = new Vector3D(
                    IDictionaryHelper.GetLengthMeters(source, "X", factory),
                    IDictionaryHelper.GetLengthMeters(source, "Y", factory),
                    IDictionaryHelper.GetLengthMeters(source, "Z", factory))
            };
            return concreteTranslation;
        }
    }
}
