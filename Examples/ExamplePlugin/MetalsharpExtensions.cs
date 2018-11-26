using Metalsharp;

namespace ExamplePlugin
{
    public static class MetalsharpExtensions
    {
        /// <summary>
        /// Invoke the Layout plugin
        /// </summary>
        /// <returns></returns>
        /// <param name="filePath">The path to the layout file</param>
        public static MetalsharpDirectory UseLayout(this MetalsharpDirectory directory, string filePath) =>
            directory.Use(new Layout());
    }
}
