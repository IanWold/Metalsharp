using CommandLine;
using System;

namespace Metalsharp;

/// <summary>
///		Represents the configuration options for a Metalsharp project.
/// </summary>
public class MetalsharpOptions
{
	/// <summary>
	/// The default value for `ClearOutputDirectory`
	/// </summary>
	public const bool DefaultClearOutputDirectory = false;

	/// <summary>
	/// The default value for `ClearOutputDirectory`
	/// </summary>
	public const string DefaultOutputDirectory = @".\";

	/// <summary>
	/// The default value for `ClearOutputDirectory`
	/// </summary>
	public const LogLevel DefaultVerbosity = LogLevel.Error;

	/// <summary>
	///		Instantiate the default configuration.
	/// </summary>
	public MetalsharpOptions(
		bool clearOutputDirectory = DefaultClearOutputDirectory,
		string outputDirectory = DefaultOutputDirectory,
		LogLevel verbosity = DefaultVerbosity
	)
	{
		ClearOutputDirectory = clearOutputDirectory;
		OutputDirectory = outputDirectory;
		Verbosity = verbosity;
	}

	/// <summary>
	///		Instantiate configuration from command line arguments.
	/// </summary>
	/// 
	/// <param name="args">
	///		The command line arguments
	/// </param>
	public static MetalsharpOptions FromArgs(string[] args)
	{
		MetalsharpOptions configuration = null;

		new Parser()
			.ParseArguments<MetalsharpOptions>(args)
			.WithParsed(c => configuration = c)
			.WithNotParsed(_ => throw new ArgumentException("Unable to parse arguments", nameof(args)));

		return configuration;
	}

	/// <summary>
	///     Whether Metalsharp should remove all the files in the output directory before writing any to that directory.
	///     
	///     `false` by default.
	/// </summary>
	[Option('c', "clear", Default = DefaultClearOutputDirectory, HelpText = "Whether Metalsharp should remove all the files in the output directory before writing any to that directory.")]
	public bool ClearOutputDirectory { get; init; }

	/// <summary>
	///     The directory to which the files will be output.
	///     
	///     `.\` by default.
	/// </summary>
	[Option('o', "output", Default = DefaultOutputDirectory, HelpText = "The directory to which the files will be output.")]
	public string OutputDirectory { get; init; }

	/// <summary>
	///		The minimum level to log.
	/// </summary>
	[Option('v', "verbosity", Default = DefaultVerbosity, HelpText = "The verbosity level for the log output.")]
	public LogLevel Verbosity { get; init; }
}
