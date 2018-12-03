using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.IO;
using System.Linq;

namespace Metalsharp.Tests
{
    public class PluginTests
    {
        [Fact]
        public void MarkdownGeneratesHtmlFile()
        {
            var directory = new MetalsharpDirectory("Scenario/Markdown/file1.md").UseMarkdown();
            
            Assert.Contains(directory.OutputFiles, i => i.Extension == ".html");
        }

        [Fact]
        public void MarkdownCopiesMetadata()
        {
            var directory = new MetalsharpDirectory("Scenario/Markdown/file1.md")
                .Use(dir => dir.InputFiles.ToList().ForEach(i => i.Metadata.Add("test", true)))
                .Use<Markdown>();

            Assert.True((bool)directory.OutputFiles[0].Metadata["test"]);
        }


    }
}
