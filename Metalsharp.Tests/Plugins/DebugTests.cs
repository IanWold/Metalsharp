using Xunit;

namespace Metalsharp.Tests;

public class DebugTests
{
	[Fact]
	public void DebugLogsAftereUse()
	{
		var project = new MetalsharpProject();
		var uses = 0;

		project.UseDebug(i =>
		{
			// onLog will be called after UseDebug is called,
			// Need to prevent the assertion before the use of TestPlugin
			if (uses == 1)
			{
				Assert.True((bool)project.Metadata["test"]);
			}
			else
			{
				uses++;
			}
		});
		project.Use<TestPlugin>();
	}
}
