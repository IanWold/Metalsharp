using Metalsharp;
using Metalsharp.ExamplePlugin;
using Metalsharp.Logging;

new MetalsharpProject(new MetalsharpConfiguration { Verbosity = LogLevel.Debug })
    .AddInput("Site")
    .Use<Frontmatter>()
    .UseDrafts()
    .Use<Markdown>()
    .Use(new Layout())
    .Build();
