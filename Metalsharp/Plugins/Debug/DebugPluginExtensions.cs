using System;

namespace Metalsharp;

/// <summary>
/// Extensions for the Debug plugin.
/// </summary>
public static class DebugPluginExtensions
{
	/// <summary>
	/// Invoke the default Debug plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .UseDebug();
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseDebug(this MetalsharpProject project) =>
		project.Use(new Debug());

	/// <summary>
	///     Invoke the Debug plugin with a log file to capture the debug logs.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .UseDebug("debug.log");
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// <param name="logPath">
	///     The path to the log file.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseDebug(this MetalsharpProject project, string logPath) =>
		project.Use(new Debug(logPath));

	/// <summary>
	///     Invoke the Debug plugin with custom log behavior.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .UseDebug(log => Console.WriteLine(log));
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// <param name="onLog">
	///     The action to execute to log a debug line.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject UseDebug(this MetalsharpProject project, Action<string> onLog) =>
		project.Use(new Debug(onLog));
}
