#:property LangVersion=preview
#:package Metalsharp@1.0.1
#:package Metalsharp.LiquidTemplates@1.0.0

using System.Text.Json;
using Metalsharp;
using Metalsharp.LiquidTemplates;

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    TypeInfoResolver = new System.Text.Json.Serialization.Metadata.DefaultJsonTypeInfoResolver()
};

var docs = JsonSerializer.Deserialize<List<Doc>>(File.ReadAllText("Docs.json"), jsonOptions) ?? [];
var plugins = JsonSerializer.Deserialize<List<Plugin>>(File.ReadAllText("Plugins.json"), jsonOptions) ?? [];

new MetalsharpProject(new MetalsharpOptions()
{
    OutputDirectory = "output",
    Verbosity = LogLevel.Debug,
	ClearOutputDirectory = true
})
.AddInput("site", ".")
.UseFrontmatter()
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
.UseLiquidTemplates("templates")
.AddOutput("static")
.Build();

record Doc(string Title, string Description, string Url);

record Plugin(string Nuget, string Github, string Description)
{
    public string GithubUsername =>
        Github.Split("/").First();
    
    public string GithubRepo =>
        Github.Split("/").Last();
}