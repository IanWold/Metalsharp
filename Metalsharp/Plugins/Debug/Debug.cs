namespace Metalsharp;

/// <summary>
///     The Debug plugin.
///     
///     Writes a log after every Use, outputting the contents of the input and output lists.
/// </summary>
/// 
/// <example>
///     <c>Debug</c> is best invoked at the beginning of a stack of plugins, so as to capture each of the events related to the project:
///     
///     <code>
///         new MetalsharpProject("Path\\To\\Dir")
///         .Debug()
///         .Use ... ;
///     </code>
/// </example>
public class Debug : IMetalsharpPlugin
{
	/// <summary>
	///     The action to execute when writing a log.
	/// </summary>
	private readonly Action<string> _onLog;

	/// <summary>
	///     A count of the number of calls to .Use() against the directory.
	/// </summary>
	private int _useCount;

	/// <summary>
	///     By default, write debug logs with <c>Debug.WriteLine()</c>.
	/// </summary>
	public Debug() : this(message => System.Diagnostics.Debug.WriteLine(message)) { }

	/// <summary>
	///     Instantiate <c>Debug</c> with a log file path to output the debug log to a log file.
	/// </summary>
	/// 
	/// <example>
	///     Given the following Metalsharp project:
	///     
	///     <code>
	///         new MetalsharpProject()
	///         .UseDebug("output.log")
	///         .Use(i => i.AddInput(new MetalsharpFile("text", "file.md")));
	///     </code>
	///     
	///     A file called <c>output.log</c> will be generated, and will look like the following:
	///     
	///     <code>
	///         Step 1.
	///         Input files:
	///         
	///         file.md
	///         
	///         Output files:
	///         
	///         ---
	///     </code>
	/// </example>
	/// 
	/// <param name="logPath">
	///     The path to the log file.
	/// </param>
	public Debug(string logPath) : this(message =>
	{
		using var writer = new StreamWriter(logPath, true);
		writer.WriteLine(message);
	})
	{ }

	/// <summary>
	///     Instantiate <c>Debug</c> with a custom action to perform each time a log is written. This can be used to output to different sources or execute different debug actions.
	/// </summary>
	/// 
	/// <param name="onLog">
	///     The action to execute when writing a log.
	/// </param>
	public Debug(Action<string> onLog) =>
		_onLog = onLog;

	/// <summary>
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The <c>MetalsharpProject</c> to output debug logs for.
	/// </param>
	public void Execute(MetalsharpProject project)
	{
		project.AfterUse += (_, _) =>
			_onLog(
				"Step " + ++_useCount + "." +
				"\r\n" +
				"Input files:" +
				"\r\n\r\n" +
				WriteDirectory(project.InputFiles) +
				"\r\n\r\n" +
				"Output files:" +
				"\r\n\r\n" +
				WriteDirectory(project.OutputFiles) +
				"\r\n\r\n" +
				"---" +
				"\r\n\r\n"
			);

		project.OnAnyLog += (_, e) =>
			_onLog(
				e.Level switch {
					LogLevel.Debug => "[DEBUG] ",
					LogLevel.Info => "[INFO] ",
					LogLevel.Error => "[ERROR] ",
					_ => "[FATAL] "
				}
				+ e.Message
			);
	}

	/// <summary>
	///     Prettify the contents of a collection of files.
	/// </summary>
	/// 
	/// <param name="directory">
	///     The collection of files to prettify/
	/// </param>
	/// 
	/// <returns>
	///     A well-formatted string listing the paths of each file in the given collection.
	/// </returns>
	private static string WriteDirectory(MetalsharpFileCollection directory) =>
		string.Join(
			"\r\n",
			directory
				.OrderBy(file => file.FilePath)
				.Select(file => "\t" + file.FilePath)
		);
}
