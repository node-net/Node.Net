using System;
using System.Windows;
using System.Windows.Threading;

namespace Node.Net.Deprecated.Extensions
{
    public static class UIElementExtension
    {
        private static Action EmptyDelegate = delegate () { };
        public static void Refresh(this UIElement element)
        {
            element.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
