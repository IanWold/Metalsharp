using System;
using System.IO;

namespace Metal.Sharp
{
    /// <summary>
    /// Represents a file to be output by Metalsharp
    /// </summary>
    public class OutputFile : MetalsharpFile
    {
        /// <summary>
        /// Create a new OutputFile
        /// </summary>
        /// <param name="path">The path to whcih the file will be output</param>
        /// <param name="text">The text of the file</param>
        public OutputFile(string path, string text)
        {
            FilePath = path;
            Text = text;
        }

        /// <summary>
        /// Create a new OutputFile from an existing file
        /// </summary>
        /// <param name="filePath">The path to the file to read</param>
        /// <returns></returns>
        public static OutputFile FromExisting(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.GetAttributes(filePath) != FileAttributes.Directory
                    ? new OutputFile(Path.GetFileName(filePath), File.ReadAllText(filePath))
                    : throw new ArgumentException("File " + filePath + " is a directory.");
            }
            else
            {
                throw new ArgumentException("File " + filePath + " does not exist.");
            }
        }
    }
}
