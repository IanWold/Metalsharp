# Quickstart your Metalsharp Project

Metalsharp is a C# library for creating static websites, and it aims to be all the good things — easy to use, easy to extend, and light on ceremony. Metalsharp works by reading in your source files and then invoking a series of plugins, each of which manipulates those files in some small way. Metalsharp exposes a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface), which lets you chain together all the plugins you need in a single expression. If you like that coding style, great — and if you don't, nothing stops you from writing more traditional, imperative code instead.

This quickstart walks through the basics of Metalsharp. [Create a Website with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md) goes into more depth with a practical project.

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

You'll need a C# project targeting .NET 10 or later (a console application works well, but Metalsharp doesn't care what kind of project hosts it). Add a reference to Metalsharp with the .NET CLI:

```plaintext
dotnet add package Metalsharp
```

If you'd rather not set up a full project just to experiment, a [file-based app](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview) is a great fit for Metalsharp — you can reference the package and run a single `.cs` file directly with `dotnet run`. The [Create a Website tutorial](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md) uses exactly this approach.

## Project Structure

The files for your website's content will typically fall into a structure similar to this:

```plaintext
MyProject
├── Site
│   └── The content files of your website (e.g. index.md).
├── Static
│   └── Files that are copied directly to the output directory (e.g. style.css).
├── (Any files and folders irrelevant to your website can live at the top level.)
└── Your C# Metalsharp program executes at this level.
```

None of these directory names is special to Metalsharp — `AddInput` and `AddOutput` will read whatever directory you point them at. If your project needs files that are neither content nor pass-through output — layout templates for a templating library you've brought in yourself, for instance — a directory like `Templates` works fine too; just read it in with `AddInput` like anything else and decide what to do with those files in your own code. Metalsharp doesn't have an opinion on templating (see [Create a Website with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md) for one straightforward approach that needs no template files at all).

## Using Metalsharp

Everything you'll do to generate your website revolves around the `MetalsharpProject` object. It holds a list of input files, a list of output files, and project-level metadata, along with the methods described below.

### Files

The first step is to add files. `AddInput` reads all the files in a directory (or a single file) from disk and places them into the input list:

```c#
new MetalsharpProject()
    .AddInput("Site")
```

If you want Metalsharp to remember the files under a different virtual directory than the one they're read from, supply a second argument:

```c#
.AddInput("Site", "New\\Path\\In\\Metalsharp")
```

Files destined for the output list — content that should pass through to the built site without further processing — are added the same way, but with `AddOutput`:

```c#
.AddOutput("Directory\\On\\Disk")
```

You can also move files from one virtual directory to another:

```c#
.MoveFiles("Directory\\On\\Disk", "New\\Path\\In\\Metalsharp")
```

This affects files in both the input and output lists. If you only want to move files in one list, use `MoveInput` or `MoveOutput` instead:

```c#
.MoveInput("Directory\\On\\Disk", "New\\Path\\In\\Metalsharp")
.MoveOutput("Directory\\On\\Disk", "New\\Path\\In\\Metalsharp")
```

Removing files works the same way:

```c#
.RemoveFiles("delete-this-file.md")
.RemoveInput("delete-this-file.md")
.RemoveOutput("delete-this-file.md")
```

### Metadata

*Metadata* is data associated with a file that isn't part of the file's own text. Every `MetalsharpFile` has a `Dictionary<string, object>` property called `Metadata`, and `MetalsharpProject` has a `Metadata` property of its own for metadata that applies to the whole project.

`MetalsharpProject.Meta` lets you set project-level metadata:

```c#
.Meta("my metadata", "hello!")
```

The `Frontmatter` plugin parses each file's YAML or JSON frontmatter out of its text and merges it into that file's metadata:

```c#
.UseFrontmatter()
```

Which brings us to...

### Plugins

Plugins are invoked by calling `MetalsharpProject.Use`. Using `Frontmatter` as an example, there are three ways to invoke a plugin:

1. By referencing its type, if it has a public parameterless constructor:

    ```c#
    .Use<Frontmatter>()
    ```

