using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class V1<T> : IList<T>
{
  protected ConcurrentQueue<T> _items = new();
  protected ConcurrentDictionary<int, T> _indices = new();

  /// <summary>
  /// Index
  /// </summary>
  /// <param name="index"></param>
  /// <returns></returns>
  public T this[int index]
  {
    get => _indices[index];
    set => UpdateInRange(index, 0, _items.Count, () => _indices[index] = value);
  }

  /// <summary>
  /// Count
  /// </summary>
  public int Count => _items.Count;

  /// <summary>
  /// Mutable
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Add
  /// </summary>
  /// <param name="input"></param>
  public void Add(T input) => _items.Enqueue(input);

  /// <summary>
  /// Contains
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public bool Contains(T input) => _items.Contains(input);

  /// <summary>
  /// Copy
  /// </summary>
  /// <param name="items"></param>
  /// <param name="index"></param>
  public void CopyTo(T[] items, int index) => _items.CopyTo(items, index);

  /// <summary>
  /// Enumerator
  /// </summary>
  /// <returns></returns>
  public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

  /// <summary>
  /// Search
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public int IndexOf(T input) => _items.ToList().IndexOf(input);

  /// <summary>
  /// Clear
  /// </summary>
  public void Clear()
  {
    _items.Clear();
    _indices.Clear();
  }

  /// <summary>
  /// Insert
  /// </summary>
  /// <param name="index"></param>
  /// <param name="input"></param>
  public void Insert(int index, T input)
  {
    if (index == _items.Count)
    {
      _items.Enqueue(input);
      _indices[_items.Count - 1] = input;

      return;
    }

    UpdateInRange(index, 0, _items.Count, () =>
    {
      _indices.Clear();

      var i = 0;
      var items = new ConcurrentQueue<T>();

      while (_items.TryDequeue(out T item))
      {
        if (Equals(i, index))
        {
          items.Enqueue(input);
          _indices[items.Count - 1] = input;
        }

        items.Enqueue(item);
        _indices[items.Count - 1] = item;

        i++;
      }

      _items = items;
    });
  }

  /// <summary>
  /// Remove
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public bool Remove(T input)
  {
    _indices.Clear();

    var response = false;
    var items = new ConcurrentQueue<T>();

    while (_items.TryDequeue(out T item))
    {
      if (Equals(input, item) is false)
      {
        response = true;
        items.Enqueue(item);
        _indices[items.Count - 1] = item;
      }
    }

    _items = items;

    return response;
  }

  /// <summary>
  /// Remove by index
  /// </summary>
  /// <param name="index"></param>
  public void RemoveAt(int index)
  {
    UpdateInRange(index, 0, _items.Count, () =>
    {
      _indices.Clear();

      var i = 0;
      var items = new ConcurrentQueue<T>();

      while (_items.TryDequeue(out T item))
      {
        if (Equals(i, index) is false)
        {
          items.Enqueue(item);
          _indices[items.Count - 1] = item;
        }

        i++;
      }

      _items = items;
    });
  }

  /// <summary>
  /// Enumerate
  /// </summary>
  /// <returns></returns>
  protected IEnumerator<T> Enumerate()
  {
    var i = 0;
    var items = new ConcurrentQueue<T>();

    foreach (var item in _items)
    {
      var o = _indices.TryGetValue(i++, out T value) ? value : item;

      items.Enqueue(o);

      yield return o;
    }

    _items = items;
  }

  /// <summary>
  /// Update with index check
  /// </summary>
  /// <param name="index"></param>
  /// <param name="min"></param>
  /// <param name="max"></param>
  /// <param name="action"></param>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  protected void UpdateInRange(int index, int min, int max, Action action)
  {
    if (index < min || index >= max)
    {
      throw new ArgumentOutOfRangeException("Incorrect index");
    }

    action();
  }

  /// <summary>
  /// Enumerate
  /// </summary>
  /// <returns></returns>
  IEnumerator<T> IEnumerable<T>.GetEnumerator() => Enumerate();

  /// <summary>
  /// Enumerate
  /// </summary>
  /// <returns></returns>
  IEnumerator IEnumerable.GetEnumerator() => Enumerate();
}
