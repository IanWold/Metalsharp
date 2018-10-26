using System;

namespace Metal.Sharp
{
    /// <summary>
    /// Extensions to Metalsharp for invoking included plugins
    /// </summary>
    public static class MetalsharpExtensions
    {
        /// <summary>
        /// Invoke the Branch plugin
        /// </summary>
        /// <returns></returns>
        /// <param name="branches">The functions to handle each of the branches</param>
        public static Metalsharp Branch(this Metalsharp directory, params Action<Metalsharp>[] branches) =>
            directory.Use(new Branch(branches));

        /// <summary>
        /// Invoke the Drafts plugin
        /// </summary>
        /// <returns></returns>
        public static Metalsharp UseDrafts(this Metalsharp directory) =>
            directory.Use(new Drafts());

        /// <summary>
        /// Invoke the Layout plugin
        /// </summary>
        /// <returns></returns>
        /// <param name="filePath">The path to the layout file</param>
        public static Metalsharp UseLayout(this Metalsharp directory, string filePath) =>
            directory.Use(new Layout(filePath));

        /// <summary>
        /// Invoke the Merkdown plugin
        /// </summary>
        /// <returns></returns>
        public static Metalsharp UseMarkdown(this Metalsharp directory) =>
            directory.Use(new Markdown());
    }
}
