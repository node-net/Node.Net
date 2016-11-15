using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
{
    public class Header : StackPanel
    {
        public Header()
        {
            Orientation = Orientation.Horizontal;
            DataContextChanged += _DataContextChanged;
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private void Update()
        {
            Children.Clear();
            var toolTipText = "";
            if(DataContext != null)
            {
                toolTipText = Internal.KeyValuePair.GetValue(DataContext).GetType().FullName;
                var dictionary = Internal.KeyValuePair.GetValue(DataContext) as IDictionary;
                if(dictionary != null && dictionary.Contains("Type"))
                {
                    toolTipText = dictionary["Type"].ToString();
                }
            }
            Children.Add(new Node.Net.Controls.Image { DataContext = DataContext, Width = 16, Height = 16, ToolTip = toolTipText });
            if (DataContext != null)
            {
                var sContent = Internal.KeyValuePair.GetKey(DataContext).ToString();
                var idictionary = Internal.KeyValuePair.GetValue(DataContext) as IDictionary;
                if(idictionary != null)
                {
                    if (sContent.Length == 0)
                    {
                        if (idictionary.Contains("Type") && idictionary["Type"].ToString().Length > 0)
                        {
                            sContent = idictionary["Type"].ToString();
                        }
                    }
                }
                
                Children.Add(new Label { Content = sContent, ToolTip = toolTipText, VerticalAlignment = VerticalAlignment.Center });
            }
        }


    }
}
