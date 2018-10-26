using System;
using System.Collections.Generic;
using System.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace Metal.Sharp
{
    /// <summary>
    /// The Branch plugin
    /// 
    /// Branches a directory for separate plugins to be computed
    /// </summary>
    public class Branch : IMetalsharpPlugin
    {
        /// <summary>
        /// </summary>
        /// <param name="branches">The functions defining each branch</param>
        public Branch(params Action<Metalsharp>[] branches) =>
            Branches = branches.ToList();

        /// <summary>
        /// The function-branches
        /// </summary>
        public List<Action<Metalsharp>> Branches { get; set; }

        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public Metalsharp Execute(Metalsharp directory)
        {
            Branches.ForEach(b => b(DeserializeObject<Metalsharp>(SerializeObject(directory))));
            return directory;
        }
    }
}
