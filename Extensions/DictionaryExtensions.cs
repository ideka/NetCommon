using System.Collections.Generic;

namespace Ideka.NetCommon;

public static class DictionaryExtensions
{
    public static IDictionary<TKey, TValue> MergeOverwrite<TKey, TValue>(this IDictionary<TKey, TValue> source,
        IDictionary<TKey, TValue> other)
    {
        foreach (var (key, value) in other)
            source[key] = value;
        return source;
    }

    public static IDictionary<TKey, TValue> MergeKeep<TKey, TValue>(this IDictionary<TKey, TValue> source,
        IDictionary<TKey, TValue> other)
    {
        foreach (var (key, value) in other)
            if (!source.ContainsKey(key))
                source[key] = value;
        return source;
    }

    public static void Deconstruct<TK, TV>(this KeyValuePair<TK, TV> pair, out TK key, out TV value)
    {
        key = pair.Key;
        value = pair.Value;
    }
}
