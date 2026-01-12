#if !IS_FRAMEWORK
using Microsoft.FluentUI.AspNetCore.Components;

namespace Node.Net.Components.Icons;

/// <summary>
/// Platform / operating system icons implemented as Fluent UI icons backed by inline SVG.
/// These are neutral metaphors (not trademarked logos) suitable for UI use.
/// </summary>
public static class OsIcons
{
    /// <summary>
    /// Windows-style platform icon (four-pane window metaphor).
    /// </summary>
    public sealed class Windows : Microsoft.FluentUI.AspNetCore.Components.Icon
    {
        private const string Svg =
            "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24'>" +
            "  <rect x='3'  y='3'  width='8' height='8' rx='1' fill='currentColor'/>" +
            "  <rect x='13' y='3'  width='8' height='8' rx='1' fill='currentColor'/>" +
            "  <rect x='3'  y='13' width='8' height='8' rx='1' fill='currentColor'/>" +
            "  <rect x='13' y='13' width='8' height='8' rx='1' fill='currentColor'/>" +
            "</svg>";

        public Windows()
            : base(nameof(Windows), IconVariant.Regular, IconSize.Size24, Svg)
        {
        }
    }

    /// <summary>
    /// Linux-style platform icon (terminal / console metaphor).
    /// </summary>
    public sealed class Linux : Microsoft.FluentUI.AspNetCore.Components.Icon
    {
        private const string Svg =
            "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24'>" +
            "  <rect x='3' y='5' width='18' height='14' rx='2' fill='none' stroke='currentColor' stroke-width='2'/>" +
            "  <path d='M7 10 L10 12 L7 14' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'/>" +
            "  <path d='M12 14 H17' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round'/>" +
            "</svg>";

        public Linux()
            : base(nameof(Linux), IconVariant.Regular, IconSize.Size24, Svg)
        {
        }
    }

    /// <summary>
    /// macOS-style platform icon (laptop metaphor).
    /// </summary>
    public sealed class Mac : Microsoft.FluentUI.AspNetCore.Components.Icon
    {
        private const string Svg =
            "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24'>" +
            "  <rect x='6' y='5' width='12' height='9' rx='1.5' fill='none' stroke='currentColor' stroke-width='2'/>" +
            "  <path d='M4 18 H20' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round'/>" +
            "  <path d='M8 18 L9 16 H15 L16 18' fill='none' stroke='currentColor' stroke-width='2' stroke-linejoin='round'/>" +
            "</svg>";

        public Mac()
            : base(nameof(Mac), IconVariant.Regular, IconSize.Size24, Svg)
        {
        }
    }
}
#endif
