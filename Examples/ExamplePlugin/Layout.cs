using Metal.Sharp;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExamplePlugin
{
    /// <summary>
    /// The Layout plugin
    /// 
    /// Applies an HTML layout to every HTML page in the output, 
    /// if that page has a layout file specified in its metadata
    /// </summary>
    public class Layout : IMetalsharpPlugin
    {
        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        public void Execute(Metalsharp directory)
        {
            foreach (var file in directory.OutputFiles.Where(i => i.Extension == "html"))
            {
                if (file.Metadata.TryGetValue("layout", out var _layoutFile)
                    && _layoutFile is string layoutFile
                    && File.Exists(layoutFile))
                {
                    var regex = new Regex("\\{\\{(\\s)*content(\\s)*\\}\\}");
                    var res = regex.Replace(File.ReadAllText(layoutFile), file.Text);

                    foreach (var meta in file.Metadata)
                    {
                        regex = new Regex("\\{\\{(\\s)*" + meta.Key + "(\\s)*\\}\\}");
                        if (regex.IsMatch(res))
                        {
                            res = regex.Replace(res, meta.Value.ToString());
                        }
                    }

                    file.Text = res;
                }
            }
        }
    }
}
