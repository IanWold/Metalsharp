using Metalsharp.Logging;

namespace Metalsharp;

/// <summary>
///		Represents the configuration options for a Metalsharp project.
/// </summary>
public class MetalsharpConfiguration
{
	/// <summary>
	///		The minimum level to log.
	/// </summary>
	public LogLevel LogLevel { get; set; }
}
