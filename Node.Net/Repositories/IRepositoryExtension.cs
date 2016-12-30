using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Repositories
{
    public static class IRepositoryExtension
    {
        public static Item[] GetItems(this IRepository repository, string filter)
        {
            var items = new List<Item>();
            foreach(string name in repository.GetNames(filter))
            {
                items.Add(new Repositories.Item { Key = name, Repository = repository });
            }
            return items.ToArray();
        }
    }
}
