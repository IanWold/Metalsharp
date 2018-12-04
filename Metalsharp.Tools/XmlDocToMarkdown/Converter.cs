using System;
using System.IO;
using System.Xml.Serialization;

namespace XmlDocToMarkdown
{
    public static class Converter
    {
        public static void Convert(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                Doc doc;

                using (var reader = new StreamReader(sourcePath))
                {
                    var serializer = new XmlSerializer(typeof(Doc));
                    doc = (Doc)serializer.Deserialize(reader);
                }

                using (var writer = new StreamWriter(destinationPath))
                {
                    writer.Write(doc.ToMarkdown());
                }
            }
            else
            {
                throw new ArgumentException("Source file does not exist: " + sourcePath);
            }
        }
    }
}
