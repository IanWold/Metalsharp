using System.Text;

namespace Metalsharp;

/// <summary>
///     Represents a file with a virtual directory structure and metadata.
/// </summary>
/// 
/// <example>
///     Create a file called <c>File.md</c> in the directory <c>Directory</c> with the content <c># File Header!</c>:
///     
///     <code>
///         new MetalsharpFile("# File Header!", "Directory\\File.md");
///     </code>
///     
///     The <c>Metadata</c> in this file will be empty. Metadata can be used to store inormation related to the file that doesn't relate to its path or content. This creates the same file, but with a metadata value "draft" = true:
///     
///     <code>
///         new MetalsharpFile("# File Header!", "Directory\\File.md", new Dictionary&lt;string, object&gt; { ["draft"] = true });
///     </code>
/// </example>
public class MetalsharpFile
{
	/// <summary>
	///     Instantiates a new MetalsharpFile with no metadata.
	/// </summary>
	/// 
	/// <param name="text">
	///     The text of the file.
	/// </param>
	/// <param name="filePath">
	///     The virtual path of the file.
	/// </param>
	public MetalsharpFile(string text, string filePath) : this(text, filePath, []) { }

	/// <summary>
	///     Instantiates a new MetalsharpFile with no metadata.
	/// </summary>
	/// 
	/// <param name="contents">
	///     The contents of the file.
	/// </param>
	/// <param name="filePath">
	///     The virtual path of the file.
	/// </param>
	public MetalsharpFile(byte[] contents, string filePath) : this(contents, filePath, []) { }

	/// <summary>
	///     Instantiate a new MetalsharpFile with the specified metadata.
	/// </summary>
	/// 
	/// <param name="text">
	///     The text of the file.
	/// </param>
	/// <param name="filePath">
	///     The virtual path of the file.
	/// </param>
	/// <param name="metadata">
	///     The metadata of the file, stored as a string, object dictionary.
	/// </param>
	public MetalsharpFile(string text, string filePath, Dictionary<string, object> metadata) : this(Encoding.Default.GetBytes(text), filePath, metadata) { }

	/// <summary>
	///     Instantiate a new MetalsharpFile with the specified metadata.
	/// </summary>
	/// 
	/// <param name="contents">
	///     The contents of the file.
	/// </param>
	/// <param name="filePath">
	///     The virtual path of the file.
	/// </param>
	/// <param name="metadata">
	///     The metadata of the file, stored as a string, object dictionary.
	/// </param>
	public MetalsharpFile(byte[] contents, string filePath, Dictionary<string, object> metadata)
	{
		_contents = contents;
		Metadata = metadata;
		FilePath = filePath;
	}

	/// <summary>
	///     Instantiate a new MetalsharpFile whose contents are read from disk lazily, on first access,
	///     rather than immediately. Used internally by <see cref="MetalsharpProject.AddInput(string)"/>
	///     and <see cref="MetalsharpProject.AddOutput(string)"/> so that files copied straight through
	///     without ever being inspected or modified by a plugin never need to be loaded into memory at all.
	/// </summary>
	///
	/// <param name="diskFile">
	///     The FileInfo representing the file on disk.
	/// </param>
	/// <param name="filePath">
	///     The virtual path of the file.
	/// </param>
	internal MetalsharpFile(FileInfo diskFile, string filePath)
	{
		_sourcePath = diskFile.FullName;
		FilePath = filePath;
	}

	#region Properties

	private byte[]? _contents;
	private string? _sourcePath;
	private string? _text;

	/// <summary>
	///     Whether this file's contents are still guaranteed identical to the file at <see cref="_sourcePath"/> on disk.
	/// </summary>
	internal string? UnmodifiedSourcePath =>
		_sourcePath;

	/// <summary>
	///     The contents of the file.
	/// </summary>
	public byte[] Contents
	{
		get =>
			_contents ??= File.ReadAllBytes(_sourcePath!);
		set
		{
			_contents = value;
			_sourcePath = null;
			_text = null;
		}
	}

	/// <summary>
	///     The virtual directory the file sits in.
	/// </summary>
	public string Directory
	{
		get => Path.GetDirectoryName(FilePath) ?? string.Empty;
		set => FilePath = Path.Combine(value, Name + Extension);
	}

	/// <summary>
	///     The extension from the file name.
	/// </summary>
	public string Extension
	{
		get => Path.GetExtension(FilePath);
		set => FilePath = Path.Combine(Directory, Name + value);
	}

	/// <summary>
	///     The full path of the file.
	/// </summary>
	public string FilePath { get; set; }

	/// <summary>
	///     Metadata from the file.
	/// </summary>
	public Dictionary<string, object> Metadata { get; set; } = [];

	/// <summary>
	///     The name of the file, without the extension.
	/// </summary>
	public string Name
	{
		get => Path.GetFileNameWithoutExtension(FilePath);
		set => FilePath = Path.Combine(Directory, value + Extension);
	}

	/// <summary>
	///     The contents of the file as a string.
	/// </summary>
	public string Text =>
		_text ??= Encoding.Default.GetString(Contents);

	#endregion

	#region Methods

	/// <summary>
	///     Checks whether a directory is an ancestor of the file, i.e. whether <paramref name="directory"/>'s path
	///     segments appear as a contiguous, aligned run anywhere in the file's path.
	/// </summary>
	///
	/// <param name="directory">
	///     The directory in question.
	/// </param>
	/// <param name="comparisonType">
	///     The kind of string comparison to use when comparing path segments.
	///
	///     <see cref="StringComparison.OrdinalIgnoreCase"/> by default.
	/// </param>
	///
	/// <returns>
	///     <c>true</c> if the file is a descendant of the directory, <c>false</c> otherwise.
	/// </returns>
	public bool IsDescendantOf(string directory, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		var fileSegments = SplitSegments(FilePath);
		var directorySegments = SplitSegments(directory);
		var comparer = StringComparer.FromComparison(comparisonType);

		for (var start = 0; start <= fileSegments.Length - directorySegments.Length - 1; start++)
		{
			if (fileSegments.Skip(start).Take(directorySegments.Length).SequenceEqual(directorySegments, comparer))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	///     Checks whether a directory is the immediate parent of the file, i.e. whether <paramref name="directory"/>'s
	///     path segments exactly match the trailing segments of the file's own directory.
	/// </summary>
	///
	/// <param name="directory">
	///     The directory in question.
	/// </param>
	/// <param name="comparisonType">
	///     The kind of string comparison to use when comparing path segments.
	///
	///     <see cref="StringComparison.OrdinalIgnoreCase"/> by default.
	/// </param>
	///
	/// <returns>
	///     <c>true</c> if the file is a child of the directory, <c>false</c> otherwise.
	/// </returns>
	public bool IsChildOf(string directory, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
	{
		var fileDirectorySegments = SplitSegments(Directory);
		var directorySegments = SplitSegments(directory);
		var comparer = StringComparer.FromComparison(comparisonType);

		return directorySegments.Length <= fileDirectorySegments.Length
			&& fileDirectorySegments.Skip(fileDirectorySegments.Length - directorySegments.Length).SequenceEqual(directorySegments, comparer);
	}

	/// <summary>
	///     Splits a virtual path into its individual directory/file segments.
	/// </summary>
	///
	/// <param name="path">
	///     The path to split.
	/// </param>
	///
	/// <returns>
	///     The non-empty segments of the path.
	/// </returns>
	private static string[] SplitSegments(string path) =>
		path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

	#endregion
}
