using static System.Convert;

namespace Metalsharp.Tests;

public class FrontmatterTests
{
	[Fact]
	public void FrontmatterSucceedsWithoutFrontmatter()
	{
		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput("Scenario\\Plugins\\FileMarkdown.md").UseFrontmatter();

		Assert.True(project.InputFiles[0].Metadata.Count == 0);
	}

	[Fact]
	public void FrontmatterParsesJsonFrontmatter()
	{
		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput("Scenario\\Plugins\\FileJsonFrontmatter.md").UseFrontmatter();

		Assert.True((bool)project.InputFiles[0].Metadata["test"]);
	}

	[Fact]
	public void FrontmatterParsesYamlFrontmatter()
	{
		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput("Scenario\\Plugins\\FileYamlFrontmatter.md").UseFrontmatter();

		Assert.True(ToBoolean(project.InputFiles[0].Metadata["test"].ToString()));
	}

	[Fact]
	public void FrontmatterParsesAllJsonValueTypes()
	{
		var file = new MetalsharpFile(
			";;;\n" +
			"{" +
			"\"boolFalse\":false," +
			"\"boolTrue\":true," +
			"\"intValue\":42," +
			"\"doubleValue\":3.14," +
			"\"stringValue\":\"hello\"," +
			"\"nullValue\":null," +
			"\"arrayValue\":[\"a\",\"b\",3]," +
			"\"objectValue\":{\"nested\":\"value\"}" +
			"}\n" +
			";;;\n" +
			"Body text",
			"file.md"
		);

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseFrontmatter();
		var metadata = project.InputFiles[0].Metadata;

		Assert.False((bool)metadata["boolFalse"]);
		Assert.True((bool)metadata["boolTrue"]);
		Assert.Equal(42L, metadata["intValue"]);
		Assert.Equal(3.14, metadata["doubleValue"]);
		Assert.Equal("hello", metadata["stringValue"]);
		Assert.Null(metadata["nullValue"]);

		var array = Assert.IsType<List<object?>>(metadata["arrayValue"]);
		Assert.Equal(["a", "b", 3L], array);

		var nested = Assert.IsType<Dictionary<string, object?>>(metadata["objectValue"]);
		Assert.Equal("value", nested["nested"]);
	}

	[Fact]
	public void FrontmatterOverwritesExistingMetadataKey()
	{
		var file = new MetalsharpFile(";;;\n{\"key\":\"fromFrontmatter\"}\n;;;\nBody text", "file.md");
		file.Metadata["key"] = "original";

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseFrontmatter();

		Assert.Equal("fromFrontmatter", project.InputFiles[0].Metadata["key"]);
	}

	[Fact]
	public void FrontmatterSucceedsWithMalformedYamlFrontmatter()
	{
		var file = new MetalsharpFile("---\nkey: [unterminated\n---\nBody text", "file.md");

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseFrontmatter();

		Assert.Empty(project.InputFiles[0].Metadata);
	}

	[Fact]
	public void FrontmatterSucceedsWithUnterminatedYamlFrontmatter()
	{
		var file = new MetalsharpFile("---\nkey: value\nno closing delimiter", "file.md");

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseFrontmatter();

		Assert.Empty(project.InputFiles[0].Metadata);
	}

	[Fact]
	public void FrontmatterSucceedsWithMalformedJsonFrontmatter()
	{
		var file = new MetalsharpFile(";;;\n{not valid json\n;;;\nBody text", "file.md");

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseFrontmatter();

		Assert.Empty(project.InputFiles[0].Metadata);
	}

	[Fact]
	public void FrontmatterSucceedsWithUnterminatedJsonFrontmatter()
	{
		var file = new MetalsharpFile(";;;\n{\"key\":\"value\"}\nno closing delimiter", "file.md");

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseFrontmatter();

		Assert.Empty(project.InputFiles[0].Metadata);
	}

	[Fact]
	public void FrontmatterSucceedsWithNullJsonFrontmatter()
	{
		var file = new MetalsharpFile(";;;\nnull\n;;;\nBody text", "file.md");

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None }).AddInput(file).UseFrontmatter();

		Assert.Empty(project.InputFiles[0].Metadata);
	}
}
