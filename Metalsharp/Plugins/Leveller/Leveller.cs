using System.IO;
using System.Linq;

namespace Metalsharp;

/// <summary>
/// The Leveller plugin
/// 
/// Adds "level" metadata to each file specifying how many directories deep the file is (1-based).
/// </summary>
/// 
/// <example>
/// The following will add a file at a directory, use leveller, and demonstrate the resulting metadata in the file:
/// 
/// ```c#
///     var file = new MetalsharpFile("Hello, World!", "dir1\dir2\file");
///     new MetalsharpProject().AddInput(file).UseLeveller();
///     
///     foreach (var metadata in file.Metadata)
///     {
///         Console.WriteLine($"{metadata.Key}: {metadata.Value});
///     }
/// ```
/// 
/// The output of the run will be "level: 3", since `file` is at the third directory from root.
/// </example>
public class Leveller : IMetalsharpPlugin
{
    /// <summary>
    ///     Invokes the plugin.
    /// </summary>
    /// 
    /// <param name="project">
    ///     The `MetalsharpProject` to level.
    /// </param>
    public void Execute(MetalsharpProject project)
    {
        foreach (var file in project.InputFiles.Concat(project.OutputFiles))
        {
            var directoryLevels = file.Directory.Split(Path.DirectorySeparatorChar);
            var directoryLevelCount = directoryLevels.Length - (directoryLevels[0] == "." ? 1 : 0);

            project.Log.Debug($"File {file.FilePath} is at level {directoryLevelCount}");

            if (file.Metadata.ContainsKey("level"))
            {
                file.Metadata["level"] = directoryLevelCount;
            }
            else
            {
                file.Metadata.Add("level", directoryLevelCount);
            }
        }
    }
}
