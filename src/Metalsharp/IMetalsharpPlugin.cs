namespace Metalsharp;

/// <summary>
///     The interface from which Metalsharp plugin must (read: should) derive. 
/// </summary>
/// <example>
///     Implementing a Metalsharp plugin is as easy as implementing this interface:
///     
///     <code>
///         public class DeleteEverything : IMetalsharpPlugin
///         {
///         
///         public void Execute(MetalsharpProject project) =>
///         project.RemoveFiles(file => true);
/// 
///         }
///     </code>
///     
///     This plugin can then be used like any other:
///     
///     <code>
///         new MetalsharpProject()
///         ... // Add files
///         .Use&lt;DeleteEverything&gt;();
///     </code>
/// </example>
public interface IMetalsharpPlugin
{
    /// <summary>
    ///     Invokes the plugin. <c>Called by Metalsharp.Use</c>.
    /// </summary>
    /// 
    /// <param name="project">
    ///     The <c>MetalsharpProject</c> to alter.
    /// </param>
    void Execute(MetalsharpProject project);
}
