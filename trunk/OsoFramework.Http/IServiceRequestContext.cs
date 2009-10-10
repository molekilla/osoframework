// Author: Rogelio Morrell , 2009
// Name: OsoFramework
// From: Panama
using System;
using System.Collections.Generic;
using System.Text;

namespace OsoFramework.Http
{
    public interface IServiceRequestContext
    {
        string Method { get; set; }
        string Query { get; set; }
        bool DoWriteStream { get; set; }
        byte[] StreamMessage { get; set; }

    }
}
