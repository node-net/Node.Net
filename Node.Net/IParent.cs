//
// Copyright (c) 2016 Lou Parslow. Subject to the MIT license, see LICENSE.txt.
//
using System.Collections.Generic;
namespace Node.Net
{
    public interface IParent
    {
        Dictionary<string,IChild> GetChildren();
    }
}
