using System.IO;
using System.Linq;
using Xunit;
using static System.Convert;

namespace Metalsharp.Tests
{
    public class PluginTests
    {
        #region Branch Plugin

        [Fact]
        public void BranchExecutesBranchesIndependently()
        {
            var firstBranchExecuted = false;
            var secondBranchExecuted = false;

            new MetalsharpProject("Scenario\\Plugins")
                .Branch(
                    proj =>
                    {
                        firstBranchExecuted = true;
                        proj.AddInput("Scenario\\Directory1");
                        Assert.DoesNotContain(proj.InputFiles, i => i.Name == "file11");
                    },
                    proj =>
                    {
                        secondBranchExecuted = true;
                        proj.AddInput("Scenario\\Directory2\\SubDirectory1");
                        Assert.DoesNotContain(proj.InputFiles, i => i.Name == "file1");
                    }
                );

            Assert.True(firstBranchExecuted);
            Assert.True(secondBranchExecuted);
        }

        #endregion

        #region Collections Plugin

        [Fact]
        public void CollectionsCorrectlyGroupsFiles()
        {
            var project = new MetalsharpProject()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .AddOutput("Scenario\\Directory2\\SubDirectory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(project.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(project.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "file11");
            Assert.DoesNotContain(project.GetCollection("test")["input"], i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");

            Assert.Contains(project.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "file11");
            Assert.DoesNotContain(project.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(project.GetCollection("test")["output"], i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
        }

        [Fact]
        public void GetFilesFromCollectionReturnsCorrectFiles()
        {
            var project = new MetalsharpProject()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .AddOutput("Scenario\\Directory2\\SubDirectory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(project.GetFilesFromCollection("test"), i => i.Name == "file1");
            Assert.Contains(project.GetFilesFromCollection("test"), i => i.Name == "file11");
            Assert.DoesNotContain(project.GetFilesFromCollection("test"), i => i.Name == "FileMarkdown");
        }

        [Fact]
        public void CollectionsCorrectlyGroupsInputFiles()
        {
            var project = new MetalsharpProject()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));
            
            Assert.Contains(project.GetInputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(project.GetInputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
        }

        [Fact]
        public void CollectionsCorrectlyGroupsOutputFiles()
        {
            var project = new MetalsharpProject()
                .AddOutput("Scenario\\Directory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(project.GetOutputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "file1");
            Assert.DoesNotContain(project.GetOutputCollection("test"), i => Path.GetFileNameWithoutExtension(i) == "FileMarkdown");
        }

        [Fact]
        public void GetInputFilesFromCollectionReturnsCorrectFiles()
        {
            var project = new MetalsharpProject()
                .AddInput("Scenario\\Directory1")
                .AddInput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(project.GetInputFilesFromCollection("test"), i => i.Name == "file1");
            Assert.DoesNotContain(project.GetInputFilesFromCollection("test"), i => i.Name == "FileMarkdown");
        }

        [Fact]
        public void GetOutputFilesFromCollectionReturnsCorrectFiles()
        {
            var project = new MetalsharpProject()
                .AddOutput("Scenario\\Directory1")
                .AddOutput("Scenario\\Plugins")
                .UseCollections("test", file => file.Name.Contains("file"));

            Assert.Contains(project.GetOutputFilesFromCollection("test"), i => i.Name == "file1");
            Assert.DoesNotContain(project.GetOutputFilesFromCollection("test"), i => i.Name == "FileMarkdown");
        }

        #endregion

        #region Debug Plugin

        [Fact]
        public void DebugLogsAftereUse()
        {
            var project = new MetalsharpProject();
            var uses = 0;

            project.UseDebug(i =>
            {
                // onLog will be called after UseDebug is called,
                // Need to prevent the assertion before the use of TestPlugin
                if (uses == 1)
                {
                    Assert.True((bool)project.Metadata["test"]);
                }
                else
                {
                    uses++;
                }
            });
            project.Use<TestPlugin>();
        }

        #endregion

        #region Frontmatter Plugin

        [Fact]
        public void FrontmatterSucceedsWithoutFrontmatter()
        {
            var project = new MetalsharpProject("Scenario\\Plugins\\FileMarkdown.md").UseFrontmatter();

            Assert.True(project.InputFiles[0].Metadata.Count == 0);
        }

        [Fact]
        public void FrontmatterParsesJsonFrontmatter()
        {
            var project = new MetalsharpProject("Scenario\\Plugins\\FileJsonFrontmatter.md").UseFrontmatter();

            Assert.True((bool)project.InputFiles[0].Metadata["test"]);
        }

        [Fact]
        public void FrontmatterParsesYamlFrontmatter()
        {
            var project = new MetalsharpProject("Scenario\\Plugins\\FileYamlFrontmatter.md").UseFrontmatter();

            Assert.True(ToBoolean(project.InputFiles[0].Metadata["test"].ToString()));
        }

        #endregion

        #region Markdown Plugin Tests

        [Fact]
        public void MarkdownGeneratesHtmlFile()
        {
            var project = new MetalsharpProject("Scenario\\Plugins\\FileMarkdown.md").UseMarkdown();
            
            Assert.Contains(project.OutputFiles, i => i.Extension == ".html");
        }

        [Fact]
        public void MarkdownCopiesMetadata()
        {
            var project = new MetalsharpProject("Scenario\\Plugins\\FileMarkdown.md")
                .Use(proj => proj.InputFiles.ToList().ForEach(i => i.Metadata.Add("test", true)))
                .Use<Markdown>();

            Assert.True((bool)project.OutputFiles[0].Metadata["test"]);
        }

        #endregion
    }
}
