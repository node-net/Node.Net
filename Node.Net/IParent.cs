//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
using System.Collections.Generic;
namespace Node.Net
{
    public interface IParent
    {
        Dictionary<string,IChild> GetChildren();
    }
}
