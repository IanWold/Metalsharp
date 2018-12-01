using System.IO;

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
