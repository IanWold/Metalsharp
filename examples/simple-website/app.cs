#:package Metalsharp@1.0.1

using Metalsharp;
using System.Text;

// A minimal Metalsharp project: read Markdown files with front matter, convert them
// to HTML, and use each page's front-matter title to wrap it in a bare HTML shell.
// That last step is just a function - see the "plugins are just functions" example
// on metalsharp's own site for more on why that's the whole extensibility story.
new MetalsharpProject(outputDirectory: "output")
    .AddInput("site", ".")
    .UseFrontmatter()
    .UseMarkdown()
    .Use(project =>
    {
        foreach (var file in project.OutputFiles)
        {
            var title = file.Metadata["title"];

            file.Contents = Encoding.UTF8.GetBytes($"""
                <!doctype html>
                <html lang="en">
                <head>
                <meta charset="utf-8">
                <title>{title}</title>
                </head>
                <body>
                {file.Text}
                </body>
                </html>
                """);
        }
    })
    .Build();
