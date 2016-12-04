//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.Net
{
    public static class GlobalFunctions
    {
        static GlobalFunctions()
        {
            Node.Net.Factories.MetaDataMap.GetMetaDataFunction = Node.Net.Collections.GlobalFunctions.GetMetaDataFunction;
            //Node.Net.Factories.MetaDataMap.GetMetaDataFunction = Node.Net.Collections.MetaDataMap.GetMetaDataFunction;
            //Node.Net.Collections.IDictionaryExtension.GetLocalToParentFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToParent;
            //Node.Net.Collections.IDictionaryExtension.GetLocalToWorldFunction = Node.Net.Factories.Helpers.IDictionaryHelper.GetLocalToWorld;
        }
       
    }
}
