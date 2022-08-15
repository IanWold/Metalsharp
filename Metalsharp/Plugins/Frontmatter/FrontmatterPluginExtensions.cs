namespace Metalsharp;

/// <summary>
/// Extensions for the Frontmatter plugin.
/// </summary>
public static class FrontmatterPluginExtensions
{
	/// <summary>
	///     Invoke the `Frontmatter` plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseFrontmatter();
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
	public static MetalsharpProject UseFrontmatter(this MetalsharpProject project) =>
		project.Use(new Frontmatter());
}
