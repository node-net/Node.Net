using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.View
{
    public class ReadOnlyTextView : TextBox
    {
        public ReadOnlyTextView()
        {
            this.DataContextChanged += TextView_DataContextChanged;
            this.IsReadOnly = true;
        }

        private void TextView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            update();
        }

        private void update()
        {
            Text = "";
            var ienumerable = Collections.KeyValuePair.GetValue(DataContext) as IEnumerable;
            if (!ReferenceEquals(null, ienumerable))
            {
                var sb = new StringBuilder();
                foreach (object item in ienumerable)
                {
                    sb.AppendLine(item.ToString());
                }
                Text = sb.ToString();
            }
        }
    }
}
