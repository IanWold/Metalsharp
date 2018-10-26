using System.Collections.Generic;

namespace Metal.Sharp
{
    /// <summary>
    /// Represents a file
    /// </summary>
    public class MetalsharpFile
    {
        /// <summary>
        /// The extension from the file name
        /// </summary>
        public string Extension
        {
            get
            {
                var splitString = FilePath.Split('.');
                return splitString[splitString.Length - 1].ToLower();
            }
        }

        /// <summary>
        /// The path of the file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Metadata from the file
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// The text of the file
        /// </summary>
        public string Text { get; set; }
    }
}
