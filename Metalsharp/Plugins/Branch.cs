using System;
using System.Collections.Generic;
using System.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace Metalsharp
{
    /// <summary>
    /// The Branch plugin
    /// 
    /// Branches a directory for separate plugins to be computed
    /// </summary>
    public class Branch : IMetalsharpPlugin
    {
        /// <summary>
        /// The function-branches
        /// </summary>
        private List<Action<MetalsharpDirectory>> _branches;

        /// <summary>
        /// </summary>
        /// <param name="branches">The functions defining each branch</param>
        public Branch(params Action<MetalsharpDirectory>[] branches) =>
            _branches = branches.ToList();

        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        public void Execute(MetalsharpDirectory directory) =>
            _branches.ForEach(b => b(DeserializeObject<MetalsharpDirectory>(SerializeObject(directory))));
    }
}
