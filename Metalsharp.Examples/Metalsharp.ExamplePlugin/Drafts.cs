namespace Metalsharp.ExamplePlugin
{
    /// <summary>
    /// The Drafts plugin
    /// 
    /// Removes any input or output file with ("draft", true) in its metadata
    /// </summary>
    public class Drafts : IMetalsharpPlugin
    {
        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="project"></param>
        public void Execute(MetalsharpProject project)
        {
            project.InputFiles.RemoveAll(file =>
                file.Metadata.TryGetValue("draft", out var _isDraft)
                && _isDraft is bool isDraft
                && isDraft
            );

            project.OutputFiles.RemoveAll(file =>
                file.Metadata.TryGetValue("draft", out var _isDraft)
                && _isDraft is bool isDraft
                && isDraft
            );
        }
    }
}
