using System;
using System.Collections.Generic;
using System.Linq;

namespace Metalsharp
{
    /// <summary>
    /// Collections plugin
    /// 
    /// Groups files matching a predicate into collections in the directory metadata
    /// </summary>
    public class Collections : IMetalsharpPlugin
    {
        /// <summary>
        /// Contains the definitions of the collections
        /// </summary>
        private (string name, Predicate<IMetalsharpFile> predicate)[] _definitions;

        /// <summary>
        /// Instantiates the plugin with the definitions of the collections
        /// </summary>
        /// <param name="definitions">The definitions of the collections, including the name of the collection and the predicate which matches its files</param>
        public Collections(params (string name, Predicate<IMetalsharpFile> predicate)[] definitions) =>
            _definitions = definitions;

        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        public void Execute(MetalsharpDirectory directory)
        {
            var collections = new Dictionary<string, Dictionary<string, string[]>>();

            foreach (var definition in _definitions)
            {
                var inputCollection = new List<string>();
                var outputCollection = new List<string>();

                directory.InputFiles.Where(i => definition.predicate(i)).ToList().ForEach(i => inputCollection.Add(i.FilePath));
                directory.OutputFiles.Where(i => definition.predicate(i)).ToList().ForEach(i => outputCollection.Add(i.FilePath));

                collections.Add(definition.name, new Dictionary<string, string[]>
                {
                    ["input"] = inputCollection.ToArray(),
                    ["output"] = outputCollection.ToArray()
                });
            }

            if (directory.Metadata.ContainsKey("collections")
                && directory.Metadata["collections"] is Dictionary<string, Dictionary<string, string[]>> dictionary)
            {
                foreach (var item in collections)
                {
                    dictionary.Add(item.Key, item.Value);
                }
            }
            else
            {
                directory.Meta("collections", collections);
            }
        }
    }
}
