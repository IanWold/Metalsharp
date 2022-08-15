using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Metalsharp.Tests
{
    public class MetalsharpProjectTests
    {
        #region Add Files

        [Theory]
        [InlineData("Scenario\\Directory1", 5)]
        [InlineData("Scenario\\Directory2", 10)]
        public void AddInputSinglePathAddsCorrectNumberOfFiles(string path, int expectedFileCount)
        {
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).AddInput(path);
            Assert.True(project.InputFiles.Count == expectedFileCount);
        }

        [Theory]
        [InlineData("Scenario\\Directory1", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1", "Dir")]
        [InlineData("Scenario\\Directory1\\file1.md", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1\\file1.md", "Dir")]
        [InlineData("Scenario\\Directory2", "Dir")]
        public void AddInputAddsFilesToCorrectDirectory(string diskPath, string virtualPath)
        {
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).AddInput(diskPath, virtualPath);

            foreach (var file in project.InputFiles)
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
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None });

            Assert.Throws<ArgumentException>(() => project.AddInput(nonexistentPath));
            Assert.Throws<ArgumentException>(() => project.AddInput(nonexistentPath, "Dir"));
        }

        [Fact]
        public void AddInputAddsMetalsharpFile()
        {
            var file = new MetalsharpFile("fileText", "filePath");
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).AddInput(file);

            Assert.True(project.InputFiles.Contains(file));
        }

        [Theory]
        [InlineData("Scenario\\Directory1", 5)]
        [InlineData("Scenario\\Directory2", 10)]
        public void AddOutputSinglePathAddsCorrectNumberOfFiles(string path, int expectedFileCount)
        {
            var project = new MetalsharpProject().AddOutput(path);
            Assert.True(project.OutputFiles.Count == expectedFileCount);
        }

        [Theory]
        [InlineData("Scenario\\Directory1", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1", "Dir")]
        [InlineData("Scenario\\Directory1\\file1.md", "Scenario\\Directory1")]
        [InlineData("Scenario\\Directory1\\file1.md", "Dir")]
        [InlineData("Scenario\\Directory2", "Dir")]
        public void AddOutputAddsFilesToCorrectDirectory(string diskPath, string virtualPath)
        {
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).AddOutput(diskPath, virtualPath);

            foreach (var file in project.OutputFiles)
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
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None });

            Assert.Throws<ArgumentException>(() => project.AddOutput(nonexistentPath));
            Assert.Throws<ArgumentException>(() => project.AddOutput(nonexistentPath, "Dir"));
        }

        [Fact]
        public void AddOutputAddsMetalsharpFile()
        {
            var file = new MetalsharpFile("fileText", "filePath");
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).AddOutput(file);

            Assert.True(project.OutputFiles.Contains(file));
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

            new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
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

            new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddOutput(new MetalsharpFile("text", "OutputFile1.txt"))
                .Build();

            Assert.True(File.Exists("ShouldNotDelete.txt"));
        }

        [Fact]
        public void BuildWritesOutputFilesToOutputDirectory()
        {
            new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(new MetalsharpFile("text", "InputFile1.txt"))
                .AddInput(new MetalsharpFile("text", "InputDir\\InputFile2.txt"))
                .AddOutput(new MetalsharpFile("text", "OutputFile1.txt"))
                .AddOutput(new MetalsharpFile("text", "OutputDir\\OutputFile2.txt"))
                .Build(new BuildOptions
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

            new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddOutput(new MetalsharpFile("text", "OutputFile2.txt"))
                .Build(proj =>
                {
                    wasExecuted = true;
                    Assert.False(File.Exists("OutputFile2.txt"));
                });

            Assert.True(wasExecuted);
        }

        [Fact]
        public void BuildInvokesBeforeBuildEvent()
        {
            var wasInvoked = false;

            if (File.Exists("OutputFile3.txt"))
            {
                File.Delete("OutputFile3.txt");
            }

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).AddOutput(new MetalsharpFile("text", "OutputFile3.txt"));

            project.BeforeBuild += (sender, e) =>
            {
                wasInvoked = true;
                Assert.False(File.Exists("OutputFile3.txt"));
            };

            project.Build(proj => Assert.True(wasInvoked), new BuildOptions());

            Assert.True(wasInvoked);
        }

        [Fact]
        public void BuildInvokesAfterBuildEvent()
        {
            var wasInvoked = false;

            if (File.Exists("OutputFile4.txt"))
            {
                File.Delete("OutputFile4.txt");
            }

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).AddOutput(new MetalsharpFile("text", "OutputFile4.txt"));

            project.AfterBuild += (sender, e) =>
            {
                wasInvoked = true;
                // File may or may not exist here - cannot test this?
            };

            project.Build(proj => Assert.False(wasInvoked), new BuildOptions());

            Assert.True(wasInvoked);
        }

        #endregion

        #region Metadata

        [Fact]
        public void MetadataSinglePairAddsAndOverwrites()
        {
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None });

            project.Meta("key", "value1");
            Assert.True(project.Metadata.ContainsKey("key"));
            Assert.True(project.Metadata["key"].ToString() == "value1");

            project.Meta("key", "value2");
            Assert.True(project.Metadata["key"].ToString() == "value2");
        }

        [Fact]
        public void MetadataMultiplePairsAddAndOverwrite()
        {
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None });

            project.Meta(("key1", "value1"), ("key2", "value1"), ("key3", "value1"));
            Assert.True(project.Metadata.ContainsKey("key1"));
            Assert.True(project.Metadata.ContainsKey("key2"));
            Assert.True(project.Metadata.ContainsKey("key3"));
            Assert.True(project.Metadata["key1"].ToString() == "value1");
            Assert.True(project.Metadata["key2"].ToString() == "value1");
            Assert.True(project.Metadata["key3"].ToString() == "value1");

            project.Meta(("key1", "value2"), ("key2", "value2"), ("key3", "value2"));
            Assert.True(project.Metadata["key1"].ToString() == "value2");
            Assert.True(project.Metadata["key2"].ToString() == "value2");
            Assert.True(project.Metadata["key3"].ToString() == "value2");
        }

        #endregion

        #region Move Files

        [Fact]
        public void MoveFilesByPathEquivalentToInputAndOutput()
        {
            var file = new MetalsharpFile("text", "dir1\\File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .AddOutput(file)
                .MoveFiles("dir1", "dir2");

            Assert.True(project.InputFiles[0].Directory == "dir2");
            Assert.True(project.OutputFiles[0].Directory == "dir2");
        }

        [Fact]
        public void MoveFilesByPredicateEquivalentToInputAndOutput()
        {
            var file = new MetalsharpFile("text", "dir1\\File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .AddOutput(file)
                .MoveFiles(f => f.Text == "text", "dir2");

            Assert.True(project.InputFiles[0].Directory == "dir2");
            Assert.True(project.OutputFiles[0].Directory == "dir2");
        }

        [Fact]
        public void MoveInputByPathMovesFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "dir1\\File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .MoveInput("dir1", "dir2");

            Assert.True(project.InputFiles[0].Directory == "dir2");
        }

        [Fact]
        public void MoveInputByPredicateSelectsFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "dir1\\File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .MoveInput(f => f.Text == "text", "dir2");

            Assert.True(project.InputFiles[0].Directory == "dir2");
        }

        [Fact]
        public void MoveOutputByPathMovesFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "dir1\\File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddOutput(file)
                .MoveOutput("dir1", "dir2");

            Assert.True(project.OutputFiles[0].Directory == "dir2");
        }

        [Fact]
        public void MoveOutputByPredicateSelectsFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "dir1\\File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddOutput(file)
                .MoveOutput(f => f.Text == "text", "dir2");

            Assert.True(project.OutputFiles[0].Directory == "dir2");
        }

        #endregion

        #region Remove Files

        [Fact]
        public void RemoveFilesByPathEquivalentToInputAndOutput()
        {
            var file = new MetalsharpFile("text", "File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .AddOutput(file)
                .RemoveFiles(file.FilePath);

            Assert.False(project.InputFiles.Contains(file));
            Assert.False(project.OutputFiles.Contains(file));
        }

        [Fact]
        public void RemoveFilesByPredicateEquivalentToInputAndOutput()
        {
            var file = new MetalsharpFile("text", "File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .AddOutput(file)
                .RemoveFiles(f => f.Text == "text");

            Assert.False(project.InputFiles.Contains(file));
            Assert.False(project.OutputFiles.Contains(file));
        }

        [Fact]
        public void RemoveInputByPathRemovesFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .RemoveInput(file.FilePath);

            Assert.False(project.InputFiles.Contains(file));
        }

        [Fact]
        public void RemoveInputByPredicateSelectsFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddInput(file)
                .RemoveInput(f => f.Text == "text");

            Assert.False(project.InputFiles.Contains(file));
        }

        [Fact]
        public void RemoveOutputByPathRemovesFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddOutput(file)
                .RemoveOutput(file.FilePath);

            Assert.False(project.OutputFiles.Contains(file));
        }

        [Fact]
        public void RemoveOutputByPredicateSelectsFilesCorrectly()
        {
            var file = new MetalsharpFile("text", "File.txt");

            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None })
                .AddOutput(file)
                .RemoveOutput(f => f.Text == "text");

            Assert.False(project.OutputFiles.Contains(file));
        }

        #endregion

        #region Use

        [Fact]
        public void UsePluginInstanceExecutesPlugin()
        {
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).Use(new TestPlugin());

            Assert.True((bool)project.Metadata["test"]);
        }

        [Fact]
        public void UsePluginTypeExecutesPlugin()
        {
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).Use<TestPlugin>();

            Assert.True((bool)project.Metadata["test"]);
        }

        [Fact]
        public void UseFuncExecutesFunc()
        {
            var wasExecuted = false;
            new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None }).Use(proj => wasExecuted = true);

            Assert.True(wasExecuted);
        }

        [Fact]
        public void UseInvokesBeforeUseEevent()
        {
            var wasExecuted = false;
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None });

            project.BeforeUse += (sender, e) =>
            {
                Assert.False(wasExecuted);
            };

            project.Use(proj => wasExecuted = true);
        }

        [Fact]
        public void UseInvokesAfterUseEvent()
        {
            var wasExecuted = false;
            var project = new MetalsharpProject(new MetalsharpConfiguration() { Verbosity = Logging.LogLevel.None });

            project.AfterUse += (sender, e) =>
            {
                Assert.True(wasExecuted);
            };

            project.Use(proj => wasExecuted = true);
        }

        #endregion
    }
}
