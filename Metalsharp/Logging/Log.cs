using System;
using System.Threading.Tasks;

namespace Metalsharp.Logging;

/// <summary>
///		Provides logging functionality for Metalsharp projects.
/// </summary>
public class Log
{
	private readonly LogLevel _verbosity;

	private bool _wasLastLogSection = false;

	internal Log(LogLevel level)
	{
		_verbosity = level;
	}

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

		if (logEventArgs.Level >= _verbosity)
		{
			Console.WriteLine(logEventArgs.Message);
			OnLog?.Invoke(this, logEventArgs);

			_wasLastLogSection = false;
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
		TryLog(new(LogLevel.Debug, message.Trim()));

	/// <summary>
	///		Log a message at `Info` level.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	/// </param>
	public void Info(string message) =>
		TryLog(new(LogLevel.Info, message.Trim()));

	/// <summary>
	///		Log a message at `Info` level as a section, separated cleanly by a single empty line.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	///	</param>
	internal void InfoSection(string message)
	{
		TryLog(new(LogLevel.Info, $"{(_wasLastLogSection ? "" : "\n")}{message.Trim()}\n"));
		_wasLastLogSection = true;
	}

	/// <summary>
	///		Log a message at `Info` level directly, without trimming.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	///	</param>
	internal void InfoDirect(string message)
	{
		TryLog(new(LogLevel.Info, message));
		_wasLastLogSection = true;
	}

	/// <summary>
	///		Log a message at `Error` level.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	/// </param>
	public void Error(string message) =>
		TryLog(new(LogLevel.Error, message.Trim()));

	/// <summary>
	///		Log a message at `Fatal` level.
	/// </summary>
	/// 
	/// <param name="message">
	///		The message to log.
	/// </param>
	public void Fatal(string message) =>
		TryLog(new(LogLevel.Fatal, message.Trim()));
}
