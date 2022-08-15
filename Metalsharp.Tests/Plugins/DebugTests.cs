using Xunit;

namespace Metalsharp.Tests;

public class DebugTests
{
	[Fact]
	public void DebugLogsAftereUse()
	{
		var didLogAfterPlugin = false;
		object testMetadataValue = null;

		var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None });
		project
			.UseDebug(i =>
			{
				if (project.Metadata.TryGetValue("test", out object value))
				{
					didLogAfterPlugin = true;
					testMetadataValue = value;
				}
			})
			.Use<TestPlugin>();

		Assert.True(didLogAfterPlugin);
		Assert.True(testMetadataValue is bool value && value);
	}
}
