using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net.Extensions
{
    public static class IMetaDataExtension
    {
        public static void Clear(IMetaData metadata,string name)
        {
            if(metadata != null)
            {
                if(metadata.MetaData.Contains(name))
                {
                    metadata.MetaData.Remove(name);
                }
            }
        }
        /*
        public static void DeepClear(IMetaData metadata, string name)
        {
            Clear(metadata, name);
            var parent = metadata as IParent;
            if(parent != null)
            {
                var children = parent.DeepCollect<IMetaData>();
                foreach(var key in children.Keys)
                {
                    Clear(children[key], name);
                }
            }

        }*/
    }
}
