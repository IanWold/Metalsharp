using System.Collections.Generic;

namespace Metalsharp
{
    /// <summary>
    ///     This is the interface for a Metalsharp file.
    /// </summary>
    public interface IMetalsharpFile
    {
        /// <summary>
        ///     Checks whether a directory is an ancestor of the file.
        /// </summary>
        /// 
        /// <param name="directory">
        ///     The directory in question.
        /// </param>
        /// 
        /// <returns>
        ///     `true` if the file is a descendant of the directory, `false` otherwise.
        /// </returns>
        bool IsDescendantOf(string directory);

        /// <summary>
        ///     Checks whether a directory is the parent of the file.
        /// </summary>
        /// 
        /// <param name="directory">
        ///     The directory in question.
        /// </param>
        /// 
        /// <returns>
        ///     `true` if the file is a child of the directory, `false` otherwise.
        /// </returns>
        bool IsChildOf(string directory);

        /// <summary>
        ///     The directory of in which the file is located. `Directory` will always be equivalent to `Path.GetDirectoryName(this.FilePath)`.
        /// </summary>
        /// 
        /// <example>
        ///     Given a file with path `Path\To\File.md`, `Directory` returns the equivalent of `Path\To\`.
        /// </example>
        string Directory { get; set; }

        /// <summary>
        ///     The extension of the file. `Extension` will always be equal to `Path.GetExtension(this.FilePath)`.
        /// </summary>
        /// 
        /// <example>
        ///     Given a file with path `Path\To\File.md`, `Extension` returns `.md`.
        /// </example>
        string Extension { get; set; }

        /// <summary>
        /// The full path of the file. `FilePath` will always be equivalent to `Path.Combine(this.Directory, this.Name + this.Extension)`.
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        ///     The metadata of the file.
        /// </summary>
        Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        ///     The name of the file, without the extension. `Name` will always be equal to `Path.GetFileNameWithoutExtension(this.FilePath)`.
        /// </summary>
        /// 
        /// <example>
        ///     Given a file with path `Path\To\File.md`, `Name` returns `File`.
        /// </example>
        string Name { get; set; }

        /// <summary>
        ///     The text of the file.
        /// </summary>
        string Text { get; set; }
    }
}
