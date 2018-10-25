using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Metal.Sharp
{
    public class Layout : IMetalsharpPlugin
    {
        public Layout(string filePath)
        {
            if (File.Exists(filePath))
            {
                LayoutText = File.ReadAllText(filePath);
            }
            else throw new ArgumentException("File: " + filePath + " does not exist.");
        }

        public string LayoutText { get; set; }

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
