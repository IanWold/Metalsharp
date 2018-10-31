using Metal.Sharp;

namespace Example1
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Metalsharp("Site");

            new Metalsharp("Site")
                .UseDrafts()
                .Use<Markdown>()
                .Use(new Layout())
                .Build();
        }
    }
}
