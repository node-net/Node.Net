using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Data.Repositories
{
    public class KeySimplifierRepository : IRepository, IGetKeys
    {
        private IReadOnlyRepository _readOnlyRepository = new MemoryRepository();
        private string _key_strip_pattern;
        public KeySimplifierRepository(IReadOnlyRepository readOnlyRepository, string key_strip_pattern)
        {
            if (readOnlyRepository != null)
            {
                _readOnlyRepository = readOnlyRepository;
            }
            _key_strip_pattern = key_strip_pattern;
        }

        public IRead Reader { get { return _readOnlyRepository.Reader; } }
        public IWrite Writer
        {
            get
            {
                var repo = _readOnlyRepository as IRepository;
                if (repo != null) { return repo.Writer; }
                return null;
            }
        }
        public object Get(string key)
        {
            var result = _readOnlyRepository.Get(key);
            if(result == null)
            {
                var fullKey = $"{_key_strip_pattern}{key}";
                result = _readOnlyRepository.Get(fullKey);
            }
            return result;
        }

        public void Set(string key,object value)
        {
            var repo = _readOnlyRepository as IRepository;
            if(repo != null) { repo.Set(key, value); }
        }

        public string[] GetKeys(bool deep)
        {
            var keys = new List<string>();
            var igetKeys = _readOnlyRepository as IGetKeys;
            if(igetKeys != null)
            {
                foreach(var key in igetKeys.GetKeys(deep))
                {
                    keys.Add(key.Replace(_key_strip_pattern, ""));
                }
            }
            return keys.ToArray();
        }
    }
}
