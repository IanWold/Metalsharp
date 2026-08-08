namespace Metalsharp;

/// <summary>
/// Extensions for the Frontmatter plugin.
/// </summary>
public static class FrontmatterPluginExtensions
{
	/// <summary>
	///     Invoke the <c>Frontmatter</c> plugin.
	/// </summary>
	/// 
	/// <example>
	///     <code>
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseFrontmatter();
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
	public static MetalsharpProject UseFrontmatter(this MetalsharpProject project) =>
		project.Use(new Frontmatter());
}
