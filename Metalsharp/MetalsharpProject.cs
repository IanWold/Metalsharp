using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Metalsharp.Logging;

namespace Metalsharp;

/// <summary>
///     This is the root of a Metalsharp project. `MetalsharpProject` controls the use of plugins against a project, the files input and output by the project, and the building of the project.
/// </summary>
/// 
/// <example>
///     The best example is always the example at the top of the [README](https://github.com/ianwold/metalsharp/):
///     
///     ```c#
///     new MetalsharpProject("Site")
///         .UseFrontmatter()
///         .UseDrafts()
///         .Use(new Markdown())
///         .Build();
///     ```
///     
///     Here, `MetalsharpProject` is instantiated with a set of files from the on-disk directory `Site`. Then, the plugins `Frontmatter`, `Drafts`, and `Markdown` are invoked against the project. Finally, the project is built with default settings. The intent is that this resulting code is easy to read and easy to understand.
/// </example>
public class MetalsharpProject
{
	private static readonly MetalsharpConfiguration _defaultConfiguration = new()
	{
		LogLevel = LogLevel.Error
	};

	#region Constructors

	/// <summary>
	///		Instantiate a `MetalsharpProject` with the specified configuration options.
	/// </summary>
	/// <param name="configuration">The configuration options for the project.</param>
	public MetalsharpProject(MetalsharpConfiguration configuration)
	{
		Configuration = configuration;
		Log.Info("Initiated Metalsharp");
	}

	/// <summary>
	///		Instantiate a `MetalsharpProject` with the configuration options derived from the build arguments.
	/// </summary>
	/// <param name="args">The build arguments.</param>
	public MetalsharpProject(string[] args)
	{

	}

	/// <summary>
	///     Instantiate a an empty `MetalsharpProject` with the default log level of `Error`
	/// </summary>
	public MetalsharpProject() : this(_defaultConfiguration) { }

	#endregion

	#region Events

	/// <summary>
	///     Invoked before `Use()`
	/// </summary>
	public event EventHandler BeforeUse;

	/// <summary>
	///     Invoked after `Use()`
	/// </summary>
	public event EventHandler AfterUse;

	/// <summary>
	///     Invoked before `Build()`
	/// </summary>
	public event EventHandler BeforeBuild;

	/// <summary>
	///     Invoked after `Build()`
	/// </summary>
	public event EventHandler AfterBuild;

	#endregion

	#region Properties

	/// <summary>
	///		The configuration options for the project.
	/// </summary>
	public MetalsharpConfiguration Configuration { get; init; }

	/// <summary>
	///		The logger.
	/// </summary>
	[Newtonsoft.Json.JsonIgnore]
	public Log Log =>
		_log ??= new Log(Configuration.LogLevel, this);
	private Log _log;

	/// <summary>
	///     The directory-level metadata.
	/// </summary>
	public Dictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>();

	/// <summary>
	///     The input files of the project.
	/// </summary>
	public IMetalsharpFileCollection<MetalsharpFile> InputFiles { get; init; } = new MetalsharpFileCollection<MetalsharpFile>();

	/// <summary>
	///     The files to output during building.
	/// </summary>
	public IMetalsharpFileCollection<MetalsharpFile> OutputFiles { get; init; } = new MetalsharpFileCollection<MetalsharpFile>();

	#endregion

	#region Methods

	#region Add Files

