using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Metalsharp;

/// <summary>
///     This is the root of a Metalsharp project. `MetalsharpProject` controls the use of plugins against a project, the files input and output by the project, and the building of the project.
/// </summary>
/// 
/// <example>
///     The best example is always the example at the top of the [README](https://github.com/ianwold/metalsharp/):
///     
///     ```c#
///         new MetalsharpProject("Site")
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
	#region Constructors

	/// <summary>
	///     Instantiatea an empty `MetalsharpProject`.
	/// </summary>
	public MetalsharpProject() { }

	/// <summary>
	///     Instantiates a `MetalsharpProject` by reading the files from an on-disk directory into the input files of the project.
	/// </summary>
	/// 
	/// <param name="path">
	///     The path to the on-disk directory or file to read.
	/// </param>
	public MetalsharpProject(string path) =>
		AddInput(path);

	/// <summary>
	///     Instantiates a `MetalsharpProject` from an on-disk directory. The root directory of each file is rewritten so as to group the files into a different virual path.
	/// </summary>
	/// 
	/// <example>
	///     Supposing the following on-disk directory structure (where `Project.exe` is the executable of our Metalsharp project):
	///     
	///     ```plaintext
	///         .
	///         ├── Site
	///         │   ├── Posts
	///         │   │   ├── Post1.md
	///         │   │   └── Post2.md
	///         │   ├── Index.md
	///         │   └── About.md
	///         ├── Project.exe
	///         └── README.md
	///     ```
	///     
	///     And then suppose we want our virtual directory (that is, the directory as `MetalsharpProject`, and the plugins we use, understand it) to be `Content` instead of `Site`. Instantiating `MetalsharpProject` as follows will achieve that:
	///     
	///     ```c#
	///         new MetalsharpProject("Site", "Content") ...
	///     ```
	///     
	///     Our virutal structure (in the project's input files) will be the following:
	///     
	///     ```plaintext
	///         Content
	///         ├── Posts
	///         │   ├── Post1.md
	///         │   └── Post2.md
	///         ├── Index.md
	///         └── About.md
	///     ```
	///     
	///     This is a virtual structure because files are each stored in a list and not a tree, so the true form of the list will be the following:
	///     
	///     - `Content\Index.md`
	///     - `Content\About.md`
	///     - `Content\Posts\Post1.md`
	///     - `Content\Posts\Post2.md`
	/// </example>
	/// 
	/// <param name="diskPath">
	///     The path to the files on disk to add.
	/// </param>
	/// <param name="virtualPath">
	///     The path of the virtual directory to put the input files in.
	/// </param>
	public MetalsharpProject(string diskPath, string virtualPath) =>
		AddInput(diskPath, virtualPath);

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
	/// 
	/// <returns>
	///     Returns `this` - the current `MetalsharpProject`. This value is passed through `AddInput` and `AddOutput` and allows them to be fluent.
	/// </returns>
	MetalsharpProject AddExisting(string diskPath, string virtualPath, Action<MetalsharpFile> add)
	{
		static MetalsharpFile GetFileWithNormalizedDirectory(string dPath, string vPath) =>
			new(File.ReadAllText(dPath), Path.Combine(vPath, Path.GetFileName(dPath)));

		if (Directory.Exists(diskPath))
		{
			foreach (var file in Directory.GetFiles(diskPath))
			{
				add(GetFileWithNormalizedDirectory(file, virtualPath));
			}

			foreach (var dir in Directory.GetDirectories(diskPath))
			{
				AddExisting(dir, dir.Replace(diskPath, virtualPath), add);
			}

			return this;
		}
		else if (File.Exists(diskPath))
		{
			add(GetFileWithNormalizedDirectory(diskPath, virtualPath));
			return this;
		}
		else
		{
			throw new ArgumentException("File " + diskPath + " does not exist.");
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
		AddExisting(diskPath, virtualPath, InputFiles.Add);

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
		InputFiles.Add(file);
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
		AddExisting(diskPath, virtualPath, OutputFiles.Add);

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
		OutputFiles.Add(file);
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
		Build(new BuildOptions());

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
	public void Build(BuildOptions options)
	{
		var buildOptions = options ?? new BuildOptions();

		if (!Directory.Exists(buildOptions.OutputDirectory))
		{
			Directory.CreateDirectory(buildOptions.OutputDirectory);
		}

		if (buildOptions.ClearOutputDirectory)
		{
			foreach (var file in Directory.GetFiles(buildOptions.OutputDirectory))
			{
				File.Delete(file);
			}
		}

		foreach (var file in OutputFiles)
		{
			var path = Path.Combine(buildOptions.OutputDirectory, file.FilePath);
			var directoryPath = Path.GetDirectoryName(path);

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			File.WriteAllText(path, file.Text);
		}
	}

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
	/// <param name="func">
	///     The function to perform before the files are output.
	/// </param>
	public void Build(Action<MetalsharpProject> func) =>
		Build(func, new BuildOptions());

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
	/// <param name="func">
	///     The function to perform before the files are output.
	/// </param>
	/// <param name="options">
	///     The options to configure the building behavior.
	/// </param>
	public void Build(Action<MetalsharpProject> func, BuildOptions options)
	{
		BeforeBuild?.Invoke(this, new EventArgs());
		func(this);
		AfterBuild?.Invoke(this, new EventArgs());
		Build(options);
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
				Metadata[key] = value;
			}
			else
			{
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
	/// <param name="oldDirectory">
	///     The directory to move the files from.
	/// </param>
	/// <param name="newDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveFiles(string oldDirectory, string newDirectory)
	{
		MoveInput(oldDirectory, newDirectory);
		MoveOutput(oldDirectory, newDirectory);
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
	/// <param name="newDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveFiles(Predicate<IMetalsharpFile> predicate, string newDirectory)
	{
		MoveInput(predicate, newDirectory);
		MoveOutput(predicate, newDirectory);
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
	/// <param name="oldDirectory">
	///     The directory to move the files from.
	/// </param>
	/// <param name="newDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveInput(string oldDirectory, string newDirectory) =>
		MoveInput(file => file.Directory == oldDirectory, newDirectory);

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
	/// <param name="newDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveInput(Predicate<IMetalsharpFile> predicate, string newDirectory)
	{
		InputFiles.Where(i => predicate(i)).ToList().ForEach(i => i.Directory = newDirectory);
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
	/// <param name="oldDirectory">
	///     The directory to move the files from.
	/// </param>
	/// <param name="newDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveOutput(string oldDirectory, string newDirectory) =>
		MoveOutput(file => file.Directory == oldDirectory, newDirectory);

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
	/// <param name="newDirectory">
	///     The directory to move the files to.
	/// </param>
	/// 
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject MoveOutput(Predicate<IMetalsharpFile> predicate, string newDirectory)
	{
		OutputFiles.Where(i => predicate(i)).ToList().ForEach(i => i.Directory = newDirectory);
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
		RemoveInput(file => file.FilePath == path);

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
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveInput(Predicate<IMetalsharpFile> predicate)
	{
		InputFiles.RemoveAll(predicate);
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
		RemoveOutput(file => file.FilePath == path);

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
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject RemoveOutput(Predicate<IMetalsharpFile> predicate)
	{
		OutputFiles.RemoveAll(predicate);
		return this;
	}

	#endregion

	#region Use

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
	/// <returns>
	///     The current `MetalsharpProject`, allowing it to be fluent.
	/// </returns>
	public MetalsharpProject Use(Action<MetalsharpProject> func)
	{
		BeforeUse?.Invoke(this, new EventArgs());
		func(this);
		AfterUse?.Invoke(this, new EventArgs());
		return this;
	}

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
		Use(i => plugin.Execute(i));

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
	///     The directory-level metadata.
	/// </summary>
	public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

	/// <summary>
	///     The input files of the project.
	/// </summary>
	public IMetalsharpFileCollection<MetalsharpFile> InputFiles { get; set; } = new MetalsharpFileCollection<MetalsharpFile>();

	/// <summary>
	///     The files to output during building.
	/// </summary>
	public IMetalsharpFileCollection<MetalsharpFile> OutputFiles { get; set; } = new MetalsharpFileCollection<MetalsharpFile>();

	#endregion
}
