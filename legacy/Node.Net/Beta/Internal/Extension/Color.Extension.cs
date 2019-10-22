using System.Windows.Media;

namespace Node.Net.Beta.Internal
{
    static class ColorExtension
    {
        public static Color GetScaledColor(this Color original, double factor)
        {
            return Color.FromArgb(original.A, (byte)(original.R * factor), (byte)(original.G * factor), (byte)(original.B * factor));
        }
    }
}
