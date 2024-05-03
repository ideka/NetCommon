using System.Collections.Generic;
using System.Diagnostics;

namespace Ideka.NetCommon;

public class DoubleSortedSet<T>(IComparer<T> comparerA, IComparer<T> comparerB)
{
    private readonly SortedSet<T> _setA = new SortedSet<T>(comparerA);
    private readonly SortedSet<T> _setB = new SortedSet<T>(comparerB);

    public IEnumerable<T> SortedA() => _setA;
    public IEnumerable<T> SortedB() => _setB;
    public IEnumerable<T> ReverseA() => _setA.Reverse();
    public IEnumerable<T> ReverseB() => _setB.Reverse();

    private readonly object _lock = new object();

    public void Add(T item)
    {
        lock (_lock)
        {
            Debug.Assert(_setA.Count == _setB.Count);
            _setA.Add(item);
            _setB.Add(item);
            Debug.Assert(_setA.Count == _setB.Count);
        }
    }

    public void Remove(T item)
    {
        lock (_lock)
        {
            Debug.Assert(_setA.Count == _setB.Count);
            _setA.Remove(item);
            _setB.Remove(item);
            Debug.Assert(_setA.Count == _setB.Count);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            Debug.Assert(_setA.Count == _setB.Count);
            _setA.Clear();
            _setB.Clear();
            Debug.Assert(_setA.Count == _setB.Count);
        }
    }
}