	/// <summary>
	///     Adds an existing directory or file to the input or output and place the files in a specific virtual path.
	///     
	///     This method is called internally by `AddInput` and `AddOutput`.
	/// </summary>
	/// 
	/// <param name="diskPath">
	///     The path to the on-disk file or directory.
	/// </param>
	/// <param name="virtualPath">
	///     The path to the virtual directory to place the files in.
	/// </param>
	/// <param name="add">
	///     The function to perform on each file. The intent is that this function will add the file to `InputFiles` or `OutputFiles`.
	/// </param>
	/// <param name="list">
	///		The list to which the files are being added - "Input" or "Output"
	/// </param>
	/// <param name="recurse">
	///     The level of recursion; used by the logger.
	/// </param>
	/// 
	/// <returns>
	///     Returns `this` - the current `MetalsharpProject`. This value is passed through `AddInput` and `AddOutput` and allows them to be fluent.
	/// </returns>
	MetalsharpProject AddFromFileSystem(string diskPath, string virtualPath, Action<MetalsharpFile> add, string list, int recurse = 0)
	{
		static MetalsharpFile GetFileWithNormalizedDirectory(string dPath, string vPath) =>
			new(File.ReadAllText(dPath), Path.Combine(vPath, Path.GetFileName(dPath)));

		if (Directory.Exists(diskPath))
		{
			Action<string> log =
				recurse > 0
				? Log.Debug
				: Log.Info;

			log($"Adding all files from file system at {diskPath} to {list}");

			foreach (var file in Directory.GetFiles(diskPath))
			{
				Log.Debug($"Adding file from file system: {Path.GetFileName(file)} to {list}");
				add(GetFileWithNormalizedDirectory(file, virtualPath));
				Log.Debug($"    Added to virtual directory {virtualPath}");
			}

			foreach (var directory in Directory.GetDirectories(diskPath))
			{
				AddFromFileSystem(directory, directory.Replace(diskPath, virtualPath), add, list, recurse + 1);
			}

			log($"Finished adding files from file system at {diskPath}");
			return this;
		}
		else if (File.Exists(diskPath))
		{
			Log.Info($"Adding file from file system to {list}: {diskPath}");
			add(GetFileWithNormalizedDirectory(diskPath, virtualPath));
			Log.Debug($"    Added to virtual directory {virtualPath}");

			return this;
		}
		else
		{
			var message = $"Directory or file {diskPath} does not exist in file system.";

			Log.Fatal(message);
			throw new ArgumentException(message, nameof(diskPath));
		}
	}

	/// <summary>
	///     Adds a file or all the files in a directory to the input. The virtual directory of the files in the input will be the same as that on disk (regardless of whether a relative or absolute path is specified).
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .AddInput("Path\\To\\Directory") // Add all files in Path\To\Directory to input.
	///         .AddInput("Path\\To\\File.md"); // Add Path\To\File.md to input.
	///     ```
	/// </example>
	/// 
	/// <param name="path">
	///     The path to the on-disk file or directory.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject AddInput(string path) =>
		AddInput(path, path);

	/// <summary>
	///     Add a file or directory to the input and place the files in a specific virtual path.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .AddInput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to input in the New\Path directory.
	///         .AddInput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to input. Its path will be New\Path\File.md.
	///     ```
	/// </example>
	/// 
	/// <param name="diskPath">
	///     The path to the on-disk file or directory.
	/// </param>
	/// <param name="virtualPath">
	///     The path to the virtual directory to place the files in.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject AddInput(string diskPath, string virtualPath) =>
		AddFromFileSystem(diskPath, virtualPath, InputFiles.Add, "Input");

