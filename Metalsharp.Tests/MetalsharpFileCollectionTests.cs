using System.Linq;
using Xunit;

namespace Metalsharp.Tests
{
    public class MetalsharpFileCollectionTests
    {
        [Theory]
        [InlineData(new[] { "Dir1\\F1.a", "Dir1\\F2.a", "Dir2\\F3.a" }, "Dir1", new[] { "F1", "F2" })]
        [InlineData(new[] { "Dir1\\Dir2\\F1.a", "Dir1\\Dir3\\F2.a", "Dir4\\F3.a" }, "Dir1", new[] { "F1", "F2" })]
        public void DescendantsOfReturnsCorrectFiles(string[] paths, string ancestorDirectory, string[] expectedFileNames)
        {
            var collection = new MetalsharpFileCollection(paths.Select(p => new MetalsharpFile("text", p)));

            foreach (var name in collection.DescendantsOf(ancestorDirectory).Select(i => i.Name))
            {
                Assert.Contains(name, expectedFileNames);
            }
        }

        [Theory]
        [InlineData(new[] { "Dir1\\F1.a", "Dir1\\F2.a", "Dir2\\F3.a" }, "Dir1", new[] { "F1", "F2" })]
        [InlineData(new[] { "Dir1\\Dir2\\F1.a", "Dir3\\Dir2\\F2.a", "Dir4\\F3.a" }, "Dir2", new[] { "F1", "F2" })]
        public void ChildrenOfReturnsCorrectFiles(string[] paths, string parentDirectory, string[] expectedFileNames)
        {
            var collection = new MetalsharpFileCollection(paths.Select(p => new MetalsharpFile("text", p)));

            foreach (var name in collection.ChildrenOf(parentDirectory).Select(i => i.Name))
            {
                Assert.Contains(name, expectedFileNames);
            }
        }
    }
}
