<h1><img src="Metalsharp.png" height="70"/></h1>

[![AppVeyor Build](https://img.shields.io/appveyor/ci/ianwold/metalsharp.svg?logo=appveyor&logoColor=white&style=flat-square)](https://ci.appveyor.com/project/IanWold/metalsharp)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/ianwold/metalsharp.svg?logo=appveyor&logoColor=white&style=flat-square)](https://ci.appveyor.com/project/IanWold/metalsharp/build/tests)
[![Codacy grade](https://img.shields.io/codacy/grade/29c18f6bc08146e884e173ad1262cead.svg?logo=codacy&logoColor=white&style=flat-square)](https://app.codacy.com/project/IanWold/Metalsharp/dashboard) 
[![Discord](https://img.shields.io/discord/517023630224523274.svg?logo=discord&logoColor=white&style=flat-square)](https://discord.gg/HrxyfFP)

A tiny and extendable C# library for generating static sites, inspired by [Metalsmith](http://www.metalsmith.io/).

This project is brand-new - like, 0.0.1 new - so things will be added fast. I hope. Probably not. The good thing, though, is that it's so simple that very significant changes are unlikely to happen. That said, if you have a suggestion or want to contribute, now's probably the easiest time for it ;)

Generating a website from a directory is as simple as the following (from [ExampleWebsite](https://github.com/IanWold/Metalsharp/tree/master/Metalsharp.Examples/Metalsharp.ExampleWebsite)):

```c#
new MetalsharpDirectory("Site")
    .UseFrontmatter()
    .Use<Drafts>()
    .Use(new Markdown())
    .Build();
```

This example uses three plugins: `Frontmatter`, `Drafts`, and `Markdown`, and they demonstrate the three standard ways (syntactically) a Metalsharp plugin can be chained onto each other. `Frontmatter` parses JSON or YAML frontmatter into the metadata of each file, `Drafts` will remove any files marked as a draft, and `Markdown` will convert Markdown files to HTML.

## Getting Started

### Configuring a Metalsharp Project Directory

See [ExampleWebsite](https://github.com/IanWold/Metalsharp/tree/master/Metalsharp.Examples/Metalsharp.ExampleWebsite) for a project implementing this. It is recommended that a Metalsharp project use the following directory structure:

```text
.
├── Site
│   ├── SomeFile.md
│   └── SomeOtherFile.md
├── bin
│   ├── SomeFile.html
│   └── SomeOtherFile.html
├── layout.template
└── README.md
```

Within a root directory is a good place for any files needed to configure plugins, such as `layout.template` for the [example `Layout` plugin](https://github.com/IanWold/Metalsharp/tree/master/Examples/ExamplePlugin). Putting all the site files in a site folder keeps them separated from config files or other project files. The default behavior for Metalsharp is to put output files in a `bin` directory, but you can configure that differently. An output folder separate and outside the folder with site files is best practice.

### Using Metalsharp

Full docs are on their way, in the meantime, here's (basically) how you use this tiny library:

1. Instantiate a new Metalsharp object with the directory containing the files you want to manipulate:

```c#
new MetalsharpDirectory("Site")
```

2. If you want to add a file to the input, you can do that:

```c#
.AddInput("C:/SomeDir/myFile.md")
```

3. You can add a file directly to the output:

```c#
.AddOutput("C:/SomeDir/ForOutput.css")
```

4. And if you have a change of heart you can remove them again too:

```c#
.RemoveInput("C:/SomeDir/myFile.md")
.RemoveOutput("ForOutput.css")
```

5. You can add a plugin by referencing its type if it has an empty constructor, like the `Frontmatter` plugin:

```c#
.Use<Frontmatter>()
```

6. And if an extension to `Metalsharp` exists for a plugin, as does for [all the plugins that come with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp/Plugins/MetalsharpExtensions.cs), you can use that extension method:

```c#
.UseDrafts()
```

7. If a plugin does not have an empty constructor, or if you prefer this syntax, you'll either need to use a provided extension method, or instantiate the plugin yourself like so:

```c#
.Use(new Markdown())
```

8. When you've configured your plugin pipeline, call `Build` to execute the stack:

```c#
.Build();
```

9. If you need to configure your output directory or other options, you can pass those into `Build` using `BuildOptions`:

```c#
.Build(new BuildOptions() { OutputDirectory = "C:/Output" });
```

And with that all your output files should be in your output directory. Here's all the code together:

```c#
new MetalsharpDirectory("Site")
	.AddInput("C:/SomeDir/myFile.md")
	.AddOutput("C:/SomeDir/ForOutput.css")
	.RemoveInput("C:/SomeDir/myFile.md")
	.RemoveOutput("ForOutput.css")
	.Use<Frontmatter>()
	.UseDrafts()
	.Use(new Markdown())
	.Build(new BuildOptions() { OutputDirectory = "C:/Output" });
```

## Creating a Custom Plugin

### By Implementing `IMetalsharpPlugin`

Any plugin just needs to inherit from [`IMetalsharpPlugin`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp/Interfaces/IMetalsharpPlugin.cs). Below is the code for the `Markdown` plugin:

```c#
public class Markdown : IMetalsharpPlugin
{
    public void Execute(MetalsharpDirectory directory)
    {
        foreach (var file in directory.InputFiles)
        {
            var name = Path.GetFileNameWithoutExtension(file.Path);
            var text = Markdig.Markdown.ToHtml(file.FileText);
            directory.OutputFiles.Add(new OutputFile(name + ".html", text) { Metadata = file.Metadata });
        }
    }
}
```

Your `Execute` funtion should always return `directory` at the end, or throw an error.

`directory` is a `MetalsharpDirectory` object and has three properties a plugin can interface with: `Metadata`, `InputFiles`, and `OutputFiles`.

`Metadata` is a `Dictionary<string, object>` which contains any metadata defined by the programmer. `InputFiles` is a list of `InputFile` objects which represent the files in the project folder. `OutputFiles` is a list of `OutpuFile` objects which are all the output files that have been generated by the plugins that ran before yours. Plugins can modify, delete, or add anything to any of these properties to modify the end result.

### With a Function (err... "Action")

If you don't plan on packaging your plugin for others to use, or you need a "quick and dirty" solution, you can just use any `Action<MetalsharpDirectory>` with `Use`. First, implement a function:

```c#
void DeleteEverything(MetalsharpDirectory directory)
{
	directory.InputFiles.Clear();
	directory.OutputFiles.Clear();
	return directory;
}
```

Then add it to the stack with `Use`:

```c#
new MetalsharpDirectory("Site")
	.Use(DeleteEverything)
	...
```

You can also use a lambda, if you wish:

```c#
new MetalsharpDirectory("Site")
	.Use(directory => {
		directory.InputFiles.Clear();
		directory.OutputFiles.Clear();
	})
	...
```

## Docs

Full documentation right around the corner...

## Contributing

If you'd like to contribute, please do! Open an Issue, submit a PR, I'm happy however you want to contribute.

## But why is this necessary?

Fun
