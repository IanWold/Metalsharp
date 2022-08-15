using Xunit;
using static System.Convert;

namespace Metalsharp.Tests;

public class FrontmatterTests
{
	[Fact]
	public void FrontmatterSucceedsWithoutFrontmatter()
	{
		var project = new MetalsharpProject(Logging.LogLevel.None).AddInput("Scenario\\Plugins\\FileMarkdown.md").UseFrontmatter();

		Assert.True(project.InputFiles[0].Metadata.Count == 0);
	}

	[Fact]
	public void FrontmatterParsesJsonFrontmatter()
	{
		var project = new MetalsharpProject(Logging.LogLevel.None).AddInput("Scenario\\Plugins\\FileJsonFrontmatter.md").UseFrontmatter();

		Assert.True((bool)project.InputFiles[0].Metadata["test"]);
	}

	[Fact]
	public void FrontmatterParsesYamlFrontmatter()
	{
		var project = new MetalsharpProject(Logging.LogLevel.None).AddInput("Scenario\\Plugins\\FileYamlFrontmatter.md").UseFrontmatter();

		Assert.True(ToBoolean(project.InputFiles[0].Metadata["test"].ToString()));
	}
}
