using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using static System.Convert;

namespace Metalsharp.Tests
{
    public class PluginTests
    {
        #region Branch Plugin

        [Fact(Skip = "https://github.com/IanWold/Metalsharp/issues/4")]
        public void BranchExecutesBranchesIndependently()
        {
            var firstBranchExecuted = false;
            var secondBranchExecuted = false;

            new MetalsharpDirectory("Scenario\\Plugins")
                .Branch(
                    dir =>
                    {
                        firstBranchExecuted = true;
                        dir.AddInput("Scenario\\Directory1");
                        Assert.DoesNotContain(dir.InputFiles, i => i.Name == "file11");
                    },
                    dir =>
                    {
                        secondBranchExecuted = true;
                        dir.AddInput("Scenario\\Directory2\\SubDirectory1");
                        Assert.DoesNotContain(dir.InputFiles, i => i.Name == "file1");
                    }
                );

            Assert.True(firstBranchExecuted);
            Assert.True(secondBranchExecuted);
        }

        #endregion

        #region Collections Plugin

        [Fact(Skip = "https://github.com/IanWold/Metalsharp/issues/4")]
        public void CollectionsCorrectlyGroupsFiles()
        {
            var directory = new MetalsharpDirectory()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .AddOutput("Scenario\\Directory2\\SubDirectory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(directory.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(directory.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "file11");
            Assert.DoesNotContain(directory.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");

            Assert.Contains(directory.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "file11");
            Assert.DoesNotContain(directory.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(directory.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
        }

        [Fact(Skip = "https://github.com/IanWold/Metalsharp/issues/4")]
        public void GetFilesFromCollectionReturnsCorrectFiles()
        {
            var directory = new MetalsharpDirectory()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .AddOutput("Scenario\\Directory2\\SubDirectory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(directory.GetFilesFromCollection("test"), i => i.Name == "file1");
            Assert.Contains(directory.GetFilesFromCollection("test"), i => i.Name == "file11");
            Assert.DoesNotContain(directory.GetFilesFromCollection("test"), i => i.Name == "FileMarkdown");
        }

        [Fact(Skip = "https://github.com/IanWold/Metalsharp/issues/4")]
        public void CollectionsCorrectlyGroupsInputFiles()
        {
            var directory = new MetalsharpDirectory()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));
            
            Assert.Contains(directory.GetInputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(directory.GetInputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
        }

        [Fact(Skip = "https://github.com/IanWold/Metalsharp/issues/4")]
        public void CollectionsCorrectlyGroupsOutputFiles()
        {
            var directory = new MetalsharpDirectory()
                .AddOutput("Scenario\\Directory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(directory.GetOutputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(directory.GetOutputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
        }

        [Fact(Skip = "https://github.com/IanWold/Metalsharp/issues/4")]
        public void GetInputFilesFromCollectionReturnsCorrectFiles()
        {
            var directory = new MetalsharpDirectory()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(directory.GetInputFilesFromCollection("test"), i => i.Name == "file1");
            Assert.DoesNotContain(directory.GetInputFilesFromCollection("test"), i => i.Name == "FileMarkdown");
        }

        [Fact(Skip = "https://github.com/IanWold/Metalsharp/issues/4")]
        public void GetOutputFilesFromCollectionReturnsCorrectFiles()
        {
            var directory = new MetalsharpDirectory()
                .AddOutput("Scenario\\Directory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(directory.GetOutputFilesFromCollection("test"), i => i.Name == "file1");
            Assert.DoesNotContain(directory.GetOutputFilesFromCollection("test"), i => i.Name == "FileMarkdown");
        }

        #endregion

        #region Debug Plugin

        [Fact]
        public void DebugLogsAftereUse()
        {
            var directory = new MetalsharpDirectory();
            var uses = 0;

            directory.UseDebug(i =>
            {
                // onLog will be called after UseDebug is called,
                // Need to prevent the assertion before the use of TestPlugin
                if (uses == 1)
                {
                    Assert.True((bool)directory.Metadata["test"]);
                }
                else
                {
                    uses++;
                }
            });
            directory.Use<TestPlugin>();
        }

        #endregion

        #region Frontmatter Plugin

        [Fact]
        public void FrontmatterSucceedsWithoutFrontmatter()
        {
            var directory = new MetalsharpDirectory("Scenario\\Plugins\\FileMarkdown.md").UseFrontmatter();

            Assert.True(directory.InputFiles[0].Metadata.Count == 0);
        }

        [Fact]
        public void FrontmatterParsesJsonFrontmatter()
        {
            var directory = new MetalsharpDirectory("Scenario\\Plugins\\FileJsonFrontmatter.md").UseFrontmatter();

            Assert.True((bool)directory.InputFiles[0].Metadata["test"]);
        }

        [Fact]
        public void FrontmatterParsesYamlFrontmatter()
        {
            var directory = new MetalsharpDirectory("Scenario\\Plugins\\FileYamlFrontmatter.md").UseFrontmatter();

            Assert.True(ToBoolean(directory.InputFiles[0].Metadata["test"].ToString()));
        }

        #endregion

        #region Markdown Plugin Tests

        [Fact]
        public void MarkdownGeneratesHtmlFile()
        {
            var directory = new MetalsharpDirectory("Scenario\\Plugins\\FileMarkdown.md").UseMarkdown();
            
            Assert.Contains(directory.OutputFiles, i => i.Extension == ".html");
        }

        [Fact]
        public void MarkdownCopiesMetadata()
        {
            var directory = new MetalsharpDirectory("Scenario\\Plugins\\FileMarkdown.md")
                .Use(dir => dir.InputFiles.ToList().ForEach(i => i.Metadata.Add("test", true)))
                .Use<Markdown>();

            Assert.True((bool)directory.OutputFiles[0].Metadata["test"]);
        }

        #endregion
    }
}
