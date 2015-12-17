namespace Node.Net.Model3D.Rendering
{
    class Base
    {
        //public Base() { }
        public Base(Node.Net.Model3D.IRenderer renderer)
        {
            Renderer = renderer;
        }
        private Node.Net.Model3D.IRenderer renderer = null;
        public Node.Net.Model3D.IRenderer Renderer 
        { 
            get { return renderer; } 
            set 
            { 
                renderer = value; 
            }
        }
        /*
        //private System.Windows.ResourceDictionary resourceDictionary = new System.Windows.ResourceDictionary();
        public System.Windows.ResourceDictionary ResourceDictionary
        {
            get 
            { 
                return resourceDictionary; 
            }
            set { SetResourceDictionary(value); }
        }*/
        /*
        protected virtual void SetResourceDictionary(System.Windows.ResourceDictionary resources)
        {
            resourceDictionary = resources;
        }*/

        public string GetTypeString(object value)
        {
            if(!object.ReferenceEquals(null,value))
            {
                System.Collections.IDictionary dictionary = value as System.Collections.IDictionary;
                if (dictionary.Contains("Type")) return dictionary["Type"].ToString();
                return value.GetType().Name;
            }
            return "";
        }
    }
}
