using System;
using System.Collections.Generic;
using System.IO;

namespace Metal.Sharp
{
    /// <summary>
    /// Represents a directory to be manipulated by Metalsharp plugins
    /// </summary>
    public class Metalsharp
    {
        public Metalsharp(string path)
        {
            AddInput(path, true);
            RootDirectory = path;
        }

        #region Methods

        /// <summary>
        /// Add a file or all the files in a directory to the input
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <param name="enforceDirectory">If true, will expect the path to lead to a directory</param>
        /// <returns></returns>
        public Metalsharp AddInput(string path, bool enforceDirectory = false)
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    InputFiles.Add(new InputFile(file));
                }

                return this;
            }
            else if (File.Exists(path) && !enforceDirectory)
            {
                InputFiles.Add(new InputFile(path));

                return this;
            }
            else if (enforceDirectory) throw new ArgumentException("Directory " + path + " does not exist.");
            else throw new ArgumentException("File " + path + " does not exist.");
        }

        /// <summary>
        /// Add a file or all the files in a directory directly to the output
        /// 
        /// The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <param name="enforceDirectory">If true, will expect the path to lead to a directory</param>
        /// <returns></returns>
        public Metalsharp AddOutput(string path, bool enforceDirectory = false)
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    OutputFiles.Add(OutputFile.FromExisting(path));
                }

                return this;
            }
            else if (File.Exists(path) && !enforceDirectory)
            {
                OutputFiles.Add(OutputFile.FromExisting(path));

                return this;
            }
            else if (enforceDirectory) throw new ArgumentException("Directory " + path + " does not exist.");
            else throw new ArgumentException("File " + path + " does not exist.");
        }

        /// <summary>
        /// Write all the output files to the output directory
        /// </summary>
        /// <param name="options">Metalsmith build configuration options</param>
        public void Build(BuildOptions options = null)
        {
            options = options ?? new BuildOptions();

            if (!Directory.Exists(options.OutputDirectory))
            {
                Directory.CreateDirectory(options.OutputDirectory);
            }

            if (options.ClearOutputDirectory)
            {
                foreach (var file in Directory.GetFiles(options.OutputDirectory))
                {
                    File.Delete(file);
                }
            }

            foreach (var file in OutputFiles)
            {
                var path = Path.Combine(options.OutputDirectory, file.FilePath);
                File.WriteAllText(path, file.Text);
            }
        }

        /// <summary>
        /// Write all the output files to the output directory after performing a function
        /// </summary>
        /// <param name="func">The function to perform</param>
        /// <param name="options">Metalsmith build configuration options</param>
        public void Build(Action<Metalsharp> func, BuildOptions options = null)
        {
            func(this);
            Build(options);
        }

        /// <summary>
        /// Add or alter directory-level metadata
        /// </summary>
        /// <param name="pairs">The key-value pairs to add/update</param>
        /// <returns></returns>
        public Metalsharp Meta(params (string key, object value)[] pairs)
        {
            foreach (var pair in pairs)
            {
                if (Metadata.ContainsKey(pair.key))
                {
                    Metadata[pair.key] = pair.value;
                }
                else
                {
                    Metadata.Add(pair.key, pair.value);
                }
            }

            return this;
        }

        /// <summary>
        /// Remove a file from the input
        /// </summary>
        /// <param name="path">The path of the file to remove</param>
        /// <returns></returns>
        public Metalsharp RemoveInput(string path) =>
            RemoveInput(file => file.FilePath == path);

        /// <summary>
        /// Remove all the files matching a predicate from the input
        /// </summary>
        /// <param name="predicate">The predicate function to identify files to delete</param>
        /// <returns></returns>
        public Metalsharp RemoveInput(Func<InputFile, bool> predicate)
        {
            InputFiles.RemoveAll(file => predicate(file));
            return this;
        }

        /// <summary>
        /// Remove a file from the output
        /// </summary>
        /// <param name="path">The path of the file to remove</param>
        /// <returns></returns>
        public Metalsharp RemoveOutput(string path) =>
            RemoveOutput(file => file.FilePath == path);

        /// <summary>
        /// Remove all the files matching a predicate from the output
        /// </summary>
        /// <param name="predicate">The predicate function to identify files to delete</param>
        /// <returns></returns>
        public Metalsharp RemoveOutput(Func<OutputFile, bool> predicate)
        {
            OutputFiles.RemoveAll(file => predicate(file));
            return this;
        }

        /// <summary>
        /// Invoke a function as a plugin
        /// </summary>
        /// <param name="func">The function to invoke</param>
        /// <returns></returns>
        public Metalsharp Use(Func<Metalsharp, Metalsharp> func) =>
            func(this);

        /// <summary>
        /// Invoke a plugin
        /// </summary>
        /// <param name="plugin">The plugin to invoke</param>
        /// <returns></returns>
        public Metalsharp Use(IMetalsharpPlugin plugin) =>
            plugin.Execute(this);

        /// <summary>
        /// Invoke a plugin by type
        /// 
        /// The plugin type must have an empty constructor
        /// </summary>
        /// <typeparam name="T">The type of the plugin to invoke</typeparam>
        /// <returns></returns>
        public Metalsharp Use<T>() where T : IMetalsharpPlugin, new() =>
            new T().Execute(this);

        #endregion

        #region Properties

        /// <summary>
        /// The directory with which Metalsharp was instantiated
        /// </summary>
        public string RootDirectory { get; set; }

        /// <summary>
        /// The directory-level metadata
        /// </summary>
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// The input files
        /// </summary>
        public List<InputFile> InputFiles { get; set; } = new List<InputFile>();

        /// <summary>
        /// The files to output
        /// </summary>
        public List<OutputFile> OutputFiles { get; set; } = new List<OutputFile>();

        #endregion
    }
}
