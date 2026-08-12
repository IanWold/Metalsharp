using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Metalsharp;

/// <summary>
///     The Frontmatter plugin.
///     
///     Adds any YAML or JSON frontmatter in the input files to the metadata.
/// </summary>
/// 
/// <example>
///     Given the following <c>file.txt</c>:
///     
///     <code>
///     ---
///     draft: true
///     ---
///     Hello, World!
///     </code>
///     
///     The assertion in the following will evaluate to <c>true</c>:
///     
///     <code>
///         var project = new MetalsharpProject()
///             .AddInput("file.txt")
///             .UseFrontmatter();
///
///         Assert.True(Convert.ToBoolean(project.InputFiles[0].Metadata["draft"]))
///     </code>
///
///     Note that YAML frontmatter values are parsed as strings (here, the literal string <c>"true"</c>), while
///     JSON frontmatter values are parsed into their inferred CLR types (here, an actual <c>bool</c>) - so
///     <see cref="Convert.ToBoolean(object)"/> is used above for portability between the two frontmatter formats.
/// </example>
public class Frontmatter : IMetalsharpPlugin
{
	private static readonly JsonSerializerOptions s_jsonOptions = new()
	{
		Converters = { new InferredTypeJsonConverter() }
	};

	/// <summary>
	///     Invokes the plugin.
	/// </summary>
	/// 
	/// <param name="project">
	///     The <c>MetalsharpProject</c> to invoke the plugin on.
	/// </param>
	public void Execute(MetalsharpProject project)
	{
		foreach (var file in project.InputFiles)
		{
			project.LogDebug($"Looking for frontmatter in {file.FilePath}");
			if (TryGetFrontmatter(file.Text, out var metadata, out var text))
			{
				project.LogDebug($"    Found frontmatter:");

				file.Contents = Encoding.Default.GetBytes(text);

				foreach (var pair in metadata)
				{
					project.LogDebug($"    [{pair.Key}] = {pair.Value}");

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
	///     <c>true</c> if frontmatter text was found and parsed; <c>false</c> otherwise.
	/// </returns>
	private static bool TryGetFrontmatter(string document, [NotNullWhen(true)] out Dictionary<string, object>? frontmatter, [NotNullWhen(true)] out string? remainder)
	{
		if (document.StartsWith("---") && TryGetYamlFrontmatter(document, out var yamlFrontmatter, out var yamlRemainder))
		{
			frontmatter = yamlFrontmatter;
			remainder = yamlRemainder;
			return true;
		}
		else if (document.StartsWith(";;;") && TryGetJsonFrontmatter(document, out var jsonFrontmatter, out var jsonRemainder))
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
	///     <c>true</c> if frontmatter text was found and parsed; <c>false</c> otherwise.
	/// </returns>
	private static bool TryGetYamlFrontmatter(string document, [NotNullWhen(true)] out Dictionary<string, object>? frontmatter, [NotNullWhen(true)] out string? remainder)
	{
		frontmatter = null;
		remainder = null;

		var closingIndex = document.IndexOf("---", 3, StringComparison.Ordinal);
		if (closingIndex < 0)
		{
			return false;
		}

		try
		{
			var yamlText = document[3..closingIndex].Trim();
			var yamlFrontmatter = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, object>>(new StringReader("---\r\n" + yamlText + "\r\n..."));

			frontmatter = yamlFrontmatter;
			remainder = document[(closingIndex + 3)..];

			return true;
		}
		catch
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
	///     <c>true</c> if frontmatter text was found and parsed; <c>false</c> otherwise.
	/// </returns>
	private static bool TryGetJsonFrontmatter(string document, [NotNullWhen(true)] out Dictionary<string, object>? frontmatter, [NotNullWhen(true)] out string? remainder)
	{
		frontmatter = null;
		remainder = null;

		var closingIndex = document.IndexOf(";;;", 3, StringComparison.Ordinal);
		if (closingIndex < 0)
		{
			return false;
		}

		try
		{
			var jsonText = document[3..closingIndex].Trim();
			var jsonFrontmatter = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonText, s_jsonOptions);

			if (jsonFrontmatter is null)
			{
				return false;
			}

			frontmatter = jsonFrontmatter;
			remainder = document[(closingIndex + 3)..];

			return true;
		}
		catch
		{
			return false;
		}
	}
}
