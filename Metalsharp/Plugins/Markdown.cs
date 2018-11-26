﻿using System.IO;

namespace Metalsharp
{
    /// <summary>
    /// The Markdown plugin
    /// 
    /// Converts any markdown files to HTML
    /// </summary>
    public class Markdown : IMetalsharpPlugin
    {
        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        public void Execute(MetalsharpDirectory directory)
        {
            foreach (var file in directory.InputFiles)
            {
                if (file.Extension == "md" || file.Extension == "markdown")
                {
                    var name = Path.GetFileNameWithoutExtension(file.FilePath);
                    var text = Markdig.Markdown.ToHtml(file.Text);
                    directory.OutputFiles.Add(new MetalsharpFile(text, name + ".html") { Metadata = file.Metadata });
                }
            }
        }
    }
}
