using System.Windows.Controls;

namespace Node.Net.View
{
    public class JsonView : TextBox// System.Windows.Controls.UserControl
    {
        public JsonView() { DataContextChanged += on_DataContextChanged; }
        //private System.Windows.Controls.TextBox textBox = null;

        void on_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }
        public void Update()
        {
            /*
            if(object.ReferenceEquals(null,DataContext))
            {
                Background = System.Windows.Media.Brushes.DarkGray;
            }
            else
            {
                Background = System.Windows.SystemColors.ControlBrush;
            }
            */
            string text =  "";
            System.Collections.IDictionary dictionary = Node.Net.View.KeyValuePair.GetValue(DataContext)
                as System.Collections.IDictionary;
            if(!object.ReferenceEquals(null,dictionary))
            {
                using(System.IO.MemoryStream memory = new System.IO.MemoryStream())
                {
                    Node.Net.Json.Writer.Write(dictionary,memory);
                    memory.Flush();
                    memory.Seek(0,System.IO.SeekOrigin.Begin);
                    System.IO.StreamReader sr = new System.IO.StreamReader(memory);
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
            /*
            textBox = new System.Windows.Controls.TextBox()
            {
                HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto,
                IsReadOnly=true
            };
            Content = textBox;
            */
            Update();
        }
    }
}
