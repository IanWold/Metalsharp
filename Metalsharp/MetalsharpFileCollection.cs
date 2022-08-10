using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Metalsharp;

/// <summary>
///     Represents a collection of Metalsharp files.
/// </summary>
/// 
/// <typeparam name="T">
///     The type of the files. These must be of type `IMetalsharpFile`.
/// </typeparam>
public class MetalsharpFileCollection<T> : IMetalsharpFileCollection<T> where T : IMetalsharpFile
{
	/// <summary>
	///     The Metalsharp files in the collection.
	/// </summary>
	readonly List<T> _items = new List<T>();

	/// <summary>
	///     Instantiate an empty collection.
	/// </summary>
	public MetalsharpFileCollection() { }

	/// <summary>
	///     Instantiate a collection with an existing one.
	/// </summary>
	/// 
	/// <param name="files">
	///     The list of files to add to the collection.
	/// </param>
	public MetalsharpFileCollection(IEnumerable<T> files) =>
		_items = files.ToList();

	/// <summary>
	///     Gets the files in the collection which descend from the given virtual directory.
	/// </summary>
	/// 
	/// <param name="directory">
	///     The ancestor directory.
	/// </param>
	/// 
	/// <returns>
	///     All of the files which descend from the given directory.
	/// </returns>
	public IMetalsharpFileCollection<T> DescendantsOf(string directory) =>
		_items.Where(i => i.IsDescendantOf(directory)).ToMetalsharpFileCollection();

	/// <summary>
	///     Gets the files in the collection which are children to the given virtual directory.
	/// </summary>
	/// 
	/// <param name="directory">
	///     The parent directory.
	/// </param>
	/// 
	/// <returns>
	///     All of the files which are children of the given directory.
	/// </returns>
	public IMetalsharpFileCollection<T> ChildrenOf(string directory) =>
		_items.Where(i => i.IsChildOf(directory)).ToMetalsharpFileCollection();

	#region Interface Implementation
#pragma warning disable CS1591 // No XML comments on members

	public bool ContainsDirectory(string directory) =>
		_items.Exists(i => i.IsDescendantOf(directory));

	public T this[int index]
	{
		get => _items[index];
		set => _items[index] = value;
	}

	public int Count =>
		_items.Count();

	public bool IsReadOnly =>
		false;

	public void Add(T item) =>
		_items.Add(item);

	public void Clear() =>
		_items.Clear();

	public bool Contains(T item) =>
		_items.Contains(item);

	public void CopyTo(T[] array, int arrayIndex) =>
		_items.CopyTo(array, arrayIndex);

	public IEnumerator<T> GetEnumerator() =>
		_items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

	public int IndexOf(T item) =>
		_items.IndexOf(item);

	public void Insert(int index, T item) =>
		_items.Insert(index, item);

	public bool Remove(T item) =>
		_items.Remove(item);

	public void RemoveAt(int index) =>
		_items.RemoveAt(index);

	public int RemoveAll(Predicate<T> match) =>
		_items.RemoveAll(match);

#pragma warning restore CS1591
	#endregion
}

/// <summary>
///     `MetalsharpFileCollection` extensions for `IEnumerable`.
/// </summary>
public static class IEnumerableExtensions
{
	/// <summary>
	///     Mimic `IEnumerable.ToList`, allowing the easy conversion of an enumerable of files to an `IMetalsharpFileCollection`
	/// </summary>
	/// 
	/// <typeparam name="T">
	///     The type of the collection. Must derive from `IMetalsharpFile`.
	/// </typeparam>
	/// <param name="list">
	///     The `IEnumerable` of `IMetalsharpFile`s to convert to an IMetalsharpFileCollection.
	/// </param>
	/// 
	/// <returns>
	///     An `IMetalsharpFileCollection` containing the files in the given list.
	/// </returns>
	public static IMetalsharpFileCollection<T> ToMetalsharpFileCollection<T>(this IEnumerable<T> list) where T : IMetalsharpFile =>
		new MetalsharpFileCollection<T>(list);
}
