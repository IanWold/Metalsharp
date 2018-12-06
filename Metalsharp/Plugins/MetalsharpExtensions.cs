using System;
using System.Linq;
using System.Collections.Generic;

namespace Metalsharp
{
    /// <summary>
    ///     Extensions to Metalsharp for invoking included plugins.
    /// </summary>
    public static class MetalsharpExtensions
    {
        #region Branch Plugin

        /// <summary>
        ///     Invoke the Branch plugin.
        /// </summary>
        /// 
        /// <example>
        ///     Branch the `MetalsharpDirectory` twice:
        /// 
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         // Add files
        ///         .Branch(
        ///         dir => {
        ///         // Do something with branch 1
        ///         },
        ///         dir => {
        ///         // Do something with branch 2
        ///         }
        ///         );
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// <param name="branches">
        ///     The functions to handle each of the branches.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory Branch(this MetalsharpDirectory directory, params Action<MetalsharpDirectory>[] branches) =>
            directory.Use(new Branch(branches));

        #endregion

        #region Collections Plugin

        /// <summary>
        ///     Invoke the Collections plugin with a single collection definition.
        /// </summary>
        /// 
        /// <example>
        ///     Only add `.md` files to a collection named `myCollection`:
        /// 
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         .UseCollections("myCollection", file => file.Extension == ".md");
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// <param name="name">
        ///     The name of the collection to define.
        /// </param>
        /// <param name="predicate">
        ///     The predicate to match the files for the collection.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory UseCollections(this MetalsharpDirectory directory, string name, Predicate<IMetalsharpFile> predicate) =>
            directory.Use(new Collections(name, predicate));

        /// <summary>
        ///     Invoke the Collections plugin with several collection definitions
        /// </summary>
        /// 
        /// <example>
        ///     Add `.md` files to a collection named `mdFiles` and `.html` files to a collection named `htmlFiles`:
        /// 
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         .UseCollections(("mdFiles", file => file.Extension == ".md"), ("htmlFiles", file => file.Extension == ".html"));
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// <param name="definitions">
        ///     The definitions of each collection.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory UseCollections(this MetalsharpDirectory directory, params (string name, Predicate<IMetalsharpFile> predicate)[] definitions) =>
            directory.Use(new Collections(definitions));

        /// <summary>
        ///     Given the name of a collection, returns that collection from the metadata of the `MetalsharpDirectory`.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         Dictionary&lt;string, string[]&gt; collection = new MetalsharpDirectory()
        ///         ... // Add Files
        ///         ... // Create a collection named "myCollection"
        ///         .GetCollection("myCollection");
        ///         
        ///         string[] collectionInputFilesArray = collection["input"];
        ///         string[] collectionOutputFilesArray = collection["output"];
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The directory holding the collection.
        /// </param>
        /// <param name="name">
        ///     The name of the collection.
        /// </param>
        /// 
        /// <returns>
        ///     A `Dictionary` containing the input and output lists of file paths in the collection.
        /// </returns>
        public static Dictionary<string, string[]> GetCollection(this MetalsharpDirectory directory, string name) =>
            new Dictionary<string, string[]>
            {
                ["input"] = directory.GetInputCollection(name),
                ["output"] = directory.GetOutputCollection(name)
            };

        /// <summary>
        ///     Given the name of a collection, returns the input *and* output files in that collection from the metadata of the `MetalsharpDirectory`.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         IMetalsharpFile[] collectionFiles = new MetalsharpDirectory()
        ///         ... // Add files
        ///         ... // Create a collection named "myCollection"
        ///         .GetFilesFromCollection("myCollection").ToArray();
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The directory holding the collection.
        /// </param>
        /// <param name="name">
        ///     The name of the collection.
        /// </param>
        /// 
        /// <returns>
        ///     An enumerable of `IMetalsharpFile`s from the input and output lists of the collection.
        /// </returns>
        public static IEnumerable<IMetalsharpFile> GetFilesFromCollection(this MetalsharpDirectory directory, string name) =>
            directory.GetInputFilesFromCollection(name).Concat(directory.GetOutputFilesFromCollection(name));

        /// <summary>
        ///     Given the name of a collection, returns the input file paths in that collection from the metadata of the `MetalsharpDirectory`.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         string[] collectionInputFilePaths = new MetalsharpDirectory()
        ///         ... // Add files
        ///         ... // Create a collection named "myCollection"
        ///         .GetInputCollection("myCollection");
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The directory holding the collection.
        /// </param>
        /// <param name="name">
        ///     The name of the collection.
        /// </param>
        ///     
        /// <returns>
        ///     An array containing the list of input file paths in the collection.
        /// </returns>
        public static string[] GetInputCollection(this MetalsharpDirectory directory, string name) =>
            directory.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> collectionsDictionary
            && collectionsDictionary[name] is Dictionary<string, string[]> collection
            && collection["input"] is string[] inputsCollection
                ? inputsCollection
                : new string[0];

