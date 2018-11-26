using System;
using System.IO;
using System.Linq;

namespace Metalsharp
{
    /// <summary>
    /// The Debug plugin
    /// 
    /// Writes a log after every Use, outputting the contents of the input and output directories.
    /// </summary>
    public class Debug : IMetalsharpPlugin
    {
        /// <summary>
        /// By default, write debug logs with Debug.WriteLine()
        /// </summary>
        public Debug() : this(message => System.Diagnostics.Debug.WriteLine(message)) { }

        /// <summary>
        /// Configure Debug to write logs to a log file
        /// </summary>
        /// <param name="logPath">The path to the log file</param>
        public Debug(string logPath)
            : this(message =>
        {
            using (var writer = new StreamWriter(logPath, true))
            {
                writer.WriteLineAsync(message);
            }
        })
        { }

        /// <summary>
        /// Configure Debug to use custom behavior when writing logs
        /// </summary>
        /// <param name="onLog">The action to execute when writing a log</param>
        public Debug(Action<string> onLog) =>
            OnLog = onLog;

        /// <summary>
        /// The action to execute when writing a log
        /// </summary>
        private Action<string> OnLog;

        /// <summary>
        /// A count of the number of "Uses" against the directory
        /// </summary>
        private int Uses = 0;

        /// <summary>
        /// Invokes the plugin
        /// </summary>
        /// <param name="directory"></param>
        public void Execute(MetalsharpDirectory directory) =>
            directory.AfterUse += (sender, e) =>
                OnLog("Step " + ++Uses + "." +
                    "\r\n" +
                    "Input directory:" +
                    "\r\n\r\n" +
                    WriteDirectory(directory.InputFiles) +
                    "\r\n\r\n" +
                    "Output directory:" +
                    "\r\n\r\n" +
                    WriteDirectory(directory.OutputFiles) +
                    "\r\n\r\n" +
                    "---" +
                    "\r\n\r\n"
                );

        /// <summary>
        /// Prettify the contents of a collection of files
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        private string WriteDirectory(IMetalsharpFileCollection<MetalsharpFile> directory) =>
            string.Join("\r\n",
                directory
                    .OrderBy(file => file.FilePath)
                    .Select(file => "\t" + file.FilePath)
            );
    }
}
