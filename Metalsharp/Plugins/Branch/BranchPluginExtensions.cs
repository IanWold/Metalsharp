using System;

namespace Metalsharp;

/// <summary>
/// Extensions for the Branch plugin.
/// </summary>
public static class BranchPluginExtensions
{
	/// <summary>
	///     Invoke the Branch plugin.
	/// </summary>
	/// 
	/// <example>
	///     Branch the `MetalsharpProject` twice:
	/// 
	///     ```c#
	///         new MetalsharpProject()
	///         // Add files
	///         .Branch(
	///         dir => {
	///         // Do something with branch 1
	///         },
	///         dir => {
	///         // Do something with branch 2
	///         }
	///         );
	///     ```
	/// </example>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` on which this method will be called.
	/// </param>
	/// <param name="branches">
	///     The functions to handle each of the branches.
	/// </param>
	/// 
	/// <returns>
	///     Combinator; returns `this` input.
	/// </returns>
	public static MetalsharpProject Branch(this MetalsharpProject project, params Action<MetalsharpProject>[] branches) =>
		project.Use(new Branch(branches));
}