        /// <summary>
        ///     Given the name of a collection, returns the input files in that collection from the metadata of the `MetalsharpDirectory`.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         IMetalsharpFile[] collectionInputFiles = new MetalsharpDirectory()
        ///         ... // Add files
        ///         ... // Create a collection named "myCollection"
        ///         .GetInputFilesFromCollection("myCollection").ToArray();
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The directory holding the collection.
        /// </param>
        /// <param name="name">
        ///     The name of the collection to return the input files from.
        /// </param>
        /// 
        /// <returns>
        ///     An enumerable containing the files from the input list in the collection.
        /// </returns>
        public static IEnumerable<IMetalsharpFile> GetInputFilesFromCollection(this MetalsharpDirectory directory, string name) =>
            directory.GetInputCollection(name) is string[] files && files.Count() > 0
                ? directory.InputFiles.Where(file => files.Contains(file.FilePath))
                : Enumerable.Empty<IMetalsharpFile>();

        /// <summary>
        ///     Given the name of a collection, returns the output file paths in that collection from the metadata of the `MetalsharpDirectory`.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         string[] collectionoutputFilePaths = new MetalsharpDirectory()
        ///         ... // Add files
        ///         ... // Create a collection named "myCollection"
        ///         .GetOutputCollection("myCollection");
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The directory holding the collection.
        /// </param>
        /// <param name="name">
        ///     The name of the collection.
        /// </param>
        ///     
        /// <returns>
        ///     An array containing the list of output file paths in the collection.
        /// </returns>
        public static string[] GetOutputCollection(this MetalsharpDirectory directory, string name) =>
            directory.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> collectionsDictionary
            && collectionsDictionary[name] is Dictionary<string, string[]> collection
            && collection["output"] is string[] outputsCollection
                ? outputsCollection
                : new string[0];

        /// <summary>
        ///     Given the name of a collection, returns the output files in that collection from the metadata of the `MetalsharpDirectory`.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         IMetalsharpFile[] collectionoutputFiles = new MetalsharpDirectory()
        ///         ... // Add files
        ///         ... // Create a collection named "myCollection"
        ///         .GetOutputFilesFromCollection("myCollection").ToArray();
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The directory holding the collection.
        /// </param>
        /// <param name="name">
        ///     The name of the collection to return the output files from.
        /// </param>
        /// 
        /// <returns>
        ///     An enumerable containing the files from the output list in the collection.
        /// </returns>
        public static IEnumerable<IMetalsharpFile> GetOutputFilesFromCollection(this MetalsharpDirectory directory, string name) =>
            directory.GetOutputCollection(name) is string[] files && files.Count() > 0
                ? directory.OutputFiles.Where(file => files.Contains(file.FilePath))
                : Enumerable.Empty<IMetalsharpFile>();

        #endregion

        #region Debug Plugin

        /// <summary>
        /// Invoke the default Debug plugin.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         .UseDebug();
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory UseDebug(this MetalsharpDirectory directory) =>
            directory.Use(new Debug());

        /// <summary>
        ///     Invoke the Debug plugin with a log file to capture the debug logs.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         .UseDebug("debug.log");
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// <param name="logPath">
        ///     The path to the log file.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory UseDebug(this MetalsharpDirectory directory, string logPath) =>
            directory.Use(new Debug(logPath));

        /// <summary>
        ///     Invoke the Debug plugin with custom log behavior.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         .UseDebug(log => Console.WriteLine(log));
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// <param name="onLog">
        ///     The action to execute to log a debug line.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory UseDebug(this MetalsharpDirectory directory, Action<string> onLog) =>
            directory.Use(new Debug(onLog));

        #endregion

        #region Frontmatter Plugin

        /// <summary>
        ///     Invoke the `Frontmatter` plugin.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         ... // Add files
        ///         .UseFrontmatter();
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory UseFrontmatter(this MetalsharpDirectory directory) =>
            directory.Use(new Frontmatter());

        #endregion

        #region Markdown Plugin

        /// <summary>
        ///     Invoke the `Markdown` plugin.
        /// </summary>
        /// 
        /// <example>
        ///     ```c#
        ///         new MetalsharpDirectory()
        ///         ... // Add files
        ///         .UseMarkdown();
        ///     ```
        /// </example>
        /// 
        /// <param name="directory">
        ///     The `MetalsharpDirectory` on which this method will be called.
        /// </param>
        /// 
        /// <returns>
        ///     Combinator; returns `this` input.
        /// </returns>
        public static MetalsharpDirectory UseMarkdown(this MetalsharpDirectory directory) =>
            directory.Use(new Markdown());

        #endregion
    }
}
