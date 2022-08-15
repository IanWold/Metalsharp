using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Metalsharp;

/// <summary>
///     The Frontmatter plugin.
///     
///     Adds any YAML or JSON frontmatter in the input files to the metadata.
/// </summary>
/// 
/// <example>
///     Given the following `file.txt`:
///     
///     ```plaintext
///     ---
///     draft: true
///     ---
///     Hello, World!
///     ```
///     
///     The assertion in the following will evaluate to `true`:
///     
///     ```c#
///         var project = new MetalsharpProject("file.txt")
///         .UseFrontmatter();
///         
///         Assert.True((bool)project.InputFiles[0].Metadata["draft"])
///     ```
/// </example>
public class Frontmatter : IMetalsharpPlugin
{
	/// <summary>
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The `MetalsharpProject` to invoke the plugin on.
	/// </param>
	public void Execute(MetalsharpProject project)
	{
		foreach (var file in project.InputFiles)
		{
			project.Log.Debug($"Looking for frontmatter in {file.FilePath}");
			if (TryGetFrontmatter(file.Text, out Dictionary<string, object> metadata, out string text))
			{
				project.Log.Debug($"    Found frontmatter:");

				file.Text = text;

				foreach (var pair in metadata)
				{
					project.Log.Debug($"    [{pair.Key}] = {pair.Value}");

					if (file.Metadata.ContainsKey(pair.Key))
					{
						file.Metadata[pair.Key] = pair.Value;
					}
					else
					{
						file.Metadata.Add(pair.Key, pair.Value);
					}
				}
			}
		}
	}

	/// <summary>
	///     Try to parse YAML or JSON frontmatter
	/// </summary>
	/// 
	/// <param name="document">
	///     The document potentially containing frontmatter.
	/// </param>
	/// <param name="frontmatter">
	///     The parsed frontmatter.
	/// </param>
	/// <param name="remainder">
	///     The document minus the frontmatter text.
	/// </param>
	/// 
	/// <returns>
	///     `true` if frontmatter text was found and parsed; `false` otherwise.
	/// </returns>
	private static bool TryGetFrontmatter(string document, out Dictionary<string, object> frontmatter, out string remainder)
	{
		if (document.StartsWith("---") && TryGetYamlFrontmatter(document, out Dictionary<string, object> yamlFrontmatter, out string yamlRemainder))
		{
			frontmatter = yamlFrontmatter;
			remainder = yamlRemainder;
			return true;
		}
		else if (document.StartsWith(";;;") && TryGetJsonFrontmatter(document, out Dictionary<string, object> jsonFrontmatter, out string jsonRemainder))
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
	///     Try to parse YAML frontmatter.
	/// </summary>
	/// 
	/// <param name="document">
	///     The document containing frontmatter.
	/// </param>
	/// <param name="frontmatter">
	///     The parsed frontmatter.
	/// </param>
	/// <param name="remainder">
	///     The document minus the frontmatter text.
	/// </param>
	/// 
	/// <returns>
	///     `true` if frontmatter text was found and parsed; `false` otherwise.
	/// </returns>
	private static bool TryGetYamlFrontmatter(string document, out Dictionary<string, object> frontmatter, out string remainder)
	{
		var split = document.Split(new[] { "---" }, StringSplitOptions.None);

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
		else
		{
			return false;
		}
	}

	/// <summary>
	///     Try to parse JSON frontmatter.
	/// </summary>
	/// 
	/// <param name="document">
	///     The document containing frontmatter.
	/// </param>
	/// <param name="frontmatter">
	///     The parsed frontmatter.
	/// </param>
	/// <param name="remainder">
	///     The document minus the frontmatter text.
	/// </param>
	/// 
	/// <returns>
	///     `true` if frontmatter text was found and parsed; `false` otherwise.
	/// </returns>
	private static bool TryGetJsonFrontmatter(string document, out Dictionary<string, object> frontmatter, out string remainder)
	{
		var split = document.Split(new[] { ";;;" }, StringSplitOptions.None);

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
		else
		{
			return false;
		}
	}
}
