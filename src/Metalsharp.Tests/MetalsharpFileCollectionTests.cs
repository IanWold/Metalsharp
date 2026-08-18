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

        [Fact]
        public void ContainsDirectoryReturnsTrueForMatchingDirectory()
        {
            var sep = Path.DirectorySeparatorChar;
            var collection = new MetalsharpFileCollection([new MetalsharpFile("text", $"Dir1{sep}F1.a")]);

            Assert.True(collection.ContainsDirectory("Dir1"));
            Assert.False(collection.ContainsDirectory("Dir2"));
        }

        [Fact]
        public void IndexerGetsAndSetsFileAtIndex()
        {
            var file1 = new MetalsharpFile("text", "F1.a");
            var file2 = new MetalsharpFile("text", "F2.a");
            var collection = new MetalsharpFileCollection([file1]);

            Assert.Same(file1, collection[0]);

            collection[0] = file2;

            Assert.Same(file2, collection[0]);
        }

        [Fact]
        public void IsReadOnlyIsFalse()
        {
            var collection = new MetalsharpFileCollection();

            Assert.False(collection.IsReadOnly);
        }

        [Fact]
        public void ClearRemovesAllFiles()
        {
            var collection = new MetalsharpFileCollection([new MetalsharpFile("text", "F1.a"), new MetalsharpFile("text", "F2.a")]);

            collection.Clear();

            Assert.Empty(collection);
        }

        [Fact]
        public void ContainsReturnsTrueForFileInCollection()
        {
#pragma warning disable xUnit2017
            var file = new MetalsharpFile("text", "F1.a");
            var otherFile = new MetalsharpFile("text", "F2.a");
            var collection = new MetalsharpFileCollection([file]);

            Assert.True(collection.Contains(file));
            Assert.False(collection.Contains(otherFile));
#pragma warning restore xUnit2017
        }

        [Fact]
        public void IndexOfReturnsCorrectIndex()
        {
            var file1 = new MetalsharpFile("text", "F1.a");
            var file2 = new MetalsharpFile("text", "F2.a");
            var collection = new MetalsharpFileCollection([file1, file2]);

            Assert.Equal(1, collection.IndexOf(file2));
            Assert.Equal(-1, collection.IndexOf(new MetalsharpFile("text", "F3.a")));
        }

        [Fact]
        public void InsertPlacesFileAtIndex()
        {
            var file1 = new MetalsharpFile("text", "F1.a");
            var file2 = new MetalsharpFile("text", "F2.a");
            var inserted = new MetalsharpFile("text", "F3.a");
            var collection = new MetalsharpFileCollection([file1, file2]);

            collection.Insert(1, inserted);

            Assert.Equal([file1, inserted, file2], collection);
        }

        [Fact]
        public void RemoveAtRemovesFileAtIndex()
        {
            var file1 = new MetalsharpFile("text", "F1.a");
            var file2 = new MetalsharpFile("text", "F2.a");
            var collection = new MetalsharpFileCollection([file1, file2]);

            collection.RemoveAt(0);

            Assert.Equal([file2], collection);
        }

        [Fact]
        public void NonGenericGetEnumeratorEnumeratesAllFiles()
        {
            var file = new MetalsharpFile("text", "F1.a");
            System.Collections.IEnumerable collection = new MetalsharpFileCollection([file]);

            var enumerated = new List<object>();
            foreach (var item in collection)
            {
                enumerated.Add(item);
            }

            Assert.Equal([file], enumerated);
        }
    }
}
