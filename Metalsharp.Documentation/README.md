# Metalsharp API Documentation

## BuildOptions

Represents the options when Metalsharp outputs a directory

### Properties

### `ClearOutputDirectory`

Whether Metalsharp should remove all the files in the output directory before writing any

### `OutputDirectory`

The directory to which the files will be output


## IMetalsharpFile

This is the interface for a Metalsharp file.

### Methods

### `IsDescendantOf(String)`

Checks whether a directory is an ancestor of the file.

#### Returns

`true` if the file is a descendant of the directory, `false` otherwise.

### `IsChildOf(String)`

Checks whether a directory is the parent of the file.

#### Returns

`true` if the file is a child of the directory, `false` otherwise.

### Properties

### `Directory`

The directory of in which the file is located. `Directory` will always be equivalent to `Path.GetDirectoryName(this.FilePath)`.

Given a file with path `Path\To\File.md`, `Directory` returns the equivalent of `Path\To\`.

### `Extension`

The extension of the file. `Extension` will always be equal to `Path.GetExtension(this.FilePath)`.

Given a file with path `Path\To\File.md`, `Extension` returns `.md`.

### `FilePath`

The full path of the file. `FilePath` will always be equivalent to `Path.Combine(this.Directory, this.Name + this.Extension)`.

### `Metadata`

The metadata of the file.

### `Name`

The name of the file, without the extension. `Name` will always be equal to `Path.GetFileNameWithoutExtension(this.FilePath)`.

Given a file with path `Path\To\File.md`, `Name` returns `File`.

### `Text`

The text of the file.


## IMetalsharpFileCollection

Represents the interface for a collection of Metalsharp files

### Methods

### `DescendantsOf(String)`

Get the descendant files of a directory

### `ChildrenOf(String)`

Get the children files of a directory

### `ContainsDirectory(String)`

Returns true if one of the files in the collection descends from the directory

### `RemoveAll(Predicate<>)`

Alias List.RemoveAll


## IMetalsharpPlugin

Represents a Metalsharp plugin

### Methods

### `Execute(MetalsharpDirectory)`

Invokes the plugin. Called by Metalsharp.Use

#### Returns

The same directory as was input


## MetalsharpDirectory

This is the root of a Metalsharp project. `MetalsharpDirectory` controls the use of plugins against a project, the files input and output by the project, and the building of the project.

The best example is always the example at the top of the [README](https://github.com/ianwold/metalsharp/):

```c#
new MetalsharpDirectory("Site")
.UseFrontmatter()
.UseDrafts()
.Use(new Markdown())
.Build();
```

Here, `MetalsharpDirectory` is instantiated with a set of files from the on-disk directory `Site`. Then, the plugins `Frontmatter`, `Drafts`, and `Markdown` are invoked against the project. Finally, the project is built with default settings. The intent is that this resulting code is easy to read and easy to understand.

### Constructors

### `MetalsharpDirectory()`

Instantiatea an empty `MetalsharpDirectory`.

### `MetalsharpDirectory(String)`

Instantiates a `MetalsharpDirectory` by reading the files from an on-disk directory into the input files of the project.

### `MetalsharpDirectory(String, String)`

Instantiates a `MetalsharpDirectory` from an on-disk directory. The root directory of each file is rewritten so as to group the files into a different virual path.

Supposing the following on-disk directory structure (where `Project.exe` is the executable of our Metalsharp project):

```plaintext
.
├── Site
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.md
│   ├── Index.md
│   └── About.md
├── Project.exe
└── README.md
```

And then suppose we want our virtual directory (that is, the directory as `MetalsharpDirectory`, and the plugins we use, understand it) to be `Content` instead of `Site`. Instantiating `MetalsharpDirectory` as follows will achieve that:

```c#
new MetalsharpDirectory("Site", "Content") ...
```

Our virutal structure (in the project's input files) will be the following:

```plaintext
Content
├── Posts
│   ├── Post1.md
│   └── Post2.md
├── Index.md
└── About.md
```

This is a virtual structure because files are each stored in a list and not a tree, so the true form of the list will be the following:

- `Content\Index.md`
- `Content\About.md`
- `Content\Posts\Post1.md`
- `Content\Posts\Post2.md`

### Methods

### `AddExisting(String, String, Action<MetalsharpFile>)`

Adds an existing directory or file to the input or output and place the files in a specific virtual path.

This method is called internally by `AddInput` and `AddOutput`.

#### Returns

Returns `this` - the current `MetalsharpDirectory`. This value is passed through `AddInput` and `AddOutput` and allows them to be used as combinators.

### `AddInput(String)`

Adds a file or all the files in a directory to the input. The virtual directory of the files in the input will be the same as that on disk (regardless of whether a relative or absolute path is specified).

```c#
new MetalsharpDirectory()
.AddInput("Path\\To\\Directory") // Add all files in Path\To\Directory to input.
.AddInput("Path\\To\\File.md"); // Add Path\To\File.md to input.
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `AddInput(String, String)`

