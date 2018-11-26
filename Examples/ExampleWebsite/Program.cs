using Metalsharp;
using ExamplePlugin;

namespace ExampleWebsite
{
    class Program
    {
        static void Main(string[] args)
        {
            new MetalsharpDirectory("Site")
                .Use<Frontmatter>()
                .UseDrafts()
                .Use<Markdown>()
                .Use(new Layout())
                .Build();
        }
    }
}
