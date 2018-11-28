using System.Collections.Generic;

namespace Metalsharp
{
    /// <summary>
    /// Represents the interface for a Metalsharp file
    /// </summary>
    public interface IMetalsharpFile
    {
        /// <summary>
        /// Returns true if the directory is an ancestor of the file
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        bool IsDescendantOf(string directory);

        /// <summary>
        /// Returns true if the directory is the parent of the file
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        bool IsChildOf(string directory);

        /// <summary>
        /// The directory of the file relative to the source directory
        /// </summary>
        string Directory { get; set; }

        /// <summary>
        /// The extension from the file name
        /// </summary>
        string Extension { get; set; }

        /// <summary>
        /// The path of the file
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Metadata from the file
        /// </summary>
        Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// The name of the file, without the extension
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The text of the file
        /// </summary>
        string Text { get; set; }
    }
}