	/// <summary>
	///     Add a MetalsharpFile to the input files
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .AddInput(new MetalsharpFile("# File Text", "path\\to\\file.md");
	///     ```
	/// </example>
	/// 
	/// <param name="file">
	///     The file to add.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject AddInput(MetalsharpFile file)
	{
		Log.Info($"Adding new virtual file {file.Name} to Input");
		InputFiles.Add(file);
		Log.Debug($"    Added to virtual directory {file.FilePath}");

		return this;
	}

	/// <summary>
	///     Adds a file or all the files in a directory to the output. The virtual directory of the files in the output will be the same as that on disk (regardless of whether a relative or absolute path is specified).
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .AddOutput("Path\\To\\Directory") // Add all files in Path\To\Directory to output
	///         .AddOutput("Path\\To\\File.md"); // Add Path\To\File.md to output
	///     ```
	/// </example>
	/// 
	/// <param name="path">
	///     The path to the on-disk file or directory.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject AddOutput(string path) =>
		AddOutput(path, path);

	/// <summary>
	///     Add a file or directory to the output and place the files in a specific virtual path.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .AddOutput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to the output in the New\Path directory.
	///         .AddOutput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to the output. Its path will be New\Path\File.md.
	///     ```
	/// </example>
	/// 
	/// <param name="diskPath">
	///     The path to the on-disk file or directory.
	/// </param>
	/// <param name="virtualPath">
	///     The path to the virtual directory to place the files in.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject AddOutput(string diskPath, string virtualPath) =>
		AddFromFileSystem(diskPath, virtualPath, OutputFiles.Add, "Output");

	/// <summary>
	///     Add a MetalsharpFile to the input files
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .AddInput(new MetalsharpFile("# File Text", "path\\to\\file.md");
	///     ```
	/// </example>
	/// 
	/// <param name="file">
	///     The file to add.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject AddOutput(MetalsharpFile file)
	{
		Log.Info($"Adding new virtual file {file.Name} to Output");
		OutputFiles.Add(file);
		Log.Debug($"    Added to virtual directory {file.FilePath}");

		return this;
	}

	#endregion

	#region Build

	/// <summary>
	///     Writes all the output files to the default output directory with default build options.
	/// </summary>
	/// 
	/// <example>
	///     The following will output a single file (`File.md`) to the current directory:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .AddOutput("text", "File.md")
	///         .Build();
	///     ```
	/// </example>
	public void Build() =>
		Build(null, new BuildOptions());

	/// <summary>
	///     Write all the output files to the default output directory with default build options after performing a function.
	/// </summary>
	/// 
	/// <example>
	///     The following will output a single file (`NewName.md`) to the current directory:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .AddOutput("text", "File.md")
	///         .Build(i => i.OutputFiles.First(file => file.Name == "File").Name = "NewName");
	///     ```
	/// </example>
	/// 
	/// <param name="prebuild">
	///     The function to perform before the files are output.
	/// </param>
	public void Build(Action<MetalsharpProject> prebuild) =>
		Build(prebuild, new BuildOptions());

	/// <summary>
	///     Writes all the output files to the output directory defined in the options.
	/// </summary>
	/// 
	/// <example>
	///     The following will output a single file (`File.md`) to `OutputDirectory`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .AddOutput("text", "File.md")
	///         .Build(new BuildOptions { OutputDirectory = "OutputDirectory" });
	///     ```
	///     
	///     If you want to clear all the files in the output directory before the files are written, set `ClearOutputDirectory` to `true`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .AddOutput("text", "File.md")
	///         .Build(new BuildOptions { OutputDirectory = "OutputDirectory", ClearOutputDirectory = true });
	///     ```
	/// </example>
	/// 
	/// <param name="options">
	///     The options to configure the building behavior.
	/// </param>
	public void Build(BuildOptions options) =>
		Build(null, options);

	/// <summary>
	///     Write all the output files to the output directory defined in the options after performing a function.
	/// </summary>
	/// 
	/// <example>
	///     The following will output a single file (`NewName.md`) to `OutputDirectory`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .AddOutput("text", "File.md")
	///         .Build(i => i.OutputFiles.First(file => file.Name == "File").Name = "NewName", new BuildOptions { OutputDirectory = "OutputDirectory" });
	///     ```
	/// </example>
	/// 
	/// <param name="prebuild">
	///     The function to perform before the files are output.
	/// </param>
	/// <param name="options">
	///     The options to configure the building behavior.
	/// </param>
	public void Build(Action<MetalsharpProject> prebuild, BuildOptions options)
	{
		Log.Debug("Invoking BeforeBuild");
		BeforeBuild?.Invoke(this, new EventArgs());

		if (prebuild is not null)
		{
			Log.Debug("Invoking PreBuild");
			prebuild(this);
		}
		else
		{
			Log.Debug("Skipped PreBuild");
		}

		Log.Info("\nBeginning Build");

		var buildOptions = options ?? new BuildOptions();

		if (!Directory.Exists(buildOptions.OutputDirectory))
		{
			Log.Debug($"Creating output directory {buildOptions.OutputDirectory}");
			Directory.CreateDirectory(buildOptions.OutputDirectory);
		}

		if (buildOptions.ClearOutputDirectory)
		{
			Log.Debug("Cleaning output directory");
			foreach (var file in Directory.GetFiles(buildOptions.OutputDirectory))
			{
				File.Delete(file);
			}
		}

		Log.Info($"\nWriting files to {buildOptions.OutputDirectory}");

		foreach (var file in OutputFiles)
		{
			var path = Path.Combine(buildOptions.OutputDirectory, file.FilePath);
			var directoryPath = Path.GetDirectoryName(path);

			if (!Directory.Exists(directoryPath))
			{
				Log.Debug($"Creating directory {directoryPath}");
				Directory.CreateDirectory(directoryPath);
			}

			Log.Debug($"Wrting file {path}");
			File.WriteAllText(path, file.Text);
		}

		Log.Info("\nFinalizing Build");
		Log.Debug("Invoking AfterBuild");
		AfterBuild?.Invoke(this, new EventArgs());

		Log.Info("Finished Build");
	}

	#endregion

	#region Meta

	/// <summary>
	///     Add or update a single item of metadata.
	/// </summary>
	/// 
	/// <example>
	///     The following will add a single item to the metadata, and will then overwrite that value:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .Meta("key", "value")
	///         .Meta("key", "new value"); // The new value overwrites the old value.
	///     ```
	/// </example>
	/// 
	/// <param name="key">
	///     The key to add or update.
	/// </param>
	/// <param name="value">
	///     The value to store with the key.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject Meta(string key, object value) =>
		Meta((key, value));

	/// <summary>
	///     Add or update several items of metadata.
	/// </summary>
	/// 
	/// <example>
	///     The following will add several items to the metadata:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         .Meta(("key1", "value1"), ("key2", "value2"), ("key3", "value3"));
	///     ```
	/// </example>
	/// 
	/// <param name="pairs">
	///     The key-value pairs to add or update.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject Meta(params (string key, object value)[] pairs)
	{
		foreach (var (key, value) in pairs)
		{
			if (Metadata.ContainsKey(key))
			{
				Log.Debug($"Updating metadata [{key}] = {value}");
				Metadata[key] = value;
			}
			else
			{
				Log.Debug($"Adding metadata [{key}] = {value}");
				Metadata.Add(key, value);
			}
		}

		return this;
	}

	#endregion

	#region Move Files

	/// <summary>
	///     Moves files in the input and output from one directory to another.
	/// </summary>
	/// 
	/// <example>
	///     Suppose we have, for the sake of argument, input *and* output files in the following virtual directory structure:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.md
	///         │   ├── Index.md
	///         │   └── About.md
	///         └── README.md
	///     ```
	///     
	///     And we want to elevate all the files in `Content` one level in each the input and output. Effectively we need to replace "\\Content" with ".\\". We can do that with `MoveFiles`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Populate `InputFiles` with the files
	///         ... // Populate `OutputFiles` with the files
	///         .MoveFiles("Content", ".\\");
	///     ```
	///     
	///     After this, our virtual directory structure will be (in both input and output):
	///     
	///     ```plaintext
	///         .
	///         ├── Posts
	///         │   ├── Post1.md
	///         │   └── Post2.md
	///         ├── Index.md
	///         ├── About.md
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="fromDirectory">
	///     The directory to move the files from.
	/// </param>
	/// <param name="toDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveFiles(string fromDirectory, string toDirectory)
	{
		MoveInput(fromDirectory, toDirectory);
		MoveOutput(fromDirectory, toDirectory);
		return this;
	}

	/// <summary>
	///     Moves files in the input and output matching a predicate from one directory to another.
	/// </summary>
	/// 
	/// <example>
	///     Suppose we have, for the sake of argument, input *and* output files in the following virtual directory structure:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.html
	///         │   ├── Index.md
	///         │   └── About.html
	///         └── README.md
	///     ```
	///     
	///     And we want to elevate all the `html` files to the root directory in each the input and output. We use `MoveFiles` to match those files with a predicate and rewrite their directory:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Populate `InputFiles` with the files
	///         ... // Populate `OutputFiles` with the files
	///         .MoveFiles(file => file.Extension == ".html", ".\\");
	///     ```
	///     
	///     After this, our virtual directory structure will be (in both the input and output):
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   └── Post1.md
	///         │   └── Index.md
	///         ├── About.html
	///         ├── Post2.html
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="predicate">
	///     The predicate to match the files to move.
	/// </param>
	/// <param name="toDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveFiles(Predicate<IMetalsharpFile> predicate, string toDirectory)
	{
		MoveInput(predicate, toDirectory);
		MoveOutput(predicate, toDirectory);
		return this;
	}

	/// <summary>
	///     Moves files in the input from one directory to another.
	/// </summary>
	/// 
	/// <example>
	///     Suppose we have input files in the following virtual directory structure:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.md
	///         │   ├── Index.md
	///         │   └── About.md
	///         └── README.md
	///     ```
	///     
	///     And we want to elevate all the files in `Content` one level. Effectively we need to replace "\\Content" with ".\\". We can do that with `MoveInput`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Populate `InputFiles` with the files
	///         .MoveInput("Content", ".\\");
	///     ```
	///     
	///     After this, our virtual directory structure will be:
	///     
	///     ```plaintext
	///         .
	///         ├── Posts
	///         │   ├── Post1.md
	///         │   └── Post2.md
	///         ├── Index.md
	///         ├── About.md
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="fromDirectory">
	///     The directory to move the files from.
	/// </param>
	/// <param name="toDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveInput(string fromDirectory, string toDirectory) =>
		MoveInput(file => file.Directory == fromDirectory, toDirectory, $"Matching path {fromDirectory}");

	/// <summary>
	///     Moves files in the input matching a predicate from one directory to another.
	/// </summary>
	/// 
	/// <example>
	///     Suppose we have input files in the following virtual directory structure:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.html
	///         │   ├── Index.md
	///         │   └── About.html
	///         └── README.md
	///     ```
	///     
	///     And we want to elevate all the `html` files to the root directory. We use `MoveInput` to match those files with a predicate and rewrite their directory:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Populate `InputFiles` with the files
	///         .MoveInput(file => file.Extension == ".html", ".\\");
	///     ```
	///     
	///     After this, our virtual directory structure will be:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   └── Post1.md
	///         │   └── Index.md
	///         ├── About.html
	///         ├── Post2.html
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="predicate">
	///     The predicate to match the files to move.
	/// </param>
	/// 
	/// <param name="toDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <param name="logMessage">
	///		The message to log indicating which files are being moved.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveInput(Predicate<IMetalsharpFile> predicate, string toDirectory, string logMessage = null)
	{
		Log.Info($"Removing files in Input to {toDirectory} from{(logMessage is not null ? $": {logMessage}" : "")}");

		foreach (var file in InputFiles.Where(i => predicate(i)))
		{
			Log.Debug($"    Moving file: {file.FilePath}");
			file.Directory = toDirectory;
		}

		return this;
	}

	/// <summary>
	///     Moves files in the output from one directory to another.
	/// </summary>
	/// 
	/// <example>
	///     Suppose we have output files in the following virtual directory structure:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.md
	///         │   ├── Index.md
	///         │   └── About.md
	///         └── README.md
	///     ```
	///     
	///     And we want to elevate all the files in `Content` one level. Effectively we need to replace "\\Content" with ".\\". We can do that with `MoveOutput`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Populate `OutputFiles` with the files
	///         .MoveOutput("Content", ".\\");
	///     ```
	///     
	///     After this, our virtual directory structure will be:
	///     
	///     ```plaintext
	///         .
	///         ├── Posts
	///         │   ├── Post1.md
	///         │   └── Post2.md
	///         ├── Index.md
	///         ├── About.md
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="fromDirectory">
	///     The directory to move the files from.
	/// </param>
	/// <param name="toDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveOutput(string fromDirectory, string toDirectory) =>
		MoveOutput(file => file.Directory == fromDirectory, toDirectory, $"Matching path {fromDirectory}");

	/// <summary>
	///     Moves files in the output matching a predicate from one directory to another.
	/// </summary>
	/// 
	/// <example>
	///     Suppose we have output files in the following virtual directory structure:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.html
	///         │   ├── Index.md
	///         │   └── About.html
	///         └── README.md
	///     ```
	///     
	///     And we want to elevate all the `html` files to the root directory. We use `MoveOutput` to match those files with a predicate and rewrite their directory:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Populate `OutputFiles` with the files
	///         .MoveOutput(file => file.Extension == ".html", ".\\");
	///     ```
	///     
	///     After this, our virtual directory structure will be:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   └── Post1.md
	///         │   └── Index.md
	///         ├── About.html
	///         ├── Post2.html
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="predicate">
	///     The predicate to match the files to move.
	/// </param>
	/// 
	/// <param name="toDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <param name="logMessage">
	///		The message to log indicating which files are being moved.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveOutput(Predicate<IMetalsharpFile> predicate, string toDirectory, string logMessage = null)
	{
		Log.Info($"Removing files in Output to {toDirectory} from{(logMessage is not null ? $": {logMessage}" : "")}");

		foreach (var file in OutputFiles.Where(i => predicate(i)))
		{
			Log.Debug($"    Moving file: {file.FilePath}");
			file.Directory = toDirectory;
		}

		return this;
	}

	#endregion

	#region Remove Files

	/// <summary>
	///     Remove a file from each the input and output based on its full path.
	/// </summary>
	/// 
	/// <example>
	///     Supposing we have `Directory\File.md` in the input and output, we can remove it from both with `RemoveFiles`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add file to input
	///         ... // Add file to output
	///         .RemoveFiles("Directory\\File.md");
	///     ```
	/// </example>
	/// 
	/// <param name="path">
	///     The path of the file to remove.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveFiles(string path)
	{
		RemoveInput(path);
		RemoveOutput(path);
		return this;
	}

	/// <summary>
	///     Remove all the files matching a predicate from each the input and output.
	/// </summary>
	/// 
	/// <example>
	///     Supposing we have, for the sake of argument, the following virtual directory structure in the input *and* output:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.html
	///         │   ├── Index.md
	///         │   └── About.html
	///         └── README.md
	///     ```
	///     
	///     We can remove all the `html` files with `RemoveFiles`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add file to input
	///         ... // Add file to output
	///         .RemoveFiles(file => file.Extension == ".html");
	///     ```
	///     
	///     Our virtual directory structure will then look like the following in the input and output:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   ├── Index.md
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="predicate">
	///     The predicate function to identify files to delete.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveFiles(Predicate<IMetalsharpFile> predicate)
	{
		RemoveInput(predicate);
		RemoveOutput(predicate);
		return this;
	}

	/// <summary>
	///     Remove a file from the input based on its full path.
	/// </summary>
	/// 
	/// <example>
	///     Supposing we have `Directory\File.md` in the input, we can remove it with `RemoveInput`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add file
	///         .RemoveInput("Directory\\File.md");
	///     ```
	/// </example>
	/// 
	/// <param name="path">
	///     The path of the file to remove.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveInput(string path) =>
		RemoveInput(file => file.FilePath.StartsWith(path), $"Matching path {path}");

	/// <summary>
	///     Remove all the files matching a predicate from the input.
	/// </summary>
	/// 
	/// <example>
	///     Supposing we have the following virtual directory structure in the input:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.html
	///         │   ├── Index.md
	///         │   └── About.html
	///         └── README.md
	///     ```
	///     
	///     We can remove all the `html` files with `RemoveInput`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add file
	///         .RemoveInput(file => file.Extension == ".html");
	///     ```
	///     
	///     Our virtual directory structure will then look like the following:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   ├── Index.md
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="predicate">
	///     The predicate function to identify files to delete.
	/// </param>
	/// 
	/// <param name="logMessage">
	///		The message to log indicating which files are being removed.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveInput(Predicate<IMetalsharpFile> predicate, string logMessage = null)
	{
		Log.Info($"Removing files from Input{(logMessage is not null ? $": {logMessage}" : "")}");

		if (Configuration.LogLevel > LogLevel.Debug)
		{
			InputFiles.RemoveAll(predicate);
		}
		else
		{
			foreach (var file in InputFiles.Where(f => predicate(f as IMetalsharpFile)))
			{
				Log.Debug($"    Removing file: {file.FilePath}");
				InputFiles.Remove(file);
			}
		}

		return this;
	}

	/// <summary>
	///     Remove a file from the output based on its full path.
	/// </summary>
	/// 
	/// <example>
	///     Supposing we have `Directory\File.md` in the output, we can remove it with `RemoveOutput`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add file
	///         .RemoveOutput("Directory\\File.md");
	///     ```
	/// </example>
	/// 
	/// <param name="path">
	///     The path of the file to remove.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveOutput(string path) =>
		RemoveOutput(file => file.FilePath.StartsWith(path), $"Matching path {path}");

	/// <summary>
	///     Remove all the files matching a predicate from the output.
	/// </summary>
	/// 
	/// <example>
	///     Supposing we have the following virtual directory structure in the output:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.html
	///         │   ├── Index.md
	///         │   └── About.html
	///         └── README.md
	///     ```
	///     
	///     We can remove all the `html` files with `RemoveOutput`:
	///     
	///     ```c#
	///         new MetalsharpProject()
	///         ... // Add file
	///         .RemoveOutput(file => file.Extension == ".html");
	///     ```
	///     
	///     Our virtual directory structure will then look like the following:
	///     
	///     ```plaintext
	///         .
	///         ├── Content
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   ├── Index.md
	///         └── README.md
	///     ```
	/// </example>
	/// 
	/// <param name="predicate">
	///     The predicate function to identify files to delete.
	/// </param>
	/// 
	/// <param name="logMessage">
	///		The message to log indicating which files are being removed.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveOutput(Predicate<IMetalsharpFile> predicate, string logMessage = null)
	{
		Log.Info($"Removing files from Output{(logMessage is not null ? $": {logMessage}" : "")}");

		if (Configuration.LogLevel > LogLevel.Debug)
		{
			OutputFiles.RemoveAll(predicate);
		}
		else
		{
			foreach (var file in InputFiles.Where(f => predicate(f as IMetalsharpFile)))
			{
				Log.Debug($"    Removing file: {file.FilePath}");
				OutputFiles.Remove(file);
			}
		}

		return this;
	}

	#endregion

	#region Use

	private MetalsharpProject Use(Action<MetalsharpProject> func, string transformKind, string transformName)
	{

		Log.Info($"\nAbout to use {transformKind} {transformName}\n");
		BeforeUse?.Invoke(this, new EventArgs());

		Log.Info($"\nUsing {transformKind} {transformName}\n");
		func(this);

		Log.Info($"\nFinishing using {transformKind} {transformName}\n");
		AfterUse?.Invoke(this, new EventArgs());

		Log.Info($"\nFinished using {transformKind} {transformName}\n");
		return this;
	}

	/// <summary>
	///     Invokes a function as a plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .Use(dir => dir.Meta("Hello", "World!"));
	///     ```
	/// </example>
	/// 
	/// <param name="func">
	///     The function to invoke.
	/// </param>
	/// 
	/// <param name="functionName">
	///     Optionally, the name of the function to log.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject Use(Action<MetalsharpProject> func, string functionName = null) =>
		Use(func, "function", functionName ?? "<anonymous>");

	/// <summary>
	///     Invoke a plugin.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .Use(new Debug()); // Invokes the Debug plugin
	///     ```
	/// </example>
	/// 
	/// <param name="plugin">
	///     The instance of the plugin to invoke.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject Use(IMetalsharpPlugin plugin) =>
		Use(i => plugin.Execute(i), "plugin", plugin.GetType().Name);

	/// <summary>
	///     Invoke a plugin by type. The plugin must have a default (no arguments) constructor.
	/// </summary>
	/// 
	/// <example>
	///     ```c#
	///         new MetalsharpProject()
	///         .Use&lt;Debug&gt;(); // Invokes the Debug plugin
	///     ```
	/// </example>
	/// 
	/// <typeparam name="T">
	///     The type of the plugin to invoke.
	/// </typeparam>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject Use<T>() where T : IMetalsharpPlugin, new() =>
		Use(new T());

	#endregion

	#endregion
}
