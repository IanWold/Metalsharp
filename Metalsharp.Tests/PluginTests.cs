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

            var directory = new MetalsharpDirectory("Scenario\\Plugins")
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
