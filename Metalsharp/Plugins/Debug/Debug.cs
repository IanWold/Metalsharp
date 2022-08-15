using System;
using System.IO;
using System.Linq;
using Metalsharp.Logging;

namespace Metalsharp;

/// <summary>
///     The Debug plugin.
///     
///     Writes a log after every Use, outputting the contents of the input and output lists.
/// </summary>
/// 
/// <example>
///     `Debug` is best invoked at the beginning of a stack of plugins, so as to capture each of the events related to the project:
///     
///     ```c#
///         new MetalsharpProject("Path\\To\\Dir")
///         .Debug()
///         .Use ... ;
///     ```
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
	///     By default, write debug logs with `Debug.WriteLine()`.
	/// </summary>
	public Debug() : this(message => System.Diagnostics.Debug.WriteLine(message)) { }

	/// <summary>
	///     Instantiate `Debug` with a log file path to output the debug log to a log file.
	/// </summary>
	/// 
	/// <example>
	///     Given the following Metalsharp project:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .UseDebug("output.log")
	///         .Use(i => i.AddInput(new MetalsharpFile("text", "file.md")));
	///     ```
	///     
	///     A file called `output.log` will be generated, and will look like the following:
	///     
	///     ```plaintext
	///         Step 1.
	///         Input files:
	///         
	///         file.md
	///         
	///         Output files:
	///         
	///         ---
	///     ```
	/// </example>
	/// 
	/// <param name="logPath">
	///     The path to the log file.
	/// </param>
	public Debug(string logPath) : this(message =>
	{
		using var writer = new StreamWriter(logPath, true);
		writer.WriteLineAsync(message);
	})
	{ }

	/// <summary>
	///     Instantiate `Debug` with a custom action to perform each time a log is written. This can be used to output to different sources or execute different debug actions.
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
	///     The `MetalsharpProject` to output debug logs for.
	/// </param>
	public void Execute(MetalsharpProject project)
	{
		project.AfterUse += (sender, e) =>
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

		project.Log.OnAnyLog += (sender, e) =>
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
