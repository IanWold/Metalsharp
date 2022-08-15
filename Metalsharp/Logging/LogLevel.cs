namespace Metalsharp.Logging;

/// <summary>
/// The verbosity level for log messages.
/// </summary>
public enum LogLevel
{
	/// <summary>
	/// `Debug` includes every loggable event useful when debugging.
	/// </summary>
	Debug = 0,

	/// <summary>
	/// `Info` includes every meaningful event while executing.
	/// </summary>
	Info = 1,

	/// <summary>
	/// `Error` includes any events that are unexpected or may be unintended by the user.
	/// </summary>
	Error = 2,

	/// <summary>
	/// `Fatal` includes any events that prevent continued execution.
	/// </summary>
	Fatal = 3,

	/// <summary>
	/// `None` prevents any logging.
	/// </summary>
	None = 4,
}
