using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Metalsharp.Tests
{
    public class MetalsharpDirectoryTests
    {
        #region Add Files

        [Theory]
        [InlineData("Scenario\\Directory1", 5)]
        [InlineData("Scenario\\Directory2", 10)]
        public void AddInputSinglePathAddsCorrectNumberOfFiles(string path, int expectedFileCount)
        {
            var directory = new MetalsharpDirectory().AddInput(path);
            Assert.True(directory.InputFiles.Count == expectedFileCount);
        }

        [Theory]
        [InlineData("Scenario\\Directory1", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1", "Dir")]
        [InlineData("Scenario\\Directory1\\file1.md", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1\\file1.md", "Dir")]
        [InlineData("Scenario\\Directory2", "Dir")]
        public void AddInputAddsFilesToCorrectDirectory(string diskPath, string virtualPath)
        {
            var directory = new MetalsharpDirectory().AddInput(diskPath, virtualPath);

            foreach (var file in directory.InputFiles)
            {
                var newPath = (Directory.Exists(diskPath) ? diskPath : Path.GetDirectoryName(diskPath)) +
                    string.Concat(file.FilePath.Skip(virtualPath.Length));

                Assert.True(File.Exists(newPath));
            }
        }

        [Fact]
        public void AddInputThrowsGivenNonexistantPath()
        {
            var nonexistentPath = "\\Does\\Not\\Exist";
            var directory = new MetalsharpDirectory();

            Assert.Throws<ArgumentException>(() => directory.AddInput(nonexistentPath));
            Assert.Throws<ArgumentException>(() => directory.AddInput(nonexistentPath, "Dir"));
        }

        [Fact]
        public void AddInputAddsMetalsharpFile()
        {
            var file = new MetalsharpFile("fileText", "filePath");
            var directory = new MetalsharpDirectory().AddInput(file);

            Assert.True(directory.InputFiles.Contains(file));
        }

        [Theory]
        [InlineData("Scenario\\Directory1", 5)]
        [InlineData("Scenario\\Directory2", 10)]
        public void AddOutputSinglePathAddsCorrectNumberOfFiles(string path, int expectedFileCount)
        {
            var directory = new MetalsharpDirectory().AddOutput(path);
            Assert.True(directory.OutputFiles.Count == expectedFileCount);
        }

        [Theory]
        [InlineData("Scenario\\Directory1", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1", "Dir")]
        [InlineData("Scenario\\Directory1\\file1.md", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1\\file1.md", "Dir")]
        [InlineData("Scenario\\Directory2", "Dir")]
        public void AddOutputAddsFilesToCorrectDirectory(string diskPath, string virtualPath)
        {
            var directory = new MetalsharpDirectory().AddOutput(diskPath, virtualPath);

            foreach (var file in directory.OutputFiles)
            {
                var newPath = (Directory.Exists(diskPath) ? diskPath : Path.GetDirectoryName(diskPath)) +
                    string.Concat(file.FilePath.Skip(virtualPath.Length));

                Assert.True(File.Exists(newPath));
            }
        }

        [Fact]
        public void AddOutputThrowsGivenNonexistantPath()
        {
            var nonexistentPath = "\\Does\\Not\\Exist";
            var directory = new MetalsharpDirectory();

            Assert.Throws<ArgumentException>(() => directory.AddOutput(nonexistentPath));
            Assert.Throws<ArgumentException>(() => directory.AddOutput(nonexistentPath, "Dir"));
        }

        [Fact]
        public void AddOutputAddsMetalsharpFile()
        {
            var file = new MetalsharpFile("fileText", "filePath");
            var directory = new MetalsharpDirectory().AddOutput(file);

            Assert.True(directory.OutputFiles.Contains(file));
        }

        #endregion

        #region Build

        [Fact]
        public void BuildWithDefaultOptionsWritesToCurrentDirectory()
        {
            if (File.Exists("OutputFile1.txt"))
            {
                File.Delete("OutputFile1.txt");
            }

            new MetalsharpDirectory()
                .AddOutput(new MetalsharpFile("text", "OutputFile1.txt"))
                .Build();

            Assert.True(File.Exists("OutputFile1.txt"));
        }

        [Fact]
        public void BuildWithDefaultOptionsDoesNotOverwriteOutputDirectory()
        {
            if (!File.Exists("ShouldNotDelete.txt"))
            {
                File.Create("ShouldNotDelete.txt");
            }

            new MetalsharpDirectory()
                .AddOutput(new MetalsharpFile("text", "OutputFile1.txt"))
                .Build();

            Assert.True(File.Exists("ShouldNotDelete.txt"));
        }

        [Fact]
        public void BuildWritesOutputFilesToOutputDirectory()
        {
            new MetalsharpDirectory()
                .AddInput(new MetalsharpFile("text", "InputFile1.txt"))
                .AddInput(new MetalsharpFile("text", "InputDir\\InputFile2.txt"))
                .AddOutput(new MetalsharpFile("text", "OutputFile1.txt"))
                .AddOutput(new MetalsharpFile("text", "OutputDir\\OutputFile2.txt"))
                .Build(new BuildOptions()
                {
                    OutputDirectory = "Output",
                    ClearOutputDirectory = true
                });

            Assert.True(File.Exists("Output\\OutputFile1.txt"));
            Assert.True(File.Exists("Output\\OutputDir\\OutputFile2.txt"));

            Assert.False(File.Exists("Output\\InputFile1.txt"));
            Assert.False(File.Exists("Output\\InputDir\\InputFile2.txt"));
        }

        [Fact]
        public void BuildExecutesProvidedFunctionBeforeBuild()
        {
            var wasExecuted = false;

            if (File.Exists("OutputFile2.txt"))
            {
                File.Delete("OutputFile2.txt");
            }

            new MetalsharpDirectory()
                .AddOutput(new MetalsharpFile("text", "OutputFile2.txt"))
                .Build(dir =>
                {
                    wasExecuted = true;
                    Assert.False(File.Exists("OutputFile2.txt"));
                });

            Assert.True(wasExecuted);
        }

        #endregion

        #region Metadata

        [Fact]
        public void MetadataSinglePairAddsAndOverwrites()
        {
            var directory = new MetalsharpDirectory();

            directory.Meta("key", "value1");
            Assert.True(directory.Metadata.ContainsKey("key"));
            Assert.True(directory.Metadata["key"].ToString() == "value1");

            directory.Meta("key", "value2");
            Assert.True(directory.Metadata["key"].ToString() == "value2");
        }

        [Fact]
        public void MetadataMultiplePairsAddAndOverwrite()
        {
            var directory = new MetalsharpDirectory();

            directory.Meta(("key1", "value1"), ("key2", "value1"), ("key3", "value1"));
            Assert.True(directory.Metadata.ContainsKey("key1"));
            Assert.True(directory.Metadata.ContainsKey("key2"));
            Assert.True(directory.Metadata.ContainsKey("key3"));
            Assert.True(directory.Metadata["key1"].ToString() == "value1");
            Assert.True(directory.Metadata["key2"].ToString() == "value1");
            Assert.True(directory.Metadata["key3"].ToString() == "value1");

            directory.Meta(("key1", "value2"), ("key2", "value2"), ("key3", "value2"));
            Assert.True(directory.Metadata["key1"].ToString() == "value2");
            Assert.True(directory.Metadata["key2"].ToString() == "value2");
            Assert.True(directory.Metadata["key3"].ToString() == "value2");
        }

        #endregion

        #region Move Files



        #endregion

        #region Remove Files



        #endregion

        #region Use



        #endregion
    }
}
