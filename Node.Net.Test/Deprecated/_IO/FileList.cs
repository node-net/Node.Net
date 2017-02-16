namespace Node.Net.IO
{
    public class FileList
    {
        private System.Collections.Generic.List<string> includePatterns
            = new System.Collections.Generic.List<string>();
        private System.Collections.Generic.List<string> excludePatterns
            = new System.Collections.Generic.List<string>();

        public FileList() { }
        public FileList(string pattern) { includePatterns.Add(pattern); }

        public void Include(string pattern) { includePatterns.Add(pattern); }
        public void Exclude(string pattern) { excludePatterns.Add(pattern); }

        public string[] Collect()
        {
            System.Collections.Generic.List<string> files
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