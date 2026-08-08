namespace Metalsharp.Tests;

public class MetalsharpOptionsTests
{
	[Fact]
	public void FromArgsParsesArguments()
	{
		var options = MetalsharpOptions.FromArgs(["--clear", "--output", "MyOutput", "--verbosity", "Debug"]);

		Assert.True(options.ClearOutputDirectory);
		Assert.Equal("MyOutput", options.OutputDirectory);
		Assert.Equal(LogLevel.Debug, options.Verbosity);
	}

	[Fact]
	public void FromArgsUsesDefaultsWhenNoArgumentsGiven()
	{
		var options = MetalsharpOptions.FromArgs([]);

		Assert.Equal(MetalsharpOptions.DefaultClearOutputDirectory, options.ClearOutputDirectory);
		Assert.Equal(MetalsharpOptions.DefaultOutputDirectory, options.OutputDirectory);
		Assert.Equal(MetalsharpOptions.DefaultVerbosity, options.Verbosity);
	}
}
