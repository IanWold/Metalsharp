# Metalsharp API Documentation

This file is generated from the XML documentation comments in the Metalsharp source. If you notice an inaccuracy here, please fix the corresponding XML comment in the source and regenerate this file, rather than editing it directly.

## Collections

Collections plugin

Groups files matching a predicate into collections in the directory metadata. Collections are stored in a `Dictionary` matching a string to another inner `Dictionary`, which itself matches a string (either "input" or "output") to an array of strings (which are the full paths of the files in the collection).

- `definitions`: The definitions of the collections, including the name of the collection and the predicate which matches its files.

Suppose I have the following files on disk:

```c#
├── Index.md
├── Post1.md
├── Post2.md
└── About.md
```

And then I create a Metalsharp project, import these into the inputs, and then use the `Markdown` plugin to generate their HTML in the outputs:

```c#
var project = new MetalsharpProject()
    .AddInput("Path\\To\\My\\Files")
    .UseMarkdown();
```

And then say that from here I want to add extra metadata to my posts, but not my `About` or `Index` files. It would be easy to be able to group those files into a collection for easy reference:

```c#
project.UseCollections("posts", file => file.Name.ToLower().Contains("post"))
```

This will match all the files in the input and output whose names contain the word "post", and will create a collection of them in the metadata of the `MetalsharpProject`. This metadata object, named `collections` will look like the following:

```c#
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

### `Collections(ValueTuple<String, Predicate<MetalsharpFile>>[])`

Collections plugin

Groups files matching a predicate into collections in the directory metadata. Collections are stored in a `Dictionary` matching a string to another inner `Dictionary`, which itself matches a string (either "input" or "output") to an array of strings (which are the full paths of the files in the collection).

- `definitions`: The definitions of the collections, including the name of the collection and the predicate which matches its files.

Suppose I have the following files on disk:

```c#
├── Index.md
├── Post1.md
├── Post2.md
└── About.md
```

And then I create a Metalsharp project, import these into the inputs, and then use the `Markdown` plugin to generate their HTML in the outputs:

```c#
var project = new MetalsharpProject()
    .AddInput("Path\\To\\My\\Files")
    .UseMarkdown();
```

And then say that from here I want to add extra metadata to my posts, but not my `About` or `Index` files. It would be easy to be able to group those files into a collection for easy reference:

```c#
project.UseCollections("posts", file => file.Name.ToLower().Contains("post"))
```

This will match all the files in the input and output whose names contain the word "post", and will create a collection of them in the metadata of the `MetalsharpProject`. This metadata object, named `collections` will look like the following:

```c#
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

### `Collections(String, Predicate<MetalsharpFile>)`

Instantiate the plugin with a single collection definition.

- `name`: The name of the collection.
- `predicate`: The predicate to match files for the collection.

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

- `project`: The `MetalsharpProject` on which the plugin will be invoked.

## CollectionsPluginExtensions

Extensions for the Collections plugin.

### Methods

### `GetCollection(MetalsharpProject, String)`

Given the name of a collection, returns that collection from the metadata of the `MetalsharpProject`.

- `project`: The `MetalsharpProject` holding the collection.
- `name`: The name of the collection.

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

Given the name of a collection, returns the input and output files in that collection from the metadata of the `MetalsharpProject`.

- `project`: The `MetalsharpProject` holding the collection.
- `name`: The name of the collection.

```c#
MetalsharpFile[] collectionFiles = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetFilesFromCollection("myCollection").ToArray();
```

#### Returns

An enumerable of `MetalsharpFile`s from the input and output lists of the collection.

### `GetInputCollection(MetalsharpProject, String)`

Given the name of a collection, returns the input file paths in that collection from the metadata of the `MetalsharpProject`.

- `project`: The `MetalsharpProject` holding the collection.
- `name`: The name of the collection.

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

- `project`: The `MetalsharpProject` holding the collection.
- `name`: The name of the collection to return the input files from.

```c#
MetalsharpFile[] collectionInputFiles = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetInputFilesFromCollection("myCollection").ToArray();
```

#### Returns

An enumerable containing the files from the input list in the collection.

### `GetOutputCollection(MetalsharpProject, String)`

Given the name of a collection, returns the output file paths in that collection from the metadata of the `MetalsharpProject`.

- `project`: The `MetalsharpProject` holding the collection.
- `name`: The name of the collection.

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

- `project`: The `MetalsharpProject` holding the collection.
- `name`: The name of the collection to return the output files from.

