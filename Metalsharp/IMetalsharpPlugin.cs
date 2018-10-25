using System;
using System.Collections.Generic;
using System.Text;

namespace Metal.Sharp
{
    public interface IMetalsharpPlugin
    {
        Metalsharp Execute(Metalsharp directory);
    }
}
