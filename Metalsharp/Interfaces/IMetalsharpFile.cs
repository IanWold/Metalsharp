﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Metal.Sharp
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
        /// The path of the file
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        /// Metadata from the file
        /// </summary>
        Dictionary<string, object> Metadata { get; set; }

        /// <summary>
        /// The text of the file
        /// </summary>
        string Text { get; set; }
    }
}