# Metalsharp API Documentation

## BuildOptions

Represents the options when Metalsharp outputs a project.

### Properties

### `ClearOutputDirectory`

Whether Metalsharp should remove all the files in the output directory before writing any to that directory.

`false` by default.

### `OutputDirectory`

The directory to which the files will be output.

`.\` by default.


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

Represents the interface for a collection of Metalsharp files.

### Methods

### `DescendantsOf(String)`

Gets the files in the collection which descend from the given virtual directory.

#### Returns

All of the files which descend from the given directory.

### `ChildrenOf(String)`

Gets the files in the collection which are children to the given virtual directory.

#### Returns

All of the files which are children of the given directory.

### `ContainsDirectory(String)`

Checks whether one of the files in the collection descends from the directory.

#### Returns

`true` if the collection contains a file descending from the given directory, `false` otherwise.

### `RemoveAll(Predicate<>)`

Alias `List.RemoveAll`.


## IMetalsharpPlugin

The interface from which Metalsharp plugin must (read: should) derive.

Implementing a Metalsharp plugin is as easy as implementing this interface:

```c#
public class DeleteEverything : IMetalsharpPlugin
{

public void Execute(MetalsharpProject project) =>
project.RemoveFiles(file => true);

}
```

This plugin can then be used like any other:

```c#
new MetalsharpProject()
... // Add files
.Use<DeleteEverything>();
```

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin. `Called by Metalsharp.Use`.


## MetalsharpFile

Represents a file with a virtual directory structure and metadata.

Create a file called `File.md` in the directory `Directory` with the content `# File Header!`:

```c#
new MetalsharpFile("# File Header!", "Directory\\File.md");
```

The `Metadata` in this file will be empty. Metadata can be used to store inormation related to the file that doesn't relate to its path or content. This creates the same file, but with a metadata value "draft" = true:

```c#
new MetalsharpFile("# File Header!", "Directory\\File.md", new Dictionary<string, object> { ["draft"] = true });
```

### Constructors

### `MetalsharpFile(String, String)`

Instantiates a new MetalsharpFile with no metadata.

### `MetalsharpFile(String, String, Dictionary<String>, Object})`

Instantiate a new MetalsharpFile with the specified metadata

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

The virtual directory the file sits in.

### `Extension`

The extension from the file name.

### `FilePath`

The full path of the file.

### `Metadata`

Metadata from the file.

### `Name`

The name of the file, without the extension.

### `Text`

The text of the file.


## MetalsharpFileCollection

Represents a collection of Metalsharp files.

### Constructors

### `MetalsharpFileCollection()`

Instantiate an empty collection.

### `MetalsharpFileCollection(IEnumerable<>)`

Instantiate a collection with an existing one.

### Methods

### `DescendantsOf(String)`

Gets the files in the collection which descend from the given virtual directory.

#### Returns

All of the files which descend from the given directory.

### `ChildrenOf(String)`

Gets the files in the collection which are children to the given virtual directory.

#### Returns

All of the files which are children of the given directory.

### Fields

### `_items`

The Metalsharp files in the collection.


## IEnumerableExtensions

`MetalsharpFileCollection` extensions for `IEnumerable`.

### Methods

### `ToMetalsharpFileCollection(IEnumerable<>)`

Mimic `IEnumerable.ToList`, allowing the easy conversion of an enumerable of files to an `IMetalsharpFileCollection`

#### Returns

An `IMetalsharpFileCollection` containing the files in the given list.


## MetalsharpProject

This is the root of a Metalsharp project. `MetalsharpProject` controls the use of plugins against a project, the files input and output by the project, and the building of the project.

