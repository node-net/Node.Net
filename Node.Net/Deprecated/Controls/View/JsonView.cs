using System.Windows.Controls;

namespace Node.Net.View
{
    public class JsonView : TextBox// System.Windows.Controls.UserControl
    {
        public JsonView() { DataContextChanged += on_DataContextChanged; }

        void on_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        public void Update()
        {
            var text =  "";
            var dictionary = Node.Net.View.KeyValuePair.GetValue(DataContext)
                as System.Collections.IDictionary;
            if (!ReferenceEquals(null,dictionary))
            {
                using(System.IO.MemoryStream memory = new System.IO.MemoryStream())
                {
                    Node.Net.Deprecated.Json.Writer.Write(dictionary,memory);
                    memory.Flush();
                    memory.Seek(0,System.IO.SeekOrigin.Begin);
                    var sr = new System.IO.StreamReader(memory);
                    text = sr.ReadToEnd();
                }
            }
            Text = text;
        }

        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            IsReadOnly = true;
            Update();
        }
    }
}
