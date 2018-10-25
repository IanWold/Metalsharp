using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Markdig;

namespace Metal.Sharp
{
    public class Markdown : IMetalsharpPlugin
    {
        Metalsharp IMetalsharpPlugin.Execute(Metalsharp directory)
        {
            foreach (var file in directory.InputFiles)
            {
                var name = Path.GetFileNameWithoutExtension(file.Path);
                var text = Markdig.Markdown.ToHtml(file.Text);
                directory.OutputFiles.Add(new OutputFile(name + ".html", text) { Metadata = file.Metadata });
            }

            return directory;
        }
    }
}
