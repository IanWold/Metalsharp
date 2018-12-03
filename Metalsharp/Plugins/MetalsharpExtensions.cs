using System;
using System.Linq;
using System.Collections.Generic;

namespace Metalsharp
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
        /// <param name="directory"></param>
        /// <param name="branches">The functions to handle each of the branches</param>
        public static MetalsharpDirectory Branch(this MetalsharpDirectory directory, params Action<MetalsharpDirectory>[] branches) =>
            directory.Use(new Branch(branches));

        /// <summary>
        /// Invoke the Collections plugin with a single collection definition
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="name">The name of the collection to define</param>
        /// <param name="predicate">The predicate to match the files for the collection</param>
        /// <returns></returns>
        public static MetalsharpDirectory UseCollections(this MetalsharpDirectory directory, string name, Predicate<IMetalsharpFile> predicate) =>
            directory.Use(new Collections(name, predicate));

        /// <summary>
        /// Invoke the Collections plugin with several collection definitions
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="definitions">The definitions of each collection</param>
        /// <returns></returns>
        public static MetalsharpDirectory UseCollections(this MetalsharpDirectory directory, params (string name, Predicate<IMetalsharpFile> predicate)[] definitions) =>
            directory.Use(new Collections(definitions));

        /// <summary>
        /// Get a collection from MetalsharpDirectory Metadata by name
        /// </summary>
        /// <param name="directory">The directory to return the collection from</param>
        /// <param name="name">The name of the collection</param>
        /// <returns></returns>
        public static Dictionary<string, string[]> GetCollection(this MetalsharpDirectory directory, string name) =>
            new Dictionary<string, string[]>
            {
                ["input"] = directory.GetInputCollection(name),
                ["output"] = directory.GetOutputCollection(name)
            };

        /// <summary>
        /// Get input and output files from a collection by name
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="name">The name of the collection to return the files from</param>
        /// <returns></returns>
        public static IEnumerable<IMetalsharpFile> GetFilesFromCollection(this MetalsharpDirectory directory, string name) =>
            directory.GetInputFilesFromCollection(name).Concat(directory.GetOutputFilesFromCollection(name));

        /// <summary>
        /// Get the input files from a collection from MetalsharpDirectory Metadata by name
        /// </summary>
        /// <param name="directory">The directory to return the input files of the collection from</param>
        /// <param name="name">The name of the collection</param>
        /// <returns></returns>
        public static string[] GetInputCollection(this MetalsharpDirectory directory, string name) =>
            directory.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> collectionsDictionary
            && collectionsDictionary[name] is Dictionary<string, string[]> collection
            && collection["input"] is string[] inputsCollection
                ? inputsCollection
                : new string[0];

        /// <summary>
        /// Get the input files from a collection by name
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="name">The name of the collection to return the input files from</param>
        /// <returns></returns>
        public static IEnumerable<IMetalsharpFile> GetInputFilesFromCollection(this MetalsharpDirectory directory, string name) =>
            directory.GetInputCollection(name) is string[] files && files.Count() > 0
                ? directory.InputFiles.Where(file => files.Contains(file.FilePath))
                : Enumerable.Empty<IMetalsharpFile>();

        /// <summary>
        /// Get the output files from a collection from MetalsharpDirectory Metadata by name
        /// </summary>
        /// <param name="directory">The directory to return the output files of the collection from</param>
        /// <param name="name">The name of the collection</param>
        /// <returns></returns>
        public static string[] GetOutputCollection(this MetalsharpDirectory directory, string name) =>
            directory.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> collectionsDictionary
            && collectionsDictionary[name] is Dictionary<string, string[]> collection
            && collection["output"] is string[] outputsCollection
                ? outputsCollection
                : new string[0];

        /// <summary>
        /// Get the output files from a collection by name
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="name">The name of the collection to return the input files from</param>
        /// <returns></returns>
        public static IEnumerable<IMetalsharpFile> GetOutputFilesFromCollection(this MetalsharpDirectory directory, string name) =>
            directory.GetOutputCollection(name) is string[] files && files.Count() > 0
                ? directory.OutputFiles.Where(file => files.Contains(file.FilePath))
                : Enumerable.Empty<IMetalsharpFile>();

        /// <summary>
        /// Invoke the default Debug plugin
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static MetalsharpDirectory UseDebug(this MetalsharpDirectory directory) =>
            directory.Use(new Debug());

        /// <summary>
        /// Invoke the Debug plugin with a log file to capture the debug logs
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="logPath">The path to the log file</param>
        /// <returns></returns>
        public static MetalsharpDirectory UseDebug(this MetalsharpDirectory directory, string logPath) =>
            directory.Use(new Debug(logPath));

        /// <summary>
        /// Invoke the Debug plugin with custom log behavior
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="onLog">The action to execute to log a debug line</param>
        /// <returns></returns>
        public static MetalsharpDirectory UseDebug(this MetalsharpDirectory directory, Action<string> onLog) =>
            directory.Use(new Debug(onLog));

        /// <summary>
        /// Invoke the frontmatter plugin
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static MetalsharpDirectory UseFrontmatter(this MetalsharpDirectory directory) =>
            directory.Use(new Frontmatter());

        /// <summary>
        /// Invoke the Merkdown plugin
        /// </summary>
        /// <returns></returns>
        public static MetalsharpDirectory UseMarkdown(this MetalsharpDirectory directory) =>
            directory.Use(new Markdown());
    }
}
