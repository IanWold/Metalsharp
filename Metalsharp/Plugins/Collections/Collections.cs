namespace Metalsharp;

/// <summary>
///     Collections plugin
///     
///     Groups files matching a predicate into collections in the directory metadata. Collections are stored in a <c>Dictionary</c> matching a string to another inner <c>Dictionary</c>, which itself matches a string (either "input" or "output") to an array of strings (which are the full paths of the files in the collection).
/// </summary>
/// 
/// <example>
///     Suppose I have the following files on disk:
///     
///     <code>
///         ├── Index.md
///         ├── Post1.md
///         ├── Post2.md
///         └── About.md
///     </code>
///     
///     And then I create a Metalsharp project, import these into the inputs, and then use the <c>Markdown</c> plugin to generate their HTML in the outputs:
///     
///     <code>
///         var project = new MetalsharpProject()
///             .AddInput("Path\\To\\My\\Files")
///             .UseMarkdown();
///     </code>
///
///     And then say that from here I want to add extra metadata to my posts, but not my <c>About</c> or <c>Index</c> files. It would be easy to be able to group those files into a collection for easy reference:
///
///     <code>
///         project.UseCollections("posts", file => file.Name.ToLower().Contains("post"))
///     </code>
///     
///     This will match all the files in the input and output whose names contain the word "post", and will create a collection of them in the metadata of the <c>MetalsharpProject</c>. This metadata object, named <c>collections</c> will look like the following:
///     
///     <code>
///         ["posts"] =
///         {
///         ["input"] = { "Post1.md", "Post2.md" },
///         ["output"] = { "Post1.html", "Post2.html" }
///         }
///     </code>
///     
///     This can be a bit confusing and messy to sort through, so there are extra extension methods supporting retrieving these collections. The following will go through each of the post html files in the output and add some custom metadata to them:
///     
///     <code>
///         project.GetOutputFilesFromCollection("posts").ToList().ForEach(post => post.Metadata["author"] = "Mickey Mouse");
///     </code>
/// </example>
/// <param name="definitions">
///     The definitions of the collections, including the name of the collection and the predicate which matches its files.
/// </param>
public class Collections(params (string name, Predicate<MetalsharpFile> predicate)[] definitions) : IMetalsharpPlugin
{
	/// <summary>
	///     Contains the definitions of the collections.
	/// </summary>
	private readonly (string name, Predicate<MetalsharpFile> predicate)[] _definitions = definitions;

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
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The <c>MetalsharpProject</c> on which the plugin will be invoked.
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
				["input"] = [.. inputCollection],
				["output"] = [.. outputCollection]
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
