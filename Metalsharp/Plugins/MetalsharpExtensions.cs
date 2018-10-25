namespace Metal.Sharp
{
    public static class MetalsharpExtensions
    {
        public static Metalsharp UseDrafts(this Metalsharp directory) =>
            directory.Use(new Drafts());

        public static Metalsharp UseLayout(this Metalsharp directory, string filePath) =>
            directory.Use(new Layout(filePath));

        public static Metalsharp UseMarkdown(this Metalsharp directory) =>
            directory.Use(new Markdown());
    }
}
