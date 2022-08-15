namespace Metalsharp;

/// <summary>
/// Extensions for the Markdown plugin.
/// </summary>
public static class MarkdownPluginExtensions
{
	/// <summary>
	///     Invoke the `Markdown` plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseMarkdown();
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
	public static MetalsharpProject UseMarkdown(this MetalsharpProject project) =>
		project.Use(new Markdown());
}
