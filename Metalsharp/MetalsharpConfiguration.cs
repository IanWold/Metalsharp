using CommandLine;
using Metalsharp.Logging;
using System;

namespace Metalsharp;

/// <summary>
///		Represents the configuration options for a Metalsharp project.
/// </summary>
public class MetalsharpConfiguration
{
	/// <summary>
	///		Instantiate the default configuration.
	/// </summary>
	public MetalsharpConfiguration()
	{
		Verbosity = LogLevel.Error;
	}

	/// <summary>
	///		Instantiate configuration from command line arguments.
	/// </summary>
	/// 
	/// <param name="args">
	///		The command line arguments
	/// </param>
	public static MetalsharpConfiguration FromArgs(string[] args)
	{
		MetalsharpConfiguration configuration = null;

		new Parser()
			.ParseArguments<MetalsharpConfiguration>(args)
			.WithParsed(c => configuration = c)
			.WithNotParsed(_ => throw new ArgumentException("Unable to parse arguments", nameof(args)));

		return configuration;
	}

	/// <summary>
	///		The minimum level to log.
	/// </summary>
	[Option('v', "verbosity", Default = LogLevel.Error, HelpText = "The verbosity level for the log output.")]
	public LogLevel Verbosity { get; set; }
}
