namespace Node.Net.IO
{
    public class TemporaryDirectory : System.IDisposable
    {

        public TemporaryDirectory()
        {
            fullName = System.IO.Path.GetTempPath() + @"\" + RandomString(8);
            if (System.IO.Directory.Exists(fullName)) System.IO.Directory.Delete(fullName, true);
            if (!System.IO.Directory.Exists(fullName)) System.IO.Directory.CreateDirectory(fullName);
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (System.IO.Directory.Exists(fullName))
                {
                    try
                    {
                        System.IO.Directory.Delete(fullName, true);
                    }
                    catch { }
                }
            }
        }

        public string FullName => fullName;
        public string GetFileName(string name)
        {
            var fileName = fullName + @"\" + name;
            var fi = new System.IO.FileInfo(fileName);
            if (!System.IO.Directory.Exists(fi.DirectoryName)) System.IO.Directory.CreateDirectory(fi.DirectoryName);
            return fileName;
        }
        private string fullName = "";
        private static string RandomString(int size)
        {
            var random = new System.Random((int)System.DateTime.Now.Ticks);
            var builder = new System.Text.StringBuilder();
            char ch;
            for(int i = 0; i < size; ++i)
            {
                ch = System.Convert.ToChar(System.Convert.ToInt32(System.Math.Floor(26*random.NextDouble()+65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}