2. By passing an instance:

    ```c#
    .Use(new Frontmatter())
    ```

3. By using an extension method the plugin provides, if one exists:

    ```c#
    .UseFrontmatter()
    ```

Metalsharp ships with a handful of fundamental plugins, described below.

> If you think you can write a better version of one of these plugins, please do — and consider publishing it! Metalsharp is meant to be a platform for plugins built by the community; the built-in plugins exist mainly to give the library a useful footing out of the box.

`Collections` groups files matching a predicate into named collections, stored in the project's metadata. The following creates a collection of all Markdown files:

```c#
.UseCollections("markdown", file => file.Extension == ".md")
```

`Debug` logs Metalsharp's internal events, which makes plugin pipelines easier to troubleshoot:

```c#
.UseDebug(log => Console.WriteLine(log));
```

`Frontmatter`, as covered above, parses frontmatter out of each file's text and merges it into that file's metadata:

```c#
.UseFrontmatter()
```

`Leveller` adds a `level` metadata record to every file, indicating how many directories deep it sits — handy for computing relative links in templates:

```c#
.UseLeveller()
```

`Markdown` converts Markdown files in the input into HTML files in the output:

```c#
.UseMarkdown()
```

### Building

Once you've invoked all the plugins you need, call `MetalsharpProject.Build` to write the files in the output list to disk:

```c#
.Build();
```

By default, `Build` writes to the current directory and leaves any existing files in place. To change the output directory, or to clear it before writing, configure `MetalsharpProject` when you construct it:

```c#
new MetalsharpProject(clearOutputDirectory: true, outputDirectory: "my\\output\\directory")
```

## Custom Plugins

Let's write a plugin that appends the text "Hello" to every `.txt` file. [Create a Plugin for Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md) goes into more detail on developing and publishing a practical plugin.

### Via `IMetalsharpPlugin`

Every published Metalsharp plugin implements (or at least should implement) `IMetalsharpPlugin`. This interface requires a single method, `Execute`, which is called when the plugin is invoked.

```c#
using System.Text;

public class SayHi : IMetalsharpPlugin
{
    public void Execute(MetalsharpProject project)
    {
        foreach (var file in project.InputFiles.Concat(project.OutputFiles))
        {
            if (file.Extension == ".txt")
            {
                file.Contents = Encoding.Default.GetBytes(file.Text + "Hello");
            }
        }
    }
}
```

> `MetalsharpFile.Text` is a read-only view over `MetalsharpFile.Contents` — to change a file's text, assign new bytes to `Contents` instead.

This can then be invoked like any other plugin:

```c#
.Use(new SayHi())
.Use<SayHi>()
```

And you can write an extension method to support the fluent style:

```c#
public static class SayHiPluginExtensions
{
    public static MetalsharpProject UseSayHi(this MetalsharpProject project) =>
        project.Use(new SayHi());
}
```

### Via Function

`MetalsharpProject.Use` has an overload that accepts a plain function, so if you don't intend to publish a plugin, you don't need a class at all.

```c#
static void SayHi(MetalsharpProject project)
{
    foreach (var file in project.InputFiles.Concat(project.OutputFiles))
    {
        if (file.Extension == ".txt")
        {
            file.Contents = Encoding.Default.GetBytes(file.Text + "Hello");
        }
    }
}
```

And use it as a method group:

```c#
.Use(SayHi)
```

Or write it inline as a lambda:

```c#
.Use(project =>
{
    foreach (var file in project.InputFiles.Concat(project.OutputFiles))
    {
        if (file.Extension == ".txt")
        {
            file.Contents = Encoding.Default.GetBytes(file.Text + "Hello");
        }
    }
})
```

---

Now you've covered the basics of Metalsharp! Ready for more? [Create a Website with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md) puts all of this together into a complete project.

Noticed a typo, an outdated example, or anything else that could make this clearer? [Editing this page](https://github.com/IanWold/Metalsharp/edit/master/Metalsharp.Documentation/quickstart.md) and submitting a PR is a great way to contribute to Metalsharp!
