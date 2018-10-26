using System;
using System.Collections.Generic;
using System.Text;

namespace Metal.Sharp
{
    public class MetalsharpFile
    {
        public string Text { get; set; }

        public string FilePath { get; set; }

        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
}
