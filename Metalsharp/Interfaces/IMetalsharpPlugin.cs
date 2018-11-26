namespace Metalsharp
{
    /// <summary>
    /// Represents a Metalsharp plugin
    /// </summary>
    public interface IMetalsharpPlugin
    {
        /// <summary>
        /// Invokes the plugin. Called by Metalsharp.Use
        /// </summary>
        /// <param name="directory">The directory to alter</param>
        /// <returns>The same directory as was input</returns>
        void Execute(MetalsharpDirectory directory);
    }
}