```c#
MetalsharpFile[] collectionoutputFiles = new MetalsharpProject()
... // Add files
... // Create a collection named "myCollection"
.GetOutputFilesFromCollection("myCollection").ToArray();
```

#### Returns

An enumerable containing the files from the output list in the collection.

### `UseCollections(MetalsharpProject, String, Predicate<MetalsharpFile>)`

Invoke the Collections plugin with a single collection definition.

- `project`: The `MetalsharpProject` on which this method will be called.
- `name`: The name of the collection to define.
- `predicate`: The predicate to match the files for the collection.

Only add `.md` files to a collection named `myCollection`:

```c#
new MetalsharpProject()
.UseCollections("myCollection", file => file.Extension == ".md");
```

#### Returns

Combinator; returns `this` input.

### `UseCollections(MetalsharpProject, ValueTuple<String, Predicate<MetalsharpFile>>[])`

Invoke the Collections plugin with several collection definitions

- `project`: The `MetalsharpProject` on which this method will be called.
- `definitions`: The definitions of each collection.

Add `.md` files to a collection named `mdFiles` and `.html` files to a collection named `htmlFiles`:

```c#
new MetalsharpProject()
.UseCollections(("mdFiles", file => file.Extension == ".md"), ("htmlFiles", file => file.Extension == ".html"));
```

#### Returns

Combinator; returns `this` input.

## Debug

The Debug plugin.

Writes a log after every Use, outputting the contents of the input and output lists.

`Debug` is best invoked at the beginning of a stack of plugins, so as to capture each of the events related to the project:

```c#
new MetalsharpProject()
    .AddInput("Path\\To\\Dir")
    .UseDebug()
    .Use ... ;
```

### Constructors

### `Debug()`

By default, write debug logs with `Debug.WriteLine()`.

### `Debug(String)`

Instantiate `Debug` with a log file path to output the debug log to a log file.

- `logPath`: The path to the log file.

Given the following Metalsharp project:

```c#
new MetalsharpProject()
.UseDebug("output.log")
.Use(i => i.AddInput(new MetalsharpFile("text", "file.md")));
```

A file called `output.log` will be generated, and will look like the following:

```c#
Step 1.
Input files:

file.md

Output files:

---
```

### `Debug(Action<String>)`

Instantiate `Debug` with a custom action to perform each time a log is written. This can be used to output to different sources or execute different debug actions.

- `onLog`: The action to execute when writing a log.

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

- `project`: The `MetalsharpProject` to output debug logs for.

## DebugPluginExtensions

Extensions for the Debug plugin.

### Methods

### `UseDebug(MetalsharpProject)`

Invoke the default Debug plugin.

- `project`: The `MetalsharpProject` on which this method will be called.

```c#
new MetalsharpProject()
.UseDebug();
```

#### Returns

Combinator; returns `this` input.

### `UseDebug(MetalsharpProject, String)`

Invoke the Debug plugin with a log file to capture the debug logs.

- `project`: The `MetalsharpProject` on which this method will be called.
- `logPath`: The path to the log file.

```c#
new MetalsharpProject()
.UseDebug("debug.log");
```

#### Returns

Combinator; returns `this` input.

### `UseDebug(MetalsharpProject, Action<String>)`

Invoke the Debug plugin with custom log behavior.

- `project`: The `MetalsharpProject` on which this method will be called.
- `onLog`: The action to execute to log a debug line.

```c#
new MetalsharpProject()
.UseDebug(log => Console.WriteLine(log));
```

#### Returns

Combinator; returns `this` input.

## Frontmatter

The Frontmatter plugin.

Adds any YAML or JSON frontmatter in the input files to the metadata.

Given the following `file.txt`:

```c#
---
draft: true
---
Hello, World!
```

The assertion in the following will evaluate to `true`:

```c#
var project = new MetalsharpProject()
    .AddInput("file.txt")
    .UseFrontmatter();

Assert.True(Convert.ToBoolean(project.InputFiles[0].Metadata["draft"]))
```

Note that YAML frontmatter values are parsed as strings (here, the literal string `"true"`), while
JSON frontmatter values are parsed into their inferred CLR types (here, an actual `bool`) - so
`ToBoolean` is used above for portability between the two frontmatter formats.

### Constructors

### `Frontmatter()`

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

- `project`: The `MetalsharpProject` to invoke the plugin on.

## FrontmatterPluginExtensions

Extensions for the Frontmatter plugin.

### Methods

### `UseFrontmatter(MetalsharpProject)`

Invoke the `Frontmatter` plugin.

