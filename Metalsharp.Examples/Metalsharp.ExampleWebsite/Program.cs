using Metalsharp;
using Metalsharp.ExamplePlugin;

new MetalsharpProject("Site")
    .Use<Frontmatter>()
    .UseDrafts()
    .Use<Markdown>()
    .Use(new Layout())
    .Build();
