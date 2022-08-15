using System.IO;
using System.Linq;

namespace Metalsharp;

/// <summary>
///     The Markdown plugin
///     
///     Converts any markdown files in the input to HTML with [Markdig](https://github.com/lunet-io/markdig). HTML files are placed in the output.
/// </summary>
/// 
/// <example>
///     ```c#
///         new MetalsharpProject()
///         .AddInput(new MetalsharpFile("# Header 1", "file.md")
///         .UseMarkdown()
///         .Build();
///     ```
///     
///     Will output the file `file.html` to the output directory. The contents of `file.html` will be:
///     
///     ```html
///     &lt;h1&gt;Header 1&lt;/h1&gt;
///     ```
/// </example>
public class Markdown : IMetalsharpPlugin
{
	/// <summary>
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` to invoke the plugin on.
	/// </param>
	public void Execute(MetalsharpProject project)
	{
		foreach (var file in project.InputFiles.Where(f => f.Extension == ".md" || f.Extension == ".markdown"))
		{
			var fileText = Markdig.Markdown.ToHtml(file.Text);
			var filePath = Path.Combine(file.Directory, file.Name + ".html");

			project.Log.Debug($"Converting Input file {file.FilePath} to Output file {filePath}");

			project.OutputFiles.Add(new MetalsharpFile(fileText, filePath)
			{
				Metadata = file.Metadata
			});
		}
	}
}
