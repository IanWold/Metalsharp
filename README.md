<h1><img src="Metalsharp.png" height="70"/></h1>

[![](https://img.shields.io/nuget/vpre/Metalsharp.svg?logo=nuget&logoColor=white&style=flat-square)](https://www.nuget.org/packages/Metalsharp/)
[![AppVeyor Build](https://img.shields.io/appveyor/ci/ianwold/metalsharp.svg?logo=appveyor&logoColor=white&style=flat-square)](https://ci.appveyor.com/project/IanWold/metalsharp)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/ianwold/metalsharp.svg?logo=appveyor&logoColor=white&style=flat-square)](https://ci.appveyor.com/project/IanWold/metalsharp/build/tests)
[![Discord](https://img.shields.io/discord/517023630224523274.svg?logo=discord&logoColor=white&style=flat-square)](https://discord.gg/HrxyfFP)

A tiny and extendable C# library for generating static sites, inspired by [Metalsmith](http://www.metalsmith.io/). Metalsharp is guided by three basic principles:

1. **Small footprint**: Use the smallest amount of code necessary,
1. **Extendable**: Make it as easy as possible for anyone to develop and release plugins, and
1. **Approachable**: Maintain thorough documentation to keep the code easy to read and the library easy to learn

Generating a website from a directory is as simple as the following (from [ExampleWebsite](https://github.com/IanWold/Metalsharp/tree/master/Metalsharp.Examples/Metalsharp.ExampleWebsite)):

```c#
new MetalsharpDirectory("Site")
    .Use<Frontmatter>()
    .Use(new Drafts())
    .UseMarkdown()
    .AddOutput("Static")
    .Build();
```

## Getting Started

To get Metalsharp, you can either build it from source, or get it on [NuGet](https://www.nuget.org/packages/Metalsharp/).

```plaintext
PM> Install-Package Metalsharp -Version 0.9.0-rc.1
```

> Note that you will need NuGet 4.3.0 or higher to install Metalsharp with NuGet.

### Configuring a Metalsharp Project Directory

It is recommended that a Metalsharp project use something like the following directory structure.

```text
ProjectFolder
├── Site
│   ├── SomeFile.md
│   └── SomeOtherFile.md
├── Static
│   └── style.css
└── README.md
```

Here we have a project in a `ProjectFolder` directory. At that level, you can place files irrelevant to the resultant website. Site content can go in a `Site` folder, and content that will be copied right through to the output can go in a `Static` folder. None of these are requirements, and as your mileage varies you can implement whichever structure you need.

### Using Metalsharp

Let's walk though the example at the top. The [quickstart](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/quickstart.md) can give you a more thorough glimpse at Metalsharp, and there's also a tutorial to [create a practical website](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-website.md).

1. Instantiate a new `MetalsharpProject` with the directory containing the files you want to manipulate:

```c#
new MetalsharpProject("Site")
```

2. You can add a plugin by referencing its type if it has an empty constructor, like the `Frontmatter` plugin. This one will add a file's frontmatter to its metadata:

```c#
.Use<Frontmatter>()
```

3. If a plugin does not have an empty constructor, or if you prefer this syntax, you'll either need to use a provided extension method, or instantiate the plugin yourself. The `Drafts` plugin (one of the [example plugins](https://github.com/IanWold/Metalsharp/tree/master/Metalsharp.Examples/Metalsharp.ExamplePlugin)) removes files marked as drafts.

```c#
.Use(new Drafts())
```

4. And if an extension to `Metalsharp` exists for a plugin, as does for [all the plugins that come with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp/Plugins/MetalsharpExtensions.cs), you can use that extension method. `Markdown` converts Markdown files in the input to HTML files in the output.

```c#
.UseMarkdown()
```

5. Finally, we've got a `Static` folder with files we want to copy right through to the output, so let's include those in the output:

```c#
.AddOutput("Static")
```

6. When you've configured your plugin pipeline, call `Build` to execute the stack:

```c#
.Build();
```

## Creating a Custom Plugin

Creating a Metalsharp plugin is very easy. [This tutorial](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/tutorial-plugin.md) demonstrates how to develop and publish a plugin. Fundamentally, all you need to do is implement [`IMetalsharpPlugin`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp/Interfaces/IMetalsharpPlugin.cs). Below is the code for the `Markdown` plugin:

```c#
public class Markdown : IMetalsharpPlugin
{
    public void Execute(MetalsharpDirectory project)
    {
        foreach (var file in project.InputFiles)
        {
            var name = Path.GetFileNameWithoutExtension(file.Path);
            var text = Markdig.Markdown.ToHtml(file.FileText);
            project.OutputFiles.Add(new OutputFile(name + ".html", text) { Metadata = file.Metadata });
        }
    }
}
```

## Docs

[Metalsharp.Documentation](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp.Documentation/README.md) houses generated API docs and hand-written tutorials.

The source code is fullly documented with XML comments, and [XmlDocToMarkdown](https://github.com/ianwold/XmlDocToMarkdown) is used to generate a markdown dump of all the API documentation.

If you notice any issues or potential improvements in the documentation or tutorials, please edit the file(s) and submit a PR, it would be a huge help! If you don't have the time or skill to do that, then opening an issue would be awesome too.

If you have a question or need help using Metalsharp, please do not open an issue. Rather, head on over to the [Metalsharp Discord](https://discord.gg/HrxyfFP) for general questions and help.

## Contributing

If you'd like to contribute, please do! Open an Issue, submit a PR, I'm happy however you want to contribute.

## Roadmap

`v0.9.0-rc.1` is on NuGet! The current goal is to test the library thoroughly in as many situations as possible to be sure of its design and usability. This will involve generating a GitHub Pages homepage for Metalsharp, and it may involve writing integration tests. `v0.9.0` will be released to Nuget when there is a concensus that Metalsharp is designed correctly.

After `v0.9.0`, it's on to `v1.0.0`!

## But why is this necessary?

Fun
