using System;
using System.Text;
using System.Xml.Serialization;

namespace XmlDocToMarkdown
{
    [Serializable]
    public class Param
    {
        [XmlText]
        public string Description { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    public static class ParamExtensions
    {
        public static string ToMarkdownTable(this Param[] parameters)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"| Name | Description |");
            sb.AppendLine($"|---|---|");

            foreach (var param in parameters)
            {
                sb.AppendLine($"| `{ param.Name }` | { param.Description.Trim() } |");
            }

            sb.AppendLine();
            return sb.ToString();
        }
    }
}
