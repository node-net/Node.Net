namespace Node.Net.Json
{
    public class Filter : IFilter
    {
        public Filter() { }
        public Filter(IFilter keyFilter) { KeyFilter = keyFilter; }
        public Filter(string value) { includeString = value; }

        private string includeString = "";
        private IFilter keyFilter = null;
        public IFilter KeyFilter
        {
            get { return keyFilter; }
            set { keyFilter = value; }
        }
        public virtual bool Include(object value)
        {
            //if (key.ToString().IndexOf(search) > -1) results.Add(key);
            //else
            //{
            //    if (dictionary[key].ToString().IndexOf(search) > -1) results.Add(key);
            //}

            if(KeyValuePair.IsKeyValuePair(value))
            {
                if(!object.ReferenceEquals(null,keyFilter))
                {
                    if (!KeyFilter.Include(KeyValuePair.GetKey(value))) return false;
                }
                return Include(KeyValuePair.GetValue(value));
            }
            return true;
        }
    }
}
