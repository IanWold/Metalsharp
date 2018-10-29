using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Metal.Sharp
{
    /// <summary>
    /// Represents a file
    /// </summary>
    public class MetalsharpFile : IMetalsharpFile
    {
        #region Properties

        /// <summary>
        /// The extension from the file name
        /// </summary>
        public string Extension
        {
            get
            {
                var splitString = FilePath.Split('.');
                return splitString[splitString.Length - 1].ToLower(CultureInfo.InvariantCulture);
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

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if the directory is an ancestor of the file
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        public bool IsDescendantOf(string directory) =>
            FilePath.ToLower().StartsWith(directory.ToLower());

        /// <summary>
        /// Returns true if the directory is the parent of the file
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        public bool IsChildOf(string directory) =>
            FilePath.ToLower().StartsWith(directory.ToLower())
            && ! FilePath.Substring(directory.Length).Contains("/");

        #endregion
    }
}
