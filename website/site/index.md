---
heading: "A tiny, extendable static site generator for C#"
subheading: "Metalsharp reads files, runs them through plugins, and writes them back out. Inspired by Metalsmith &mdash; no conventions forced on you, no magic you can't read."
---

Generating a static website from a directory is as simple as:

```csharp
new MetalsharpProject()
    .AddInput("Site")
    .UseFrontmatter()
    .UseMarkdown()
    .AddOutput("Static")
    .Build();
```