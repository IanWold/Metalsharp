using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Metalsharp
{
    /// <summary>
    /// Represents a file
    /// </summary>
    public class MetalsharpFile : IMetalsharpFile
    {
        /// <summary>
        /// Instantiate a new MetalsharpFile with no metadata
        /// </summary>
        /// <param name="text">The text of the file</param>
        /// <param name="filePath">The path of the file</param>
        public MetalsharpFile(string text, string filePath)
            : this(text, filePath, new Dictionary<string, object>())
        { }

        /// <summary>
        /// Instantiate a new MetalsharpFile with the specified metadata
        /// </summary>
        /// <param name="text">The text of the file</param>
        /// <param name="filePath">The path of the file</param>
        /// <param name="metadata">The metadata of the file</param>
        [JsonConstructor]
        public MetalsharpFile(string text, string filePath, Dictionary<string, object> metadata)
        {
            Text = text;
            Metadata = metadata;
            FilePath = filePath;
        }

        #region Properties

        /// <summary>
        /// THe directory of the file relative to the source directory
        /// </summary>
        public string Directory
        {
            get => Path.GetDirectoryName(FilePath);
            set => FilePath = Path.Combine(value, Name + Extension);
        }

        /// <summary>
        /// The extension from the file name
        /// </summary>
        public string Extension
        {
            get => Path.GetExtension(FilePath);
            set => FilePath = Path.Combine(Directory, Name + value);
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
        /// The name of the file, without the extension
        /// </summary>
        public string Name
        {
            get => Path.GetFileNameWithoutExtension(FilePath);
            set => FilePath = Path.Combine(Directory, value + Extension);
        }

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
            FilePath.Contains(directory + Path.DirectorySeparatorChar);

        /// <summary>
        /// Returns true if the directory is the parent of the file
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        public bool IsChildOf(string directory) =>
            FilePath.Contains(Path.Combine(directory, Name + Extension));

        #endregion
    }
}