Add a file or directory to the input and place the files in a specific virtual path.

```c#
new MetalsharpDirectory()
.AddInput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to input in the New\Path directory.
.AddInput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to input. Its path will be New\Path\File.md.
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `AddInput(MetalsharpFile)`

Add a MetalsharpFile to the input files

```c#
new MetalsharpDirectory()
.AddInput(new MetalsharpFile("# File Text", "path\\to\\file.md");
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `AddOutput(String)`

Adds a file or all the files in a directory to the output. The virtual directory of the files in the output will be the same as that on disk (regardless of whether a relative or absolute path is specified).

```c#
new MetalsharpDirectory()
.AddOutput("Path\\To\\Directory") // Add all files in Path\To\Directory to output
.AddOutput("Path\\To\\File.md"); // Add Path\To\File.md to output
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `AddOutput(String, String)`

Add a file or directory to the output and place the files in a specific virtual path.

```c#
new MetalsharpDirectory()
.AddOutput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to the output in the New\Path directory.
.AddOutput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to the output. Its path will be New\Path\File.md.
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `AddOutput(MetalsharpFile)`

Add a MetalsharpFile to the input files

```c#
new MetalsharpDirectory()
.AddInput(new MetalsharpFile("# File Text", "path\\to\\file.md");
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `Build()`

Writes all the output files to the default output directory with default build options.

The following will output a single file (`File.md`) to the current directory:

```c#
new MetalsharpDirectory()
.AddOutput("text", "File.md")
.Build();
```

### `Build(BuildOptions)`

Writes all the output files to the output directory defined in the options.

The following will output a single file (`File.md`) to `OutputDirectory`:

```c#
new MetalsharpDirectory()
.AddOutput("text", "File.md")
.Build(new BuildOptions { OutputDirectory = "OutputDirectory" });
```

If you want to clear all the files in the output directory before the files are written, set `ClearOutputDirectory` to `true`:

```c#
new MetalsharpDirectory()
.AddOutput("text", "File.md")
.Build(new BuildOptions { OutputDirectory = "OutputDirectory", ClearOutputDirectory = true });
```

### `Build(Action<MetalsharpDirectory>)`

Write all the output files to the default output directory with default build options after performing a function.

The following will output a single file (`NewName.md`) to the current directory:

```c#
new MetalsharpDirectory()
.AddOutput("text", "File.md")
.Build(i => i.OutputFiles.First(file => file.Name == "File").Name = "NewName");
```

### `Build(Action<MetalsharpDirectory>, BuildOptions)`

Write all the output files to the output directory defined in the options after performing a function.

The following will output a single file (`NewName.md`) to `OutputDirectory`:

```c#
new MetalsharpDirectory()
.AddOutput("text", "File.md")
.Build(i => i.OutputFiles.First(file => file.Name == "File").Name = "NewName", new BuildOptions { OutputDirectory = "OutputDirectory" });
```

### `Meta(String, Object)`

Add or update a single item of metadata.

The following will add a single item to the metadata, and will then overwrite that value:

```c#
new MetalsharpDirectory()
.Meta("key", "value")
.Meta("key", "new value"); // The new value overwrites the old value.
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `Meta(ValueTuple<String>, Object}[])`

Add or update several items of metadata.

The following will add several items to the metadata:

```c#
new MetalsharpDirectory()
.Meta(("key1", "value1"), ("key2", "value2"), ("key3", "value3"));
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `MoveFiles(String, String)`

Moves files in the input and output from one directory to another.

Suppose we have, for the sake of argument, input *and* output files in the following virtual directory structure:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.md
│   ├── Index.md
│   └── About.md
└── README.md
```

And we want to elevate all the files in `Content` one level in each the input and output. Effectively we need to replace "\\Content" with ".\\". We can do that with `MoveFiles`:

```c#
new MetalsharpDirectory()
... // Populate `InputFiles` with the files
... // Populate `OutputFiles` with the files
.MoveFiles("Content", ".\\");
```

After this, our virtual directory structure will be (in both input and output):

```plaintext
.
├── Posts
│   ├── Post1.md
│   └── Post2.md
├── Index.md
├── About.md
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `MoveFiles(Predicate<IMetalsharpFile>, String)`

Moves files in the input and output matching a predicate from one directory to another.

Suppose we have, for the sake of argument, input *and* output files in the following virtual directory structure:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.html
│   ├── Index.md
│   └── About.html
└── README.md
```

And we want to elevate all the `html` files to the root directory in each the input and output. We use `MoveFiles` to match those files with a predicate and rewrite their directory:

```c#
new MetalsharpDirectory()
... // Populate `InputFiles` with the files
... // Populate `OutputFiles` with the files
.MoveFiles(file => file.Extension == ".html", ".\\");
```

After this, our virtual directory structure will be (in both the input and output):

```plaintext
.
├── Content
│   ├── Posts
│   │   └── Post1.md
│   └── Index.md
├── About.html
├── Post2.html
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `MoveInput(String, String)`

Moves files in the input from one directory to another.

Suppose we have input files in the following virtual directory structure:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.md
│   ├── Index.md
│   └── About.md
└── README.md
```

And we want to elevate all the files in `Content` one level. Effectively we need to replace "\\Content" with ".\\". We can do that with `MoveInput`:

```c#
new MetalsharpDirectory()
... // Populate `InputFiles` with the files
.MoveInput("Content", ".\\");
```

After this, our virtual directory structure will be:

```plaintext
.
├── Posts
│   ├── Post1.md
│   └── Post2.md
├── Index.md
├── About.md
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `MoveInput(Predicate<IMetalsharpFile>, String)`

Moves files in the input matching a predicate from one directory to another.

Suppose we have input files in the following virtual directory structure:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.html
│   ├── Index.md
│   └── About.html
└── README.md
```

And we want to elevate all the `html` files to the root directory. We use `MoveInput` to match those files with a predicate and rewrite their directory:

```c#
new MetalsharpDirectory()
... // Populate `InputFiles` with the files
.MoveInput(file => file.Extension == ".html", ".\\");
```

After this, our virtual directory structure will be:

```plaintext
.
├── Content
│   ├── Posts
│   │   └── Post1.md
│   └── Index.md
├── About.html
├── Post2.html
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `MoveOutput(String, String)`

Moves files in the output from one directory to another.

Suppose we have output files in the following virtual directory structure:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.md
│   ├── Index.md
│   └── About.md
└── README.md
```

And we want to elevate all the files in `Content` one level. Effectively we need to replace "\\Content" with ".\\". We can do that with `MoveOutput`:

```c#
new MetalsharpDirectory()
... // Populate `OutputFiles` with the files
.MoveOutput("Content", ".\\");
```

After this, our virtual directory structure will be:

```plaintext
.
├── Posts
│   ├── Post1.md
│   └── Post2.md
├── Index.md
├── About.md
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `MoveOutput(Predicate<IMetalsharpFile>, String)`

Moves files in the output matching a predicate from one directory to another.

Suppose we have output files in the following virtual directory structure:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.html
│   ├── Index.md
│   └── About.html
└── README.md
```

And we want to elevate all the `html` files to the root directory. We use `MoveOutput` to match those files with a predicate and rewrite their directory:

```c#
new MetalsharpDirectory()
... // Populate `OutputFiles` with the files
.MoveOutput(file => file.Extension == ".html", ".\\");
```

After this, our virtual directory structure will be:

```plaintext
.
├── Content
│   ├── Posts
│   │   └── Post1.md
│   └── Index.md
├── About.html
├── Post2.html
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `RemoveFiles(String)`

Remove a file from each the input and output based on its full path.

Supposing we have `Directory\File.md` in the input and output, we can remove it from both with `RemoveFiles`:

```c#
new MetalsharpDirectory()
... // Add file to input
... // Add file to output
.RemoveFiles("Directory\\File.md");
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `RemoveFiles(Predicate<IMetalsharpFile>)`

Remove all the files matching a predicate from each the input and output.

Supposing we have, for the sake of argument, the following virtual directory structure in the input *and* output:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.html
│   ├── Index.md
│   └── About.html
└── README.md
```

We can remove all the `html` files with `RemoveFiles`:

```c#
new MetalsharpDirectory()
... // Add file to input
... // Add file to output
.RemoveFiles(file => file.Extension == ".html");
```

Our virtual directory structure will then look like the following in the input and output:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   ├── Index.md
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `RemoveInput(String)`

Remove a file from the input based on its full path.

Supposing we have `Directory\File.md` in the input, we can remove it with `RemoveInput`:

```c#
new MetalsharpDirectory()
... // Add file
.RemoveInput("Directory\\File.md");
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `RemoveInput(Predicate<IMetalsharpFile>)`

Remove all the files matching a predicate from the input.

Supposing we have the following virtual directory structure in the input:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.html
│   ├── Index.md
│   └── About.html
└── README.md
```

We can remove all the `html` files with `RemoveInput`:

```c#
new MetalsharpDirectory()
... // Add file
.RemoveInput(file => file.Extension == ".html");
```

Our virtual directory structure will then look like the following:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   ├── Index.md
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `RemoveOutput(String)`

Remove a file from the output based on its full path.

Supposing we have `Directory\File.md` in the output, we can remove it with `RemoveOutput`:

```c#
new MetalsharpDirectory()
... // Add file
.RemoveOutput("Directory\\File.md");
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `RemoveOutput(Predicate<IMetalsharpFile>)`

Remove all the files matching a predicate from the output.

Supposing we have the following virtual directory structure in the output:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   │   └── Post2.html
│   ├── Index.md
│   └── About.html
└── README.md
```

We can remove all the `html` files with `RemoveOutput`:

```c#
new MetalsharpDirectory()
... // Add file
.RemoveOutput(file => file.Extension == ".html");
```

Our virtual directory structure will then look like the following:

```plaintext
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   ├── Index.md
└── README.md
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `Use(Action<MetalsharpDirectory>)`

Invokes a function as a plugin.

```c#
new MetalsharpDirectory()
.Use(dir => dir.Meta("Hello", "World!"));
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `Use(IMetalsharpPlugin)`

Invoke a plugin.

```c#
new MetalsharpDirectory()
.Use(new Debug()); // Invokes the Debug plugin
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### `Use`()`

Invoke a plugin by type. The plugin must have a default (no arguments) constructor.

```c#
new MetalsharpDirectory()
.Use<Debug>(); // Invokes the Debug plugin
```

#### Returns

The current `MetalsharpDirectory`, allowing it to be used as a combinator.

### Properties

### `Metadata`

The directory-level metadata.

### `InputFiles`

The input files of the project.

### `OutputFiles`

The files to output during building.

### Events

### `BeforeUse`

Invoked before `Use()`

### `AfterUse`

Invoked after `Use()`

### `BeforeBuild`

Invoked before `Build()`

### `AfterBuild`

Invoked after `Build()`


## MetalsharpFile

Represents a file

### Constructors

### `MetalsharpFile(String, String)`

Instantiate a new MetalsharpFile with no metadata

### `MetalsharpFile(String, String, Dictionary<String>, Object})`

Instantiate a new MetalsharpFile with the specified metadata

### Methods

### `IsDescendantOf(String)`

Returns true if the directory is an ancestor of the file

### `IsChildOf(String)`

Returns true if the directory is the parent of the file

### Properties

### `Directory`

THe directory of the file relative to the source directory

### `Extension`

The extension from the file name

### `FilePath`

The path of the file

### `Metadata`

Metadata from the file

### `Name`

The name of the file, without the extension

### `Text`

The text of the file


## MetalsharpFileCollection

Represents a collection of Metalsharp files

Implements members to handle "virtual" directories

### Constructors

### `MetalsharpFileCollection()`

Instantiate an empty collection

### `MetalsharpFileCollection(IEnumerable<>)`

Instantiate a collection with an existing one

### Methods

### `DescendantsOf(String)`

Get the descendant files of a directory

### `ChildrenOf(String)`

Get the children files of a directory

### `ContainsDirectory(String)`

Returns true if one of the files in the collection descends from the directory

### Fields

### `_items`

The Metalsharp files in the collection


## IEnumerableExtensions

MetalsharpFileCollection extensions for IEnumerable

### Methods

### `ToMetalsharpFileCollection`(IEnumerable<`>)`

Mimic IEnumerable.ToList


## Branch

The Branch plugin

Branches a directory for separate plugins to be computed

### Constructors

### `Branch(Action<MetalsharpDirectory[]>)`



### Methods

### `Execute(MetalsharpDirectory)`

Invokes the plugin

### Fields

### `_branches`

The function-branches


## Collections

Collections plugin

Groups files matching a predicate into collections in the directory metadata

### Constructors

### `Collections(String, Predicate<IMetalsharpFile>)`

Instantiate the plugin with a single collection definition

### `Collections(ValueTuple<String>, Predicate<IMetalsharpFile[]>)`

Instantiates the plugin with the definitions of the collections

### Methods

### `Execute(MetalsharpDirectory)`

Invokes the plugin

### Fields

### `_definitions`

Contains the definitions of the collections


## Debug

The Debug plugin

Writes a log after every Use, outputting the contents of the input and output directories.

### Constructors

### `Debug()`

By default, write debug logs with Debug.WriteLine()

### `Debug(String)`

Configure Debug to write logs to a log file

### `Debug(Action<String>)`

Configure Debug to use custom behavior when writing logs

### Methods

### `Execute(MetalsharpDirectory)`

Invokes the plugin

### `WriteDirectory(IMetalsharpFileCollection<MetalsharpFile>)`

Prettify the contents of a collection of files

### Fields

### `_onLog`

The action to execute when writing a log

### `_useCount`

A count of the number of calls to .Use() against the directory


## Frontmatter

The Frontmatter plugin

Adds any YAML or JSON frontmatter in the input files to the metadata

### Methods

### `Execute(MetalsharpDirectory)`

Invokes the plugin

### `TryGetFrontmatter(String, Dictionary<String>, Object}@, String@)`

Try to parse YAML or JSON frontmatter

### `TryGetYamlFrontmatter(String, Dictionary<String>, Object}@, String@)`

Try to parse YAML frontmatter

### `TryGetJsonFrontmatter(String, Dictionary<String>, Object}@, String@)`

Try to parse JSON frontmatter


## Markdown

The Markdown plugin

Converts any markdown files to HTML

### Methods

### `Execute(MetalsharpDirectory)`

Invokes the plugin


## MetalsharpExtensions

Extensions to Metalsharp for invoking included plugins

### Methods

### `Branch(MetalsharpDirectory, Action<MetalsharpDirectory[]>)`

Invoke the Branch plugin

### `UseCollections(MetalsharpDirectory, String, Predicate<IMetalsharpFile>)`

Invoke the Collections plugin with a single collection definition

### `UseCollections(MetalsharpDirectory, ValueTuple<String>, Predicate<IMetalsharpFile[]>)`

Invoke the Collections plugin with several collection definitions

### `GetCollection(MetalsharpDirectory, String)`

Get a collection from MetalsharpDirectory Metadata by name

### `GetFilesFromCollection(MetalsharpDirectory, String)`

Get input and output files from a collection by name

### `GetInputCollection(MetalsharpDirectory, String)`

Get the input files from a collection from MetalsharpDirectory Metadata by name

### `GetInputFilesFromCollection(MetalsharpDirectory, String)`

Get the input files from a collection by name

### `GetOutputCollection(MetalsharpDirectory, String)`

Get the output files from a collection from MetalsharpDirectory Metadata by name

### `GetOutputFilesFromCollection(MetalsharpDirectory, String)`

Get the output files from a collection by name

### `UseDebug(MetalsharpDirectory)`

Invoke the default Debug plugin

### `UseDebug(MetalsharpDirectory, String)`

Invoke the Debug plugin with a log file to capture the debug logs

### `UseDebug(MetalsharpDirectory, Action<String>)`

Invoke the Debug plugin with custom log behavior

### `UseFrontmatter(MetalsharpDirectory)`

Invoke the frontmatter plugin

### `UseMarkdown(MetalsharpDirectory)`

Invoke the Merkdown plugin


