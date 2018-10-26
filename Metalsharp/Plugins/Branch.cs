using System;
using System.Collections.Generic;
using System.Linq;
using static Newtonsoft.Json.JsonConvert;

namespace Metal.Sharp
{
    public class Branch : IMetalsharpPlugin
    {
        public Branch(params Action<Metalsharp>[] branches) =>
            Branches = branches.ToList();

        public List<Action<Metalsharp>> Branches { get; set; }

        public Metalsharp Execute(Metalsharp directory)
        {
            Branches.ForEach(b => b(DeserializeObject<Metalsharp>(SerializeObject(directory))));
            return directory;
        }
    }
}