- `project`: The `MetalsharpProject` on which this method will be called.

```c#
new MetalsharpProject()
... // Add files
.UseFrontmatter();
```

#### Returns

Combinator; returns `this` input.

## IEnumerableExtensions

`MetalsharpFileCollection` extensions for `IEnumerable`.

### Methods

### `ToMetalsharpFileCollection(IEnumerable<MetalsharpFile>)`

Mimic `IEnumerable.ToList`, allowing the easy conversion of an enumerable of files to a `MetalsharpFileCollection`.

- `list`: The `IEnumerable` of `MetalsharpFile`s to convert to a `MetalsharpFileCollection`.

#### Returns

A `MetalsharpFileCollection` containing the files in the given list.

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

- `project`: The `MetalsharpProject` to alter.

## Leveller

The Leveller plugin

Adds "level" metadata to each file specifying how many directories deep the file is (1-based).

The following will add a file at a directory, use leveller, and demonstrate the resulting metadata in the file:

```c#
var file = new MetalsharpFile("Hello, World!", Path.Combine("dir1", "dir2", "file.txt"));
new MetalsharpProject().AddInput(file).UseLeveller();

foreach (var (key, value) in file.Metadata)
{
    Console.WriteLine($"{key}: {value}");
}
```

The output of the run will be "level: 2", since `file` is two directories deep from the root.

### Constructors

### `Leveller()`

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

- `project`: The `MetalsharpProject` to level.

## LevellerPluginExtensions

Extensions for the Leveller plugin.

### Methods

### `UseLeveller(MetalsharpProject)`

Invoke the `Leveller` plugin.

- `project`: The `MetalsharpProject` on which this method will be called.

```c#
new MetalsharpProject()
... // Add files
.UseLeveller();
```

#### Returns

Combinator; returns `this` input.

## LogEventArgs

Event args for log events.

- `Level`: The log level of the log.
- `Message`: The message of the log.

### Constructors

### `LogEventArgs(LogLevel, String)`

Event args for log events.

- `Level`: The log level of the log.
- `Message`: The message of the log.

### Properties

### `Level`

The log level of the log.

### `Message`

The message of the log.

## LogLevel

The verbosity level for log messages.

### Values

### `Debug`

`Debug` includes every loggable event useful when debugging.

### `Info`

`Info` includes every meaningful event while executing.

### `Error`

`Error` includes any events that are unexpected or may be unintended by the user.

### `Fatal`

`Fatal` includes any events that prevent continued execution.

### `None`

`None` prevents any logging.

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

```c#
<h1>Header 1</h1>
```

### Constructors

### `Markdown()`

### Methods

### `Execute(MetalsharpProject)`

Invokes the plugin.

- `project`: The `MetalsharpProject` to invoke the plugin on.

## MarkdownPluginExtensions

Extensions for the Markdown plugin.

### Methods

### `UseMarkdown(MetalsharpProject)`

Invoke the `Markdown` plugin.

- `project`: The `MetalsharpProject` on which this method will be called.

```c#
new MetalsharpProject()
... // Add files
.UseMarkdown();
```

#### Returns

Combinator; returns `this` input.

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

- `text`: The text of the file.
- `filePath`: The virtual path of the file.

### `MetalsharpFile(Byte[], String)`

Instantiates a new MetalsharpFile with no metadata.

- `contents`: The contents of the file.
- `filePath`: The virtual path of the file.

### `MetalsharpFile(String, String, Dictionary<String, Object>)`

Instantiate a new MetalsharpFile with the specified metadata.

- `text`: The text of the file.
- `filePath`: The virtual path of the file.
- `metadata`: The metadata of the file, stored as a string, object dictionary.

### `MetalsharpFile(Byte[], String, Dictionary<String, Object>)`

Instantiate a new MetalsharpFile with the specified metadata.

- `contents`: The contents of the file.
- `filePath`: The virtual path of the file.
- `metadata`: The metadata of the file, stored as a string, object dictionary.

### Methods

### `IsChildOf(String, StringComparison = OrdinalIgnoreCase)`

Checks whether a directory is the immediate parent of the file, i.e. whether `directory`'s
path segments exactly match the trailing segments of the file's own directory.

- `directory`: The directory in question.
- `comparisonType`: The kind of string comparison to use when comparing path segments.

`OrdinalIgnoreCase` by default.

#### Returns

`true` if the file is a child of the directory, `false` otherwise.

### `IsDescendantOf(String, StringComparison = OrdinalIgnoreCase)`

