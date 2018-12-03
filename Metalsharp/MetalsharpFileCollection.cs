using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Metalsharp
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
        /// The Metalsharp files in the collection
        /// </summary>
        readonly List<T> _items = new List<T>();

        /// <summary>
        /// Instantiate an empty collection
        /// </summary>
        public MetalsharpFileCollection() { }

        /// <summary>
        /// Instantiate a collection with an existing one
        /// </summary>
        /// <param name="files"></param>
        public MetalsharpFileCollection(IEnumerable<T> files) =>
            _items = files.ToList();

        /// <summary>
        /// Get the descendant files of a directory
        /// </summary>
        /// <param name="directory">The ancestor directory</param>
        /// <returns></returns>
        public IMetalsharpFileCollection<T> DescendantsOf(string directory) =>
            _items.Where(i => i.IsDescendantOf(directory)).ToMetalsharpFileCollection();

        /// <summary>
        /// Get the children files of a directory
        /// </summary>
        /// <param name="directory">The parent directory</param>
        /// <returns></returns>
        public IMetalsharpFileCollection<T> ChildrenOf(string directory) =>
            _items.Where(i => i.IsChildOf(directory)).ToMetalsharpFileCollection();

        /// <summary>
        /// Returns true if one of the files in the collection descends from the directory
        /// </summary>
        /// <param name="directory">The directory in question</param>
        /// <returns></returns>
        public bool ContainsDirectory(string directory) =>
            _items.Exists(i => i.IsDescendantOf(directory));

        /// <summary>
        /// Implements IList
        /// </summary>
        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        /// <summary>
        /// Implements IList
        /// </summary>
        public int Count =>
            _items.Count();

        /// <summary>
        /// Implements IList
        /// </summary>
        public bool IsReadOnly =>
            false;

        /// <summary>
        /// Implements IList
        /// </summary>
        public void Add(T item) =>
            _items.Add(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void Clear() =>
            _items.Clear();

        /// <summary>
        /// Implements IList
        /// </summary>
        public bool Contains(T item) =>
            _items.Contains(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void CopyTo(T[] array, int arrayIndex) =>
            _items.CopyTo(array, arrayIndex);

        /// <summary>
        /// Implements IList
        /// </summary>
        public IEnumerator<T> GetEnumerator() =>
            _items.GetEnumerator();

        /// <summary>
        /// Implements IList
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Implements IList
        /// </summary>
        public int IndexOf(T item) =>
            _items.IndexOf(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void Insert(int index, T item) =>
            _items.Insert(index, item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public bool Remove(T item) =>
            _items.Remove(item);

        /// <summary>
        /// Implements IList
        /// </summary>
        public void RemoveAt(int index) =>
            _items.RemoveAt(index);

        /// <summary>
        /// Implements IMetalsharpCollection
        /// </summary>
        public int RemoveAll(Predicate<T> match) =>
            _items.RemoveAll(match);
    }

    /// <summary>
    /// MetalsharpFileCollection extensions for IEnumerable
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Mimic IEnumerable.ToList
        /// </summary>
        /// <typeparam name="T">The type of the collection</typeparam>
        /// <param name="list">The IEnumerable to convert to an IMetalsharpFileCollection</param>
        /// <returns></returns>
        public static IMetalsharpFileCollection<T> ToMetalsharpFileCollection<T>(this IEnumerable<T> list) where T : IMetalsharpFile =>
            new MetalsharpFileCollection<T>(list);
    }
}
