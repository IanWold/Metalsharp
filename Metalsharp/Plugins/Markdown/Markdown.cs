namespace Metalsharp;

/// <summary>
///     The Markdown plugin
///     
///     Converts any markdown files in the input to HTML with <see href="https://github.com/lunet-io/markdig">Markdig</see>. HTML files are placed in the output.
/// </summary>
/// 
/// <example>
///     <code>
///         new MetalsharpProject()
///         .AddInput(new MetalsharpFile("# Header 1", "file.md")
///         .UseMarkdown()
///         .Build();
///     </code>
///     
///     Will output the file <c>file.html</c> to the output directory. The contents of <c>file.html</c> will be:
///     
///     <code>
///     &lt;h1&gt;Header 1&lt;/h1&gt;
///     </code>
/// </example>
public class Markdown : IMetalsharpPlugin
{
	/// <summary>
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The <c>MetalsharpProject</c> to invoke the plugin on.
	/// </param>
	public void Execute(MetalsharpProject project)
	{
		foreach (var file in project.InputFiles.Where(f => f.Extension is ".md" or ".markdown"))
		{
			var fileText = Markdig.Markdown.ToHtml(file.Text);
			var filePath = Path.Combine(file.Directory, file.Name + ".html");

			project.LogDebug($"Converting Input file {file.FilePath} to Output file {filePath}");

			project.OutputFiles.Add(new MetalsharpFile(fileText, filePath)
			{
				Metadata = new Dictionary<string, object>(file.Metadata)
			});
		}
	}
}
