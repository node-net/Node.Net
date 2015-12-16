namespace Node.Net.Model3D.Rendering
{
    class Base
    {
        public Base(IRenderer renderer)
        {
            Renderer = renderer;
        }
        private IRenderer renderer = null;
        public IRenderer Renderer 
        { 
            get { return renderer; } 
            set 
            { 
                renderer = value; 
            }
        }

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
