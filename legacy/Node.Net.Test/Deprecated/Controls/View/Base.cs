namespace Node.Net.View
{
    public class Base : System.Windows.Controls.UserControl
    {
        public Base() 
        {
            DataContextChanged += On_DataContextChanged;
            Update();
        }
        public Base(object value) 
        { 
            DataContext = value;
            DataContextChanged += On_DataContextChanged;
            Update();
        }

        void On_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        public virtual void Update()
        {
        }
    }
}
