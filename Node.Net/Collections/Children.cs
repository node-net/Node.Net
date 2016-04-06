namespace Node.Net.Collections
{
    public class Children : System.Collections.ObjectModel.ObservableCollection<object>
    {
        public Children() { }
        public Children(object value) { model = value; Update(); }
        private object model = null;

        public void Update()
        {
            System.Collections.Generic.List<object> children = new System.Collections.Generic.List<object>();
            System.Collections.IDictionary idictionary = model as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, idictionary))
            {
                foreach (object key in idictionary.Keys)
                {
                    System.Collections.Generic.KeyValuePair<object, object> kvp
                        = new System.Collections.Generic.KeyValuePair<object, object>(key, idictionary[key]);
                    children.Add(kvp);
                }
            }
            else
            {
                System.Collections.IEnumerable ienumerable = model as System.Collections.IEnumerable;
                if (!object.ReferenceEquals(null, ienumerable) && !typeof(string).IsAssignableFrom(model.GetType()))
                {
                    foreach (object item in ienumerable) children.Add(item);
                }
            }
            foreach (object item in children)
            {
                if (!Contains(item)) Add(item);
            }
            foreach (object item in this)
            {
                if (!children.Contains(item)) Remove(item);
            }
        }
    }
}
