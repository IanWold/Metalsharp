namespace Metalsharp;

/// <summary>
/// Extensions for the Markdown plugin.
/// </summary>
public static class MarkdownPluginExtensions
{
	/// <summary>
	///     Invoke the <c>Markdown</c> plugin.
	/// </summary>
	/// 
	/// <example>
	///     <code>
	///         new MetalsharpProject()
	///         ... // Add files
	///         .UseMarkdown();
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
	public static MetalsharpProject UseMarkdown(this MetalsharpProject project) =>
		project.Use(new Markdown());
}
