namespace Node.Net.Collections.Filters
{
    public class ValueFilter<T> : IFilter
    {
        public bool? DefaultResult { get; set; } = false;
        public T Value { get; set; } = default(T);
        public virtual bool? Include(object value)
        {
            if (value != null && Value != null)
            {
                if (typeof(T).IsAssignableFrom(value.GetType()))
                {
                    if (Value.Equals((T)value)) return true;
                }
            }
            return DefaultResult;
        }
    }
}
