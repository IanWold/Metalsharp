namespace Metal.Sharp
{
    public class Drafts : IMetalsharpPlugin
    {
        public Drafts() { }

        public Metalsharp Execute(Metalsharp directory)
        {
            directory.InputFiles.RemoveAll(file =>
                file.Metadata.TryGetValue("draft", out var _isDraft)
                && _isDraft is bool isDraft
                && isDraft
            );

            directory.OutputFiles.RemoveAll(file =>
                file.Metadata.TryGetValue("draft", out var _isDraft)
                && _isDraft is bool isDraft
                && isDraft
            );

            return directory;
        }
    }
}
