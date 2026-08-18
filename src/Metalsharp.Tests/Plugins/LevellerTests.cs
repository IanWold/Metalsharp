namespace Metalsharp.Tests;

public class LevellerTests
{
	[Fact]
	public void LevellerAddsLevelMetadata()
	{
		var file = new MetalsharpFile("", "file");
		_ = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseLeveller();

		Assert.Contains(file.Metadata, m => m.Key == "level");
	}

	[Fact]
	public void LevellerCorrectlyLevelsFiles()
	{
		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput("Scenario\\Directory2", "").UseLeveller();

		Assert.Equal(1, project.InputFiles.Single(f => f.Name == "file10").Metadata["level"]);
		Assert.Equal(2, project.InputFiles.Single(f => f.Name == "file11").Metadata["level"]);
	}

	[Fact]
	public void LevellerOverwritesExistingLevelMetadata()
	{
		var sep = Path.DirectorySeparatorChar;
		var file = new MetalsharpFile("", $"Directory1{sep}Directory2{sep}file");
		file.Metadata["level"] = 999;

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseLeveller();

		Assert.Equal(2, project.InputFiles[0].Metadata["level"]);
	}
}
