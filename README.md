<div align="center">
    
# <img src="https://raw.githubusercontent.com/IanWold/Metalsharp/refs/heads/master/Icon.png" height="23"> Metalsharp

[![NuGet](https://img.shields.io/nuget/v/Metalsharp.svg?logo=nuget&logoColor=white&style=for-the-badge)](https://www.nuget.org/packages/Metalsharp/)

A **small**, **simple**, and **extendable** C# library for generating static sites, inspired by [Metalsmith](http://www.metalsmith.io/)

</div>

---

Generating a static website from a directory is as simple as this:

```c#
new MetalsharpProject()
    .AddInput("Site")
    .UseFrontmatter()
    .UseMarkdown()
    .AddOutput("Static")
    .Build();
```

Metalsharp is:

1. **Small** — uses the smallest amount of code necessary to get the job done,
2. **Simple** — straightforward API with easy-to-understand documentation, and
3. **Extendable** — creating and publishing plugins is as easy as possible.

## Getting Started

Metalsharp targets .NET 10 and is available on [NuGet](https://www.nuget.org/packages/Metalsharp/):

```plaintext
dotnet add package Metalsharp
```

Prefer to try it without setting up a project? Metalsharp works great with [.NET file-based apps](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10/overview) — see the [Create a Website](https://github.com/IanWold/Metalsharp/wiki/Tutorial:-Create-a-Website) tutorial for a walkthrough that builds a complete site with nothing but a single `.cs` file.

### Project Structure

A Metalsharp project doesn't require any particular directory layout, but the following structure works well for most sites:

```text
ProjectFolder
├── Site
│   ├── SomeFile.md
│   └── SomeOtherFile.md
├── Static
│   └── style.css
└── README.md
```

Here, `ProjectFolder` is the root of the project. Anything unrelated to the generated site — your build script, README, and so on — can live at this level. Content that Metalsharp will process goes in `Site`, and files that should be copied straight through to the output, such as stylesheets and images, go in `Static`. Neither of these directory names is a requirement; use whatever structure fits your project.

### Using Metalsharp

Let's walk through the example above. The [quickstart](https://github.com/IanWold/Metalsharp/wiki/Quickstart) covers the basics in more depth, and [Create a Website with Metalsharp](https://github.com/IanWold/Metalsharp/wiki/Tutorial:-Create-a-Website) walks through a complete, practical project.

1. Instantiate a `MetalsharpProject` and read in the files you want to work with:

    ```c#
    new MetalsharpProject()
        .AddInput("Site")
    ```

2. Invoke a plugin by passing an instance to `Use`. `Frontmatter` reads each file's frontmatter into its metadata:

    ```c#
    .Use(new Frontmatter())
    ```

3. If a plugin has a public parameterless constructor, you can reference its type instead:

    ```c#
    .Use<Frontmatter>()
    ```

4. Most of the plugins that ship with Metalsharp — `Frontmatter` included — also provide an extension method that does the same thing, for convenience:

    ```c#
    .UseFrontmatter()
    ```

5. `Markdown` works the same way; it converts Markdown files in the input into HTML files in the output:

    ```c#
    .UseMarkdown()
    ```

6. Add any files that should be copied straight through to the output, such as static assets:

    ```c#
    .AddOutput("Static")
    ```

7. Finally, call `Build` to write the output files to disk:

    ```c#
    .Build();
    ```

## Creating a Custom Plugin

Creating a Metalsharp plugin is straightforward — see [Create a Plugin for Metalsharp](https://github.com/IanWold/Metalsharp/wiki/Tutorial:-Create-a-Plugin) for a full walkthrough. At its core, a plugin only needs to implement [`IMetalsharpPlugin`](https://github.com/IanWold/Metalsharp/blob/master/Metalsharp/IMetalsharpPlugin.cs), which has a single method, `Execute`. Here's the `Markdown` plugin that ships with Metalsharp:

```c#
public class Markdown : IMetalsharpPlugin
{
    public void Execute(MetalsharpProject project)
    {
        foreach (var file in project.InputFiles.Where(f => f.Extension is ".md" or ".markdown"))
        {
            var fileText = Markdig.Markdown.ToHtml(file.Text);
            var filePath = Path.Combine(file.Directory, file.Name + ".html");

            project.LogDebug($"Converting Input file {file.FilePath} to Output file {filePath}");

            project.OutputFiles.Add(new MetalsharpFile(fileText, filePath)
            {
                Metadata = new Dictionary<string, object>(file.Metadata)
            });
        }
    }
}
```

## Docs

[The wiki](https://github.com/IanWold/Metalsharp/wiki) contains generated API reference documentation and hand-written tutorials.

The source is fully documented with XML comments. [The API docs on the wiki](https://github.com/IanWold/Metalsharp/wiki/API-Documentation) are generated from those comments by [`GenerateApiDoc.cs`](https://github.com/IanWold/Metalsharp/blob/master/GenerateApiDoc.cs).

For questions or help using Metalsharp, please use the [Metalsharp Discord](https://discord.gg/KwBtSan) rather than opening an issue.

## Contributing

Contributions are welcome in whatever form suits you — bug reports, documentation fixes, new plugins, or pull requests against the core library.

## License

Metalsharp is licensed under the [MIT License](LICENSE).
