using Xunit;

namespace Metalsharp.Tests;

public class BranchTests
{
	[Fact]
	public void BranchExecutesBranchesIndependently()
	{
		var firstBranchExecuted = false;
		var secondBranchExecuted = false;

		new MetalsharpProject(new MetalsharpConfiguration()
		{
			LogLevel = Logging.LogLevel.None
		})
			.AddInput("Scenario\\Plugins")
			.Branch(
				proj =>
				{
					firstBranchExecuted = true;
					proj.AddInput("Scenario\\Directory1");
					Assert.DoesNotContain(proj.InputFiles, i => i.Name == "file11");
				},
				proj =>
				{
					secondBranchExecuted = true;
					proj.AddInput("Scenario\\Directory2\\SubDirectory1");
					Assert.DoesNotContain(proj.InputFiles, i => i.Name == "file1");
				}
			);

		Assert.True(firstBranchExecuted);
		Assert.True(secondBranchExecuted);
	}
}
