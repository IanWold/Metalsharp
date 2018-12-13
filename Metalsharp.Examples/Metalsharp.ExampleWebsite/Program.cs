using Metalsharp.ExamplePlugin;

namespace Metalsharp.ExampleWebsite
{
    class Program
    {
        static void Main(string[] args)
        {
            new MetalsharpProject("Site")
                .Use<Frontmatter>()
                .UseDrafts()
                .Use<Markdown>()
                .Use(new Layout())
                .Build();
        }
    }
}
