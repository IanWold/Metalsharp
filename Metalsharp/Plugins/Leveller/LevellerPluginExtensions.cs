namespace Metalsharp;

/// <summary>
/// Extensions for the Leveller plugin.
/// </summary>
public static class LevellerPluginExtensions
{
	/// <summary>
	///     Invoke the `Leveller` plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseLeveller();
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
	public static MetalsharpProject UseLeveller(this MetalsharpProject project) =>
		project.Use(new Leveller());
}
