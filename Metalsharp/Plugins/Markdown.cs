using System.IO;

namespace Metalsharp
{
    /// <summary>
    ///     The Markdown plugin
    ///     
    ///     Converts any markdown files in the input to HTML with [Markdig](https://github.com/lunet-io/markdig). HTML files are placed in the output.
    /// </summary>
    /// 
    /// <example>
    ///     ```c#
    ///         new MetalsharpDirectory()
    ///         .AddInput(new MetalsharpFile("# Header 1", "file.md")
    ///         .UseMarkdown()
    ///         .Build();
    ///     ```
    ///     
    ///     Will output the file `file.html` to the output directory. The contents of `file.html` will be:
    ///     
    ///     ```html
    ///     &lt;h1&gt;Header 1&lt;/h1&gt;
    ///     ```
    /// </example>
    public class Markdown : IMetalsharpPlugin
    {
        /// <summary>
        ///     Invokes the plugin.
        /// </summary>
        /// 
        /// <param name="directory">
        ///     The directory to invoke the plugin on.
        /// </param>
        public void Execute(MetalsharpDirectory directory)
        {
            foreach (var file in directory.InputFiles)
            {
                if (file.Extension == ".md" || file.Extension == ".markdown")
                {
                    var fileText = Markdig.Markdown.ToHtml(file.Text);
                    var filePath = Path.Combine(file.Directory, file.Name + ".html");

                    directory.OutputFiles.Add(new MetalsharpFile(fileText, filePath) { Metadata = file.Metadata });
                }
            }
        }
    }
}
