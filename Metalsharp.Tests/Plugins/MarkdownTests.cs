using System.Linq;
using Xunit;

namespace Metalsharp.Tests;

public class MarkdownTests
{
	[Fact]
	public void MarkdownGeneratesHtmlFile()
	{
		var project = new MetalsharpProject("Scenario\\Plugins\\FileMarkdown.md").UseMarkdown();

		Assert.Contains(project.OutputFiles, i => i.Extension == ".html");
	}

	[Fact]
	public void MarkdownCopiesMetadata()
	{
		var project = new MetalsharpProject("Scenario\\Plugins\\FileMarkdown.md")
			.Use(proj => proj.InputFiles.ToList().ForEach(i => i.Metadata.Add("test", true)))
			.Use<Markdown>();

		Assert.True((bool)project.OutputFiles[0].Metadata["test"]);
	}
}

