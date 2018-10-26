namespace Metal.Sharp
{
    /// <summary>
    /// Represents the options when Metalsharp outputs a directory
    /// </summary>
    public class BuildOptions
    {
        /// <summary>
        /// Whether Metalsharp should remove all the files in the output directory before writing any
        /// </summary>
        public bool ClearOutputDirectory { get; set; } = true;

        /// <summary>
        /// The directory to which the files will be output
        /// </summary>
        public string OutputDirectory { get; set; } = "bin/";
    }
}
