using System.IO;
using Xunit;

namespace Metalsharp.Tests;

public class CollectionsTests
{
	[Fact]
	public void CollectionsCorrectlyGroupsFiles()
	{
		var project = new MetalsharpProject()
			.AddInput("Scenario\\Directory1")
			.AddInput("Scenario\\Plugins")
			.AddOutput("Scenario\\Directory2\\SubDirectory1")
			.AddOutput("Scenario\\Plugins")
			.UseCollections("test", file => file.Name.Contains("file"));

		Assert.Contains(project.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "file1");
		Assert.DoesNotContain(project.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "file11");
		Assert.DoesNotContain(project.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");

		Assert.Contains(project.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "file11");
		Assert.DoesNotContain(project.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "file1");
		Assert.DoesNotContain(project.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
	}

	[Fact]
	public void GetFilesFromCollectionReturnsCorrectFiles()
	{
		var project = new MetalsharpProject()
			.AddInput("Scenario\\Directory1")
			.AddInput("Scenario\\Plugins")
			.AddOutput("Scenario\\Directory2\\SubDirectory1")
			.AddOutput("Scenario\\Plugins")
			.UseCollections("test", file => file.Name.Contains("file"));

		Assert.Contains(project.GetFilesFromCollection("test"), i => i.Name == "file1");
		Assert.Contains(project.GetFilesFromCollection("test"), i => i.Name == "file11");
		Assert.DoesNotContain(project.GetFilesFromCollection("test"), i => i.Name == "FileMarkdown");
	}

	[Fact]
	public void CollectionsCorrectlyGroupsInputFiles()
	{
		var project = new MetalsharpProject()
			.AddInput("Scenario\\Directory1")
			.AddInput("Scenario\\Plugins")
			.UseCollections("test", file => file.Name.Contains("file"));

		Assert.Contains(project.GetInputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "file1");
		Assert.DoesNotContain(project.GetInputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
	}

	[Fact]
	public void CollectionsCorrectlyGroupsOutputFiles()
	{
		var project = new MetalsharpProject()
			.AddOutput("Scenario\\Directory1")
			.AddOutput("Scenario\\Plugins")
			.UseCollections("test", file => file.Name.Contains("file"));

		Assert.Contains(project.GetOutputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "file1");
		Assert.DoesNotContain(project.GetOutputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
	}

	[Fact]
	public void GetInputFilesFromCollectionReturnsCorrectFiles()
	{
		var project = new MetalsharpProject()
			.AddInput("Scenario\\Directory1")
			.AddInput("Scenario\\Plugins")
			.UseCollections("test", file => file.Name.Contains("file"));

		Assert.Contains(project.GetInputFilesFromCollection("test"), i => i.Name == "file1");
		Assert.DoesNotContain(project.GetInputFilesFromCollection("test"), i => i.Name == "FileMarkdown");
	}

	[Fact]
	public void GetOutputFilesFromCollectionReturnsCorrectFiles()
	{
		var project = new MetalsharpProject()
			.AddOutput("Scenario\\Directory1")
			.AddOutput("Scenario\\Plugins")
			.UseCollections("test", file => file.Name.Contains("file"));

		Assert.Contains(project.GetOutputFilesFromCollection("test"), i => i.Name == "file1");
		Assert.DoesNotContain(project.GetOutputFilesFromCollection("test"), i => i.Name == "FileMarkdown");
	}
}
