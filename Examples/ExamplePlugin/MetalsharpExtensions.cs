using Metal.Sharp;

namespace ExamplePlugin
{
    public static class MetalsharpExtensions
    {
        /// <summary>
        /// Invoke the Layout plugin
        /// </summary>
        /// <returns></returns>
        /// <param name="filePath">The path to the layout file</param>
        public static Metalsharp UseLayout(this Metalsharp directory, string filePath) =>
            directory.Use(new Layout());
    }
}
