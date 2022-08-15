using System;
using System.Collections.Generic;
using System.Linq;
using Utf8Json;

namespace Metalsharp;

/// <summary>
///     The Branch plugin
///     
///     Creates copies of a `MetalsharpProject` for separate stacks of plugins to be independently invoked on it.
/// </summary>
/// 
/// <example>
///     The following will create a file and output it to two different directories by branching the `MetalsharpProject` and calling `Build` on each branch:
///     
///     ```c#
///         new MetalsharpProject()
///         .AddOutput(new MetalsharpFile("# Header!", "file.md")
///         .Branch(
///         // The first branch:
///         proj => proj.Build(new BuildOptions { OutputDirectory = "Directory1" }),
///         
///         // The second branch:
///         proj => proj.Build(new BuildOptions { OutputDirectory = "Directory2" })
///         );
///     ```
/// </example>
public class Branch : IMetalsharpPlugin
{
	/// <summary>
	///     The actions of each branch.
	/// </summary>
	private List<Action<MetalsharpProject>> _branches;

	/// <summary>
	///     Instantiate the Branch plugin by providing a list of actions to specify the behavior of each branch.
	/// </summary>
	/// 
	/// <param name="branches">
	///     The functions defining each branch.
	/// </param>
	public Branch(params Action<MetalsharpProject>[] branches) =>
		_branches = branches.ToList();

	/// <summary>
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` to branch.
	/// </param>
	public void Execute(MetalsharpProject project) => _branches.ForEach(b =>
	{
		var projectClone = JsonSerializer.Deserialize<MetalsharpProject>(JsonSerializer.Serialize(project));

		projectClone.Log.Debug("Executing new branch");
		b(projectClone);
	});
}
