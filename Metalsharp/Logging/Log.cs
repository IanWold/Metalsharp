using System;
using System.Threading.Tasks;

namespace Metalsharp.Logging;

/// <summary>
///		Provides logging functionality for Metalsharp projects.
/// </summary>
public class Log
{
	private readonly LogLevel _level;

	internal Log(LogLevel level, MetalsharpProject project)
	{
		_level = level;
		Project = project;
	}

	/// <summary>
	///		The `MetalsharpProject` to which this log belongs.
	/// </summary>
	public MetalsharpProject Project { get; init; }

	/// <summary>
	///		Invoked when any message is sent to the logger, irrespective of the specified log level.
	/// </summary>
	public EventHandler<LogEventArgs> OnAnyLog;
	
	/// <summary>
	///		Invoked when a message is logged at or above the specified log level.
	/// </summary>
	public EventHandler<LogEventArgs> OnLog;

	private void TryLog(LogEventArgs logEventArgs)
	{
		OnAnyLog?.Invoke(this, logEventArgs);

		if (logEventArgs.Level >= _level)
		{
			Console.WriteLine(logEventArgs.Message);
			OnLog?.Invoke(this, logEventArgs);
		}
	}

	/// <summary>
	///		Log a message at `Debug` level.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	///	</param>
	public void Debug(string message) =>
		TryLog(new(LogLevel.Debug, message));

	/// <summary>
	///		Log a message at `Info` level.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	/// </param>
	public void Info(string message) =>
		TryLog(new(LogLevel.Info, message));

	/// <summary>
	///		Log a message at `Error` level.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	/// </param>
	public void Error(string message) =>
		TryLog(new(LogLevel.Error, message));

	/// <summary>
	///		Log a message at `Fatal` level.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	/// </param>
	public void Fatal(string message) =>
		TryLog(new(LogLevel.Fatal, message));
}
