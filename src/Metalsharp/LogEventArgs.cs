namespace Metalsharp;

/// <summary>
/// Event args for log events.
/// </summary>
/// <param name="Level">The log level of the log.</param>
/// <param name="Message">The message of the log.</param>
public record LogEventArgs(LogLevel Level, string Message);
