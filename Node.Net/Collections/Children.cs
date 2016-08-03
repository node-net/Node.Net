namespace Node.Net.Collections
{
    public class Children : System.Collections.ObjectModel.ObservableCollection<object>
    {
        public Children() { }
        public Children(object value) { model = value; Update(); }
        private object model = null;

        public void Update()
        {
            var children = new System.Collections.Generic.List<object>();
            var idictionary = model as System.Collections.IDictionary;
            if (!object.ReferenceEquals(null, idictionary))
            {
                foreach (object key in idictionary.Keys)
                {
                    var kvp
                        = new System.Collections.Generic.KeyValuePair<object, object>(key, idictionary[key]);
                    children.Add(kvp);
                }
            }
            else
            {
                var ienumerable = model as System.Collections.IEnumerable;
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
