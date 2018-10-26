using System;
using System.Collections.Generic;
using System.IO;

namespace Metal.Sharp
{
    public class Metalsharp
    {
        public Metalsharp(string path)
        {
            AddInput(path, true);
            RootDirectory = path;
        }

        #region Methods

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

        public Metalsharp AddOutput(string path, bool enforceDirectory = false)
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    OutputFiles.Add(new OutputFile(path));
                }

                return this;
            }
            else if (File.Exists(path) && !enforceDirectory)
            {
                OutputFiles.Add(new OutputFile(path));

                return this;
            }
            else if (enforceDirectory) throw new ArgumentException("Directory " + path + " does not exist.");
            else throw new ArgumentException("File " + path + " does not exist.");
        }

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

        public void Build(Action<Metalsharp> func, BuildOptions options = null)
        {
            func(this);
            Build(options);
        }

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

        public Metalsharp RemoveInput(string path) =>
            RemoveInput(file => file.FilePath == path);

        public Metalsharp RemoveInput(Func<InputFile, bool> predicate)
        {
            InputFiles.RemoveAll(file => predicate(file));
            return this;
        }

        public Metalsharp RemoveOutput(string path) =>
            RemoveOutput(file => file.FilePath == path);

        public Metalsharp RemoveOutput(Func<OutputFile, bool> predicate)
        {
            OutputFiles.RemoveAll(file => predicate(file));
            return this;
        }

        public Metalsharp Use(Func<Metalsharp, Metalsharp> func) =>
            func(this);

        public Metalsharp Use(IMetalsharpPlugin plugin) =>
            plugin.Execute(this);

        public Metalsharp Use<T>() where T : IMetalsharpPlugin, new() =>
            new T().Execute(this);

        #endregion

        #region Properties

        public string RootDirectory { get; set; }

        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public List<InputFile> InputFiles { get; set; } = new List<InputFile>();

        public List<OutputFile> OutputFiles { get; set; } = new List<OutputFile>();

        #endregion
    }
}
