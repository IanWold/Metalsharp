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
        /// Invoke the default Debug plugin
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static Metalsharp UseDebug(this Metalsharp directory) =>
            directory.Use(new Debug());

        /// <summary>
        /// Invoke the Debug plugin with a log file to capture the debug logs
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="logPath">The path to the log file</param>
        /// <returns></returns>
        public static Metalsharp UseDebug(this Metalsharp directory, string logPath) =>
            directory.Use(new Debug(logPath));

        /// <summary>
        /// Invoke the Debug plugin with custom log behavior
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="onLog">The action to execute to log a debug line</param>
        /// <returns></returns>
        public static Metalsharp UseDebug(this Metalsharp directory, Action<string> onLog) =>
            directory.Use(new Debug(onLog));

        /// <summary>
        /// Invoke the Drafts plugin
        /// </summary>
        /// <returns></returns>
        public static Metalsharp UseDrafts(this Metalsharp directory) =>
            directory.Use(new Drafts());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static Metalsharp UseFrontmatter(this Metalsharp directory) =>
            directory.Use(new Frontmatter());

        /// <summary>
        /// Invoke the Layout plugin
        /// </summary>
        /// <returns></returns>
        /// <param name="filePath">The path to the layout file</param>
        public static Metalsharp UseLayout(this Metalsharp directory, string filePath) =>
            directory.Use(new Layout());

        /// <summary>
        /// Invoke the Merkdown plugin
        /// </summary>
        /// <returns></returns>
        public static Metalsharp UseMarkdown(this Metalsharp directory) =>
            directory.Use(new Markdown());
    }
}
