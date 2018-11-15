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
        /// <summary>
        /// Used by Metalsharp.From to pass an empty Metalsharp to a plugin
        /// </summary>
        private Metalsharp() { }

        /// <summary>
        /// Instantiate Metalsharp from an existing directory
        /// </summary>
        /// <param name="path">The path to the directory</param>
        public Metalsharp(string path)
        {
            RootDirectory = Path.Combine(path, Path.DirectorySeparatorChar.ToString());
            AddInput(path, true);
        }

        #region Static Methods

        /// <summary>
        /// Instantiate Metalsharp by invoking a function as a plugin
        /// </summary>
        /// <param name="func">The function to invoke</param>
        /// <returns></returns>
        public static Metalsharp From(Func<Metalsharp, Metalsharp> func) =>
            func(new Metalsharp());

        /// <summary>
        /// Instantiate Metalsharp by invoking a plugin
        /// </summary>
        /// <param name="plugin">The plugin to invoke</param>
        /// <returns></returns>
        public static Metalsharp From(IMetalsharpPlugin plugin) =>
            new Metalsharp().Use(i => plugin.Execute(i));

        /// <summary>
        /// Instantiate Metalsharp by invoking a plugin by type
        /// 
        /// The plugin type must have an empty constructor
        /// </summary>
        /// <typeparam name="T">The type of the plugin to invoke</typeparam>
        /// <returns></returns>
        public static Metalsharp From<T>() where T : IMetalsharpPlugin, new() =>
            new Metalsharp().Use<T>();

        #endregion

        #region Methods

        /// <summary>
        /// Add an existing file to the input or output
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <param name="enforceDirectory">If true, will expect the path to lead to a directory</param>
        /// <param name="add">The function to add the file</param>
        /// <returns></returns>
        Metalsharp AddExisting(string path, bool enforceDirectory, Action<MetalsharpFile> add)
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    add(GetFileWithNormalizedDirectory(file));
                }

                foreach (var dir in Directory.GetDirectories(path))
                {
                    AddExisting(dir, false, add);
                }

                return this;
            }
            else if (File.Exists(path) && !enforceDirectory)
            {
                add(GetFileWithNormalizedDirectory(path));
                return this;
            }
            else if (enforceDirectory)
            {
                throw new ArgumentException("Directory " + path + " does not exist.");
            }
            else
            {
                throw new ArgumentException("File " + path + " does not exist.");
            }
        }

        /// <summary>
        /// Add a file or all the files in a directory to the input
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <returns></returns>
        public Metalsharp AddInput(string path) =>
            AddInput(path, false);

        /// <summary>
        /// Add a file or all the files in a directory to the input
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <param name="enforceDirectory">If true, will expect the path to lead to a directory</param>
        /// <returns></returns>
        public Metalsharp AddInput(string path, bool enforceDirectory) =>
            AddExisting(path, enforceDirectory, InputFiles.Add);

        /// <summary>
        /// Add a file or all the files in a directory directly to the output
        /// 
        /// The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <returns></returns>
        public Metalsharp AddOutput(string path) =>
            AddOutput(path, false);

        /// <summary>
        /// Add a file or all the files in a directory directly to the output
        /// 
        /// The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <param name="enforceDirectory">If true, will expect the path to lead to a directory</param>
        /// <returns></returns>
        public Metalsharp AddOutput(string path, bool enforceDirectory) =>
            AddExisting(path, enforceDirectory, OutputFiles.Add);

        /// <summary>
        /// Write all the output files to the default output directory
        /// with default build options
        /// </summary>
        public void Build() =>
            Build(new BuildOptions());

        /// <summary>
        /// Write all the output files to the output directory
        /// </summary>
        /// <param name="options">Metalsmith build configuration options</param>
        public void Build(BuildOptions options)
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
        /// Write all the output files to the default output directory after performing a function
        /// with default build options
        /// </summary>
        /// <param name="func">The function to perform</param>
        public void Build(Action<Metalsharp> func) =>
            Build(func, new BuildOptions());

        /// <summary>
        /// Write all the output files to the output directory after performing a function
        /// </summary>
        /// <param name="func">The function to perform</param>
        /// <param name="options">Metalsmith build configuration options</param>
        public void Build(Action<Metalsharp> func, BuildOptions options)
        {
            BeforeBuild(this, new EventArgs());
            func(this);
            AfterBuild(this, new EventArgs());
            Build(options);
        }

        /// <summary>
        /// Gets a MetalsharpFile with the RootDirectory removed from its path
        /// </summary>
        /// <param name="path">The path to the file to read</param>
        /// <returns></returns>
        MetalsharpFile GetFileWithNormalizedDirectory(string path) =>
            new MetalsharpFile(
                File.ReadAllText(path),
                path.StartsWith(RootDirectory)
                    ? path.Substring(RootDirectory.Length)
                    : path
            );

        /// <summary>
        /// Add or alter a single item of metadata
        /// </summary>
        /// <param name="key">The key to add/update</param>
        /// <param name="value">The value to store with the key</param>
        /// <returns></returns>
        public Metalsharp Meta(string key, object value) =>
            Meta((key, value));

        /// <summary>
        /// Add or alter directory-level metadata
        /// </summary>
        /// <param name="pairs">The key-value pairs to add/update</param>
        /// <returns></returns>
        public Metalsharp Meta(params (string key, object value)[] pairs)
        {
            foreach (var (key, value) in pairs)
            {
                if (Metadata.ContainsKey(key))
                {
                    Metadata[key] = value;
                }
                else
                {
                    Metadata.Add(key, value);
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
        public Metalsharp RemoveInput(Predicate<MetalsharpFile> match)
        {
            InputFiles.RemoveAll(match);
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
        public Metalsharp RemoveOutput(Predicate<MetalsharpFile> match)
        {
            OutputFiles.RemoveAll(match);
            return this;
        }

        /// <summary>
        /// Invoke a function as a plugin
        /// </summary>
        /// <param name="func">The function to invoke</param>
        /// <returns></returns>
        public Metalsharp Use(Action<Metalsharp> func)
        {
            BeforeUse(this, new EventArgs());
            func(this);
            AfterUse(this, new EventArgs());
            return this;
        }

        /// <summary>
        /// Invoke a plugin
        /// </summary>
        /// <param name="plugin">The plugin to invoke</param>
        /// <returns></returns>
        public Metalsharp Use(IMetalsharpPlugin plugin) =>
            Use(i => plugin.Execute(i));

        /// <summary>
        /// Invoke a plugin by type
        /// 
        /// The plugin type must have an empty constructor
        /// </summary>
        /// <typeparam name="T">The type of the plugin to invoke</typeparam>
        /// <returns></returns>
        public Metalsharp Use<T>() where T : IMetalsharpPlugin, new() =>
            Use(new T());

        #endregion

        #region Events

        public event EventHandler BeforeUse;

        public event EventHandler AfterUse;

        public event EventHandler BeforeBuild;

        public event EventHandler AfterBuild;

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
        public IMetalsharpFileCollection<MetalsharpFile> InputFiles { get; set; } = new MetalsharpFileCollection<MetalsharpFile>();

        /// <summary>
        /// The files to output
        /// </summary>
        public IMetalsharpFileCollection<MetalsharpFile> OutputFiles { get; set; } = new MetalsharpFileCollection<MetalsharpFile>();

        #endregion
    }
}
