namespace Node.Net.Deprecated.Collections.Internal
{
    class StringFilter : ValueFilter<string>
    {
        public string Pattern { get; set; } = null;
        public override bool? Include(object value)
        {
            var base_result = base.Include(value);
            if (base_result.HasValue && base_result.Value) return true;
            if (value != null && Pattern != null && value.GetType() == typeof(string))
            {
                if (value.ToString().Contains(Pattern)) return true;
            }
            return DefaultResult;
        }
    }
}
