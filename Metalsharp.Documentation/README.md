# Metalsharp API Documentation

## BuildOptions


Represents the options when Metalsharp outputs a directory



### Properties

#### `ClearOutputDirectory`


Whether Metalsharp should remove all the files in the output directory before writing any



#### `OutputDirectory`


The directory to which the files will be output




## IMetalsharpFile


Represents the interface for a Metalsharp file



### Methods

#### `IsDescendantOf(String)`


Returns true if the directory is an ancestor of the file



#### `IsChildOf(String)`


Returns true if the directory is the parent of the file



### Properties

#### `Directory`


The directory of the file relative to the source directory



#### `Extension`


The extension from the file name



#### `FilePath`


The path of the file



#### `Metadata`


Metadata from the file



#### `Name`


The name of the file, without the extension



#### `Text`


The text of the file




## IMetalsharpFileCollection


Represents the interface for a collection of Metalsharp files



### Methods

#### `DescendantsOf(String)`


Get the descendant files of a directory



#### `ChildrenOf(String)`


Get the children files of a directory



#### `ContainsDirectory(String)`


Returns true if one of the files in the collection descends from the directory



#### `RemoveAll(Predicate<>)`


Alias List.RemoveAll




## IMetalsharpPlugin


Represents a Metalsharp plugin



### Methods

#### `Execute(MetalsharpDirectory)`


Invokes the plugin. Called by Metalsharp.Use



##### Returns

The same directory as was input



## MetalsharpDirectory


Represents a root directory to be manipulated by Metalsharp plugins



### Constructors

#### `MetalsharpDirectory()`


Instantiate an empty MetalsharpDirectory



#### `MetalsharpDirectory(String)`


Instantiate Metalsharp from an existing directory



#### `MetalsharpDirectory(String, String)`


Instantiate Metalsharp from an existing directory and add the contents to a specific virtual path



### Methods

#### `AddExisting(String, String, Action<MetalsharpFile>)`


Add an existing file to the input or output and place the files in a specific virtual path



#### `AddInput(String)`


Add a file or all the files in a directory to the input



#### `AddInput(String, String)`


Add a file or all the files in a directory to the input and place the files in a specific virtual path



#### `AddInput(MetalsharpFile)`


Add a MetalsharpFile to the input files



#### `AddOutput(String)`


Add a file or all the files in a directory directly to the output

The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed



#### `AddOutput(String, String)`


Add a file or all the files in a directory directly to the output and place the files in a specific virtual path

The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed



#### `AddOutput(MetalsharpFile)`


Add a MetalsharpFile to output files



#### `GetFileWithNormalizedDirectory(String, String)`


Gets a MetalsharpFile with the RootDirectory removed from its path



#### `Build()`


Write all the output files to the default output directory
with default build options



#### `Build(BuildOptions)`


Write all the output files to the output directory



#### `Build(Action<MetalsharpDirectory>)`


Write all the output files to the default output directory after performing a function
with default build options



#### `Build(Action<MetalsharpDirectory>, BuildOptions)`


Write all the output files to the output directory after performing a function



#### `Meta(String, Object)`


Add or alter a single item of metadata



#### `Meta(ValueTuple<String>, Object}[])`


Add or alter directory-level metadata



#### `MoveFiles(String, String)`


Move files in the input and output from one directory to another



#### `MoveFiles(Predicate<IMetalsharpFile>, String)`


Move files matching a predicate in the input and output from one directory to another



#### `MoveInput(String, String)`


Move files in the input from one directory to another



#### `MoveInput(Predicate<IMetalsharpFile>, String)`


Move files in the input matching a predicate from one directory to another



#### `MoveOutput(String, String)`


Move files in the output from one directory to another



#### `MoveOutput(Predicate<IMetalsharpFile>, String)`


Move files in the output matching a predicate from one directory to another



#### `RemoveFiles(String)`


Remove a file from the input and output



#### `RemoveFiles(Predicate<IMetalsharpFile>)`


Remove all the files matching a predicate from the input and output



#### `RemoveInput(String)`


Remove a file from the input



#### `RemoveInput(Predicate<IMetalsharpFile>)`


Remove all the files matching a predicate from the input



#### `RemoveOutput(String)`


Remove a file from the output



#### `RemoveOutput(Predicate<IMetalsharpFile>)`


Remove all the files matching a predicate from the output



#### `Use(Action<MetalsharpDirectory>)`


Invoke a function as a plugin



#### `Use(IMetalsharpPlugin)`


Invoke a plugin



#### `Use`()`


Invoke a plugin by type

The plugin type must have an empty constructor



### Properties

#### `Metadata`


The directory-level metadata



#### `InputFiles`


The input files



#### `OutputFiles`


The files to output



### Events

#### `BeforeUse`


Invoked before .Use()



#### `AfterUse`


Invoked after .Use()



#### `BeforeBuild`


Invoked before .Build()



#### `AfterBuild`


Invoked after .Build()




## MetalsharpFile


Represents a file



### Constructors

#### `MetalsharpFile(String, String)`


Instantiate a new MetalsharpFile with no metadata



#### `MetalsharpFile(String, String, Dictionary<String>, Object})`


Instantiate a new MetalsharpFile with the specified metadata



