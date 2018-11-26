using System;
using System.Collections.Generic;

namespace Metalsharp
{
    /// <summary>
    /// Represents the interface for a collection of Metalsharp files
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMetalsharpFileCollection<T> : IList<T> where T : IMetalsharpFile
    {
        /// <summary>
        /// Get the descendant files of a directory
        /// </summary>
        /// <param name="directory">The ancestor directory</param>
        /// <returns></returns>
        IMetalsharpFileCollection<T> DescendantsOf(string directory);

        /// <summary>
        /// Get the children files of a directory
        /// </summary>
        /// <param name="directory">The parent directory</param>
        /// <returns></returns>
        IMetalsharpFileCollection<T> ChildrenOf(string directory);

        /// <summary>
        /// Returns true if one of the files in the collection descends from the directory
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        bool ContainsDirectory(string directory);

        /// <summary>
        /// Alias List.RemoveAll
        /// </summary>
        int RemoveAll(Predicate<T> match);
    }
}
