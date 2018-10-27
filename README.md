# ![logo](Metalsharp.png)

A [Metalsmith](http://www.metalsmith.io/)-inspired *"pluggable"* directory manipulator, intended for static website generation, written in C#. It's very small and easy to extend.

This project is brand-new - like, 0.0.1 new - so things will be added fast. I hope. Probably not. The good thing, though, is that it's so simple that very significant changes are unlikely to happen. That said, if you have a suggestion or want to contribute, now's probably the easiest time for it ;)

Generating a website from a directory is as simple as the following (from [Examples/Example1](https://github.com/IanWold/Metalsharp/tree/master/Examples/Example1)):

```c#
new Metalsharp("Site")
    .UseDrafts()
    .Use<Markdown>()
    .Use(new Layout("layout.template"))
    .Build();
```

This example uses three plugins: `Drafts`, `Markdown`, and `Layout`, and they demonstrate the three standard ways (syntactically) a Metalsharp plugin can be chained onto each other. `Drafts` will remove any files marked as a draft, `Markdown` will convert Markdown files to HTML, and `Layout` will fit each of the HTML files into a simple template file.

# Setting up a Directory for Metalsharp

See [Examples/Example1](https://github.com/IanWold/Metalsharp/tree/master/Examples/Example1) for a project implementing this. It is recommended that a Metalsharp project use the following directory structure:

```
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

Within a root directory is a good place for any files needed to configure plugins, such as `layout.template` for the `Layout` plugin. Putting all the site files in a site folder keeps them separated from config files or other project files. The default behavior for Metalsharp is to put output files in a `bin` directory, but you can configure that differently. An output folder separate and outside the folder with site files is best practice.

#  Using Metalsharp

Full docs are on their way, in the meantime, here's (basically) how you use this tiny library:

1. Instantiate a new Metalsharp object with the directory containing the files you want to manipulate:

```c#
new Metalsharp("Site")
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

5. You can add a plugin by referencing its type if it has an empty constructor, like the `Drafts` plugin:

```c#
.Use<Drafts>()
```

6. And if an extension to `Metalsharp` exists for a plugin, as does for [all the plugins that come with Metalsharp](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp/Plugins/MetalsharpExtensions.cs), you can use that extension method:

```c#
.UseMarkdown()
```

7. If a plugin does not have an empty constructor, like the `Layout` plugin, you'll either need to use a provided extension method, or instantiate the plugin yourself like so:

```c#
.Use(new Layout("layout.template"))
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
new Metalsharp("Site")
	.AddInput("C:/SomeDir/myFile.md")
	.AddOutput("C:/SomeDir/ForOutput.css")
	.RemoveInput("C:/SomeDir/myFile.md")
	.RemoveOutput("ForOutput.css")
	.Use<Drafts>()
	.UseMarkdown()
	.Use(new Layout("layout.template"))
	.Build(new BuildOptions() { OutputDirectory = "C:/Output" });
```

# Creating a Custom Plugin

### By Implementing `IMetalsharpPlugin`

Any plugin just needs to inherit from [`IMetalsharpPlugin`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp/IMetalsharpPlugin.cs). Below is the code for the Markdown plugin:

```c#
public class Markdown : IMetalsharpPlugin
{
    Metalsharp IMetalsharpPlugin.Execute(Metalsharp directory)
    {
        foreach (var file in directory.InputFiles)
        {
            var name = Path.GetFileNameWithoutExtension(file.Path);
            var text = Markdig.Markdown.ToHtml(file.FileText);
            directory.OutputFiles.Add(new OutputFile(name + ".html", text) { Metadata = file.Metadata });
        }

        return directory;
    }
}
```

Your `Execute` funtion should always return `directory` at the end, or throw an error.

`directory` is a `Metalsharp` object and has three properties a plugin can interface with: `Metadata`, `InputFiles`, and `OutputFiles`.

`Metadata` is a `Dictionary<string, object>` which contains any metadata defined by the programmer. `InputFiles` is a list of `InputFile` objects which represent the files in the project folder. `OutputFiles` is a list of `OutpuFile` objects which are all the output files that have been generated by the plugins that ran before yours. Plugins can modify, delete, or add anything to any of these properties to modify the end result.

### With a Function

If you don't plan on packaging your plugin for others to use, or you need a "quick and dirty" solution, you can just use any `Metalsharp -> Metalsharp` function with `Use`. First, implement a function:

```c#
Metalsharp DeleteEverything(Metalsharp directory)
{
	directory.InputFiles.Clear();
	directory.OutputFiles.Clear();
	return directory;
}
```

Then add it to the stack with `Use`:

```c#
new Metalsharp("Site")
	.Use(DeleteEverything)
	...
```

You can also use a lambda, if you wish:

```c#
new Metalsharp("Site")
	.Use(directory => {
		directory.InputFiles.Clear();
		directory.OutputFiles.Clear();
		return directory;
	})
	...
```

# Docs

Full documentation right around the corner...

# Contributing

If you'd like to contribute, please do! Open an Issue, submit a PR, I'm happy however you want to contribute.