Checks whether a directory is an ancestor of the file, i.e. whether `directory`'s path
segments appear as a contiguous, aligned run anywhere in the file's path.

- `directory`: The directory in question.
- `comparisonType`: The kind of string comparison to use when comparing path segments.

`OrdinalIgnoreCase` by default.

#### Returns

`true` if the file is a descendant of the directory, `false` otherwise.

### Properties

### `Contents`

The contents of the file.

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

The contents of the file as a string.

## MetalsharpFileCollection

Represents a collection of Metalsharp files.

### Constructors

### `MetalsharpFileCollection()`

Instantiate an empty collection.

### `MetalsharpFileCollection(IEnumerable<MetalsharpFile>)`

Instantiate a collection with an existing one.

- `files`: The list of files to add to the collection.

### Methods

### `Add(MetalsharpFile)`

### `ChildrenOf(String, StringComparison = OrdinalIgnoreCase)`

Gets the files in the collection which are children to the given virtual directory.

- `directory`: The parent directory.
- `comparisonType`: The kind of string comparison to use when comparing path segments.

`OrdinalIgnoreCase` by default.

#### Returns

All of the files which are children of the given directory.

### `Clear()`

### `Contains(MetalsharpFile)`

### `ContainsDirectory(String, StringComparison = OrdinalIgnoreCase)`

### `CopyTo(MetalsharpFile[], Int32)`

### `DescendantsOf(String, StringComparison = OrdinalIgnoreCase)`

Gets the files in the collection which descend from the given virtual directory.

- `directory`: The ancestor directory.
- `comparisonType`: The kind of string comparison to use when comparing path segments.

`OrdinalIgnoreCase` by default.

#### Returns

All of the files which descend from the given directory.

### `GetEnumerator()`

### `IndexOf(MetalsharpFile)`

### `Insert(Int32, MetalsharpFile)`

### `Remove(MetalsharpFile)`

### `RemoveAll(Predicate<MetalsharpFile>)`

### `RemoveAt(Int32)`

### Properties

### `Count`

### `IsReadOnly`

### `Item`

### `Items`

## MetalsharpOptions

Represents the configuration options for a Metalsharp project.

- `clearOutputDirectory`: Whether Metalsharp should remove all the files in the output directory before writing any to that directory.

`false` by default.
- `outputDirectory`: The directory to which the files will be output.

