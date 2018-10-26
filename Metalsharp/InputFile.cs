using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Metal.Sharp
{
    public class InputFile : MetalsharpFile
    {
        public InputFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (File.GetAttributes(filePath) != FileAttributes.Directory)
                {
                    OriginalText = File.ReadAllText(filePath);
                    FilePath = filePath;
                }
                else throw new ArgumentException("File " + filePath + " is a directory.");
            }
            else throw new ArgumentException("File " + filePath + " does not exist.");
        }

        #region Properties

        private string _originalText;
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
                    catch (Exception ex) { var a = ex.Message; }
                }
            }
        }

        public string JsonText { get; private set; }

        #endregion
    }
}
