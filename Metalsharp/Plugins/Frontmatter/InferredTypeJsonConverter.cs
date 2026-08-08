using System.Text.Json;
using System.Text.Json.Serialization;

namespace Metalsharp;

/// <summary>
///     Deserializes arbitrary JSON values into plain CLR objects (<c>bool</c>, <c>long</c>/<c>double</c>, <c>string</c>,
///     <c>List&lt;object&gt;</c>, <c>Dictionary&lt;string, object&gt;</c>) rather than <c>JsonElement</c>, matching the
///     behavior consumers expect when casting frontmatter metadata values.
/// </summary>
internal class InferredTypeJsonConverter : JsonConverter<object>
{
	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		ReadValue(ref reader);

	private static object? ReadValue(ref Utf8JsonReader reader) =>
		reader.TokenType switch
		{
			JsonTokenType.True => true,
			JsonTokenType.False => false,
			JsonTokenType.Number => reader.TryGetInt64(out var integer) ? integer : reader.GetDouble(),
			JsonTokenType.String => reader.GetString(),
			JsonTokenType.Null => null,
			JsonTokenType.StartArray => ReadArray(ref reader),
			JsonTokenType.StartObject => ReadObject(ref reader),
			_ => throw new JsonException($"Unexpected token {reader.TokenType} while parsing frontmatter.")
		};

	private static List<object?> ReadArray(ref Utf8JsonReader reader)
	{
		var list = new List<object?>();

		while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
		{
			list.Add(ReadValue(ref reader));
		}

		return list;
	}

	private static Dictionary<string, object?> ReadObject(ref Utf8JsonReader reader)
	{
		var dictionary = new Dictionary<string, object?>();

		while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
		{
			var key = reader.GetString()!;
			reader.Read();
			dictionary[key] = ReadValue(ref reader);
		}

		return dictionary;
	}

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
		JsonSerializer.Serialize(writer, value, value.GetType(), options);
}
