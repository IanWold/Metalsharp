using CommandLine;
using System;

namespace Metalsharp;

/// <summary>
///		Represents the configuration options for a Metalsharp project.
/// </summary>
/// <remarks>
///		Instantiate the default configuration.
/// </remarks>
public class MetalsharpOptions(
    bool clearOutputDirectory = MetalsharpOptions.DefaultClearOutputDirectory,
    string outputDirectory = MetalsharpOptions.DefaultOutputDirectory,
    LogLevel verbosity = MetalsharpOptions.DefaultVerbosity
)
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
    ///		Instantiate configuration from command line arguments.
    /// </summary>
    /// 
    /// <param name="args">
    ///		The command line arguments
    /// </param>
    public static MetalsharpOptions FromArgs(string[] args)
	{
		MetalsharpOptions configuration = new();

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
    public bool ClearOutputDirectory { get; init; } = clearOutputDirectory;

    /// <summary>
    ///     The directory to which the files will be output.
    ///     
    ///     `.\` by default.
    /// </summary>
    [Option('o', "output", Default = DefaultOutputDirectory, HelpText = "The directory to which the files will be output.")]
    public string OutputDirectory { get; init; } = outputDirectory;

    /// <summary>
    ///		The minimum level to log.
    /// </summary>
    [Option('v', "verbosity", Default = DefaultVerbosity, HelpText = "The verbosity level for the log output.")]
    public LogLevel Verbosity { get; init; } = verbosity;
}
