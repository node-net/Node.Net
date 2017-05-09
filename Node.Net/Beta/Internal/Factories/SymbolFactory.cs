using System;
using System.Collections;
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
                var labelText = $"{source.GetType().Name[0]}";
                if (typeof(IDictionary).IsAssignableFrom(source.GetType()))
                {
                    var dictionary = source as IDictionary;
                    var type = dictionary.Get<string>("Type");
                    if (type.Length > 0)
                    {
                        labelText = $"{type[0]}";
                        if (ParentFactory != null)
                        {
                            var uielement = ParentFactory.Create<object>($"Symbol.{type}.xaml") as UIElement;
                            if (uielement != null)
                            {
                                return new ViewboxSymbol { Child = uielement };
                            }
                        }
                    }
                }
                return GetDefaultsymbol(labelText, Brushes.Blue, Brushes.White);
            }
            return null;
        }

        private ISymbol GetDefaultsymbol(string text, Brush backgroundBrush, Brush foregroundBrush)
        {
            var canvas = new Canvas { Width = 16, Height = 16, ClipToBounds = true };
            canvas.Children.Add(new Ellipse
            {
                Fill = backgroundBrush,
                Width = 16,
                Height = 16
            });
            var label = new Label
            {
                Content = text,
                Foreground = foregroundBrush,
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
        public IFactory ParentFactory { get; set; }
    }
}
