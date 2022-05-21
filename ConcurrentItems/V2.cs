using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

public class V2<T> : IList<T>
{
  protected ConcurrentDictionary<int, T> _items = new();

  /// <summary>
  /// Index
  /// </summary>
  /// <param name="index"></param>
  /// <returns></returns>
  public T this[int index]
  {
    get => _items[index];
    set => UpdateInRange(index, 0, _items.Count, () => _items[index] = value);
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
  public void Add(T input) => _items[_items.Count] = input;

  /// <summary>
  /// Contains
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public bool Contains(T input) => _items.Values.Contains(input);

  /// <summary>
  /// Copy
  /// </summary>
  /// <param name="items"></param>
  /// <param name="index"></param>
  public void CopyTo(T[] items, int index) => _items.Values.CopyTo(items, index);

  /// <summary>
  /// Enumerator
  /// </summary>
  /// <returns></returns>
  public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();

  /// <summary>
  /// Search
  /// </summary>
  /// <param name="input"></param>
  /// <returns></returns>
  public int IndexOf(T input) => _items.First(o => Equals(o, input)).Key;

  /// <summary>
  /// Clear
  /// </summary>
  public void Clear() => _items.Clear();

  /// <summary>
  /// Insert
  /// </summary>
  /// <param name="index"></param>
  /// <param name="input"></param>
  public void Insert(int index, T input)
  {
    if (Equals(index, _items.Count))
    {
      Add(input);
      return;
    }

    UpdateInRange(index, 0, _items.Count, () =>
    {
      var count = _items.Count;
      var items = new ConcurrentDictionary<int, T>();

      for (int i = 0, ii = 0; i < count; i++, ii++)
      {
        if (Equals(i, index))
        {
          items[i] = input;
          count++;
          i++;
        }

        items[i] = _items[ii];
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
    var response = false;
    var count = _items.Count;
    var items = new ConcurrentDictionary<int, T>();

    for (int i = 0, ii = 0; ii < count; ii++)
    {
      if (Equals(input, _items[ii]) is false)
      {
        items[i] = _items[ii];
        i++;
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
      var count = _items.Count;
      var items = new ConcurrentDictionary<int, T>();

      for (int i = 0, ii = 0; ii < count; ii++)
      {
        if (Equals(ii, index) is false)
        {
          items[i] = _items[ii];
          i++;
        }
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
    for (var i = 0; i < _items.Count; i++)
    {
      yield return _items[i];
    }
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
