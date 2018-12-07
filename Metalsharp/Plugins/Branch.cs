using System;
using System.Collections.Generic;
using System.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace Metalsharp
{
    /// <summary>.
    /// The Branch plugin
    /// 
    /// Creates copies of a `MetalsharpDirectory` for separate stacks of plugins to be independently invoked on it.
    /// </summary>
    /// 
    /// <example>
    ///     The following will create a file and output it to two different directories by branching the `MetalsharpDirectory` and calling `Build` on each branch:
    ///     
    ///     ```c#
    ///         new MetalsharpDirectory()
    ///         .AddOutput(new MetalsharpFile("# Header!", "file.md")
    ///         .Branch(
    ///         // The first branch:
    ///         dir => dir.Build(new BuildOptions { OutputDirectory = "Directory1" }),
    ///         
    ///         // The second branch:
    ///         dir => dir.Build(new BuildOptions { OutputDirectory = "Directory2" })
    ///         );
    ///     ```
    /// </example>
    public class Branch : IMetalsharpPlugin
    {
        /// <summary>
        ///     The actions of each branch.
        /// </summary>
        private List<Action<MetalsharpDirectory>> _branches;

        /// <summary>
        ///     Instantiate the Branch plugin by providing a list of actions to specify the behavior of each branch.
        /// </summary>
        /// 
        /// <param name="branches">
        ///     The functions defining each branch.
        /// </param>
        public Branch(params Action<MetalsharpDirectory>[] branches) =>
            _branches = branches.ToList();

        /// <summary>
        ///     Invokes the plugin.
        /// </summary>
        /// 
        /// <param name="directory">
        ///     The directory to branch.
        /// </param>
        public void Execute(MetalsharpDirectory directory) =>
            _branches.ForEach(b => b(DeserializeObject<MetalsharpDirectory>(SerializeObject(directory))));
    }
}
