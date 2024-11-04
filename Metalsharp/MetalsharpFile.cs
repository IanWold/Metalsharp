using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Metalsharp;

/// <summary>
///     Represents a file with a virtual directory structure and metadata.
/// </summary>
/// 
/// <example>
///     Create a file called `File.md` in the directory `Directory` with the content `# File Header!`:
///     
///     ```c#
///         new MetalsharpFile("# File Header!", "Directory\\File.md");
///     ```
///     
///     The `Metadata` in this file will be empty. Metadata can be used to store inormation related to the file that doesn't relate to its path or content. This creates the same file, but with a metadata value "draft" = true:
///     
///     ```c#
///         new MetalsharpFile("# File Header!", "Directory\\File.md", new Dictionary&lt;string, object&gt; { ["draft"] = true });
///     ```
/// </example>
/// <remarks>
///     Instantiate a new MetalsharpFile with the specified metadata.
/// </remarks>
/// 
/// <param name="bytes">
///     The contents of the file.
/// </param>
/// <param name="filePath">
///     The virtual path of the file.
/// </param>
/// <param name="metadata">
///     The metadata of the file, stored as a string, object dictionary.
/// </param>
public class MetalsharpFile(byte[] bytes, string filePath, Dictionary<string, object> metadata)
{
	private byte[] _bytes = bytes;
	private string? _text;

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
	/// <param name="bytes">
	///     The contents of the file.
	/// </param>
	/// <param name="filePath">
	///     The virtual path of the file.
	/// </param>
	public MetalsharpFile(byte[] bytes, string filePath) : this(bytes, filePath, []) { }

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
	public MetalsharpFile(string text, string filePath, Dictionary<string, object> metadata) : this(Encoding.Default.GetBytes(text), filePath, metadata) =>
		_text = text;

    #region Properties

    /// <summary>
    ///     The contents of the file.
    /// </summary>
    public byte[] Bytes
	{
		get => _bytes;
		set
		{
			_bytes = value;
			_text = null;
		}
	}

    /// <summary>
    ///     The virtual directory the file sits in.
    /// </summary>
    public string Directory
	{
		get => Path.GetDirectoryName(FilePath) ?? "./";
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
    public string FilePath { get; set; } = filePath;

    /// <summary>
    ///     Metadata from the file.
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = metadata;

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
	public string Text
	{
		get => _text ??= Encoding.Default.GetString(Bytes);
		set
		{
			_bytes = Encoding.Default.GetBytes(value);
			_text = value;
		}
	}

	#endregion

	#region Methods

	/// <summary>
	///     Checks whether a directory is an ancestor of the file.
	/// </summary>
	/// 
	/// <param name="directory">
	///     The directory in question.
	/// </param>
	/// 
	/// <returns>
	///     `true` if the file is a descendant of the directory, `false` otherwise.
	/// </returns>
	public bool IsDescendantOf(string directory) =>
		FilePath.Contains(directory + Path.DirectorySeparatorChar);

	/// <summary>
	///     Checks whether a directory is the parent of the file.
	/// </summary>
	/// 
	/// <param name="directory">
	///     The directory in question.
	/// </param>
	/// 
	/// <returns>
	///     `true` if the file is a child of the directory, `false` otherwise.
	/// </returns>
	public bool IsChildOf(string directory) =>
		FilePath.Contains(Path.Combine(directory, Name + Extension));

	#endregion
}
