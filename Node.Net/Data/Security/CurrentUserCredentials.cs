using System.Collections;
using System.Collections.Generic;
using System.IO;
using static System.Environment;

namespace Node.Net.Data.Security
{
    public class CurrentUserCredentials : Dictionary<string, dynamic>, ICredentials, Model.IModel
    {
        private static string FileName
        {
            get
            {
                return $"{GetFolderPath(SpecialFolder.UserProfile)}\\Credentials.json";
            }
        }
        public CurrentUserCredentials()
        {
            this.SetTypeName();

        }

        private void Load()
        {
            if (File.Exists(FileName))
            {
                var reader = new Readers.Reader(typeof(Credential).Assembly);
                var stored_creds = reader.Read(FileName) as IDictionary;
                foreach (var key in stored_creds.Keys)
                {
                    this[key.ToString()] = stored_creds[key];
                }
            }
        }

        public void Save()
        {
            Writers.Writer.Default.Write(FileName, this);
        }

        private readonly IProtection _protection = new CurrentUserProtection();
        public IProtection Protection
        {
            get
            {
                return _protection;
            }
        }
        public ICredential Get(string domain, string userName)
        {
            if (Count < 2) Load();
            if (ContainsKey($"{domain}|{userName}"))
            {
                return this[$"{domain}|{userName}"] as ICredential;
            }
            return null;
        }
        public void Set(string domain, string userName, System.Security.SecureString password)
        {
            this[$"{domain}|{userName}"] = new Credential(Protection)
            {
                Domain = domain,
                UserName = userName,
                Password = password
            };
            Save();
        }
        public void Remove(string domain,string userName)
        {
            var key = $"{domain}|{userName}";
            if(ContainsKey(key))
            {
                Remove(key);
                Save();
            }
        }
    }
}
