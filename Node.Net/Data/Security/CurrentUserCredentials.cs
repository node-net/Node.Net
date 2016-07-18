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
        private CurrentUserCredentials()
        {
            this.SetTypeName();
            //Load();
        }

        private static CurrentUserCredentials _default;
        public static CurrentUserCredentials Default
        {
            get
            {
                if(_default == null)
                {
                    _default = new CurrentUserCredentials();
                    if (File.Exists(FileName))
                    {
                        var reader = new Readers.JsonReader();
                        var stored_creds = reader.Read(FileName) as IDictionary;
                        //var converter = new Readers.DictionaryTypeConverter();
                        //converter.Types.Add(nameof(Credential), typeof(Credential));
                        //var credentials = converter.Convert(stored_creds);
                        foreach (var key in stored_creds.Keys)
                        {
                            var dictionary = stored_creds[key] as IDictionary;
                            if (dictionary != null)
                            {
                                var converter = new Readers.DictionaryTypeConverter();
                                converter.Types.Add(nameof(Credential), typeof(Credential));
                                var credential = converter.Convert(dictionary) as ICredential;
                                if (credential != null)
                                {
                                    _default[key.ToString()] = credential;
                                }
                                //var credential = stored_creds[key] as ICredential;
                                //_default[key.ToString()] = credential;
                            }
                        }
                    }
                }
                return _default;
            }
        }

        private void Load()
        {
            var tmp = new Dictionary<string, dynamic>();
            foreach(var key in Default.Keys)
            {
                this[key.ToString()] = Default[key];
                tmp[key.ToString()] = Default[key];
            }
            foreach (var key in tmp.Keys)
            {
                this[key.ToString()] = tmp[key];

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
