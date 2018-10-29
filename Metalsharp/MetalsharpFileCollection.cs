using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Metal.Sharp
{
    /// <summary>
    /// Represents a collection of Metalsharp files
    /// 
    /// Implements members to handle "virtual" directories
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MetalsharpFileCollection<T> : IMetalsharpFileCollection<T> where T : IMetalsharpFile
    {
        /// <summary>
        /// Instantiate an empty collection
        /// </summary>
        public MetalsharpFileCollection() { }

        /// <summary>
        /// Instantiate a collection with an existing one
        /// </summary>
        /// <param name="files"></param>
        public MetalsharpFileCollection(IEnumerable<T> files) =>
            items = files.ToList();

        /// <summary>
        /// The Metalsharp files in the collection
        /// </summary>
        private List<T> items = new List<T>();

        /// <summary>
        /// Get the descendant files of a directory
        /// </summary>
        /// <param name="directory">The ancestor directory</param>
        /// <returns></returns>
        public IMetalsharpFileCollection<T> DescendantsOf(string directory) =>
            items.Where(i => i.IsDescendantOf(directory)).ToMetalsharpFileCollection();

        /// <summary>
        /// Get the children files of a directory
        /// </summary>
        /// <param name="directory">The parent directory</param>
        /// <returns></returns>
        public IMetalsharpFileCollection<T> ChildrenOf(string directory) =>
            items.Where(i => i.IsChildOf(directory)).ToMetalsharpFileCollection();

        /// <summary>
        /// Returns true if one of the files in the collection descends from the directory
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        public bool ContainsDirectory(string directory) =>
            items.Exists(i => i.IsDescendantOf(directory));

        /// <summary>
        /// Implements IList
        /// </summary>
        public T this[int index]
        {
            get => items[index];
            set => items[index] = value;
        }

        /// <summary>
        /// Implements IList
        /// </summary>
        public int Count =>
            items.Count();

        /// <summary>
        /// Implements IList
        /// </summary>
        public bool IsReadOnly =>
            false;

        /// <summary>
        /// Implements IList
        /// </summary>
        public void Add(T item) =>
            items.Add(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void Clear() =>
            items.Clear();

        /// <summary>
        /// Implements IList
        /// </summary>
        public bool Contains(T item) =>
            items.Contains(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex) =>
            items.CopyTo(array, arrayIndex);

        /// <summary>
        /// Implements IList
        /// </summary>
        public IEnumerator<T> GetEnumerator() =>
            items.GetEnumerator();

        /// <summary>
        /// Implements IList
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Implements IList
        /// </summary>
        public int IndexOf(T item) =>
            items.IndexOf(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void Insert(int index, T item) =>
            items.Insert(index, item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public bool Remove(T item) =>
            items.Remove(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void RemoveAt(int index) =>
            items.RemoveAt(index);

        /// <summary>
        /// Implements IMetalsharpCollection
        /// </summary>
        public int RemoveAll(Predicate<T> match) =>
            items.RemoveAll(match);
    }

    public static class IEnumerableExtensions
    {
        public static IMetalsharpFileCollection<T> ToMetalsharpFileCollection<T>(this IEnumerable<T> list) where T : IMetalsharpFile =>
            new MetalsharpFileCollection<T>(list);
    }
}
