namespace Node.Net.Environment
{
    public class MyDocuments
    {
        public static string GetFileName(string name)
        {
            return System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.MyDocuments)
                     + @"\" + name;
        }
    }
}