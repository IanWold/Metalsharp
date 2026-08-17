#:property LangVersion=preview
#:package Metalsharp@1.0.1
#:package Metalsharp.LiquidTemplates@1.0.0

using System.Net.Http;
using System.Text.Json;
using Metalsharp;
using Metalsharp.LiquidTemplates;

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver()
};

var docInfos = JsonSerializer.Deserialize<IEnumerable<DocInfo>>(File.ReadAllText("Docs.json"), jsonOptions) ?? [];
var plugins = JsonSerializer.Deserialize<IEnumerable<Plugin>>(File.ReadAllText("Plugins.json"), jsonOptions) ?? [];
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

var docsClient = new HttpClient();

var docs = await Task.WhenAll(docInfos.Select(async d =>
{
    var content = string.Empty;

    try
    {
        content = await docsClient.GetStringAsync($"{d.Url}.md");
    }
    catch { }

    return new Doc(d.Title, d.Description, d.Slug, content);
}));

new MetalsharpProject(new MetalsharpOptions()
{
    OutputDirectory = "output",
    Verbosity = LogLevel.Debug,
	ClearOutputDirectory = true
})
.AddInput("site", ".")
.UseFrontmatter()
.Use(project =>
{
    foreach (var doc in docs)
    {
        project.AddInput(new MetalsharpFile(doc.Content, $"docs/{doc.Slug}.md", new Dictionary<string, object>()
        {
            ["heading"] = doc.Title,
            ["subheading"] = doc.Description
        }));
    }
})
.UseMarkdown()
.AddOutput(new MetalsharpFile(string.Empty, "docs.html", new Dictionary<string, object>()
{
    ["template"] = "docs",
    ["heading"] = "Documentation",
    ["subheading"] = "Guides for getting started, and a full reference for everything Metalsharp exposes.",
    ["docs"] = docs
}))
.AddOutput(new MetalsharpFile(string.Empty, "plugins.html", new Dictionary<string, object>()
{
    ["template"] = "plugins",
    ["heading"] = "Plugins",
    ["subheading"] = "Published by the community, installed from NuGet.",
    ["plugins"] = plugins
}))
.UseLeveller()
.UseLiquidTemplates("templates")
.AddOutput("static")
.Build();

record DocInfo(string Title, string Description, string Slug)
{
    public string Url =>
        $"https://github.com/IanWold/Metalsharp/wiki/{Slug}";
}

record Doc(string Title, string Description, string Slug, string Content);

record Plugin(string Nuget, string Github, string Description)
{
    public string GithubUsername =>
        Github.Split("/").First();
    
    public string GithubRepo =>
        Github.Split("/").Last();
}