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



        #endregion

        #region Metadata



        #endregion

        #region Move Files



        #endregion

        #region Remove Files



        #endregion

        #region Use



        #endregion
    }
}