### Methods

#### `IsDescendantOf(String)`


Returns true if the directory is an ancestor of the file



#### `IsChildOf(String)`


Returns true if the directory is the parent of the file



### Properties

#### `Directory`


THe directory of the file relative to the source directory



#### `Extension`


The extension from the file name



#### `FilePath`


The path of the file



#### `Metadata`


Metadata from the file



#### `Name`


The name of the file, without the extension



#### `Text`


The text of the file




## MetalsharpFileCollection


Represents a collection of Metalsharp files

Implements members to handle "virtual" directories



### Constructors

#### `MetalsharpFileCollection()`


Instantiate an empty collection



#### `MetalsharpFileCollection(IEnumerable<>)`


Instantiate a collection with an existing one



### Methods

#### `DescendantsOf(String)`


Get the descendant files of a directory



#### `ChildrenOf(String)`


Get the children files of a directory



#### `ContainsDirectory(String)`


Returns true if one of the files in the collection descends from the directory



### Fields

#### `_items`


The Metalsharp files in the collection




## IEnumerableExtensions


MetalsharpFileCollection extensions for IEnumerable



### Methods

#### `ToMetalsharpFileCollection`(IEnumerable<`>)`


Mimic IEnumerable.ToList




## Branch


The Branch plugin

Branches a directory for separate plugins to be computed



### Constructors

#### `Branch(Action<MetalsharpDirectory[]>)`




### Methods

#### `Execute(MetalsharpDirectory)`


Invokes the plugin



### Fields

#### `_branches`


The function-branches




## Collections


Collections plugin

Groups files matching a predicate into collections in the directory metadata



### Constructors

#### `Collections(String, Predicate<IMetalsharpFile>)`


Instantiate the plugin with a single collection definition



#### `Collections(ValueTuple<String>, Predicate<IMetalsharpFile[]>)`


Instantiates the plugin with the definitions of the collections



### Methods

#### `Execute(MetalsharpDirectory)`


Invokes the plugin



### Fields

#### `_definitions`


Contains the definitions of the collections




## Debug


The Debug plugin

Writes a log after every Use, outputting the contents of the input and output directories.



### Constructors

#### `Debug()`


By default, write debug logs with Debug.WriteLine()



#### `Debug(String)`


Configure Debug to write logs to a log file



#### `Debug(Action<String>)`


Configure Debug to use custom behavior when writing logs



### Methods

#### `Execute(MetalsharpDirectory)`


Invokes the plugin



#### `WriteDirectory(IMetalsharpFileCollection<MetalsharpFile>)`


Prettify the contents of a collection of files



### Fields

#### `_onLog`


The action to execute when writing a log



#### `_useCount`


A count of the number of calls to .Use() against the directory




## Frontmatter


The Frontmatter plugin

Adds any YAML or JSON frontmatter in the input files to the metadata



### Methods

#### `Execute(MetalsharpDirectory)`


Invokes the plugin



#### `TryGetFrontmatter(String, Dictionary<String>, Object}@, String@)`


Try to parse YAML or JSON frontmatter



#### `TryGetYamlFrontmatter(String, Dictionary<String>, Object}@, String@)`


Try to parse YAML frontmatter



#### `TryGetJsonFrontmatter(String, Dictionary<String>, Object}@, String@)`


Try to parse JSON frontmatter




## Markdown


The Markdown plugin

Converts any markdown files to HTML



### Methods

#### `Execute(MetalsharpDirectory)`


Invokes the plugin




## MetalsharpExtensions


Extensions to Metalsharp for invoking included plugins



### Methods

#### `Branch(MetalsharpDirectory, Action<MetalsharpDirectory[]>)`


Invoke the Branch plugin



#### `UseCollections(MetalsharpDirectory, String, Predicate<IMetalsharpFile>)`


Invoke the Collections plugin with a single collection definition



#### `UseCollections(MetalsharpDirectory, ValueTuple<String>, Predicate<IMetalsharpFile[]>)`


Invoke the Collections plugin with several collection definitions



#### `GetCollection(MetalsharpDirectory, String)`


Get a collection from MetalsharpDirectory Metadata by name



#### `GetFilesFromCollection(MetalsharpDirectory, String)`


Get input and output files from a collection by name



#### `GetInputCollection(MetalsharpDirectory, String)`


Get the input files from a collection from MetalsharpDirectory Metadata by name



#### `GetInputFilesFromCollection(MetalsharpDirectory, String)`


Get the input files from a collection by name



#### `GetOutputCollection(MetalsharpDirectory, String)`


Get the output files from a collection from MetalsharpDirectory Metadata by name



#### `GetOutputFilesFromCollection(MetalsharpDirectory, String)`


Get the output files from a collection by name



#### `UseDebug(MetalsharpDirectory)`


Invoke the default Debug plugin



#### `UseDebug(MetalsharpDirectory, String)`


Invoke the Debug plugin with a log file to capture the debug logs



#### `UseDebug(MetalsharpDirectory, Action<String>)`


Invoke the Debug plugin with custom log behavior



#### `UseFrontmatter(MetalsharpDirectory)`


Invoke the frontmatter plugin



#### `UseMarkdown(MetalsharpDirectory)`


Invoke the Merkdown plugin