The best example is always the example at the top of the [README](https://github.com/ianwold/metalsharp/):

```c#
new MetalsharpProject("Site")
.UseFrontmatter()
.UseDrafts()
.Use(new Markdown())
.Build();
```

Here, `MetalsharpProject` is instantiated with a set of files from the on-disk directory `Site`. Then, the plugins `Frontmatter`, `Drafts`, and `Markdown` are invoked against the project. Finally, the project is built with default settings. The intent is that this resulting code is easy to read and easy to understand.

### Constructors

### `MetalsharpProject()`

Instantiatea an empty `MetalsharpProject`.

### `MetalsharpProject(String)`

Instantiates a `MetalsharpProject` by reading the files from an on-disk directory into the input files of the project.

### `MetalsharpProject(String, String)`

Instantiates a `MetalsharpProject` from an on-disk directory. The root directory of each file is rewritten so as to group the files into a different virual path.

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

And then suppose we want our virtual directory (that is, the directory as `MetalsharpProject`, and the plugins we use, understand it) to be `Content` instead of `Site`. Instantiating `MetalsharpProject` as follows will achieve that:

```c#
new MetalsharpProject("Site", "Content") ...
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

Returns `this` - the current `MetalsharpProject`. This value is passed through `AddInput` and `AddOutput` and allows them to be fluent.

### `AddInput(String)`

Adds a file or all the files in a directory to the input. The virtual directory of the files in the input will be the same as that on disk (regardless of whether a relative or absolute path is specified).

```c#
new MetalsharpProject()
.AddInput("Path\\To\\Directory") // Add all files in Path\To\Directory to input.
.AddInput("Path\\To\\File.md"); // Add Path\To\File.md to input.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddInput(String, String)`

Add a file or directory to the input and place the files in a specific virtual path.

```c#
new MetalsharpProject()
.AddInput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to input in the New\Path directory.
.AddInput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to input. Its path will be New\Path\File.md.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddInput(MetalsharpFile)`

Add a MetalsharpFile to the input files

```c#
new MetalsharpProject()
.AddInput(new MetalsharpFile("# File Text", "path\\to\\file.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddOutput(String)`

Adds a file or all the files in a directory to the output. The virtual directory of the files in the output will be the same as that on disk (regardless of whether a relative or absolute path is specified).

```c#
new MetalsharpProject()
.AddOutput("Path\\To\\Directory") // Add all files in Path\To\Directory to output
.AddOutput("Path\\To\\File.md"); // Add Path\To\File.md to output
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddOutput(String, String)`

Add a file or directory to the output and place the files in a specific virtual path.

```c#
new MetalsharpProject()
.AddOutput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to the output in the New\Path directory.
.AddOutput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to the output. Its path will be New\Path\File.md.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddOutput(MetalsharpFile)`

Add a MetalsharpFile to the input files

```c#
new MetalsharpProject()
.AddInput(new MetalsharpFile("# File Text", "path\\to\\file.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Build()`

Writes all the output files to the default output directory with default build options.

The following will output a single file (`File.md`) to the current directory:

```c#
new MetalsharpProject()
.AddOutput("text", "File.md")
.Build();
```

### `Build(BuildOptions)`

Writes all the output files to the output directory defined in the options.

The following will output a single file (`File.md`) to `OutputDirectory`:

```c#
new MetalsharpProject()
.AddOutput("text", "File.md")
.Build(new BuildOptions { OutputDirectory = "OutputDirectory" });
```

If you want to clear all the files in the output directory before the files are written, set `ClearOutputDirectory` to `true`:

```c#
new MetalsharpProject()
.AddOutput("text", "File.md")
.Build(new BuildOptions { OutputDirectory = "OutputDirectory", ClearOutputDirectory = true });
```

### `Build(Action<MetalsharpProject>)`

Write all the output files to the default output directory with default build options after performing a function.

The following will output a single file (`NewName.md`) to the current directory:

```c#
new MetalsharpProject()
.AddOutput("text", "File.md")
.Build(i => i.OutputFiles.First(file => file.Name == "File").Name = "NewName");
```

### `Build(Action<MetalsharpProject>, BuildOptions)`

Write all the output files to the output directory defined in the options after performing a function.

The following will output a single file (`NewName.md`) to `OutputDirectory`:

```c#
new MetalsharpProject()
.AddOutput("text", "File.md")
.Build(i => i.OutputFiles.First(file => file.Name == "File").Name = "NewName", new BuildOptions { OutputDirectory = "OutputDirectory" });
```

### `Meta(String, Object)`

Add or update a single item of metadata.

The following will add a single item to the metadata, and will then overwrite that value:

```c#
new MetalsharpProject()
.Meta("key", "value")
.Meta("key", "new value"); // The new value overwrites the old value.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Meta(ValueTuple<String>, Object}[])`

Add or update several items of metadata.

The following will add several items to the metadata:

```c#
new MetalsharpProject()
.Meta(("key1", "value1"), ("key2", "value2"), ("key3", "value3"));
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

### `RemoveFiles(String)`

Remove a file from each the input and output based on its full path.

Supposing we have `Directory\File.md` in the input and output, we can remove it from both with `RemoveFiles`:

```c#
new MetalsharpProject()
... // Add file to input
... // Add file to output
.RemoveFiles("Directory\\File.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

### `RemoveInput(String)`

Remove a file from the input based on its full path.

Supposing we have `Directory\File.md` in the input, we can remove it with `RemoveInput`:

```c#
new MetalsharpProject()
... // Add file
.RemoveInput("Directory\\File.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

### `RemoveOutput(String)`

Remove a file from the output based on its full path.

Supposing we have `Directory\File.md` in the output, we can remove it with `RemoveOutput`:

```c#
new MetalsharpProject()
... // Add file
.RemoveOutput("Directory\\File.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

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
new MetalsharpProject()
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

The current `MetalsharpProject`, allowing it to be fluent.

### `Use(Action<MetalsharpProject>)`

Invokes a function as a plugin.

```c#
new MetalsharpProject()
.Use(dir => dir.Meta("Hello", "World!"));
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Use(IMetalsharpPlugin)`

Invoke a plugin.

```c#
new MetalsharpProject()
.Use(new Debug()); // Invokes the Debug plugin
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Use()`

Invoke a plugin by type. The plugin must have a default (no arguments) constructor.

```c#
new MetalsharpProject()
.Use<Debug>(); // Invokes the Debug plugin
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

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


## Branch

.
The Branch plugin

Creates copies of a `MetalsharpProject` for separate stacks of plugins to be independently invoked on it.

The following will create a file and output it to two different directories by branching the `MetalsharpProject` and calling `Build` on each branch:

```c#
new MetalsharpProject()
.AddOutput(new MetalsharpFile("# Header!", "file.md")
.Branch(
// The first branch:
proj => proj.Build(new BuildOptions { OutputDirectory = "Directory1" }),

// The second branch:
proj => proj.Build(new BuildOptions { OutputDirectory = "Directory2" })
);
```

### Constructors

### `Branch(Action<MetalsharpProject[]>)`

Instantiate the Branch plugin by providing a list of actions to specify the behavior of each branch.

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

### Fields

### `_branches`

The actions of each branch.


## Collections

Collections plugin

Groups files matching a predicate into collections in the directory metadata. Collections are stored in a `Dictionary` matching a string to another inner `Dictionary`, which itself matches a string (either "input" or "output") to an array of strings (which are the full paths of the files in the collection).

Suppose I have the following files on disk:

```plaintext
├── Index.md
├── Post1.md
├── Post2.md
└── About.md
```

And then I create a Metalsharp project, import these into the inputs, and then use the `Markdown` plugin to generate their HTML in the outputs:

```c#
var project = new MetalsharpProject("Path\\To\\My\\Files")
.UseMarkdown();
```

And then say that from here I want to add extra metadata to my posts, but not my `About` or `Index` files. It would be easy to be able to group those files into a collection for easy reference:

```c#
directory.UseCollections("posts", file => file.Name.ToLower().Contains("post"))
```

This will match all the files in the input and output whose names contain the word "post", and will create a collection of them in the metadata of the `MetalsharpProject`. This metadata object, named `collections` will look like the following:

```plaintext
["posts"] =
{
["input"] = { "Post1.md", "Post2.md" },
["output"] = { "Post1.html", "Post2.html" }
}
```

This can be a bit confusing and messy to sort through, so there are extra extension methods supporting retrieving these collections. The following will go through each of the post html files in the output and add some custom metadata to them:

```c#
project.GetOutputFilesFromCollection("posts").ToList().ForEach(post => post.Metadata.Add("author", "Mickey Mouse"));
```

### Constructors

### `Collections(String, Predicate<IMetalsharpFile>)`

Instantiate the plugin with a single collection definition.

### `Collections(ValueTuple<String>, Predicate<IMetalsharpFile[]>)`

Instantiates the plugin with the definitions of the collections.

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

### Fields

### `_definitions`

Contains the definitions of the collections.


## Debug

The Debug plugin.

Writes a log after every Use, outputting the contents of the input and output lists.

`Debug` is best invoked at the beginning of a stack of plugins, so as to capture each of the events related to the project:

```c#
new MetalsharpProject("Path\\To\\Dir")
.Debug()
.Use ... ;
```

### Constructors

### `Debug()`

By default, write debug logs with `Debug.WriteLine()`.

### `Debug(String)`

Instantiate `Debug` with a log file path to output the debug log to a log file.

Given the following Metalsharp project:

```c#
new MetalsharpProject()
.UseDebug("output.log")
.Use(i => i.AddInput(new MetalsharpFile("text", "file.md")));
```

A file called `output.log` will be generated, and will look like the following:

```plaintext
Step 1.
Input files:

file.md

Output files:

---
```

### `Debug(Action<String>)`

Instantiate `Debug` with a custom action to perform each time a log is written. This can be used to output to different sources or execute different debug actions.

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

### `WriteDirectory(IMetalsharpFileCollection<MetalsharpFile>)`

Prettify the contents of a collection of files.

#### Returns

A well-formatted string listing the paths of each file in the given collection.

### Fields

### `_onLog`

The action to execute when writing a log.

### `_useCount`

A count of the number of calls to .Use() against the directory.


## Frontmatter

The Frontmatter plugin.

Adds any YAML or JSON frontmatter in the input files to the metadata.

Given the following `file.txt`:

```plaintext
---
draft: true
---
Hello, World!
```

The assertion in the following will evaluate to `true`:

```c#
var project = new MetalsharpProject("file.txt")
.UseFrontmatter();

Assert.True((bool)project.InputFiles[0].Metadata["draft"])
```

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

### `TryGetFrontmatter(String, Dictionary<String>, Object}@, String@)`

Try to parse YAML or JSON frontmatter

#### Returns

`true` if frontmatter text was found and parsed; `false` otherwise.

### `TryGetYamlFrontmatter(String, Dictionary<String>, Object}@, String@)`

Try to parse YAML frontmatter.

#### Returns

`true` if frontmatter text was found and parsed; `false` otherwise.

### `TryGetJsonFrontmatter(String, Dictionary<String>, Object}@, String@)`

Try to parse JSON frontmatter.

#### Returns

`true` if frontmatter text was found and parsed; `false` otherwise.


## Markdown

The Markdown plugin

Converts any markdown files in the input to HTML with [Markdig](https://github.com/lunet-io/markdig). HTML files are placed in the output.

```c#
new MetalsharpProject()
.AddInput(new MetalsharpFile("# Header 1", "file.md")
.UseMarkdown()
.Build();
```

Will output the file `file.html` to the output directory. The contents of `file.html` will be:

```html
<h1>Header 1</h1>
```

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.


## MetalsharpExtensions

Extensions to Metalsharp for invoking included plugins.

### Methods

### `Branch(MetalsharpProject, Action<MetalsharpProject[]>)`

Invoke the Branch plugin.

Branch the `MetalsharpProject` twice:

```c#
new MetalsharpProject()
// Add files
.Branch(
dir => {
// Do something with branch 1
},
dir => {
// Do something with branch 2
}
);
```

#### Returns

Combinator; returns `this` input.

### `UseCollections(MetalsharpProject, String, Predicate<IMetalsharpFile>)`

Invoke the Collections plugin with a single collection definition.

Only add `.md` files to a collection named `myCollection`:

```c#
new MetalsharpProject()
.UseCollections("myCollection", file => file.Extension == ".md");
```

#### Returns

Combinator; returns `this` input.

### `UseCollections(MetalsharpProject, ValueTuple<String>, Predicate<IMetalsharpFile[]>)`

Invoke the Collections plugin with several collection definitions

Add `.md` files to a collection named `mdFiles` and `.html` files to a collection named `htmlFiles`:

```c#
new MetalsharpProject()
.UseCollections(("mdFiles", file => file.Extension == ".md"), ("htmlFiles", file => file.Extension == ".html"));
```

#### Returns

Combinator; returns `this` input.

### `GetCollection(MetalsharpProject, String)`

Given the name of a collection, returns that collection from the metadata of the `MetalsharpProject`.

```c#
Dictionary<string, string[]> collection = new MetalsharpProject()
... // Add Files
... // Create a collection named "myCollection"
.GetCollection("myCollection");

string[] collectionInputFilesArray = collection["input"];
string[] collectionOutputFilesArray = collection["output"];
```

#### Returns

A `Dictionary` containing the input and output lists of file paths in the collection.

### `GetFilesFromCollection(MetalsharpProject, String)`

Given the name of a collection, returns the input *and* output files in that collection from the metadata of the `MetalsharpProject`.

```c#
IMetalsharpFile[] collectionFiles = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetFilesFromCollection("myCollection").ToArray();
```

#### Returns

An enumerable of `IMetalsharpFile`s from the input and output lists of the collection.

### `GetInputCollection(MetalsharpProject, String)`

Given the name of a collection, returns the input file paths in that collection from the metadata of the `MetalsharpProject`.

```c#
string[] collectionInputFilePaths = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetInputCollection("myCollection");
```

#### Returns

An array containing the list of input file paths in the collection.

### `GetInputFilesFromCollection(MetalsharpProject, String)`

Given the name of a collection, returns the input files in that collection from the metadata of the `MetalsharpProject`.

```c#
IMetalsharpFile[] collectionInputFiles = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetInputFilesFromCollection("myCollection").ToArray();
```

#### Returns

An enumerable containing the files from the input list in the collection.

### `GetOutputCollection(MetalsharpProject, String)`

Given the name of a collection, returns the output file paths in that collection from the metadata of the `MetalsharpProject`.

```c#
string[] collectionoutputFilePaths = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetOutputCollection("myCollection");
```

#### Returns

An array containing the list of output file paths in the collection.

### `GetOutputFilesFromCollection(MetalsharpProject, String)`

Given the name of a collection, returns the output files in that collection from the metadata of the `MetalsharpProject`.

```c#
IMetalsharpFile[] collectionoutputFiles = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetOutputFilesFromCollection("myCollection").ToArray();
```

#### Returns

An enumerable containing the files from the output list in the collection.

### `UseDebug(MetalsharpProject)`

Invoke the default Debug plugin.

```c#
new MetalsharpProject()
.UseDebug();
```

#### Returns

Combinator; returns `this` input.

### `UseDebug(MetalsharpProject, String)`

Invoke the Debug plugin with a log file to capture the debug logs.

```c#
new MetalsharpProject()
.UseDebug("debug.log");
```

#### Returns

Combinator; returns `this` input.

### `UseDebug(MetalsharpProject, Action<String>)`

Invoke the Debug plugin with custom log behavior.

```c#
new MetalsharpProject()
.UseDebug(log => Console.WriteLine(log));
```

#### Returns

Combinator; returns `this` input.

### `UseFrontmatter(MetalsharpProject)`

Invoke the `Frontmatter` plugin.

```c#
new MetalsharpProject()
... // Add files
.UseFrontmatter();
```

#### Returns

Combinator; returns `this` input.

### `UseMarkdown(MetalsharpProject)`

Invoke the `Markdown` plugin.

```c#
new MetalsharpProject()
... // Add files
.UseMarkdown();
```

#### Returns

Combinator; returns `this` input.


