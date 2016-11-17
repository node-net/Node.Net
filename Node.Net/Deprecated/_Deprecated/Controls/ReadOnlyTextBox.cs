using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Deprecated.Controls
{
    public class ReadOnlyTextBox : TextBox
    {
        public ReadOnlyTextBox()
        {
            this.DataContextChanged += _DataContextChanged;
            this.IsReadOnly = true;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private void _DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnDataContextChanged();
        }

        protected virtual void OnDataContextChanged()
        {
            Text = "";

            Text = GetText();
        }

        protected virtual string GetText()
        {
            var dictionary = Node.Net.Collections.KeyValuePair.GetValue(DataContext) as IDictionary;
            if (!ReferenceEquals(null, dictionary))
            {
                return Json.Writer.ToString(dictionary, Json.JsonFormat.Indented);
            }
            var ienumerable = Node.Net.Collections.KeyValuePair.GetValue(DataContext) as IEnumerable;
            if (!ReferenceEquals(null, ienumerable))
            {
                var sb = new StringBuilder();
                foreach (object item in ienumerable)
                {
                    sb.AppendLine(item.ToString());
                }
                return sb.ToString();
            }
            return "";
        }
    }
}
