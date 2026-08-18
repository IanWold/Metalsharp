namespace Metalsharp;

/// <summary>
/// Extensions for the Leveller plugin.
/// </summary>
public static class LevellerPluginExtensions
{
	/// <summary>
	///     Invoke the <c>Leveller</c> plugin.
	/// </summary>
	/// 
	/// <example>
	///     <code>
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseLeveller();
	///     </code>
	/// </example>
	/// 
	/// <param name="project">
	///     The <c>MetalsharpProject</c> on which this method will be called.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns <c>this</c> input.
	/// </returns>
	public static MetalsharpProject UseLeveller(this MetalsharpProject project) =>
		project.Use(new Leveller());
}
