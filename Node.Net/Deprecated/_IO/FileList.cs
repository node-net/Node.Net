namespace Node.Net._IO
{
    public class FileList
    {
        private readonly System.Collections.Generic.List<string> includePatterns
            = new System.Collections.Generic.List<string>();
        private readonly System.Collections.Generic.List<string> excludePatterns
            = new System.Collections.Generic.List<string>();

        public FileList() { }
        public FileList(string pattern) { includePatterns.Add(pattern); }

        public void Include(string pattern) { includePatterns.Add(pattern); }
        public void Exclude(string pattern) { excludePatterns.Add(pattern); }

        public string[] Collect()
        {
            var files
                = new System.Collections.Generic.List<string>();

            foreach (string includePattern in includePatterns)
            {
                foreach(string name in Directory.Glob(System.Environment.CurrentDirectory, includePattern))
                {
                    files.Add(name);
                }
            }
            foreach(string excludePattern in excludePatterns)
            {
                foreach (string name in Directory.Glob(System.Environment.CurrentDirectory, excludePattern))
                {
                    if (files.Contains(name))
                    {
                        files.Remove(name);
                    }
                }
            }
            return files.ToArray();
        }
    }
}