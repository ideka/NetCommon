using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ideka.NetCommon;

public class FixedSizedQueue<T>(int size = 100) : IEnumerable<T>
{
    public int Size { get; set; } = size;

    private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
    private readonly object _lock = new object();

    public void Enqueue(T obj)
    {
        lock (_lock)
        {
            _queue.Enqueue(obj);
            while (_queue.Count > Size && _queue.TryDequeue(out var _)) ;
        }
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_queue).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_queue).GetEnumerator();
}
