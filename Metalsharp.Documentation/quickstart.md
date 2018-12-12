# Quickstart your Metalsharp Project

Metalsharp is a C# library for creating static websites that claims to be all the good things - easy to use, easy to extend, light, fast, so on and so forth. Metalsharp work by reading in your source files and then invoking several plugins which each manipulate those files in small ways. Further, Metalsharp provides something called a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface) to allow you to do this - that means that you can invoke all the plugins you need to by chaining method calls together. If you like this coding style, that's great! But if you don't like it, no pressure, you can code in a more traditional manner if you like.

This quickstart will walk you through all the basics in Metalsharp. The tutorial [Create a Website with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md) goes into more detail on a practical project.

## Content

* [Acquiring Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#acquiring-metalsharp)
* [Project Structure](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#project-structure)
* [Using Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#using-metalsharp)
  * [Files](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#files)
  * [Metadata](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#metadata)
  * [Plugins](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#plugins)
  * [Building](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#building)
* [Custom Plugins](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#custom-plugins)
  * [Via `IMetalsharpPlugin`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#via-imetalsharpplugin)
  * [Via Function](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md#via-function)

## Acquiring Metalsharp

You'll need a C# console application (let's call it `MyProject`). This can be either .NET Core or .NET Framework, it shouldn't matter which. You'll need to add an assembly reference to `Metalsharp.dll`, which you can only acquire by building Metalsharp from source.

When Metalsharp hits `v0.9.0-rc1`, it will be available on NuGet, but until then, you'll need to build from source and add an assembly reference.

## Project Structure

The files for your website content could/should fall into a structure like the following:

```plaintext
MyProject
├── Site
│   └── The content files of your website (eg. index.md).
├── Static
│   └── Files that will be copied directly to the output directory (eg. style.css).
├── Templates
│   └── Files that will be used by your project but not output should be placed in top-level directories (like templates).
├── Files and folders irrelevant to your website can go at the top-level
└── Your C# Metalsharp application should execute at this level.
```

## Using Metalsharp

Everything you'll do towards generating your website will be with the `MetalsharpDirectory` object. This object has a list of input files, a list of output files, and metadata, as well as several methods detailed below.

### Files

The first step is to add files. We can do that right when we instantiate `MetalsharpDirectory`:

```c#
new MetalsharpDirectory("Site")
```

This reads all the files in the `Site` directory on disk and places them into the input list. If you want to change the directory stored with the file in Metalsharp, you can do that to:

```c#
new MetalsharpDirectory("Site", "New\\Path\\In\\Metalsharp")
```

If you need to add more files to the input:

```c#
.AddInput("Directory\\On\\Disk")
```

And if you need to add files to the output list:

```c#
.AddOutput("Directory\\On\\Disk")
```

You can move files inside Metalsharp from one directory to another:

```c#
.MoveFiles("Directory\\On\\Disk", "New\\Path\\In\\Metalsharp")
```

This work on files in both the input and output list. If you want to segregate which list you're working with:

```c#
.MoveInput("Directory\\On\\Disk", "New\\Path\\In\\Metalsharp")
.MoveOutput("Directory\\On\\Disk", "New\\Path\\In\\Metalsharp")
```

And deleting files works very much the same:

```c#
.RemoveFiles("delete-this-file.md")
.RemoveInput("delete-this-file.md")
.RemoveOutput("delete-this-file.md")
```

### Metadata

*Metadata* is data associated with a file that does not belong in the text of the file. Each `MetalsharpFile` has a `Dictionary<string, object>` property called `Metadata` to store metadata records. `MetalsharpDirectory` also has a `Metadata` property to store metadata for the whole project.

`MetalsharpDirectory.Meta` allows you to create metadata for the project:

```c#
.Meta("my metadata", "hello!")
```

The `Frontmatter` plugin parses each file's frontmatter from its text and insert it into the file's metadata.

```c#
.UseFrontmatter()
```

Which brings us to...

### Plugins

Plugins are invoked by calling `MetalsharpDirectory.Use`. Using the `Frontmatter` plugin as an example, there are three ways to call a plugin:

1. By referencing its type

```c#
.Use<Frontmatter>()
```

2. By using an instance

```c#
.Use(new Frontmatter())
```

3. By using an extension method provided by the plugin

```c#
.UseFrontmatter()
```

Metalsharp comes with a few fundamental plugins, described here.

> Side note: if you think you can create a better plugin to replace one of the built-in ones, please do make it and publish it! It would be better for Metalsharp as a whole if all of the plugins are community-made. These just exist to give Metalsharp a footing in the early stages.

`Branch` allows you to copy the `MetalsharpDirectory` you're working with a number of times for different functions. The following will build the project to two separate files.

```c#
.Branch(
    dir => dir.Build(new BuildOptions { OutputDirectory = "output" }),
    dir => dir.Build(new BuildOptions { OutputDirectory = "build" })
)
```

`Collections` allows you to create collections of your files. The following creates a collection of all markdown files.

```c#
.UseCollections("markdown", file => file.Extension == ".md")
```

`Debug` allows you to log Metalsharp events to make it easier to debug.

```c#
.UseDebug(log => Console.WriteLine(log));
```

`Frontmatter`, as we discussed, parses the frontmatter out of the text of each of your files and inserts it into that file's metadata.

```c#
.UseFrontmatter()
```

`Markdown` searches for any Markdown files in the input list and creates an HTML file for each in the output list.

```c#
.UseMarkdown()
```

### Building

To build the project after you've invoked all the plugins you need, just call `MetalsharpDirectory.Build`. This writes the files in the output list to the current directory.

```c#
.Build();
```

Optionally, you can execute a function on the directory before you build.

```c#
.Build(dir => dir.AddOutput("my-last-minute-file.txt"));
```

And if you want to change which directory the files write to, or if you want to delete all the files in the output directory before you write any, you can give `Build` a `BuildOptions` object with the settings you want:

```c#
.Build(new BuildOptions { OutputDirectory = "my\\output\\directory", ClearOutputDirectory = true });
```

## Custom Plugins

Let's develop a plugin that inserts the text "Hello" into every `.txt` file called `SayHi`. The tutorial [Create a Plugin for Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md) goes into more detail on creating and publishing a practical plugin.

### Via `IMetalsharpPlugin`

Every published Metalsharp plugin is (or at least should be) implemented by implementing `IMetalsharpPlugin`. This interface requires one method, `Execute`, which is called when the plugin is invoked.

```c#
public class SayHi : IMetalsharpPlugin
{
    public void Execute(MetalsharpDirectory directory)
    {
        foreach (var file in directory.InputFiles.Concat(directory.OutputFiles))
        {
            if (file.Extension == ".txt") file.Text += "Hello";
        }
    }
}
```

And then this can be invoked as you regularly would:

```c#
.Use(new SayHi())
.Use<SayHi>()
```

And we can write an extension to support it:

```c#
public static class Extensions
{
    public static MetalsharpDirectory UseSayHi(this MetalsharpDirectory directory) =>
        directory.Use(new SayHi());
}
```

### Via Function

`MetalsharpDirectory.Use` has an overload that allows us to pass in a function. If we didn't want to publish this plugin, we could just make it a function.

```c#
public void SayHi(MetalsharpDirectory directory)
{
    foreach (var file in directory.InputFiles.Concat(directory.OutputFiles))
    {
        if (file.Extension == ".txt") file.Text += "Hello";
    }
}
```

And we can use it as a delegate.

```c#
.Use(SayHi)
```

Or, we could even write it as a lambda.

```c#
.Use(directory =>
{
    foreach (var file in directory.InputFiles.Concat(directory.OutputFiles))
    {
        if (file.Extension == ".txt") file.Text += "Hello";
    }
})
```

---

Now you've mastered the basics of Metalsharp! Congrats!

Did you notice something odd with this tutorial - a typo, old information, or just something that could make it a bit better? [Editing this tutorial](https://github.com/IanWold/Metalsharp/edit/master/Metalsharp.Documentation/quickstart.md) and submitting a PR would be a great way to contribute to Metalsharp!
