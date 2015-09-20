namespace Node.Net.View
{
    public class DynamicView : System.Windows.Controls.ContentControl
    {
        public DynamicView() 
        {
            DataContextChanged += DynamicView_DataContextChanged;
        }

        
        public DynamicView(System.Windows.FrameworkElement defaultElement) 
        { 
            frameworkElements["Default"] = defaultElement;
            DataContextChanged += DynamicView_DataContextChanged;
        }
        private FrameworkElements frameworkElements = new FrameworkElements();
        public FrameworkElements Elements => frameworkElements;

        private System.Collections.Generic.Dictionary<System.Type, string> typeNames
            = new System.Collections.Generic.Dictionary<System.Type, string>();
        public System.Collections.Generic.Dictionary<System.Type, string> TypeNames => typeNames;
        void DynamicView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Update();
        }

        private string current = "";
        public string Current
        {
            get { return current; }
            set
            {
                if(current != value)
                {
                    current = value;
                    Update();
                }
            }
        }
        private void Update()
        {
            current = "";
            if(TypeNames.Count > 0)
            {
                if(!object.ReferenceEquals(null,KeyValuePair.GetValue(DataContext)))
                {
                    foreach(System.Type type in TypeNames.Keys)
                    {
                        if (type.IsAssignableFrom(KeyValuePair.GetValue(DataContext).GetType())) current = TypeNames[type];
                    }
                }
            }
            Content = GetContent();
            System.Windows.FrameworkElement element = Content as System.Windows.FrameworkElement;
            if (!object.ReferenceEquals(null, element)) element.DataContext = DataContext;
        }

        protected virtual object GetContent()
        {
            if (Elements.ContainsKey(current)) return Elements[current];
            if (Elements.ContainsKey("Default")) return Elements["Default"];
            return null;
        }
    }
}
