using CommandLine;

namespace Metalsharp;

/// <summary>
///		Represents the configuration options for a Metalsharp project.
/// </summary>
///
/// <param name="clearOutputDirectory">
///     Whether Metalsharp should remove all the files in the output directory before writing any to that directory.
///
///     <c>false</c> by default.
/// </param>
/// <param name="outputDirectory">
///     The directory to which the files will be output.
///
///     <c>.\</c> by default.
/// </param>
/// <param name="verbosity">
///		The minimum level to log.
/// </param>
public class MetalsharpOptions(
	bool clearOutputDirectory = MetalsharpOptions.DefaultClearOutputDirectory,
	string outputDirectory = MetalsharpOptions.DefaultOutputDirectory,
	LogLevel verbosity = MetalsharpOptions.DefaultVerbosity
)
{
	/// <summary>
	///     Instantiate the default configuration.
	/// </summary>
	///
	/// <remarks>
	///     This overload exists because <c>CommandLineParser</c> constructs instances via reflection using
	///     <c>Activator.CreateInstance&lt;T&gt;()</c>, which requires a genuinely parameterless constructor -
	///     a constructor whose parameters merely all have default values does not qualify. Without this,
	///     <see cref="FromArgs"/> throws <see cref="MissingMethodException"/>.
	/// </remarks>
	public MetalsharpOptions() : this(DefaultClearOutputDirectory, DefaultOutputDirectory, DefaultVerbosity) { }

	/// <summary>
	/// The default value for <c>ClearOutputDirectory</c>
	/// </summary>
	public const bool DefaultClearOutputDirectory = false;

	/// <summary>
	/// The default value for <c>OutputDirectory</c>
	/// </summary>
	public const string DefaultOutputDirectory = @".\";

	/// <summary>
	/// The default value for <c>Verbosity</c>
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
		MetalsharpOptions? configuration = null;

		new Parser()
			.ParseArguments<MetalsharpOptions>(args)
			.WithParsed(c => configuration = c)
			.WithNotParsed(_ => throw new ArgumentException("Unable to parse arguments", nameof(args)));

		return configuration!;
	}

	/// <summary>
	///     Whether Metalsharp should remove all the files in the output directory before writing any to that directory.
	///
	///     <c>false</c> by default.
	/// </summary>
	[Option('c', "clear", Default = DefaultClearOutputDirectory, HelpText = "Whether Metalsharp should remove all the files in the output directory before writing any to that directory.")]
	public bool ClearOutputDirectory { get; init; } = clearOutputDirectory;

	/// <summary>
	///     The directory to which the files will be output.
	///
	///     <c>.\</c> by default.
	/// </summary>
	[Option('o', "output", Default = DefaultOutputDirectory, HelpText = "The directory to which the files will be output.")]
	public string OutputDirectory { get; init; } = outputDirectory;

	/// <summary>
	///		The minimum level to log.
	/// </summary>
	[Option('v', "verbosity", Default = DefaultVerbosity, HelpText = "The verbosity level for the log output.")]
	public LogLevel Verbosity { get; init; } = verbosity;
}
