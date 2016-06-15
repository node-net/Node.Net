namespace Node.Net.View
{
    public class TextView : System.Windows.Controls.UserControl
    {
        public TextView() { DataContextChanged += on_DataContextChanged; }
        public TextView(object value)
        {
            DataContext = value;
            DataContextChanged += on_DataContextChanged;
            update();
        }

        private System.Windows.Controls.TextBox textBox = null;
        protected override void OnInitialized(System.EventArgs e)
        {
            base.OnInitialized(e);

            textBox = new System.Windows.Controls.TextBox() 
                          { HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto, 
                            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto };
            textBox.AcceptsReturn = true;
            textBox.TextChanged += textBox_TextChanged;
            Content = textBox;
            update();
        }
        
        void textBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!updating)
            {
                object model = KeyValuePair.GetValue(DataContext);
                System.Collections.Generic.List<string> list = model as System.Collections.Generic.List<string>;
                if (!object.ReferenceEquals(null, list))
                {
                    System.Collections.Generic.List<string> newText = new System.Collections.Generic.List<string>(
                                  textBox.Text.Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));
                    list.Clear();
                    foreach (string line in newText) list.Add(line);
                }
            }
        }
        void on_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            update();
        }

        private bool updating = false;
        private void update()
        {
            if (object.ReferenceEquals(null, textBox)) return;

            updating = true;
            textBox.Text = "";
            try
            {
                object model = KeyValuePair.GetValue(DataContext);
                if (object.ReferenceEquals(null, model)) textBox.Text = "";
                else
                {
                    System.Collections.IEnumerable ienum = model as System.Collections.IEnumerable;
                    if (!object.ReferenceEquals(null, ienum))
                    {
                        foreach (object item in ienum)
                        {
                            textBox.AppendText(item.ToString() + System.Environment.NewLine);
                        }
                    }
                    else textBox.Text = model.ToString();
                }
            }
            finally
            {
                updating = false;
            }
        }
    }
}