`.\` by default.
- `verbosity`: The minimum level to log.

### Constructors

### `MetalsharpOptions()`

Instantiate the default configuration.

This overload exists because `CommandLineParser` constructs instances via reflection using
`Activator.CreateInstance<T>()`, which requires a genuinely parameterless constructor -
a constructor whose parameters merely all have default values does not qualify. Without this,
`FromArgs` throws `MissingMethodException`.

### `MetalsharpOptions(Boolean = false, String = ".\", LogLevel = Error)`

Represents the configuration options for a Metalsharp project.

- `clearOutputDirectory`: Whether Metalsharp should remove all the files in the output directory before writing any to that directory.

`false` by default.
- `outputDirectory`: The directory to which the files will be output.

`.\` by default.
- `verbosity`: The minimum level to log.

### Methods

### `FromArgs(String[])`

Instantiate configuration from command line arguments.

- `args`: The command line arguments

### Properties

### `ClearOutputDirectory`

Whether Metalsharp should remove all the files in the output directory before writing any to that directory.

`false` by default.

### `OutputDirectory`

The directory to which the files will be output.

`.\` by default.

### `Verbosity`

The minimum level to log.

### Constants

### `DefaultClearOutputDirectory`

The default value for `ClearOutputDirectory`

### `DefaultOutputDirectory`

The default value for `OutputDirectory`

### `DefaultVerbosity`

The default value for `Verbosity`

## MetalsharpProject

This is the root of a Metalsharp project. `MetalsharpProject` controls the use of plugins against a project, the files input and output by the project, and the building of the project.

The best example is always the example at the top of the [README](https://github.com/IanWold/Metalsharp/blob/master/README.md):

```c#
new MetalsharpProject()
    .AddInput("Site")
    .UseFrontmatter()
    .UseMarkdown()
    .AddOutput("Static")
    .Build();
```

Here, `MetalsharpProject` is instantiated and given a set of files from the on-disk directory `Site`. Then, the plugins `Frontmatter` and `Markdown` are invoked against the project, and the on-disk directory `Static` is added straight to the output. Finally, the project is built with default settings. The intent is that this resulting code is easy to read and easy to understand.

### Constructors

### `MetalsharpProject(MetalsharpOptions)`

Instantiate a `MetalsharpProject` with the specified configuration options.

- `options`: The configuration options for the project.

### `MetalsharpProject(Boolean = false, String = ".\", LogLevel = Error)`

Instantiate a `MetalsharpProject` with the specified configuration options.

- `clearOutputDirectory`: Whether Metalsharp should remove all the files in the output directory before writing any to that directory.

`false` by default.
- `outputDirectory`: The directory to which the files will be output.

`.\` by default.
- `verbosity`: The minimum level to log.

### Methods

### `AddInput(String)`

Adds a file or all the files in a directory to the input. The virtual directory of the files in the input will be the same as that on disk (regardless of whether a relative or absolute path is specified).

- `path`: The path to the on-disk file or directory.

```c#
new MetalsharpProject()
.AddInput("Path\\To\\Directory") // Add all files in Path\To\Directory to input.
.AddInput("Path\\To\\File.md"); // Add Path\To\File.md to input.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddInput(String, String)`

Add a file or directory to the input and place the files in a specific virtual path.

- `diskPath`: The path to the on-disk file or directory.
- `virtualPath`: The path to the virtual directory to place the files in.

```c#
new MetalsharpProject()
.AddInput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to input in the New\Path directory.
.AddInput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to input. Its path will be New\Path\File.md.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddInput(MetalsharpFile)`

Add a MetalsharpFile to the input files

- `file`: The file to add.

```c#
new MetalsharpProject()
.AddInput(new MetalsharpFile("# File Text", "path\\to\\file.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddOutput(String)`

Adds a file or all the files in a directory to the output. The virtual directory of the files in the output will be the same as that on disk (regardless of whether a relative or absolute path is specified).

- `path`: The path to the on-disk file or directory.

```c#
new MetalsharpProject()
.AddOutput("Path\\To\\Directory") // Add all files in Path\To\Directory to output
.AddOutput("Path\\To\\File.md"); // Add Path\To\File.md to output
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddOutput(String, String)`

Add a file or directory to the output and place the files in a specific virtual path.

- `diskPath`: The path to the on-disk file or directory.
- `virtualPath`: The path to the virtual directory to place the files in.

```c#
new MetalsharpProject()
.AddOutput("Path\\To\\Directory", "New\\Path") // Add all files in Path\To\Directory to the output in the New\Path directory.
.AddOutput("Path\\To\\File.md", "New\\Path"); // Add Path\To\File.md to the output. Its path will be New\Path\File.md.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `AddOutput(MetalsharpFile)`

Add a MetalsharpFile to the output files

- `file`: The file to add.

```c#
new MetalsharpProject()
.AddOutput(new MetalsharpFile("# File Text", "path\\to\\file.md");
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

### `LogDebug(String)`

Log a message at `Debug` level.

- `message`: The message to log.

The following will log a debug message between using two plugins:

```c#
new MetalsharpProject()
.UsePlugin1()
.LogDebug("About to use plugin 2...")
.UsePlugin2()
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `LogError(String)`

Log a message at `Error` level.

- `message`: The message to log.

The following will log a debug message between using two plugins:

```c#
new MetalsharpProject()
.UsePlugin1()
.LogError("About to use plugin 2...")
.UsePlugin2()
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `LogFatal(String)`

Log a message at `Fatal` level.

- `message`: The message to log.

The following will log a debug message between using two plugins:

```c#
new MetalsharpProject()
.UsePlugin1()
.LogFatal("About to use plugin 2...")
.UsePlugin2()
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `LogInfo(String)`

Log a message at `Info` level.

- `message`: The message to log.

The following will log a debug message between using two plugins:

```c#
new MetalsharpProject()
.UsePlugin1()
.LogInfo("About to use plugin 2...")
.UsePlugin2()
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Meta(String, Object)`

Add or update a single item of metadata.

- `key`: The key to add or update.
- `value`: The value to store with the key.

The following will add a single item to the metadata, and will then overwrite that value:

```c#
new MetalsharpProject()
.Meta("key", "value")
.Meta("key", "new value"); // The new value overwrites the old value.
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Meta(ValueTuple<String, Object>[])`

Add or update several items of metadata.

- `pairs`: The key-value pairs to add or update.

The following will add several items to the metadata:

```c#
new MetalsharpProject()
.Meta(("key1", "value1"), ("key2", "value2"), ("key3", "value3"));
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `MoveFiles(String, String)`

Moves files in the input and output from one directory to another.

- `fromDirectory`: The directory to move the files from.
- `toDirectory`: The directory to move the files to.

Suppose we have, for the sake of argument, input and output files in the following virtual directory structure:

```c#
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

```c#
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

### `MoveFiles(Predicate<MetalsharpFile>, String)`

Moves files in the input and output matching a predicate from one directory to another.

- `predicate`: The predicate to match the files to move.
- `toDirectory`: The directory to move the files to.

Suppose we have, for the sake of argument, input and output files in the following virtual directory structure:

```c#
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

```c#
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

- `fromDirectory`: The directory to move the files from.
- `toDirectory`: The directory to move the files to.

Suppose we have input files in the following virtual directory structure:

```c#
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

```c#
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

### `MoveInput(Predicate<MetalsharpFile>, String, String = null)`

Moves files in the input matching a predicate from one directory to another.

- `predicate`: The predicate to match the files to move.
- `toDirectory`: The directory to move the files to.
- `logMessage`: The message to log indicating which files are being moved.

Suppose we have input files in the following virtual directory structure:

```c#
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

```c#
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

- `fromDirectory`: The directory to move the files from.
- `toDirectory`: The directory to move the files to.

Suppose we have output files in the following virtual directory structure:

```c#
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

```c#
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

### `MoveOutput(Predicate<MetalsharpFile>, String, String = null)`

Moves files in the output matching a predicate from one directory to another.

- `predicate`: The predicate to match the files to move.
- `toDirectory`: The directory to move the files to.
- `logMessage`: The message to log indicating which files are being moved.

Suppose we have output files in the following virtual directory structure:

```c#
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

```c#
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

- `path`: The path of the file to remove.

Supposing we have `Directory\File.md` in the input and output, we can remove it from both with `RemoveFiles`:

```c#
new MetalsharpProject()
... // Add file to input
... // Add file to output
.RemoveFiles("Directory\\File.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `RemoveFiles(Predicate<MetalsharpFile>)`

Remove all the files matching a predicate from each the input and output.

- `predicate`: The predicate function to identify files to delete.

Supposing we have, for the sake of argument, the following virtual directory structure in the input and output:

```c#
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

```c#
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

- `path`: The path of the file to remove.

Supposing we have `Directory\File.md` in the input, we can remove it with `RemoveInput`:

```c#
new MetalsharpProject()
... // Add file
.RemoveInput("Directory\\File.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `RemoveInput(Predicate<MetalsharpFile>, String = null)`

Remove all the files matching a predicate from the input.

- `predicate`: The predicate function to identify files to delete.
- `logMessage`: The message to log indicating which files are being removed.

Supposing we have the following virtual directory structure in the input:

```c#
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

```c#
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

- `path`: The path of the file to remove.

Supposing we have `Directory\File.md` in the output, we can remove it with `RemoveOutput`:

```c#
new MetalsharpProject()
... // Add file
.RemoveOutput("Directory\\File.md");
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `RemoveOutput(Predicate<MetalsharpFile>, String = null)`

Remove all the files matching a predicate from the output.

- `predicate`: The predicate function to identify files to delete.
- `logMessage`: The message to log indicating which files are being removed.

Supposing we have the following virtual directory structure in the output:

```c#
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

```c#
.
├── Content
│   ├── Posts
│   │   ├── Post1.md
│   ├── Index.md
└── README.md
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Use(Action<MetalsharpProject>, String = null)`

Invokes a function as a plugin.

- `func`: The function to invoke.
- `functionName`: Optionally, the name of the function to log.

```c#
new MetalsharpProject()
.Use(dir => dir.Meta("Hello", "World!"));
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Use(IMetalsharpPlugin)`

Invoke a plugin.

- `plugin`: The instance of the plugin to invoke.

```c#
new MetalsharpProject()
.Use(new Debug()); // Invokes the Debug plugin
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### `Use<T>()`

Invoke a plugin by type. The plugin must have a default (no arguments) constructor.

```c#
new MetalsharpProject()
.Use<Debug>(); // Invokes the Debug plugin
```

#### Returns

The current `MetalsharpProject`, allowing it to be fluent.

### Properties

### `InputFiles`

The input files of the project.

### `Metadata`

The directory-level metadata.

### `Options`

The configuration options for the project.

### `OutputFiles`

The files to output during building.

### Events

### `AfterBuild`

Invoked after `Build()`

### `AfterUse`

Invoked after `Use()`

### `BeforeBuild`

Invoked before `Build()`

### `BeforeUse`

Invoked before `Use()`

