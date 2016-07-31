namespace Node.Net.Data.Factories
{
    public class RepositoryFactory : IFactory
    {
        public IReadOnlyRepository Repository { get; set; }

        public T Create<T>(object value)
        {
            if (Repository != null)
            {
                var repo_value = Repository.Get(value.ToString());
                if (repo_value != null && typeof(T).IsAssignableFrom(repo_value.GetType()))
                {
                    return (T)repo_value;
                }
            }
            return default(T);
        }
    }
}
