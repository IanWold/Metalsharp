﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Metalsharp
{
    /// <summary>
    /// Represents a root directory to be manipulated by Metalsharp plugins
    /// </summary>
    public class MetalsharpDirectory
    {
        #region Constructors

        /// <summary>
        /// Instantiate an empty MetalsharpDirectory
        /// </summary>
        public MetalsharpDirectory() { }

        /// <summary>
        /// Instantiate Metalsharp from an existing directory
        /// </summary>
        /// <param name="path">The path to the directory</param>
        public MetalsharpDirectory(string path) =>
            AddInput(path);

        /// <summary>
        /// Instantiate Metalsharp from an existing directory and add the contents to a specific virtual path
        /// </summary>
        /// <param name="diskPath">The path to the files on disk to add</param>
        /// <param name="virtualPath">The path of the virtual directory to put the input files into</param>
        public MetalsharpDirectory(string diskPath, string virtualPath) =>
            AddInput(diskPath, virtualPath);

        #endregion

        #region Methods

        #region Add Files

        /// <summary>
        /// Add an existing file to the input or output and place the files in a specific virtual path
        /// </summary>
        /// <param name="diskPath">The path to the file or directory</param>
        /// <param name="virtualPath">The path to the virtual directory to place the files in</param>
        /// <param name="add">The function to add the file</param>
        /// <returns></returns>
        MetalsharpDirectory AddExisting(string diskPath, string virtualPath, Action<MetalsharpFile> add)
        {
            if (Directory.Exists(diskPath))
            {
                foreach (var file in Directory.GetFiles(diskPath))
                {
                    add(GetFileWithNormalizedDirectory(file, virtualPath));
                }

                foreach (var dir in Directory.GetDirectories(diskPath))
                {
                    AddExisting(dir, dir.Replace(diskPath, virtualPath), add);
                }

                return this;
            }
            else if (File.Exists(diskPath))
            {
                add(GetFileWithNormalizedDirectory(diskPath, virtualPath));
                return this;
            }
            else
            {
                throw new ArgumentException("File " + diskPath + " does not exist.");
            }
        }

        /// <summary>
        /// Add a file or all the files in a directory to the input
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <returns></returns>
        public MetalsharpDirectory AddInput(string path) =>
            AddInput(path, path);

        /// <summary>
        /// Add a file or all the files in a directory to the input and place the files in a specific virtual path
        /// </summary>
        /// <param name="diskPath">The path to the file or directory</param>
        /// <param name="virtualPath">The path to the virtual directory to place the files in</param>
        /// <returns></returns>
        public MetalsharpDirectory AddInput(string diskPath, string virtualPath) =>
            AddExisting(diskPath, virtualPath, InputFiles.Add);

        /// <summary>
        /// Add a MetalsharpFile to the input files
        /// </summary>
        /// <param name="file">The file to add</param>
        /// <returns></returns>
        public MetalsharpDirectory AddInput(MetalsharpFile file)
        {
            InputFiles.Add(file);
            return this;
        }

        /// <summary>
        /// Add a file or all the files in a directory directly to the output
        /// 
        /// The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed
        /// </summary>
        /// <param name="path">The path to the file or directory</param>
        /// <returns></returns>
        public MetalsharpDirectory AddOutput(string path) =>
            AddOutput(path, path);

        /// <summary>
        /// Add a file or all the files in a directory directly to the output and place the files in a specific virtual path
        /// 
        /// The file(s) will not be added to the input and JSON metadata in the file(s) will not be parsed
        /// </summary>
        /// <param name="diskPath">The path to the file or directory</param>
        /// <param name="virtualPath">The path to the virtual directory to place the files in</param>
        /// <returns></returns>
        public MetalsharpDirectory AddOutput(string diskPath, string virtualPath) =>
            AddExisting(diskPath, virtualPath, OutputFiles.Add);

        /// <summary>
        /// Add a MetalsharpFile to output files
        /// </summary>
        /// <param name="file">The file to add</param>
        /// <returns></returns>
        public MetalsharpDirectory AddOutput(MetalsharpFile file)
        {
            OutputFiles.Add(file);
            return this;
        }

        /// <summary>
        /// Gets a MetalsharpFile with the RootDirectory removed from its path
        /// </summary>
        /// <param name="diskPath">The path to the file or directory</param>
        /// <param name="virtualPath">The path to the virtual directory to place the files in</param>
        /// <returns></returns>
        MetalsharpFile GetFileWithNormalizedDirectory(string diskPath, string virtualPath) =>
            new MetalsharpFile(File.ReadAllText(diskPath), Path.Combine(virtualPath, Path.GetFileName(diskPath)));

        #endregion

        #region Build

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
            var buildOptions = options ?? new BuildOptions();

            if (!Directory.Exists(buildOptions.OutputDirectory))
            {
                Directory.CreateDirectory(buildOptions.OutputDirectory);
            }

            if (buildOptions.ClearOutputDirectory)
            {
                foreach (var file in Directory.GetFiles(buildOptions.OutputDirectory))
                {
                    File.Delete(file);
                }
            }

            foreach (var file in OutputFiles)
            {
                var path = Path.Combine(buildOptions.OutputDirectory, file.FilePath);
                var directoryPath = Path.GetDirectoryName(path);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(path, file.Text);
            }
        }

        /// <summary>
        /// Write all the output files to the default output directory after performing a function
        /// with default build options
        /// </summary>
        /// <param name="func">The function to perform</param>
        public void Build(Action<MetalsharpDirectory> func) =>
            Build(func, new BuildOptions());

        /// <summary>
        /// Write all the output files to the output directory after performing a function
        /// </summary>
        /// <param name="func">The function to perform</param>
        /// <param name="options">Metalsmith build configuration options</param>
        public void Build(Action<MetalsharpDirectory> func, BuildOptions options)
        {
            BeforeBuild?.Invoke(this, new EventArgs());
            func(this);
            AfterBuild?.Invoke(this, new EventArgs());
            Build(options);
        }

        #endregion

        #region Meta

        /// <summary>
        /// Add or alter a single item of metadata
        /// </summary>
        /// <param name="key">The key to add/update</param>
        /// <param name="value">The value to store with the key</param>
        /// <returns></returns>
        public MetalsharpDirectory Meta(string key, object value) =>
            Meta((key, value));

        /// <summary>
        /// Add or alter directory-level metadata
        /// </summary>
        /// <param name="pairs">The key-value pairs to add/update</param>
        /// <returns></returns>
        public MetalsharpDirectory Meta(params (string key, object value)[] pairs)
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

        #endregion

        #region Move Files

        /// <summary>
        /// Move files in the input and output from one directory to another
        /// </summary>
        /// <param name="oldDirectory">The directory to move the files from</param>
        /// <param name="newDirectory">The directory to move the files into</param>
        /// <returns></returns>
        public MetalsharpDirectory MoveFiles(string oldDirectory, string newDirectory)
        {
            MoveInput(oldDirectory, newDirectory);
            MoveOutput(oldDirectory, newDirectory);
            return this;
        }

        /// <summary>
        /// Move files matching a predicate in the input and output from one directory to another
        /// </summary>
        /// <param name="predicate">The predicate to match the files to move</param>
        /// <param name="newDirectory">The directory to move the files into</param>
        /// <returns></returns>
        public MetalsharpDirectory MoveFiles(Predicate<IMetalsharpFile> predicate, string newDirectory)
        {
            MoveInput(predicate, newDirectory);
            MoveOutput(predicate, newDirectory);
            return this;
        }

        /// <summary>
        /// Move files in the input from one directory to another
        /// </summary>
        /// <param name="oldDirectory">The directory to move the files from</param>
        /// <param name="newDirectory">The directory to move the files into</param>
        /// <returns></returns>
        public MetalsharpDirectory MoveInput(string oldDirectory, string newDirectory) =>
            MoveInput(file => file.Directory == oldDirectory, newDirectory);

        /// <summary>
        /// Move files in the input matching a predicate from one directory to another
        /// </summary>
        /// <param name="predicate">The predicate to match the files to move</param>
        /// <param name="newDirectory">The directory to move the files into</param>
        /// <returns></returns>
        public MetalsharpDirectory MoveInput(Predicate<IMetalsharpFile> predicate, string newDirectory)
        {
            InputFiles.Where(i => predicate(i)).ToList().ForEach(i => i.Directory = newDirectory);
            return this;
        }

        /// <summary>
        /// Move files in the output from one directory to another
        /// </summary>
        /// <param name="oldDirectory">The directory to move the files from</param>
        /// <param name="newDirectory">The directory to move the files into</param>
        /// <returns></returns>
        public MetalsharpDirectory MoveOutput(string oldDirectory, string newDirectory) =>
            MoveOutput(file => file.Directory == oldDirectory, newDirectory);

        /// <summary>
        /// Move files in the output matching a predicate from one directory to another
        /// </summary>
        /// <param name="predicate">The predicate to match the files to move</param>
        /// <param name="newDirectory">The directory to move the files into</param>
        /// <returns></returns>
        public MetalsharpDirectory MoveOutput(Predicate<IMetalsharpFile> predicate, string newDirectory)
        {
            OutputFiles.Where(i => predicate(i)).ToList().ForEach(i => i.Directory = newDirectory);
            return this;
        }

        #endregion

        #region Remove Files

        /// <summary>
        /// Remove a file from the input and output
        /// </summary>
        /// <param name="path">The path of the file to remove</param>
        /// <returns></returns>
        public MetalsharpDirectory RemoveFiles(string path)
        {
            RemoveInput(path);
            RemoveOutput(path);
            return this;
        }

        /// <summary>
        /// Remove all the files matching a predicate from the input and output
        /// </summary>
        /// <param name="predicate">The predicate function to identify files to delete</param>
        /// <returns></returns>
        public MetalsharpDirectory RemoveFiles(Predicate<IMetalsharpFile> predicate)
        {
            RemoveInput(predicate);
            RemoveOutput(predicate);
            return this;
        }

        /// <summary>
        /// Remove a file from the input
        /// </summary>
        /// <param name="path">The path of the file to remove</param>
        /// <returns></returns>
        public MetalsharpDirectory RemoveInput(string path) =>
            RemoveInput(file => file.FilePath == path);

        /// <summary>
        /// Remove all the files matching a predicate from the input
        /// </summary>
        /// <param name="predicate">The predicate function to identify files to delete</param>
        /// <returns></returns>
        public MetalsharpDirectory RemoveInput(Predicate<IMetalsharpFile> predicate)
        {
            InputFiles.RemoveAll(predicate);
            return this;
        }

        /// <summary>
        /// Remove a file from the output
        /// </summary>
        /// <param name="path">The path of the file to remove</param>
        /// <returns></returns>
        public MetalsharpDirectory RemoveOutput(string path) =>
            RemoveOutput(file => file.FilePath == path);

        /// <summary>
        /// Remove all the files matching a predicate from the output
        /// </summary>
        /// <param name="predicate">The predicate function to identify files to delete</param>
        /// <returns></returns>
        public MetalsharpDirectory RemoveOutput(Predicate<IMetalsharpFile> predicate)
        {
            OutputFiles.RemoveAll(predicate);
            return this;
        }

        #endregion

        #region Use

        /// <summary>
        /// Invoke a function as a plugin
        /// </summary>
        /// <param name="func">The function to invoke</param>
        /// <returns></returns>
        public MetalsharpDirectory Use(Action<MetalsharpDirectory> func)
        {
            BeforeUse?.Invoke(this, new EventArgs());
            func(this);
            AfterUse?.Invoke(this, new EventArgs());
            return this;
        }

        /// <summary>
        /// Invoke a plugin
        /// </summary>
        /// <param name="plugin">The plugin to invoke</param>
        /// <returns></returns>
        public MetalsharpDirectory Use(IMetalsharpPlugin plugin) =>
            Use(i => plugin.Execute(i));

        /// <summary>
        /// Invoke a plugin by type
        /// 
        /// The plugin type must have an empty constructor
        /// </summary>
        /// <typeparam name="T">The type of the plugin to invoke</typeparam>
        /// <returns></returns>
        public MetalsharpDirectory Use<T>() where T : IMetalsharpPlugin, new() =>
            Use(new T());


        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Invoked before .Use()
        /// </summary>
        public event EventHandler BeforeUse;

        /// <summary>
        /// Invoked after .Use()
        /// </summary>
        public event EventHandler AfterUse;

        /// <summary>
        /// Invoked before .Build()
        /// </summary>
        public event EventHandler BeforeBuild;

        /// <summary>
        /// Invoked after .Build()
        /// </summary>
        public event EventHandler AfterBuild;

        #endregion

        #region Properties

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
