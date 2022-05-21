using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

public class V3<T>
{
  private int tailIndex;
  private int headIndex;
  private readonly ConcurrentDictionary<int, T> dictionary;

  public V3()
  {
    tailIndex = -1;
    headIndex = 0;
    dictionary = new ConcurrentDictionary<int, T>();
  }

  public int Count { get { return tailIndex - headIndex + 1; } }

  public bool IsEmpty { get { return Count == 0; } }

  public void Enqueue(T item)
  {
    var enqueuePosition = Interlocked.Increment(ref tailIndex);
    dictionary.AddOrUpdate(enqueuePosition, k => item, (k, v) => item);
  }

  public T Peek(int index)
  {
    T item;

    return dictionary.TryGetValue(index, out item) ?
        item :
        default(T);
  }

  public long TryDequeue(out T item)
  {
    if (headIndex > tailIndex)
    {
      item = default(T);
      return -1;
    }

    var dequeuePosition = Interlocked.Increment(ref headIndex) - 1;

    dictionary.TryRemove(dequeuePosition, out item);

    return dequeuePosition;
  }

  public List<T> GetSnapshot()
  {
    List<T> snapshot = new List<T>();
    long snapshotTail = tailIndex;
    long snapshotHead = headIndex;

    for (long i = snapshotHead; i < snapshotTail; i++)
    {
      T item;

      if (TryDequeue(out item) >= 0)
      {
        snapshot.Add(item);
      }
    }

    return snapshot;
  }
}
