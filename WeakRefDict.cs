using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Ideka.NetCommon;

public abstract class WeakRefDict<TKey, TValue>
    where TKey : struct
    where TValue : class
{
    protected virtual int GCFrequency => 10;

    private readonly ConcurrentDictionary<TKey, WeakReference> _cache = [];
    private int _gcCycle = 0;

    public TValue Get(TKey key)
    {
        if (_gcCycle++ > GCFrequency)
        {
            _gcCycle = 0;
            foreach (var deadKey in _cache.Where(x => !x.Value.IsAlive).Select(x => x.Key).ToList())
                _cache.TryRemove(deadKey, out _);
        }

        if (!_cache.TryGetValue(key, out var output) || !output.IsAlive)
            _cache[key] = output = new WeakReference(Load(key));

        return (TValue)output.Target;
    }

    public abstract TValue Load(TKey key);
}
