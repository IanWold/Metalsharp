using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Metal.Sharp
{
    /// <summary>
    /// Represents a file input to Metalsharp
    /// </summary>
    public class InputFile : MetalsharpFile
    {
        /// <summary>
        /// Create a new InputFile
        /// </summary>
        /// <param name="filePath">The path to the file to open</param>
        public InputFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (File.GetAttributes(filePath) != FileAttributes.Directory)
                {
                    OriginalText = File.ReadAllText(filePath);
                    FilePath = filePath;
                }
                else
                {
                    throw new ArgumentException("File " + filePath + " is a directory.");
                }
            }
            else
            {
                throw new ArgumentException("File " + filePath + " does not exist.");
            }
        }

        /// <summary>
        /// Create new InputFile and overwrite FilePath
        /// </summary>
        /// <param name="pathToRead">The path to the file to open</param>
        /// <param name="pathToSave">The value to save in FilePath</param>
        public InputFile(string pathToRead, string pathToSave)
            : this(pathToRead) =>
            FilePath = pathToSave;

        #region Properties

        /// <summary>
        /// The original text of a file before metadata was parsed out of it
        /// </summary>
        public string OriginalText
        {
            get => _originalText;
            private set
            {
                _originalText = value;

                if (TryGetFrontmatter(value, out Dictionary<string, object> metadata, out string text))
                {
                    Metadata = metadata;
                    Text = text;
                }
            }
        }
        private string _originalText;

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Try to parse YAML or JSON frontmatter
        /// </summary>
        /// <param name="document">The document containing frontmatter</param>
        /// <param name="frontmatter">The parsed frontmatter</param>
        /// <param name="remainder">The document minus the frontmatter text</param>
        /// <returns></returns>
        static bool TryGetFrontmatter(string document, out Dictionary<string, object> frontmatter, out string remainder)
        {
            if (document.StartsWith("---")
                && TryGetYamlFrontmatter(document, out Dictionary<string, object> yamlFrontmatter, out string yamlRemainder))
            {
                frontmatter = yamlFrontmatter;
                remainder = yamlRemainder;
                return true;
            }
            else if (document.StartsWith(";;;")
                && TryGetJsonFrontmatter(document, out Dictionary<string, object> jsonFrontmatter, out string jsonRemainder))
            {
                frontmatter = jsonFrontmatter;
                remainder = jsonRemainder;
                return true;
            }
            else
            {
                frontmatter = null;
                remainder = null;
                return false;
            }
        }

        /// <summary>
        /// Try to parse YAML frontmatter
        /// </summary>
        /// <param name="document">The document containing frontmatter</param>
        /// <param name="frontmatter">The parsed frontmatter</param>
        /// <param name="remainder">The document minus the frontmatter text</param>
        /// <returns></returns>
        static bool TryGetYamlFrontmatter(string document, out Dictionary<string, object> frontmatter, out string remainder)
        {
            var split = document.Split(new string[] { "---" }, StringSplitOptions.None);

            frontmatter = null;
            remainder = null;

            if (split.Length >= 3 && split[0].Length == 0)
            {
                try
                {
                    var yamlFrontmatter = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, object>>(new StringReader("---\r\n" + split[1].Trim() + "\r\n..."));
                    var arrayRemainder = string.Join("---", split.Skip(2));

                    frontmatter = yamlFrontmatter;
                    remainder = arrayRemainder;

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Try to parse JSON frontmatter
        /// </summary>
        /// <param name="document">The document containing frontmatter</param>
        /// <param name="frontmatter">The parsed frontmatter</param>
        /// <param name="remainder">The document minus the frontmatter text</param>
        /// <returns></returns>
        static bool TryGetJsonFrontmatter(string document, out Dictionary<string, object> frontmatter, out string remainder)
        {
            var split = document.Split(new string[] { ";;;" }, StringSplitOptions.None);

            frontmatter = null;
            remainder = null;

            if (split.Length >= 3 && split[0].Length == 0)
            {
                try
                {
                    var jsonFrontmatter = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(split[1].Trim());
                    var arrayRemainder = string.Join(";;;", split.Skip(2));

                    frontmatter = jsonFrontmatter;
                    remainder = arrayRemainder;

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else return false;
        }

        #endregion
    }
}
