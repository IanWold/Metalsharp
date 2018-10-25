using System.Collections.Generic;

namespace Metal.Sharp
{
    public class OutputFile : MetalsharpFile
    {
        public OutputFile(string path, string text)
        {
            OutputPath = path;
            Text = text;
        }

        #region Properties

        public string OutputPath { get; set; }

        public string Extension
        {
            get
            {
                var splitString = OutputPath.Split('.');
                return splitString[splitString.Length - 1].ToLower();
            }
        }

        #endregion
    }
}
