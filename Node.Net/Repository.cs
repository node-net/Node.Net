namespace Node.Net
{
    public class Repository
    {
        private static IRepository _default = null;
        public static IRepository Default
        {
            get { return _default; }
            set { _default = value; }
        }
    }
}
