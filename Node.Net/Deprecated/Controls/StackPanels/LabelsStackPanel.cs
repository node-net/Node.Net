using System.Collections.Generic;
using System.Windows;

namespace Node.Net.Controls.StackPanels
{
    public abstract class LabelsStackPanel : StackPanel
    {
        protected override FrameworkElement[] GetFrameworkElements()
        {
            var elements = new List<FrameworkElement>();
            foreach (var name in GetValues())
            {
                elements.Add(new System.Windows.Controls.Label { Content = name });
            }
            var widths = GetWidths();
            if(widths != null)
            {
                for (int i = 0; i < widths.Length; ++i)
                {
                    var width = widths[i];
                    if (width >= 0.0 && elements.Count > i)
                    {
                        var label = elements[i] as System.Windows.Controls.Label;
                        label.Width = width;
                    }
                }
            }
            return elements.ToArray();
        }

        protected abstract string[] GetValues();
        protected virtual double[] GetWidths() { return null; }
    }
}
