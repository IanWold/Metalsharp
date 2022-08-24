using System.Linq;
using Xunit;

namespace Metalsharp.Tests;

public class LevellerTests
{
	[Fact]
	public void LevellerAddsLevelMetadata()
	{
		var file = new MetalsharpFile("", "file");
		_ = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = LogLevel.None }).AddInput(file).UseLeveller();

		Assert.Contains(file.Metadata, m => m.Key == "level");
	}

	[Fact]
	public void LevellerCorrectlyLevelsFiles()
	{
		var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = LogLevel.None }).AddInput("Scenario\\Directory2", "").UseLeveller();

		Assert.Equal(1, project.InputFiles.Single(f => f.Name == "file10").Metadata["level"]);
		Assert.Equal(2, project.InputFiles.Single(f => f.Name == "file11").Metadata["level"]);
	}
}
