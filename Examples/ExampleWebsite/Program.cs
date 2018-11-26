using Metal.Sharp;
using ExamplePlugin;

namespace ExampleWebsite
{
    class Program
    {
        static void Main(string[] args)
        {
            new Metalsharp("Site")
                .Use<Frontmatter>()
                .UseDrafts()
                .Use<Markdown>()
                .Use(new Layout())
                .Build();
        }
    }
}
