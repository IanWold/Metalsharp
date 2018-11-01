using Metal.Sharp;

namespace Example1
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
