using System;
using System.Linq;
using System.Collections.Generic;

namespace Metalsharp;

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
	///     Branch the `MetalsharpProject` twice:
	/// 
	///     ```c#
	///         new MetalsharpProject()
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
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// <param name="branches">
	///     The functions to handle each of the branches.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject Branch(this MetalsharpProject project, params Action<MetalsharpProject>[] branches) =>
		project.Use(new Branch(branches));

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
	///         new MetalsharpProject()
	///         .UseCollections("myCollection", file => file.Extension == ".md");
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
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
	public static MetalsharpProject UseCollections(this MetalsharpProject project, string name, Predicate<IMetalsharpFile> predicate) =>
		project.Use(new Collections(name, predicate));

	/// <summary>
	///     Invoke the Collections plugin with several collection definitions
	/// </summary>
	/// 
	/// <example>
	///     Add `.md` files to a collection named `mdFiles` and `.html` files to a collection named `htmlFiles`:
	/// 
	///     ```c#
	///         new MetalsharpProject()
	///         .UseCollections(("mdFiles", file => file.Extension == ".md"), ("htmlFiles", file => file.Extension == ".html"));
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// <param name="definitions">
	///     The definitions of each collection.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseCollections(this MetalsharpProject project, params (string name, Predicate<IMetalsharpFile> predicate)[] definitions) =>
		project.Use(new Collections(definitions));

	/// <summary>
	///     Given the name of a collection, returns that collection from the metadata of the `MetalsharpProject`.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         Dictionary&lt;string, string[]&gt; collection = new MetalsharpProject()
	///         ... // Add Files
	///         ... // Create a collection named "myCollection"
	///         .GetCollection("myCollection");
	///         
	///         string[] collectionInputFilesArray = collection["input"];
	///         string[] collectionOutputFilesArray = collection["output"];
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The directory holding the collection.
	/// </param>
	/// <param name="name">
	///     The name of the collection.
	/// </param>
	/// 
	/// <returns>
	///     A `Dictionary` containing the input and output lists of file paths in the collection.
	/// </returns>
	public static Dictionary<string, string[]> GetCollection(this MetalsharpProject project, string name) =>
		new()
		{
			["input"] = project.GetInputCollection(name),
			["output"] = project.GetOutputCollection(name)
		};

	/// <summary>
	///     Given the name of a collection, returns the input *and* output files in that collection from the metadata of the `MetalsharpProject`.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         IMetalsharpFile[] collectionFiles = new MetalsharpProject()
	///         ... // Add files
	///         ... // Create a collection named "myCollection"
	///         .GetFilesFromCollection("myCollection").ToArray();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The directory holding the collection.
	/// </param>
	/// <param name="name">
	///     The name of the collection.
	/// </param>
	/// 
	/// <returns>
	///     An enumerable of `IMetalsharpFile`s from the input and output lists of the collection.
	/// </returns>
	public static IEnumerable<IMetalsharpFile> GetFilesFromCollection(this MetalsharpProject project, string name) =>
		project.GetInputFilesFromCollection(name).Concat(project.GetOutputFilesFromCollection(name));

	/// <summary>
	///     Given the name of a collection, returns the input file paths in that collection from the metadata of the `MetalsharpProject`.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         string[] collectionInputFilePaths = new MetalsharpProject()
	///         ... // Add files
	///         ... // Create a collection named "myCollection"
	///         .GetInputCollection("myCollection");
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The directory holding the collection.
	/// </param>
	/// <param name="name">
	///     The name of the collection.
	/// </param>
	///     
	/// <returns>
	///     An array containing the list of input file paths in the collection.
	/// </returns>
	public static string[] GetInputCollection(this MetalsharpProject project, string name) =>
		project.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> collectionsDictionary
		&& collectionsDictionary[name] is Dictionary<string, string[]> collection
		&& collection["input"] is string[] inputsCollection
			? inputsCollection
			: Array.Empty<string>();

	/// <summary>
	///     Given the name of a collection, returns the input files in that collection from the metadata of the `MetalsharpProject`.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         IMetalsharpFile[] collectionInputFiles = new MetalsharpProject()
	///         ... // Add files
	///         ... // Create a collection named "myCollection"
	///         .GetInputFilesFromCollection("myCollection").ToArray();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The directory holding the collection.
	/// </param>
	/// <param name="name">
	///     The name of the collection to return the input files from.
	/// </param>
	/// 
	/// <returns>
	///     An enumerable containing the files from the input list in the collection.
	/// </returns>
	public static IEnumerable<IMetalsharpFile> GetInputFilesFromCollection(this MetalsharpProject project, string name) =>
		project.GetInputCollection(name) is string[] files && files.Length > 0
			? project.InputFiles.Where(file => files.Contains(file.FilePath))
			: Enumerable.Empty<IMetalsharpFile>();

	/// <summary>
	///     Given the name of a collection, returns the output file paths in that collection from the metadata of the `MetalsharpProject`.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         string[] collectionoutputFilePaths = new MetalsharpProject()
	///         ... // Add files
	///         ... // Create a collection named "myCollection"
	///         .GetOutputCollection("myCollection");
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The directory holding the collection.
	/// </param>
	/// <param name="name">
	///     The name of the collection.
	/// </param>
	///     
	/// <returns>
	///     An array containing the list of output file paths in the collection.
	/// </returns>
	public static string[] GetOutputCollection(this MetalsharpProject project, string name) =>
		project.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> collectionsDictionary
		&& collectionsDictionary[name] is Dictionary<string, string[]> collection
		&& collection["output"] is string[] outputsCollection
			? outputsCollection
			: Array.Empty<string>();

	/// <summary>
	///     Given the name of a collection, returns the output files in that collection from the metadata of the `MetalsharpProject`.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         IMetalsharpFile[] collectionoutputFiles = new MetalsharpProject()
	///         ... // Add files
	///         ... // Create a collection named "myCollection"
	///         .GetOutputFilesFromCollection("myCollection").ToArray();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The directory holding the collection.
	/// </param>
	/// <param name="name">
	///     The name of the collection to return the output files from.
	/// </param>
	/// 
	/// <returns>
	///     An enumerable containing the files from the output list in the collection.
	/// </returns>
	public static IEnumerable<IMetalsharpFile> GetOutputFilesFromCollection(this MetalsharpProject project, string name) =>
		project.GetOutputCollection(name) is string[] files && files.Length > 0
			? project.OutputFiles.Where(file => files.Contains(file.FilePath))
			: Enumerable.Empty<IMetalsharpFile>();

	#endregion

	#region Debug Plugin

	/// <summary>
	/// Invoke the default Debug plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .UseDebug();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseDebug(this MetalsharpProject project) =>
		project.Use(new Debug());

	/// <summary>
	///     Invoke the Debug plugin with a log file to capture the debug logs.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .UseDebug("debug.log");
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// <param name="logPath">
	///     The path to the log file.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseDebug(this MetalsharpProject project, string logPath) =>
		project.Use(new Debug(logPath));

	/// <summary>
	///     Invoke the Debug plugin with custom log behavior.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .UseDebug(log => Console.WriteLine(log));
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// <param name="onLog">
	///     The action to execute to log a debug line.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseDebug(this MetalsharpProject project, Action<string> onLog) =>
		project.Use(new Debug(onLog));

	#endregion

	#region Frontmatter Plugin

	/// <summary>
	///     Invoke the `Frontmatter` plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseFrontmatter();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseFrontmatter(this MetalsharpProject project) =>
		project.Use(new Frontmatter());

	#endregion

	#region Leveller Plugin

	/// <summary>
	///     Invoke the `Leveller` plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseLeveller();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseLeveller(this MetalsharpProject project) =>
		project.Use(new Leveller());

	#endregion

	#region Markdown Plugin

	/// <summary>
	///     Invoke the `Markdown` plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseMarkdown();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseMarkdown(this MetalsharpProject project) =>
		project.Use(new Markdown());

	#endregion
}
