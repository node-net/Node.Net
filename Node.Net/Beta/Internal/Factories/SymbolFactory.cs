using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Node.Net.Beta.Internal.Factories
{
    class SymbolFactory
    {
        class ViewboxSymbol : Viewbox, ISymbol { }
        public object Create(Type target_type, object source)
        {
            if (target_type != typeof(ISymbol)) return null;
            if (source != null)
            {
                var labelText = source.GetType().Name[0];

                if (typeof(IDictionary).IsAssignableFrom(source.GetType()))
                {
                    var dictionary = source as IDictionary;
                    if (dictionary.Contains("Type"))
                    {
                        var value = dictionary["Type"];
                        if (value != null && labelText.ToString().Length > 0)
                        {
                            labelText = labelText.ToString()[0];
                        }
                    }
                }
                var canvas = new Canvas { Width = 16, Height = 16, ClipToBounds = true };
                canvas.Children.Add(new Ellipse
                {
                    Fill = Brushes.Blue,
                    Width = 16,
                    Height = 16
                });
                var label = new Label
                {
                    Content = labelText,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                canvas.Children.Add(label);
                Canvas.SetTop(label, -5.5);
                Canvas.SetLeft(label, -1);
                return new ViewboxSymbol
                {
                    Child = canvas
                };
            }
            return null;
        }
        public IFactory ParentFactory { get; set; }
    }
}
