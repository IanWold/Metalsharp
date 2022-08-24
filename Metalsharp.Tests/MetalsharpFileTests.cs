using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Metalsharp.Tests
{
    public class MetalsharpFileTests
    {
        [Theory]
        [InlineData("file.txt")]
        [InlineData("Directory1\\file.md")]
        [InlineData("Directory1\\Directory2\\file.html")]
        public void DirectoryReturnsCorrectResult(string path)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());

            Assert.True(file.Directory == Path.GetDirectoryName(path));
        }

        [Theory]
        [InlineData("file.txt", "Dir")]
        [InlineData("Directory1\\file.md", "Dir")]
        [InlineData("Directory1\\Directory2\\file.html", "Dir")]
        public void DirectoryAssignsCorrectValue(string path, string directoryToAssign)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());
            file.Directory = directoryToAssign;

            Assert.True(file.FilePath == Path.Combine(directoryToAssign, Path.GetFileName(file.FilePath)));
        }
        
        [Theory]
        [InlineData("file.txt")]
        [InlineData("Directory1\\file.md")]
        [InlineData("Directory1\\Directory2\\file.html")]
        public void ExtensionReturnsCorrectResult(string path)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());

            Assert.True(file.Extension == Path.GetExtension(path));
        }

        [Theory]
        [InlineData("file.txt", ".cs")]
        [InlineData("Directory1\\file.md", ".db")]
        [InlineData("Directory1\\Directory2\\file.html", ".h")]
        public void ExtensionAssignsCorrectValue(string path, string extensionToAssign)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());
            file.Extension = extensionToAssign;

            Assert.True(file.FilePath == Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + extensionToAssign));
        }

        [Fact]
        public void FilePathWorks()
        {
            var file = new MetalsharpFile("text", "path", new Dictionary<string, object>());
            Assert.True(file.FilePath == "path");

            file.FilePath = "newpath";
            Assert.True(file.FilePath == "newpath");
        }

        [Fact]
        public void MetadataWorks()
        {
            var file = new MetalsharpFile("text", "path", new Dictionary<string, object>
            {
                ["test1"] = "value"
            });

            Assert.True(file.Metadata["test1"].ToString() == "value");
        }

        [Theory]
        [InlineData("file.txt")]
        [InlineData("Directory1\\file.md")]
        [InlineData("Directory1\\Directory2\\file.html")]
        public void NameReturnsCorrectResult(string path)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());

            Assert.True(file.Name == Path.GetFileNameWithoutExtension(path));
        }

        [Theory]
        [InlineData("file.txt", "newfile1")]
        [InlineData("Directory1\\file.md", "newfile2")]
        [InlineData("Directory1\\Directory2\\file.html", "newfile3")]
        public void NameAssignsCorrectValue(string path, string nameToAssign)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());
            file.Name = nameToAssign;

            Assert.True(file.FilePath == Path.Combine(Path.GetDirectoryName(path), nameToAssign + Path.GetExtension(path)));
        }

        [Fact]
        public void TextWorks()
        {
            var file = new MetalsharpFile("text", "path", new Dictionary<string, object>());
            Assert.True(file.Text == "text");

            file.Contents = Encoding.Default.GetBytes("newtext");
            Assert.True(file.Text == "newtext");
        }

        [Theory]
        [InlineData("Directory1\\file.txt", "Directory1", true)]
        [InlineData("Directory1\\file.txt", "Directory2", false)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory2", true)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory1\\Directory2", true)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory3", false)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory1\\Directory3", false)]
        public void IsDescendantOfReturnsCorrectResult(string path, string ancestorDirectory, bool expectedResult)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());

            Assert.True(file.IsDescendantOf(ancestorDirectory) == expectedResult);
        }

        [Theory]
        [InlineData("Directory1\\file.txt", "Directory1", true)]
        [InlineData("Directory1\\file.txt", "Directory2", false)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory1", false)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory2", true)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory1\\Directory2", true)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory3", false)]
        [InlineData("Directory1\\Directory2\\file.txt", "Directory1\\Directory3", false)]
        public void IsChildOfReturnsCorrectResult(string path, string parentDirectory, bool expectedResult)
        {
            var file = new MetalsharpFile("text", path, new Dictionary<string, object>());

            Assert.True(file.IsChildOf(parentDirectory) == expectedResult);
        }
    }
}
