using System;
using System.Collections.Generic;
using System.Linq;

namespace Metalsharp;

/// <summary>
///     Collections plugin
///     
///     Groups files matching a predicate into collections in the directory metadata. Collections are stored in a `Dictionary` matching a string to another inner `Dictionary`, which itself matches a string (either "input" or "output") to an array of strings (which are the full paths of the files in the collection).
/// </summary>
/// 
/// <example>
///     Suppose I have the following files on disk:
///     
///     ```plaintext
///         ├── Index.md
///         ├── Post1.md
///         ├── Post2.md
///         └── About.md
///     ```
///     
///     And then I create a Metalsharp project, import these into the inputs, and then use the `Markdown` plugin to generate their HTML in the outputs:
///     
///     ```c#
///         var project = new MetalsharpProject("Path\\To\\My\\Files")
///         .UseMarkdown();
///     ```
///     
///     And then say that from here I want to add extra metadata to my posts, but not my `About` or `Index` files. It would be easy to be able to group those files into a collection for easy reference:
///     
///     ```c#
///         directory.UseCollections("posts", file => file.Name.ToLower().Contains("post"))
///     ```
///     
///     This will match all the files in the input and output whose names contain the word "post", and will create a collection of them in the metadata of the `MetalsharpProject`. This metadata object, named `collections` will look like the following:
///     
///     ```plaintext
///         ["posts"] =
///         {
///         ["input"] = { "Post1.md", "Post2.md" },
///         ["output"] = { "Post1.html", "Post2.html" }
///         }
///     ```
///     
///     This can be a bit confusing and messy to sort through, so there are extra extension methods supporting retrieving these collections. The following will go through each of the post html files in the output and add some custom metadata to them:
///     
///     ```c#
///         project.GetOutputFilesFromCollection("posts").ToList().ForEach(post => post.Metadata.Add("author", "Mickey Mouse"));
///     ```
/// </example>
public class Collections : IMetalsharpPlugin
{
	/// <summary>
	///     Contains the definitions of the collections.
	/// </summary>
	private readonly (string name, Predicate<MetalsharpFile> predicate)[] _definitions;

	/// <summary>
	///     Instantiate the plugin with a single collection definition.
	/// </summary>
	/// 
	/// <param name="name">
	///     The name of the collection.
	/// </param>
	/// <param name="predicate">
	///     The predicate to match files for the collection.
	/// </param>
	public Collections(string name, Predicate<MetalsharpFile> predicate) : this((name, predicate)) { }

	/// <summary>
	///     Instantiates the plugin with the definitions of the collections.
	/// </summary>
	/// <param name="definitions">
	///     The definitions of the collections, including the name of the collection and the predicate which matches its files.
	/// </param>
	public Collections(params (string name, Predicate<MetalsharpFile> predicate)[] definitions) =>
		_definitions = definitions;

	/// <summary>
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which the plugin will be invoked.
	/// </param>
	public void Execute(MetalsharpProject project)
	{
		var collections = new Dictionary<string, Dictionary<string, string[]>>();

		foreach (var (name, predicate) in _definitions)
		{
			project.LogDebug($"Calculating collection {name}:");

			var inputCollection = new List<string>();
			var outputCollection = new List<string>();

			project.LogDebug("    Input:");
			foreach (var file in project.InputFiles.Where(i => predicate(i)))
			{
				project.LogDebug($"        {file.FilePath}");
				inputCollection.Add(file.FilePath);
			}

			project.LogDebug("    Output:");
			foreach (var file in project.OutputFiles.Where(i => predicate(i)))
			{
				project.LogDebug($"        {file.FilePath}");
				outputCollection.Add(file.FilePath);
			}

			collections.Add(name, new Dictionary<string, string[]>
			{
				["input"] = inputCollection.ToArray(),
				["output"] = outputCollection.ToArray()
			});
		}

		if (project.Metadata.ContainsKey("collections") && project.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> dictionary)
		{
			foreach (var item in collections)
			{
				dictionary.Add(item.Key, item.Value);
			}
		}
		else
		{
			project.Meta("collections", collections);
		}
	}
}
