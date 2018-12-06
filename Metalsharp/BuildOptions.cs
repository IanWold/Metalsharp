namespace Metalsharp
{
    /// <summary>
    ///     Represents the options when Metalsharp outputs a directory.
    /// </summary>
    public class BuildOptions
    {
        /// <summary>
        ///     Whether Metalsharp should remove all the files in the output directory before writing any to that directory.
        ///     
        ///     `false` by default.
        /// </summary>
        public bool ClearOutputDirectory { get; set; } = false;

        /// <summary>
        ///     The directory to which the files will be output.
        ///     
        ///     `.\` by default.
        /// </summary>
        public string OutputDirectory { get; set; } = ".\\";
    }
}
