//
// Copyright (c) 2016 Lou Parslow. Subject to the Apache 2.0 license, see LICENSE.txt.
//
namespace Node.Net
{
    public interface IChild
    {
        IParent Parent { get; set; }
    }
}
