using System;
using System.Collections.Generic;

namespace Metalsharp
{
    /// <summary>
    ///     Represents the interface for a collection of Metalsharp files.
    /// </summary>
    /// 
    /// <typeparam name="T">
    ///     The type of file. This type must inherit from `IMetalsharpFile`.
    /// </typeparam>
    public interface IMetalsharpFileCollection<T> : IList<T> where T : IMetalsharpFile
    {
        /// <summary>
        ///     Gets the files in the collection which descend from the given virtual directory.
        /// </summary>
        /// 
        /// <param name="directory">
        ///     The ancestor directory.
        /// </param>
        /// 
        /// <returns>
        ///     All of the files which descend from the given directory.
        /// </returns>
        IMetalsharpFileCollection<T> DescendantsOf(string directory);

        /// <summary>
        ///     Gets the files in the collection which are children to the given virtual directory.
        /// </summary>
        /// 
        /// <param name="directory">
        ///     The parent directory.
        /// </param>
        /// 
        /// <returns>
        ///     All of the files which are children of the given directory.
        /// </returns>
        IMetalsharpFileCollection<T> ChildrenOf(string directory);

        /// <summary>
        ///     Checks whether one of the files in the collection descends from the directory.
        /// </summary>
        /// 
        /// <param name="directory">
        ///     The directory in question.
        /// </param>
        /// 
        /// <returns>
        ///     `true` if the collection contains a file descending from the given directory, `false` otherwise.
        /// </returns>
        bool ContainsDirectory(string directory);

        /// <summary>
        ///     Alias `List.RemoveAll`.
        /// </summary>
        int RemoveAll(Predicate<T> match);
    }
}
