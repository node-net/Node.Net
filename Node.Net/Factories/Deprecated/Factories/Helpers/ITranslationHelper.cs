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
                    Node.Net.Factories.Extension.IDictionaryExtension.GetLengthMeters(source, "X"),
                    Node.Net.Factories.Extension.IDictionaryExtension.GetLengthMeters(source, "Y"),
                    Node.Net.Factories.Extension.IDictionaryExtension.GetLengthMeters(source, "Z"))
            };
            return concreteTranslation;
        }
    }
}
