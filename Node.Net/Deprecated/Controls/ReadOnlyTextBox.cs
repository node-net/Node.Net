using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Node.Net.Controls
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
            var dictionary = Collections.KeyValuePair.GetValue(DataContext) as IDictionary;
            if (!object.ReferenceEquals(null, dictionary))
            {
                return Json.Writer.ToString(dictionary, Json.JsonFormat.Indented);
            }
            var ienumerable = Collections.KeyValuePair.GetValue(DataContext) as IEnumerable;
            if (!object.ReferenceEquals(null, ienumerable))
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
