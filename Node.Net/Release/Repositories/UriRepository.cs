using System;
using System.IO;
using System.Net;

namespace Node.Net.Repositories
{
    public class UriRepository
    {
        public Func<Stream, object> ReadFunction { get; set; } = Repository.DefaultReadFunction;
        public object Get(string name)
        {
            if (ReadFunction == null) return null;
            var uri = new System.Uri(name);
            switch (uri.Scheme)
            {
                case "http":
                case "https":
                    {
                        using (var webClient = new WebClient())
                        {
                            using (Stream stream = webClient.OpenRead(name))
                            {
                                return ReadFunction(stream);
                            }
                        }
                    }
                case "file":
                    {
                        using (FileStream fs = new FileStream(uri.LocalPath, FileMode.Open))
                        {
                            return ReadFunction(fs);
                        }
                    }
            }

            return null;
        }
    }
}
