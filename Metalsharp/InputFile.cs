using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

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
                JsonText = null;
                Text = _originalText;

                if (_originalText.Contains(";"))
                {
                    try
                    {
                        var semicolonPos = _originalText.IndexOf(';');
                        var jsonText = _originalText.Substring(0, semicolonPos);
                        var fileText = _originalText.Substring(++semicolonPos, _originalText.Length - semicolonPos);

                        var jObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonText);

                        JsonText = jsonText;
                        Metadata = jObject;
                        Text = fileText;

                        return;
                    }
                    catch (JsonException) // Metalsharp errantly assumed the pre-; text was JSON metadata
                    {
                    }
                }
            }
        }
        private string _originalText;

        /// <summary>
        /// The text of the JSON metadata before it was parsed into Metadata
        /// </summary>
        public string JsonText { get; private set; }

        #endregion
    }
}
