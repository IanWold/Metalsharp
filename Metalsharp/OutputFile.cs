using System;
using System.IO;

namespace Metal.Sharp
{
    public class OutputFile : MetalsharpFile
    {
        public OutputFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (File.GetAttributes(filePath) != FileAttributes.Directory)
                {
                    Text = File.ReadAllText(filePath);
                    FilePath = Path.GetFileName(filePath);
                }
                else throw new ArgumentException("File " + filePath + " is a directory.");
            }
            else throw new ArgumentException("File " + filePath + " does not exist.");
        }

        public OutputFile(string path, string text)
        {
            FilePath = path;
            Text = text;
        }

        #region Properties

        public string Extension
        {
            get
            {
                var splitString = FilePath.Split('.');
                return splitString[splitString.Length - 1].ToLower();
            }
        }

        #endregion
    }
}
