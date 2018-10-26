﻿namespace Metal.Sharp
{
    /// <summary>
    /// The Drafts plugin
    /// 
    /// Removes any input or output file with ("draft", true) in its metadata
    /// </summary>
    public class Drafts : IMetalsharpPlugin
    {
        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public Metalsharp Execute(Metalsharp directory)
        {
            directory.InputFiles.RemoveAll(file =>
                file.Metadata.TryGetValue("draft", out var _isDraft)
                && _isDraft is bool isDraft
                && isDraft
            );

            directory.OutputFiles.RemoveAll(file =>
                file.Metadata.TryGetValue("draft", out var _isDraft)
                && _isDraft is bool isDraft
                && isDraft
            );

            return directory;
        }
    }
}
