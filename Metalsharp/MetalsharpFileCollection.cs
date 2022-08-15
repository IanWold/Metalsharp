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
public class MetalsharpFileCollection : IList<MetalsharpFile>
{
	/// <summary>
	///     The Metalsharp files in the collection.
	/// </summary>
	readonly List<MetalsharpFile> _items = new();

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
	public MetalsharpFileCollection(IEnumerable<MetalsharpFile> files) =>
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
	public MetalsharpFileCollection DescendantsOf(string directory) =>
		Items.Where(i => i.IsDescendantOf(directory)).ToMetalsharpFileCollection();

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
	public MetalsharpFileCollection ChildrenOf(string directory) =>
		Items.Where(i => i.IsChildOf(directory)).ToMetalsharpFileCollection();

	#region Interface Implementation
#pragma warning disable CS1591 // No XML comments on members

	public bool ContainsDirectory(string directory) =>
		Items.Exists(i => i.IsDescendantOf(directory));

	public MetalsharpFile this[int index]
	{
		get => Items[index];
		set => Items[index] = value;
	}

	public int Count =>
		Items.Count();

	public bool IsReadOnly =>
		false;

	public List<MetalsharpFile> Items => _items;

	public void Add(MetalsharpFile item) =>
		Items.Add(item);

	public void Clear() =>
		Items.Clear();

	public bool Contains(MetalsharpFile item) =>
		Items.Contains(item);

	public void CopyTo(MetalsharpFile[] array, int arrayIndex) =>
		Items.CopyTo(array, arrayIndex);

	public IEnumerator<MetalsharpFile> GetEnumerator() =>
		Items.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() =>
		GetEnumerator();

	public int IndexOf(MetalsharpFile item) =>
		Items.IndexOf(item);

	public void Insert(int index, MetalsharpFile item) =>
		Items.Insert(index, item);

	public bool Remove(MetalsharpFile item) =>
		Items.Remove(item);

	public void RemoveAt(int index) =>
		Items.RemoveAt(index);

	public int RemoveAll(Predicate<MetalsharpFile> match) =>
		Items.RemoveAll(match);

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
	public static MetalsharpFileCollection ToMetalsharpFileCollection(this IEnumerable<MetalsharpFile> list) =>
		new(list);
}
