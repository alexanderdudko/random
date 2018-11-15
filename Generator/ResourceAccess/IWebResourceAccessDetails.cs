using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Generator.ResourceAccess
{
    public interface IWebResourceAccessDetails
    {
        string RequestUrl { get; }
        byte[] ProcessResponseToNumbers(Stream response);
    }
}
