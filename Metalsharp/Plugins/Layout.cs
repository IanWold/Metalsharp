using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Metal.Sharp
{
    /// <summary>
    /// The Layout plugin
    /// 
    /// Applies an HTML layout to every HTML page in the output
    /// </summary>
    public class Layout : IMetalsharpPlugin
    {
        /// <summary>
        /// </summary>
        /// <param name="filePath">The path to the layout file</param>
        public Layout(string filePath)
        {
            if (File.Exists(filePath))
            {
                LayoutText = File.ReadAllText(filePath);
            }
            else throw new ArgumentException("File: " + filePath + " does not exist.");
        }

        /// <summary>
        /// The full text from the layout file
        /// </summary>
        public string LayoutText { get; set; }

        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public Metalsharp Execute(Metalsharp directory)
        {
            foreach (var file in directory.OutputFiles.Where(i => i.Extension == "html"))
            {
                var regex = new Regex("\\{\\{(\\s)*content(\\s)*\\}\\}");
                var res = regex.Replace(LayoutText, file.Text);

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

            return directory;
        }
    }
}
