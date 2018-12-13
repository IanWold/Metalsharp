namespace Metalsharp.ExamplePlugin
{
    public static class MetalsharpExtensions
    {
        /// <summary>
        /// Invoke the Drafts plugin
        /// </summary>
        /// <returns></returns>
        public static MetalsharpProject UseDrafts(this MetalsharpProject project) =>
            project.Use(new Drafts());

        /// <summary>
        /// Invoke the Layout plugin
        /// </summary>
        /// <returns></returns>
        /// <param name="filePath">The path to the layout file</param>
        public static MetalsharpProject UseLayout(this MetalsharpProject project, string filePath) =>
            project.Use(new Layout());
    }
}
