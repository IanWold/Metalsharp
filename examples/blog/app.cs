#:package Metalsharp@1.0.1
#:package Metalsharp.SimpleBlog@1.0.0
#:package Metalsharp.LiquidTemplates@1.0.0

using Metalsharp;
using Metalsharp.SimpleBlog;
using Metalsharp.LiquidTemplates;

new MetalsharpProject(outputDirectory: "output")
    .AddInput("posts", "./posts")
    .UseFrontmatter()
    .UseMarkdown()
    .UseSimpleBlog(new SimpleBlogOptions
    {
        PostsDirectory = "./posts",
        BlogFilePath = "./blog.html",
        PostsOrderQuery = file => DateTime.Parse((string)file.Metadata["date"])
    })
    .Use(project =>
    {
        var blog = project.OutputFiles.First(f => f.FilePath == "./blog.html");
        blog.Metadata["title"] = "My Blog";
        blog.Metadata["template"] = "blog";
    })
    .UseLeveller()
    .UseLiquidTemplates("templates")
    .AddOutput("static")
    .Build();
