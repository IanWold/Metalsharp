namespace Metalsharp;

/// <summary>
/// The verbosity level for log messages.
/// </summary>
[Flags]
public enum LogLevel
{
	/// <summary>
	/// <c>Debug</c> includes every loggable event useful when debugging.
	/// </summary>
	Debug = 0x0,

	/// <summary>
	/// <c>Info</c> includes every meaningful event while executing.
	/// </summary>
	Info = 0x1,

	/// <summary>
	/// <c>Error</c> includes any events that are unexpected or may be unintended by the user.
	/// </summary>
	Error = 0x2,

	/// <summary>
	/// <c>Fatal</c> includes any events that prevent continued execution.
	/// </summary>
	Fatal = 0x4,

	/// <summary>
	/// <c>None</c> prevents any logging.
	/// </summary>
	None = 0x8,
}
