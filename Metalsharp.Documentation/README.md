<a name='assembly'></a>
# Metalsharp

## Contents

- [Branch](#T-Metalsharp-Branch 'Metalsharp.Branch')
  - [#ctor(branches)](#M-Metalsharp-Branch-#ctor-System-Action{Metalsharp-MetalsharpDirectory}[]- 'Metalsharp.Branch.#ctor(System.Action{Metalsharp.MetalsharpDirectory}[])')
  - [_branches](#P-Metalsharp-Branch-_branches 'Metalsharp.Branch._branches')
  - [Execute(directory)](#M-Metalsharp-Branch-Execute-Metalsharp-MetalsharpDirectory- 'Metalsharp.Branch.Execute(Metalsharp.MetalsharpDirectory)')
- [BuildOptions](#T-Metalsharp-BuildOptions 'Metalsharp.BuildOptions')
  - [ClearOutputDirectory](#P-Metalsharp-BuildOptions-ClearOutputDirectory 'Metalsharp.BuildOptions.ClearOutputDirectory')
  - [OutputDirectory](#P-Metalsharp-BuildOptions-OutputDirectory 'Metalsharp.BuildOptions.OutputDirectory')
- [Collections](#T-Metalsharp-Collections 'Metalsharp.Collections')
  - [#ctor(name,predicate)](#M-Metalsharp-Collections-#ctor-System-String,System-Predicate{Metalsharp-IMetalsharpFile}- 'Metalsharp.Collections.#ctor(System.String,System.Predicate{Metalsharp.IMetalsharpFile})')
  - [#ctor(definitions)](#M-Metalsharp-Collections-#ctor-System-ValueTuple{System-String,System-Predicate{Metalsharp-IMetalsharpFile}}[]- 'Metalsharp.Collections.#ctor(System.ValueTuple{System.String,System.Predicate{Metalsharp.IMetalsharpFile}}[])')
  - [_definitions](#F-Metalsharp-Collections-_definitions 'Metalsharp.Collections._definitions')
  - [Execute(directory)](#M-Metalsharp-Collections-Execute-Metalsharp-MetalsharpDirectory- 'Metalsharp.Collections.Execute(Metalsharp.MetalsharpDirectory)')
- [Debug](#T-Metalsharp-Debug 'Metalsharp.Debug')
  - [#ctor()](#M-Metalsharp-Debug-#ctor 'Metalsharp.Debug.#ctor')
  - [#ctor(logPath)](#M-Metalsharp-Debug-#ctor-System-String- 'Metalsharp.Debug.#ctor(System.String)')
  - [#ctor(onLog)](#M-Metalsharp-Debug-#ctor-System-Action{System-String}- 'Metalsharp.Debug.#ctor(System.Action{System.String})')
  - [_onLog](#F-Metalsharp-Debug-_onLog 'Metalsharp.Debug._onLog')
  - [_useCount](#F-Metalsharp-Debug-_useCount 'Metalsharp.Debug._useCount')
  - [Execute(directory)](#M-Metalsharp-Debug-Execute-Metalsharp-MetalsharpDirectory- 'Metalsharp.Debug.Execute(Metalsharp.MetalsharpDirectory)')
  - [WriteDirectory(directory)](#M-Metalsharp-Debug-WriteDirectory-Metalsharp-IMetalsharpFileCollection{Metalsharp-MetalsharpFile}- 'Metalsharp.Debug.WriteDirectory(Metalsharp.IMetalsharpFileCollection{Metalsharp.MetalsharpFile})')
- [Frontmatter](#T-Metalsharp-Frontmatter 'Metalsharp.Frontmatter')
  - [Execute(directory)](#M-Metalsharp-Frontmatter-Execute-Metalsharp-MetalsharpDirectory- 'Metalsharp.Frontmatter.Execute(Metalsharp.MetalsharpDirectory)')
  - [TryGetFrontmatter(document,frontmatter,remainder)](#M-Metalsharp-Frontmatter-TryGetFrontmatter-System-String,System-Collections-Generic-Dictionary{System-String,System-Object}@,System-String@- 'Metalsharp.Frontmatter.TryGetFrontmatter(System.String,System.Collections.Generic.Dictionary{System.String,System.Object}@,System.String@)')
  - [TryGetJsonFrontmatter(document,frontmatter,remainder)](#M-Metalsharp-Frontmatter-TryGetJsonFrontmatter-System-String,System-Collections-Generic-Dictionary{System-String,System-Object}@,System-String@- 'Metalsharp.Frontmatter.TryGetJsonFrontmatter(System.String,System.Collections.Generic.Dictionary{System.String,System.Object}@,System.String@)')
  - [TryGetYamlFrontmatter(document,frontmatter,remainder)](#M-Metalsharp-Frontmatter-TryGetYamlFrontmatter-System-String,System-Collections-Generic-Dictionary{System-String,System-Object}@,System-String@- 'Metalsharp.Frontmatter.TryGetYamlFrontmatter(System.String,System.Collections.Generic.Dictionary{System.String,System.Object}@,System.String@)')
- [IEnumerableExtensions](#T-Metalsharp-IEnumerableExtensions 'Metalsharp.IEnumerableExtensions')
  - [ToMetalsharpFileCollection\`\`1(list)](#M-Metalsharp-IEnumerableExtensions-ToMetalsharpFileCollection``1-System-Collections-Generic-IEnumerable{``0}- 'Metalsharp.IEnumerableExtensions.ToMetalsharpFileCollection``1(System.Collections.Generic.IEnumerable{``0})')
- [IMetalsharpFile](#T-Metalsharp-IMetalsharpFile 'Metalsharp.IMetalsharpFile')
  - [Directory](#P-Metalsharp-IMetalsharpFile-Directory 'Metalsharp.IMetalsharpFile.Directory')
  - [Extension](#P-Metalsharp-IMetalsharpFile-Extension 'Metalsharp.IMetalsharpFile.Extension')
  - [FilePath](#P-Metalsharp-IMetalsharpFile-FilePath 'Metalsharp.IMetalsharpFile.FilePath')
  - [Metadata](#P-Metalsharp-IMetalsharpFile-Metadata 'Metalsharp.IMetalsharpFile.Metadata')
  - [Name](#P-Metalsharp-IMetalsharpFile-Name 'Metalsharp.IMetalsharpFile.Name')
  - [Text](#P-Metalsharp-IMetalsharpFile-Text 'Metalsharp.IMetalsharpFile.Text')
  - [IsChildOf(directory)](#M-Metalsharp-IMetalsharpFile-IsChildOf-System-String- 'Metalsharp.IMetalsharpFile.IsChildOf(System.String)')
  - [IsDescendantOf(directory)](#M-Metalsharp-IMetalsharpFile-IsDescendantOf-System-String- 'Metalsharp.IMetalsharpFile.IsDescendantOf(System.String)')
- [IMetalsharpFileCollection\`1](#T-Metalsharp-IMetalsharpFileCollection`1 'Metalsharp.IMetalsharpFileCollection`1')
  - [ChildrenOf(directory)](#M-Metalsharp-IMetalsharpFileCollection`1-ChildrenOf-System-String- 'Metalsharp.IMetalsharpFileCollection`1.ChildrenOf(System.String)')
  - [ContainsDirectory(directory)](#M-Metalsharp-IMetalsharpFileCollection`1-ContainsDirectory-System-String- 'Metalsharp.IMetalsharpFileCollection`1.ContainsDirectory(System.String)')
  - [DescendantsOf(directory)](#M-Metalsharp-IMetalsharpFileCollection`1-DescendantsOf-System-String- 'Metalsharp.IMetalsharpFileCollection`1.DescendantsOf(System.String)')
  - [RemoveAll()](#M-Metalsharp-IMetalsharpFileCollection`1-RemoveAll-System-Predicate{`0}- 'Metalsharp.IMetalsharpFileCollection`1.RemoveAll(System.Predicate{`0})')
- [IMetalsharpPlugin](#T-Metalsharp-IMetalsharpPlugin 'Metalsharp.IMetalsharpPlugin')
  - [Execute(directory)](#M-Metalsharp-IMetalsharpPlugin-Execute-Metalsharp-MetalsharpDirectory- 'Metalsharp.IMetalsharpPlugin.Execute(Metalsharp.MetalsharpDirectory)')
- [Markdown](#T-Metalsharp-Markdown 'Metalsharp.Markdown')
  - [Execute(directory)](#M-Metalsharp-Markdown-Execute-Metalsharp-MetalsharpDirectory- 'Metalsharp.Markdown.Execute(Metalsharp.MetalsharpDirectory)')
- [MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory')
  - [#ctor()](#M-Metalsharp-MetalsharpDirectory-#ctor 'Metalsharp.MetalsharpDirectory.#ctor')
  - [#ctor(path)](#M-Metalsharp-MetalsharpDirectory-#ctor-System-String- 'Metalsharp.MetalsharpDirectory.#ctor(System.String)')
  - [#ctor(diskPath,virtualPath)](#M-Metalsharp-MetalsharpDirectory-#ctor-System-String,System-String- 'Metalsharp.MetalsharpDirectory.#ctor(System.String,System.String)')
  - [InputFiles](#P-Metalsharp-MetalsharpDirectory-InputFiles 'Metalsharp.MetalsharpDirectory.InputFiles')
  - [Metadata](#P-Metalsharp-MetalsharpDirectory-Metadata 'Metalsharp.MetalsharpDirectory.Metadata')
  - [OutputFiles](#P-Metalsharp-MetalsharpDirectory-OutputFiles 'Metalsharp.MetalsharpDirectory.OutputFiles')
  - [AddExisting(diskPath,virtualPath,add)](#M-Metalsharp-MetalsharpDirectory-AddExisting-System-String,System-String,System-Action{Metalsharp-MetalsharpFile}- 'Metalsharp.MetalsharpDirectory.AddExisting(System.String,System.String,System.Action{Metalsharp.MetalsharpFile})')
  - [AddInput(path)](#M-Metalsharp-MetalsharpDirectory-AddInput-System-String- 'Metalsharp.MetalsharpDirectory.AddInput(System.String)')
  - [AddInput(diskPath,virtualPath)](#M-Metalsharp-MetalsharpDirectory-AddInput-System-String,System-String- 'Metalsharp.MetalsharpDirectory.AddInput(System.String,System.String)')
  - [AddInput(file)](#M-Metalsharp-MetalsharpDirectory-AddInput-Metalsharp-MetalsharpFile- 'Metalsharp.MetalsharpDirectory.AddInput(Metalsharp.MetalsharpFile)')
  - [AddOutput(path)](#M-Metalsharp-MetalsharpDirectory-AddOutput-System-String- 'Metalsharp.MetalsharpDirectory.AddOutput(System.String)')
  - [AddOutput(diskPath,virtualPath)](#M-Metalsharp-MetalsharpDirectory-AddOutput-System-String,System-String- 'Metalsharp.MetalsharpDirectory.AddOutput(System.String,System.String)')
  - [AddOutput(file)](#M-Metalsharp-MetalsharpDirectory-AddOutput-Metalsharp-MetalsharpFile- 'Metalsharp.MetalsharpDirectory.AddOutput(Metalsharp.MetalsharpFile)')
  - [Build()](#M-Metalsharp-MetalsharpDirectory-Build 'Metalsharp.MetalsharpDirectory.Build')
  - [Build(options)](#M-Metalsharp-MetalsharpDirectory-Build-Metalsharp-BuildOptions- 'Metalsharp.MetalsharpDirectory.Build(Metalsharp.BuildOptions)')
  - [Build(func)](#M-Metalsharp-MetalsharpDirectory-Build-System-Action{Metalsharp-MetalsharpDirectory}- 'Metalsharp.MetalsharpDirectory.Build(System.Action{Metalsharp.MetalsharpDirectory})')
  - [Build(func,options)](#M-Metalsharp-MetalsharpDirectory-Build-System-Action{Metalsharp-MetalsharpDirectory},Metalsharp-BuildOptions- 'Metalsharp.MetalsharpDirectory.Build(System.Action{Metalsharp.MetalsharpDirectory},Metalsharp.BuildOptions)')
  - [GetFileWithNormalizedDirectory(diskPath,virtualPath)](#M-Metalsharp-MetalsharpDirectory-GetFileWithNormalizedDirectory-System-String,System-String- 'Metalsharp.MetalsharpDirectory.GetFileWithNormalizedDirectory(System.String,System.String)')
  - [Meta(key,value)](#M-Metalsharp-MetalsharpDirectory-Meta-System-String,System-Object- 'Metalsharp.MetalsharpDirectory.Meta(System.String,System.Object)')
  - [Meta(pairs)](#M-Metalsharp-MetalsharpDirectory-Meta-System-ValueTuple{System-String,System-Object}[]- 'Metalsharp.MetalsharpDirectory.Meta(System.ValueTuple{System.String,System.Object}[])')
  - [MoveFiles(oldDirectory,newDirectory)](#M-Metalsharp-MetalsharpDirectory-MoveFiles-System-String,System-String- 'Metalsharp.MetalsharpDirectory.MoveFiles(System.String,System.String)')
  - [MoveFiles(predicate,newDirectory)](#M-Metalsharp-MetalsharpDirectory-MoveFiles-System-Predicate{Metalsharp-IMetalsharpFile},System-String- 'Metalsharp.MetalsharpDirectory.MoveFiles(System.Predicate{Metalsharp.IMetalsharpFile},System.String)')
  - [MoveInput(oldDirectory,newDirectory)](#M-Metalsharp-MetalsharpDirectory-MoveInput-System-String,System-String- 'Metalsharp.MetalsharpDirectory.MoveInput(System.String,System.String)')
  - [MoveInput(predicate,newDirectory)](#M-Metalsharp-MetalsharpDirectory-MoveInput-System-Predicate{Metalsharp-IMetalsharpFile},System-String- 'Metalsharp.MetalsharpDirectory.MoveInput(System.Predicate{Metalsharp.IMetalsharpFile},System.String)')
  - [MoveOutput(oldDirectory,newDirectory)](#M-Metalsharp-MetalsharpDirectory-MoveOutput-System-String,System-String- 'Metalsharp.MetalsharpDirectory.MoveOutput(System.String,System.String)')
  - [MoveOutput(predicate,newDirectory)](#M-Metalsharp-MetalsharpDirectory-MoveOutput-System-Predicate{Metalsharp-IMetalsharpFile},System-String- 'Metalsharp.MetalsharpDirectory.MoveOutput(System.Predicate{Metalsharp.IMetalsharpFile},System.String)')
  - [RemoveFiles(path)](#M-Metalsharp-MetalsharpDirectory-RemoveFiles-System-String- 'Metalsharp.MetalsharpDirectory.RemoveFiles(System.String)')
  - [RemoveFiles(predicate)](#M-Metalsharp-MetalsharpDirectory-RemoveFiles-System-Predicate{Metalsharp-IMetalsharpFile}- 'Metalsharp.MetalsharpDirectory.RemoveFiles(System.Predicate{Metalsharp.IMetalsharpFile})')
  - [RemoveInput(path)](#M-Metalsharp-MetalsharpDirectory-RemoveInput-System-String- 'Metalsharp.MetalsharpDirectory.RemoveInput(System.String)')
  - [RemoveInput(predicate)](#M-Metalsharp-MetalsharpDirectory-RemoveInput-System-Predicate{Metalsharp-IMetalsharpFile}- 'Metalsharp.MetalsharpDirectory.RemoveInput(System.Predicate{Metalsharp.IMetalsharpFile})')
  - [RemoveOutput(path)](#M-Metalsharp-MetalsharpDirectory-RemoveOutput-System-String- 'Metalsharp.MetalsharpDirectory.RemoveOutput(System.String)')
  - [RemoveOutput(predicate)](#M-Metalsharp-MetalsharpDirectory-RemoveOutput-System-Predicate{Metalsharp-IMetalsharpFile}- 'Metalsharp.MetalsharpDirectory.RemoveOutput(System.Predicate{Metalsharp.IMetalsharpFile})')
  - [Use(func)](#M-Metalsharp-MetalsharpDirectory-Use-System-Action{Metalsharp-MetalsharpDirectory}- 'Metalsharp.MetalsharpDirectory.Use(System.Action{Metalsharp.MetalsharpDirectory})')
  - [Use(plugin)](#M-Metalsharp-MetalsharpDirectory-Use-Metalsharp-IMetalsharpPlugin- 'Metalsharp.MetalsharpDirectory.Use(Metalsharp.IMetalsharpPlugin)')
  - [Use\`\`1()](#M-Metalsharp-MetalsharpDirectory-Use``1 'Metalsharp.MetalsharpDirectory.Use``1')
- [MetalsharpExtensions](#T-Metalsharp-MetalsharpExtensions 'Metalsharp.MetalsharpExtensions')
  - [Branch(directory,branches)](#M-Metalsharp-MetalsharpExtensions-Branch-Metalsharp-MetalsharpDirectory,System-Action{Metalsharp-MetalsharpDirectory}[]- 'Metalsharp.MetalsharpExtensions.Branch(Metalsharp.MetalsharpDirectory,System.Action{Metalsharp.MetalsharpDirectory}[])')
  - [GetCollection(directory,name)](#M-Metalsharp-MetalsharpExtensions-GetCollection-Metalsharp-MetalsharpDirectory,System-String- 'Metalsharp.MetalsharpExtensions.GetCollection(Metalsharp.MetalsharpDirectory,System.String)')
  - [GetFilesFromCollection(directory,name)](#M-Metalsharp-MetalsharpExtensions-GetFilesFromCollection-Metalsharp-MetalsharpDirectory,System-String- 'Metalsharp.MetalsharpExtensions.GetFilesFromCollection(Metalsharp.MetalsharpDirectory,System.String)')
  - [GetInputCollection(directory,name)](#M-Metalsharp-MetalsharpExtensions-GetInputCollection-Metalsharp-MetalsharpDirectory,System-String- 'Metalsharp.MetalsharpExtensions.GetInputCollection(Metalsharp.MetalsharpDirectory,System.String)')
  - [GetInputFilesFromCollection(directory,name)](#M-Metalsharp-MetalsharpExtensions-GetInputFilesFromCollection-Metalsharp-MetalsharpDirectory,System-String- 'Metalsharp.MetalsharpExtensions.GetInputFilesFromCollection(Metalsharp.MetalsharpDirectory,System.String)')
  - [GetOutputCollection(directory,name)](#M-Metalsharp-MetalsharpExtensions-GetOutputCollection-Metalsharp-MetalsharpDirectory,System-String- 'Metalsharp.MetalsharpExtensions.GetOutputCollection(Metalsharp.MetalsharpDirectory,System.String)')
  - [GetOutputFilesFromCollection(directory,name)](#M-Metalsharp-MetalsharpExtensions-GetOutputFilesFromCollection-Metalsharp-MetalsharpDirectory,System-String- 'Metalsharp.MetalsharpExtensions.GetOutputFilesFromCollection(Metalsharp.MetalsharpDirectory,System.String)')
  - [UseCollections(directory,name,predicate)](#M-Metalsharp-MetalsharpExtensions-UseCollections-Metalsharp-MetalsharpDirectory,System-String,System-Predicate{Metalsharp-IMetalsharpFile}- 'Metalsharp.MetalsharpExtensions.UseCollections(Metalsharp.MetalsharpDirectory,System.String,System.Predicate{Metalsharp.IMetalsharpFile})')
  - [UseCollections(directory,definitions)](#M-Metalsharp-MetalsharpExtensions-UseCollections-Metalsharp-MetalsharpDirectory,System-ValueTuple{System-String,System-Predicate{Metalsharp-IMetalsharpFile}}[]- 'Metalsharp.MetalsharpExtensions.UseCollections(Metalsharp.MetalsharpDirectory,System.ValueTuple{System.String,System.Predicate{Metalsharp.IMetalsharpFile}}[])')
  - [UseDebug(directory)](#M-Metalsharp-MetalsharpExtensions-UseDebug-Metalsharp-MetalsharpDirectory- 'Metalsharp.MetalsharpExtensions.UseDebug(Metalsharp.MetalsharpDirectory)')
  - [UseDebug(directory,logPath)](#M-Metalsharp-MetalsharpExtensions-UseDebug-Metalsharp-MetalsharpDirectory,System-String- 'Metalsharp.MetalsharpExtensions.UseDebug(Metalsharp.MetalsharpDirectory,System.String)')
  - [UseDebug(directory,onLog)](#M-Metalsharp-MetalsharpExtensions-UseDebug-Metalsharp-MetalsharpDirectory,System-Action{System-String}- 'Metalsharp.MetalsharpExtensions.UseDebug(Metalsharp.MetalsharpDirectory,System.Action{System.String})')
  - [UseFrontmatter(directory)](#M-Metalsharp-MetalsharpExtensions-UseFrontmatter-Metalsharp-MetalsharpDirectory- 'Metalsharp.MetalsharpExtensions.UseFrontmatter(Metalsharp.MetalsharpDirectory)')
  - [UseMarkdown()](#M-Metalsharp-MetalsharpExtensions-UseMarkdown-Metalsharp-MetalsharpDirectory- 'Metalsharp.MetalsharpExtensions.UseMarkdown(Metalsharp.MetalsharpDirectory)')
- [MetalsharpFile](#T-Metalsharp-MetalsharpFile 'Metalsharp.MetalsharpFile')
  - [#ctor(text,filePath)](#M-Metalsharp-MetalsharpFile-#ctor-System-String,System-String- 'Metalsharp.MetalsharpFile.#ctor(System.String,System.String)')
  - [#ctor(text,filePath,metadata)](#M-Metalsharp-MetalsharpFile-#ctor-System-String,System-String,System-Collections-Generic-Dictionary{System-String,System-Object}- 'Metalsharp.MetalsharpFile.#ctor(System.String,System.String,System.Collections.Generic.Dictionary{System.String,System.Object})')
  - [Directory](#P-Metalsharp-MetalsharpFile-Directory 'Metalsharp.MetalsharpFile.Directory')
  - [Extension](#P-Metalsharp-MetalsharpFile-Extension 'Metalsharp.MetalsharpFile.Extension')
  - [FilePath](#P-Metalsharp-MetalsharpFile-FilePath 'Metalsharp.MetalsharpFile.FilePath')
  - [Metadata](#P-Metalsharp-MetalsharpFile-Metadata 'Metalsharp.MetalsharpFile.Metadata')
  - [Name](#P-Metalsharp-MetalsharpFile-Name 'Metalsharp.MetalsharpFile.Name')
  - [Text](#P-Metalsharp-MetalsharpFile-Text 'Metalsharp.MetalsharpFile.Text')
  - [IsChildOf(directory)](#M-Metalsharp-MetalsharpFile-IsChildOf-System-String- 'Metalsharp.MetalsharpFile.IsChildOf(System.String)')
  - [IsDescendantOf(directory)](#M-Metalsharp-MetalsharpFile-IsDescendantOf-System-String- 'Metalsharp.MetalsharpFile.IsDescendantOf(System.String)')
- [MetalsharpFileCollection\`1](#T-Metalsharp-MetalsharpFileCollection`1 'Metalsharp.MetalsharpFileCollection`1')
  - [#ctor()](#M-Metalsharp-MetalsharpFileCollection`1-#ctor 'Metalsharp.MetalsharpFileCollection`1.#ctor')
  - [#ctor(files)](#M-Metalsharp-MetalsharpFileCollection`1-#ctor-System-Collections-Generic-IEnumerable{`0}- 'Metalsharp.MetalsharpFileCollection`1.#ctor(System.Collections.Generic.IEnumerable{`0})')
  - [_items](#F-Metalsharp-MetalsharpFileCollection`1-_items 'Metalsharp.MetalsharpFileCollection`1._items')
  - [Count](#P-Metalsharp-MetalsharpFileCollection`1-Count 'Metalsharp.MetalsharpFileCollection`1.Count')
  - [IsReadOnly](#P-Metalsharp-MetalsharpFileCollection`1-IsReadOnly 'Metalsharp.MetalsharpFileCollection`1.IsReadOnly')
  - [Item](#P-Metalsharp-MetalsharpFileCollection`1-Item-System-Int32- 'Metalsharp.MetalsharpFileCollection`1.Item(System.Int32)')
  - [Add()](#M-Metalsharp-MetalsharpFileCollection`1-Add-`0- 'Metalsharp.MetalsharpFileCollection`1.Add(`0)')
  - [ChildrenOf(directory)](#M-Metalsharp-MetalsharpFileCollection`1-ChildrenOf-System-String- 'Metalsharp.MetalsharpFileCollection`1.ChildrenOf(System.String)')
  - [Clear()](#M-Metalsharp-MetalsharpFileCollection`1-Clear 'Metalsharp.MetalsharpFileCollection`1.Clear')
  - [Contains()](#M-Metalsharp-MetalsharpFileCollection`1-Contains-`0- 'Metalsharp.MetalsharpFileCollection`1.Contains(`0)')
  - [ContainsDirectory(directory)](#M-Metalsharp-MetalsharpFileCollection`1-ContainsDirectory-System-String- 'Metalsharp.MetalsharpFileCollection`1.ContainsDirectory(System.String)')
  - [CopyTo()](#M-Metalsharp-MetalsharpFileCollection`1-CopyTo-`0[],System-Int32- 'Metalsharp.MetalsharpFileCollection`1.CopyTo(`0[],System.Int32)')
  - [DescendantsOf(directory)](#M-Metalsharp-MetalsharpFileCollection`1-DescendantsOf-System-String- 'Metalsharp.MetalsharpFileCollection`1.DescendantsOf(System.String)')
  - [GetEnumerator()](#M-Metalsharp-MetalsharpFileCollection`1-GetEnumerator 'Metalsharp.MetalsharpFileCollection`1.GetEnumerator')
  - [IndexOf()](#M-Metalsharp-MetalsharpFileCollection`1-IndexOf-`0- 'Metalsharp.MetalsharpFileCollection`1.IndexOf(`0)')
  - [Insert()](#M-Metalsharp-MetalsharpFileCollection`1-Insert-System-Int32,`0- 'Metalsharp.MetalsharpFileCollection`1.Insert(System.Int32,`0)')
  - [Remove()](#M-Metalsharp-MetalsharpFileCollection`1-Remove-`0- 'Metalsharp.MetalsharpFileCollection`1.Remove(`0)')
  - [RemoveAll()](#M-Metalsharp-MetalsharpFileCollection`1-RemoveAll-System-Predicate{`0}- 'Metalsharp.MetalsharpFileCollection`1.RemoveAll(System.Predicate{`0})')
  - [RemoveAt()](#M-Metalsharp-MetalsharpFileCollection`1-RemoveAt-System-Int32- 'Metalsharp.MetalsharpFileCollection`1.RemoveAt(System.Int32)')
  - [System#Collections#IEnumerable#GetEnumerator()](#M-Metalsharp-MetalsharpFileCollection`1-System#Collections#IEnumerable#GetEnumerator 'Metalsharp.MetalsharpFileCollection`1.System#Collections#IEnumerable#GetEnumerator')

<a name='T-Metalsharp-Branch'></a>
## Branch `type`

##### Namespace

Metalsharp

##### Summary

The Branch plugin

Branches a directory for separate plugins to be computed

<a name='M-Metalsharp-Branch-#ctor-System-Action{Metalsharp-MetalsharpDirectory}[]-'></a>
### #ctor(branches) `constructor`

##### Summary



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| branches | [System.Action{Metalsharp.MetalsharpDirectory}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{Metalsharp.MetalsharpDirectory}[]') | The functions defining each branch |

<a name='P-Metalsharp-Branch-_branches'></a>
### _branches `property`

##### Summary

The function-branches

<a name='M-Metalsharp-Branch-Execute-Metalsharp-MetalsharpDirectory-'></a>
### Execute(directory) `method`

##### Summary

Invokes the plugin

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |

<a name='T-Metalsharp-BuildOptions'></a>
## BuildOptions `type`

##### Namespace

Metalsharp

##### Summary

Represents the options when Metalsharp outputs a directory

<a name='P-Metalsharp-BuildOptions-ClearOutputDirectory'></a>
### ClearOutputDirectory `property`

##### Summary

Whether Metalsharp should remove all the files in the output directory before writing any

<a name='P-Metalsharp-BuildOptions-OutputDirectory'></a>
### OutputDirectory `property`

##### Summary

The directory to which the files will be output

<a name='T-Metalsharp-Collections'></a>
## Collections `type`

##### Namespace

Metalsharp

##### Summary

Collections plugin

Groups files matching a predicate into collections in the directory metadata

<a name='M-Metalsharp-Collections-#ctor-System-String,System-Predicate{Metalsharp-IMetalsharpFile}-'></a>
### #ctor(name,predicate) `constructor`

##### Summary

Instantiate the plugin with a single collection definition

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate to match files for the collection |

<a name='M-Metalsharp-Collections-#ctor-System-ValueTuple{System-String,System-Predicate{Metalsharp-IMetalsharpFile}}[]-'></a>
### #ctor(definitions) `constructor`

##### Summary

Instantiates the plugin with the definitions of the collections

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| definitions | [System.ValueTuple{System.String,System.Predicate{Metalsharp.IMetalsharpFile}}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ValueTuple 'System.ValueTuple{System.String,System.Predicate{Metalsharp.IMetalsharpFile}}[]') | The definitions of the collections, including the name of the collection and the predicate which matches its files |

<a name='F-Metalsharp-Collections-_definitions'></a>
### _definitions `constants`

##### Summary

Contains the definitions of the collections

<a name='M-Metalsharp-Collections-Execute-Metalsharp-MetalsharpDirectory-'></a>
### Execute(directory) `method`

##### Summary

Invokes the plugin

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |

<a name='T-Metalsharp-Debug'></a>
## Debug `type`

##### Namespace

Metalsharp

##### Summary

The Debug plugin

Writes a log after every Use, outputting the contents of the input and output directories.

<a name='M-Metalsharp-Debug-#ctor'></a>
### #ctor() `constructor`

##### Summary

By default, write debug logs with Debug.WriteLine()

##### Parameters

This constructor has no parameters.

<a name='M-Metalsharp-Debug-#ctor-System-String-'></a>
### #ctor(logPath) `constructor`

##### Summary

Configure Debug to write logs to a log file

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| logPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the log file |

<a name='M-Metalsharp-Debug-#ctor-System-Action{System-String}-'></a>
### #ctor(onLog) `constructor`

##### Summary

Configure Debug to use custom behavior when writing logs

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| onLog | [System.Action{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{System.String}') | The action to execute when writing a log |

<a name='F-Metalsharp-Debug-_onLog'></a>
### _onLog `constants`

##### Summary

The action to execute when writing a log

<a name='F-Metalsharp-Debug-_useCount'></a>
### _useCount `constants`

##### Summary

A count of the number of calls to .Use() against the directory

<a name='M-Metalsharp-Debug-Execute-Metalsharp-MetalsharpDirectory-'></a>
### Execute(directory) `method`

##### Summary

Invokes the plugin

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |

<a name='M-Metalsharp-Debug-WriteDirectory-Metalsharp-IMetalsharpFileCollection{Metalsharp-MetalsharpFile}-'></a>
### WriteDirectory(directory) `method`

##### Summary

Prettify the contents of a collection of files

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.IMetalsharpFileCollection{Metalsharp.MetalsharpFile}](#T-Metalsharp-IMetalsharpFileCollection{Metalsharp-MetalsharpFile} 'Metalsharp.IMetalsharpFileCollection{Metalsharp.MetalsharpFile}') |  |

<a name='T-Metalsharp-Frontmatter'></a>
## Frontmatter `type`

##### Namespace

Metalsharp

##### Summary

The Frontmatter plugin

Adds any YAML or JSON frontmatter in the input files to the metadata

<a name='M-Metalsharp-Frontmatter-Execute-Metalsharp-MetalsharpDirectory-'></a>
### Execute(directory) `method`

##### Summary

Invokes the plugin

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |

<a name='M-Metalsharp-Frontmatter-TryGetFrontmatter-System-String,System-Collections-Generic-Dictionary{System-String,System-Object}@,System-String@-'></a>
### TryGetFrontmatter(document,frontmatter,remainder) `method`

##### Summary

Try to parse YAML or JSON frontmatter

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| document | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The document containing frontmatter |
| frontmatter | [System.Collections.Generic.Dictionary{System.String,System.Object}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{System.String,System.Object}@') | The parsed frontmatter |
| remainder | [System.String@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String@ 'System.String@') | The document minus the frontmatter text |

<a name='M-Metalsharp-Frontmatter-TryGetJsonFrontmatter-System-String,System-Collections-Generic-Dictionary{System-String,System-Object}@,System-String@-'></a>
### TryGetJsonFrontmatter(document,frontmatter,remainder) `method`

##### Summary

Try to parse JSON frontmatter

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| document | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The document containing frontmatter |
| frontmatter | [System.Collections.Generic.Dictionary{System.String,System.Object}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{System.String,System.Object}@') | The parsed frontmatter |
| remainder | [System.String@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String@ 'System.String@') | The document minus the frontmatter text |

<a name='M-Metalsharp-Frontmatter-TryGetYamlFrontmatter-System-String,System-Collections-Generic-Dictionary{System-String,System-Object}@,System-String@-'></a>
### TryGetYamlFrontmatter(document,frontmatter,remainder) `method`

##### Summary

Try to parse YAML frontmatter

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| document | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The document containing frontmatter |
| frontmatter | [System.Collections.Generic.Dictionary{System.String,System.Object}@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{System.String,System.Object}@') | The parsed frontmatter |
| remainder | [System.String@](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String@ 'System.String@') | The document minus the frontmatter text |

<a name='T-Metalsharp-IEnumerableExtensions'></a>
## IEnumerableExtensions `type`

##### Namespace

Metalsharp

##### Summary

MetalsharpFileCollection extensions for IEnumerable

<a name='M-Metalsharp-IEnumerableExtensions-ToMetalsharpFileCollection``1-System-Collections-Generic-IEnumerable{``0}-'></a>
### ToMetalsharpFileCollection\`\`1(list) `method`

##### Summary

Mimic IEnumerable.ToList

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| list | [System.Collections.Generic.IEnumerable{\`\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{``0}') | The IEnumerable to convert to an IMetalsharpFileCollection |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of the collection |

<a name='T-Metalsharp-IMetalsharpFile'></a>
## IMetalsharpFile `type`

##### Namespace

Metalsharp

##### Summary

Represents the interface for a Metalsharp file

<a name='P-Metalsharp-IMetalsharpFile-Directory'></a>
### Directory `property`

##### Summary

The directory of the file relative to the source directory

<a name='P-Metalsharp-IMetalsharpFile-Extension'></a>
### Extension `property`

##### Summary

The extension from the file name

<a name='P-Metalsharp-IMetalsharpFile-FilePath'></a>
### FilePath `property`

##### Summary

The path of the file

<a name='P-Metalsharp-IMetalsharpFile-Metadata'></a>
### Metadata `property`

##### Summary

Metadata from the file

<a name='P-Metalsharp-IMetalsharpFile-Name'></a>
### Name `property`

##### Summary

The name of the file, without the extension

<a name='P-Metalsharp-IMetalsharpFile-Text'></a>
### Text `property`

##### Summary

The text of the file

<a name='M-Metalsharp-IMetalsharpFile-IsChildOf-System-String-'></a>
### IsChildOf(directory) `method`

##### Summary

Returns true if the directory is the parent of the file

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory in question |

<a name='M-Metalsharp-IMetalsharpFile-IsDescendantOf-System-String-'></a>
### IsDescendantOf(directory) `method`

##### Summary

Returns true if the directory is an ancestor of the file

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory in question |

<a name='T-Metalsharp-IMetalsharpFileCollection`1'></a>
## IMetalsharpFileCollection\`1 `type`

##### Namespace

Metalsharp

##### Summary

Represents the interface for a collection of Metalsharp files

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T |  |

<a name='M-Metalsharp-IMetalsharpFileCollection`1-ChildrenOf-System-String-'></a>
### ChildrenOf(directory) `method`

##### Summary

Get the children files of a directory

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The parent directory |

<a name='M-Metalsharp-IMetalsharpFileCollection`1-ContainsDirectory-System-String-'></a>
### ContainsDirectory(directory) `method`

##### Summary

Returns true if one of the files in the collection descends from the directory

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory in question |

<a name='M-Metalsharp-IMetalsharpFileCollection`1-DescendantsOf-System-String-'></a>
### DescendantsOf(directory) `method`

##### Summary

Get the descendant files of a directory

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ancestor directory |

<a name='M-Metalsharp-IMetalsharpFileCollection`1-RemoveAll-System-Predicate{`0}-'></a>
### RemoveAll() `method`

##### Summary

Alias List.RemoveAll

##### Parameters

This method has no parameters.

<a name='T-Metalsharp-IMetalsharpPlugin'></a>
## IMetalsharpPlugin `type`

##### Namespace

Metalsharp

##### Summary

Represents a Metalsharp plugin

<a name='M-Metalsharp-IMetalsharpPlugin-Execute-Metalsharp-MetalsharpDirectory-'></a>
### Execute(directory) `method`

##### Summary

Invokes the plugin. Called by Metalsharp.Use

##### Returns

The same directory as was input

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') | The directory to alter |

<a name='T-Metalsharp-Markdown'></a>
## Markdown `type`

##### Namespace

Metalsharp

##### Summary

The Markdown plugin

Converts any markdown files to HTML

<a name='M-Metalsharp-Markdown-Execute-Metalsharp-MetalsharpDirectory-'></a>
### Execute(directory) `method`

##### Summary

Invokes the plugin

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |

<a name='T-Metalsharp-MetalsharpDirectory'></a>
## MetalsharpDirectory `type`

##### Namespace

Metalsharp

##### Summary

Represents a root directory to be manipulated by Metalsharp plugins

<a name='M-Metalsharp-MetalsharpDirectory-#ctor'></a>
### #ctor() `constructor`

##### Summary

Instantiate an empty MetalsharpDirectory

##### Parameters

This constructor has no parameters.

<a name='M-Metalsharp-MetalsharpDirectory-#ctor-System-String-'></a>
### #ctor(path) `constructor`

##### Summary

Instantiate Metalsharp from an existing directory

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the directory |

<a name='M-Metalsharp-MetalsharpDirectory-#ctor-System-String,System-String-'></a>
### #ctor(diskPath,virtualPath) `constructor`

##### Summary

Instantiate Metalsharp from an existing directory and add the contents to a specific virtual path

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| diskPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the files on disk to add |
| virtualPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path of the virtual directory to put the input files into |

<a name='P-Metalsharp-MetalsharpDirectory-InputFiles'></a>
### InputFiles `property`

##### Summary

The input files

<a name='P-Metalsharp-MetalsharpDirectory-Metadata'></a>
### Metadata `property`

##### Summary

The directory-level metadata

<a name='P-Metalsharp-MetalsharpDirectory-OutputFiles'></a>
### OutputFiles `property`

##### Summary

The files to output

<a name='M-Metalsharp-MetalsharpDirectory-AddExisting-System-String,System-String,System-Action{Metalsharp-MetalsharpFile}-'></a>
### AddExisting(diskPath,virtualPath,add) `method`

##### Summary

Add an existing file to the input or output and place the files in a specific virtual path

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| diskPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the file or directory |
| virtualPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the virtual directory to place the files in |
| add | [System.Action{Metalsharp.MetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{Metalsharp.MetalsharpFile}') | The function to add the file |

<a name='M-Metalsharp-MetalsharpDirectory-AddInput-System-String-'></a>
### AddInput(path) `method`

##### Summary

Add a file or all the files in a directory to the input

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the file or directory |

<a name='M-Metalsharp-MetalsharpDirectory-AddInput-System-String,System-String-'></a>
### AddInput(diskPath,virtualPath) `method`

##### Summary

Add a file or all the files in a directory to the input and place the files in a specific virtual path

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| diskPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the file or directory |
| virtualPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the virtual directory to place the files in |

<a name='M-Metalsharp-MetalsharpDirectory-AddInput-Metalsharp-MetalsharpFile-'></a>
### AddInput(file) `method`

##### Summary

Add a MetalsharpFile to the input files

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| file | [Metalsharp.MetalsharpFile](#T-Metalsharp-MetalsharpFile 'Metalsharp.MetalsharpFile') | The file to add |

<a name='M-Metalsharp-MetalsharpDirectory-AddOutput-System-String-'></a>
### AddOutput(path) `method`

##### Summary

Add a file or all the files in a directory directly to the output

The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the file or directory |

<a name='M-Metalsharp-MetalsharpDirectory-AddOutput-System-String,System-String-'></a>
### AddOutput(diskPath,virtualPath) `method`

##### Summary

Add a file or all the files in a directory directly to the output and place the files in a specific virtual path

The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| diskPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the file or directory |
| virtualPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the virtual directory to place the files in |

<a name='M-Metalsharp-MetalsharpDirectory-AddOutput-Metalsharp-MetalsharpFile-'></a>
### AddOutput(file) `method`

##### Summary

Add a MetalsharpFile to output files

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| file | [Metalsharp.MetalsharpFile](#T-Metalsharp-MetalsharpFile 'Metalsharp.MetalsharpFile') | The file to add |

<a name='M-Metalsharp-MetalsharpDirectory-Build'></a>
### Build() `method`

##### Summary

Write all the output files to the default output directory
with default build options

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpDirectory-Build-Metalsharp-BuildOptions-'></a>
### Build(options) `method`

##### Summary

Write all the output files to the output directory

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| options | [Metalsharp.BuildOptions](#T-Metalsharp-BuildOptions 'Metalsharp.BuildOptions') | Metalsmith build configuration options |

<a name='M-Metalsharp-MetalsharpDirectory-Build-System-Action{Metalsharp-MetalsharpDirectory}-'></a>
### Build(func) `method`

##### Summary

Write all the output files to the default output directory after performing a function
with default build options

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| func | [System.Action{Metalsharp.MetalsharpDirectory}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{Metalsharp.MetalsharpDirectory}') | The function to perform |

<a name='M-Metalsharp-MetalsharpDirectory-Build-System-Action{Metalsharp-MetalsharpDirectory},Metalsharp-BuildOptions-'></a>
### Build(func,options) `method`

##### Summary

Write all the output files to the output directory after performing a function

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| func | [System.Action{Metalsharp.MetalsharpDirectory}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{Metalsharp.MetalsharpDirectory}') | The function to perform |
| options | [Metalsharp.BuildOptions](#T-Metalsharp-BuildOptions 'Metalsharp.BuildOptions') | Metalsmith build configuration options |

<a name='M-Metalsharp-MetalsharpDirectory-GetFileWithNormalizedDirectory-System-String,System-String-'></a>
### GetFileWithNormalizedDirectory(diskPath,virtualPath) `method`

##### Summary

Gets a MetalsharpFile with the RootDirectory removed from its path

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| diskPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the file or directory |
| virtualPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the virtual directory to place the files in |

<a name='M-Metalsharp-MetalsharpDirectory-Meta-System-String,System-Object-'></a>
### Meta(key,value) `method`

##### Summary

Add or alter a single item of metadata

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The key to add/update |
| value | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The value to store with the key |

<a name='M-Metalsharp-MetalsharpDirectory-Meta-System-ValueTuple{System-String,System-Object}[]-'></a>
### Meta(pairs) `method`

##### Summary

Add or alter directory-level metadata

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pairs | [System.ValueTuple{System.String,System.Object}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ValueTuple 'System.ValueTuple{System.String,System.Object}[]') | The key-value pairs to add/update |

<a name='M-Metalsharp-MetalsharpDirectory-MoveFiles-System-String,System-String-'></a>
### MoveFiles(oldDirectory,newDirectory) `method`

##### Summary

Move files in the input and output from one directory to another

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| oldDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files from |
| newDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files into |

<a name='M-Metalsharp-MetalsharpDirectory-MoveFiles-System-Predicate{Metalsharp-IMetalsharpFile},System-String-'></a>
### MoveFiles(predicate,newDirectory) `method`

##### Summary

Move files matching a predicate in the input and output from one directory to another

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate to match the files to move |
| newDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files into |

<a name='M-Metalsharp-MetalsharpDirectory-MoveInput-System-String,System-String-'></a>
### MoveInput(oldDirectory,newDirectory) `method`

##### Summary

Move files in the input from one directory to another

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| oldDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files from |
| newDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files into |

<a name='M-Metalsharp-MetalsharpDirectory-MoveInput-System-Predicate{Metalsharp-IMetalsharpFile},System-String-'></a>
### MoveInput(predicate,newDirectory) `method`

##### Summary

Move files in the input matching a predicate from one directory to another

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate to match the files to move |
| newDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files into |

<a name='M-Metalsharp-MetalsharpDirectory-MoveOutput-System-String,System-String-'></a>
### MoveOutput(oldDirectory,newDirectory) `method`

##### Summary

Move files in the output from one directory to another

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| oldDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files from |
| newDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files into |

<a name='M-Metalsharp-MetalsharpDirectory-MoveOutput-System-Predicate{Metalsharp-IMetalsharpFile},System-String-'></a>
### MoveOutput(predicate,newDirectory) `method`

##### Summary

Move files in the output matching a predicate from one directory to another

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate to match the files to move |
| newDirectory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory to move the files into |

<a name='M-Metalsharp-MetalsharpDirectory-RemoveFiles-System-String-'></a>
### RemoveFiles(path) `method`

##### Summary

Remove a file from the input and output

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path of the file to remove |

<a name='M-Metalsharp-MetalsharpDirectory-RemoveFiles-System-Predicate{Metalsharp-IMetalsharpFile}-'></a>
### RemoveFiles(predicate) `method`

##### Summary

Remove all the files matching a predicate from the input and output

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate function to identify files to delete |

<a name='M-Metalsharp-MetalsharpDirectory-RemoveInput-System-String-'></a>
### RemoveInput(path) `method`

##### Summary

Remove a file from the input

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path of the file to remove |

<a name='M-Metalsharp-MetalsharpDirectory-RemoveInput-System-Predicate{Metalsharp-IMetalsharpFile}-'></a>
### RemoveInput(predicate) `method`

##### Summary

Remove all the files matching a predicate from the input

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate function to identify files to delete |

<a name='M-Metalsharp-MetalsharpDirectory-RemoveOutput-System-String-'></a>
### RemoveOutput(path) `method`

##### Summary

Remove a file from the output

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path of the file to remove |

<a name='M-Metalsharp-MetalsharpDirectory-RemoveOutput-System-Predicate{Metalsharp-IMetalsharpFile}-'></a>
### RemoveOutput(predicate) `method`

##### Summary

Remove all the files matching a predicate from the output

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate function to identify files to delete |

<a name='M-Metalsharp-MetalsharpDirectory-Use-System-Action{Metalsharp-MetalsharpDirectory}-'></a>
### Use(func) `method`

##### Summary

Invoke a function as a plugin

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| func | [System.Action{Metalsharp.MetalsharpDirectory}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{Metalsharp.MetalsharpDirectory}') | The function to invoke |

<a name='M-Metalsharp-MetalsharpDirectory-Use-Metalsharp-IMetalsharpPlugin-'></a>
### Use(plugin) `method`

##### Summary

Invoke a plugin

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| plugin | [Metalsharp.IMetalsharpPlugin](#T-Metalsharp-IMetalsharpPlugin 'Metalsharp.IMetalsharpPlugin') | The plugin to invoke |

<a name='M-Metalsharp-MetalsharpDirectory-Use``1'></a>
### Use\`\`1() `method`

##### Summary

Invoke a plugin by type

The plugin type must have an empty constructor

##### Returns



##### Parameters

This method has no parameters.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | The type of the plugin to invoke |

<a name='T-Metalsharp-MetalsharpExtensions'></a>
## MetalsharpExtensions `type`

##### Namespace

Metalsharp

##### Summary

Extensions to Metalsharp for invoking included plugins

<a name='M-Metalsharp-MetalsharpExtensions-Branch-Metalsharp-MetalsharpDirectory,System-Action{Metalsharp-MetalsharpDirectory}[]-'></a>
### Branch(directory,branches) `method`

##### Summary

Invoke the Branch plugin

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| branches | [System.Action{Metalsharp.MetalsharpDirectory}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{Metalsharp.MetalsharpDirectory}[]') | The functions to handle each of the branches |

<a name='M-Metalsharp-MetalsharpExtensions-GetCollection-Metalsharp-MetalsharpDirectory,System-String-'></a>
### GetCollection(directory,name) `method`

##### Summary

Get a collection from MetalsharpDirectory Metadata by name

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') | The directory to return the collection from |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection |

<a name='M-Metalsharp-MetalsharpExtensions-GetFilesFromCollection-Metalsharp-MetalsharpDirectory,System-String-'></a>
### GetFilesFromCollection(directory,name) `method`

##### Summary

Get input and output files from a collection by name

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection to return the files from |

<a name='M-Metalsharp-MetalsharpExtensions-GetInputCollection-Metalsharp-MetalsharpDirectory,System-String-'></a>
### GetInputCollection(directory,name) `method`

##### Summary

Get the input files from a collection from MetalsharpDirectory Metadata by name

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') | The directory to return the input files of the collection from |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection |

<a name='M-Metalsharp-MetalsharpExtensions-GetInputFilesFromCollection-Metalsharp-MetalsharpDirectory,System-String-'></a>
### GetInputFilesFromCollection(directory,name) `method`

##### Summary

Get the input files from a collection by name

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection to return the input files from |

<a name='M-Metalsharp-MetalsharpExtensions-GetOutputCollection-Metalsharp-MetalsharpDirectory,System-String-'></a>
### GetOutputCollection(directory,name) `method`

##### Summary

Get the output files from a collection from MetalsharpDirectory Metadata by name

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') | The directory to return the output files of the collection from |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection |

<a name='M-Metalsharp-MetalsharpExtensions-GetOutputFilesFromCollection-Metalsharp-MetalsharpDirectory,System-String-'></a>
### GetOutputFilesFromCollection(directory,name) `method`

##### Summary

Get the output files from a collection by name

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection to return the input files from |

<a name='M-Metalsharp-MetalsharpExtensions-UseCollections-Metalsharp-MetalsharpDirectory,System-String,System-Predicate{Metalsharp-IMetalsharpFile}-'></a>
### UseCollections(directory,name,predicate) `method`

##### Summary

Invoke the Collections plugin with a single collection definition

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the collection to define |
| predicate | [System.Predicate{Metalsharp.IMetalsharpFile}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Predicate 'System.Predicate{Metalsharp.IMetalsharpFile}') | The predicate to match the files for the collection |

<a name='M-Metalsharp-MetalsharpExtensions-UseCollections-Metalsharp-MetalsharpDirectory,System-ValueTuple{System-String,System-Predicate{Metalsharp-IMetalsharpFile}}[]-'></a>
### UseCollections(directory,definitions) `method`

##### Summary

Invoke the Collections plugin with several collection definitions

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| definitions | [System.ValueTuple{System.String,System.Predicate{Metalsharp.IMetalsharpFile}}[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ValueTuple 'System.ValueTuple{System.String,System.Predicate{Metalsharp.IMetalsharpFile}}[]') | The definitions of each collection |

<a name='M-Metalsharp-MetalsharpExtensions-UseDebug-Metalsharp-MetalsharpDirectory-'></a>
### UseDebug(directory) `method`

##### Summary

Invoke the default Debug plugin

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |

<a name='M-Metalsharp-MetalsharpExtensions-UseDebug-Metalsharp-MetalsharpDirectory,System-String-'></a>
### UseDebug(directory,logPath) `method`

##### Summary

Invoke the Debug plugin with a log file to capture the debug logs

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| logPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path to the log file |

<a name='M-Metalsharp-MetalsharpExtensions-UseDebug-Metalsharp-MetalsharpDirectory,System-Action{System-String}-'></a>
### UseDebug(directory,onLog) `method`

##### Summary

Invoke the Debug plugin with custom log behavior

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |
| onLog | [System.Action{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action{System.String}') | The action to execute to log a debug line |

<a name='M-Metalsharp-MetalsharpExtensions-UseFrontmatter-Metalsharp-MetalsharpDirectory-'></a>
### UseFrontmatter(directory) `method`

##### Summary

Invoke the frontmatter plugin

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [Metalsharp.MetalsharpDirectory](#T-Metalsharp-MetalsharpDirectory 'Metalsharp.MetalsharpDirectory') |  |

<a name='M-Metalsharp-MetalsharpExtensions-UseMarkdown-Metalsharp-MetalsharpDirectory-'></a>
### UseMarkdown() `method`

##### Summary

Invoke the Merkdown plugin

##### Returns



##### Parameters

This method has no parameters.

<a name='T-Metalsharp-MetalsharpFile'></a>
## MetalsharpFile `type`

##### Namespace

Metalsharp

##### Summary

Represents a file

<a name='M-Metalsharp-MetalsharpFile-#ctor-System-String,System-String-'></a>
### #ctor(text,filePath) `constructor`

##### Summary

Instantiate a new MetalsharpFile with no metadata

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text of the file |
| filePath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path of the file |

<a name='M-Metalsharp-MetalsharpFile-#ctor-System-String,System-String,System-Collections-Generic-Dictionary{System-String,System-Object}-'></a>
### #ctor(text,filePath,metadata) `constructor`

##### Summary

Instantiate a new MetalsharpFile with the specified metadata

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text of the file |
| filePath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The path of the file |
| metadata | [System.Collections.Generic.Dictionary{System.String,System.Object}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary 'System.Collections.Generic.Dictionary{System.String,System.Object}') | The metadata of the file |

<a name='P-Metalsharp-MetalsharpFile-Directory'></a>
### Directory `property`

##### Summary

THe directory of the file relative to the source directory

<a name='P-Metalsharp-MetalsharpFile-Extension'></a>
### Extension `property`

##### Summary

The extension from the file name

<a name='P-Metalsharp-MetalsharpFile-FilePath'></a>
### FilePath `property`

##### Summary

The path of the file

<a name='P-Metalsharp-MetalsharpFile-Metadata'></a>
### Metadata `property`

##### Summary

Metadata from the file

<a name='P-Metalsharp-MetalsharpFile-Name'></a>
### Name `property`

##### Summary

The name of the file, without the extension

<a name='P-Metalsharp-MetalsharpFile-Text'></a>
### Text `property`

##### Summary

The text of the file

<a name='M-Metalsharp-MetalsharpFile-IsChildOf-System-String-'></a>
### IsChildOf(directory) `method`

##### Summary

Returns true if the directory is the parent of the file

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory in question |

<a name='M-Metalsharp-MetalsharpFile-IsDescendantOf-System-String-'></a>
### IsDescendantOf(directory) `method`

##### Summary

Returns true if the directory is an ancestor of the file

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory in question |

<a name='T-Metalsharp-MetalsharpFileCollection`1'></a>
## MetalsharpFileCollection\`1 `type`

##### Namespace

Metalsharp

##### Summary

Represents a collection of Metalsharp files

Implements members to handle "virtual" directories

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T |  |

<a name='M-Metalsharp-MetalsharpFileCollection`1-#ctor'></a>
### #ctor() `constructor`

##### Summary

Instantiate an empty collection

##### Parameters

This constructor has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-#ctor-System-Collections-Generic-IEnumerable{`0}-'></a>
### #ctor(files) `constructor`

##### Summary

Instantiate a collection with an existing one

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| files | [System.Collections.Generic.IEnumerable{\`0}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{`0}') |  |

<a name='F-Metalsharp-MetalsharpFileCollection`1-_items'></a>
### _items `constants`

##### Summary

The Metalsharp files in the collection

<a name='P-Metalsharp-MetalsharpFileCollection`1-Count'></a>
### Count `property`

##### Summary

Implements IList

<a name='P-Metalsharp-MetalsharpFileCollection`1-IsReadOnly'></a>
### IsReadOnly `property`

##### Summary

Implements IList

<a name='P-Metalsharp-MetalsharpFileCollection`1-Item-System-Int32-'></a>
### Item `property`

##### Summary

Implements IList

<a name='M-Metalsharp-MetalsharpFileCollection`1-Add-`0-'></a>
### Add() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-ChildrenOf-System-String-'></a>
### ChildrenOf(directory) `method`

##### Summary

Get the children files of a directory

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The parent directory |

<a name='M-Metalsharp-MetalsharpFileCollection`1-Clear'></a>
### Clear() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-Contains-`0-'></a>
### Contains() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-ContainsDirectory-System-String-'></a>
### ContainsDirectory(directory) `method`

##### Summary

Returns true if one of the files in the collection descends from the directory

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The directory in question |

<a name='M-Metalsharp-MetalsharpFileCollection`1-CopyTo-`0[],System-Int32-'></a>
### CopyTo() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-DescendantsOf-System-String-'></a>
### DescendantsOf(directory) `method`

##### Summary

Get the descendant files of a directory

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| directory | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The ancestor directory |

<a name='M-Metalsharp-MetalsharpFileCollection`1-GetEnumerator'></a>
### GetEnumerator() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-IndexOf-`0-'></a>
### IndexOf() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-Insert-System-Int32,`0-'></a>
### Insert() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-Remove-`0-'></a>
### Remove() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-RemoveAll-System-Predicate{`0}-'></a>
### RemoveAll() `method`

##### Summary

Implements IMetalsharpCollection

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-RemoveAt-System-Int32-'></a>
### RemoveAt() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.

<a name='M-Metalsharp-MetalsharpFileCollection`1-System#Collections#IEnumerable#GetEnumerator'></a>
### System#Collections#IEnumerable#GetEnumerator() `method`

##### Summary

Implements IList

##### Parameters

This method has no parameters.
