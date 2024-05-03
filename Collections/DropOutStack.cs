using System.Collections.Generic;
using System.Linq;

namespace Ideka.NetCommon;

public class DropOutStack<T>(int capacity) : LinkedList<T>
{
    public int Capacity { get; } = capacity;

    private readonly object _lock = new();

    public void Push(T item)
    {
        lock (_lock)
        {
            while (Count >= Capacity)
                RemoveFirst();

            AddLast(item);
        }
    }

    public bool TryGetLast(out T? last)
    {
        lock (_lock)
        {
            var any = this.Any();
            last = any ? Last.Value : default;
            return any;
        }
    }
}
