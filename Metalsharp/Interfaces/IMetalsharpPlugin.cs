namespace Metalsharp
{
    /// <summary>
    ///     The interface from which Metalsharp plugin must (read: should) derive. 
    /// </summary>
    /// <example>
    ///     Implementing a Metalsharp plugin is as easy as implementing this interface:
    ///     
    ///     ```c#
    ///         public class DeleteEverything : IMetalsharpPlugin
    ///         {
    ///         
    ///         public void Execute(MetalsharpProject project) =>
    ///         project.RemoveFiles(file => true);
    /// 
    ///         }
    ///     ```
    ///     
    ///     This plugin can then be used like any other:
    ///     
    ///     ```c#
    ///         new MetalsharpProject()
    ///         ... // Add files
    ///         .Use&lt;DeleteEverything&gt;();
    ///     ```
    /// </example>
    public interface IMetalsharpPlugin
    {
        /// <summary>
        ///     Invokes the plugin. `Called by Metalsharp.Use`.
        /// </summary>
        /// 
        /// <param name="project">
        ///     The `MetalsharpProject` to alter.
        /// </param>
        void Execute(MetalsharpProject project);
    }
}
