namespace Metalsharp.Tests;

public class DebugTests
{
	[Fact]
	public void DebugLogsAftereUse()
	{
		var didLogAfterPlugin = false;
		object? testMetadataValue = null;

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None });
		project
			.UseDebug(i =>
			{
				if (project.Metadata.TryGetValue("test", out var value))
				{
					didLogAfterPlugin = true;
					testMetadataValue = value;
				}
			})
			.Use<TestPlugin>();

		Assert.True(didLogAfterPlugin);
		Assert.True(testMetadataValue is bool value && value);
	}

	[Fact]
	public void DefaultUseDebugDoesNotThrow()
	{
		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None })
			.UseDebug()
			.Use<TestPlugin>();

		Assert.True((bool)project.Metadata["test"]);
	}

	[Fact]
	public void UseDebugWithLogPathWritesToFile()
	{
		var logPath = Path.Combine(Path.GetTempPath(), $"metalsharp-debug-{Guid.NewGuid()}.log");

		try
		{
			new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None })
				.UseDebug(logPath)
				.AddInput(new MetalsharpFile("text", "file.md"));

			Assert.True(File.Exists(logPath));

			var content = File.ReadAllText(logPath);
			Assert.Contains("Step 1.", content);
			Assert.Contains("file.md", content);
		}
		finally
		{
			if (File.Exists(logPath))
			{
				File.Delete(logPath);
			}
		}
	}

	[Fact]
	public void DebugLogListsInputAndOutputFilesInSortedOrder()
	{
		var capturedMessages = new List<string>();

		new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.None })
			.AddInput(new MetalsharpFile("text", "b.md"))
			.AddInput(new MetalsharpFile("text", "a.md"))
			.AddOutput(new MetalsharpFile("text", "b.html"))
			.AddOutput(new MetalsharpFile("text", "a.html"))
			.UseDebug(capturedMessages.Add);

		var capturedMessage = Assert.Single(capturedMessages, m => m.Contains("Input files:"));

		Assert.Contains("Output files:", capturedMessage);

		var aMdIndex = capturedMessage.IndexOf("\ta.md", StringComparison.Ordinal);
		var bMdIndex = capturedMessage.IndexOf("\tb.md", StringComparison.Ordinal);
		Assert.True(aMdIndex >= 0 && bMdIndex >= 0 && aMdIndex < bMdIndex);
	}

	[Fact]
	public void DebugLogPrefixesMessagesByLogLevel()
	{
		var capturedMessages = new List<string>();

		var project = new MetalsharpProject(new MetalsharpOptions() { Verbosity = LogLevel.Debug })
			.UseDebug(message => capturedMessages.Add(message));

		project.LogError("an error occurred");
		project.LogFatal("a fatal error occurred");

		Assert.Contains(capturedMessages, m => m.StartsWith("[ERROR] "));
		Assert.Contains(capturedMessages, m => m.StartsWith("[FATAL] "));
	}
}
