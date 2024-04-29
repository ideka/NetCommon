using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ideka.NetCommon;

public static class EnumerableExtensions
{
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        => source.ToDictionary(x => x.Key, x => x.Value);

    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        => source.GroupBy(selector).Select(x => x.First());

    public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source)
        => source.Select((t, i) => (i, t));

    public static IEnumerable<(T, T)> By2<T>(this IEnumerable<T> source)
        => source.Zip(source.Skip(1));

    public static IEnumerable<(TA, TB)> Zip<TA, TB>(this IEnumerable<TA> source, IEnumerable<TB> other)
        => source.Zip(other, (a, b) => (a, b));

    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        => source.MaxBy(selector, null);

    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector,
        IComparer<TKey>? comparer)
        => source.MostBy(selector, comparer, true);

    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        => source.MinBy(selector, null);

    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector,
        IComparer<TKey>? comparer)
        => source.MostBy(selector, comparer, false);

    public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> source) where TSource : struct
    {
        foreach (var item in source)
            if (item is TSource notNull)
                yield return notNull;
    }

    public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> source) where TSource : class
    {
        foreach (var item in source)
            if (item is TSource notNull)
                yield return notNull;
    }

    private static TSource MostBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey>? comparer, bool max)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));
        comparer ??= Comparer<TKey>.Default;
        int factor = max ? -1 : 1;

        using var sourceIterator = source.GetEnumerator();
        if (!sourceIterator.MoveNext())
            throw new InvalidOperationException("Sequence contains no elements");

        var most = sourceIterator.Current;
        var mostKey = selector(most);
        while (sourceIterator.MoveNext())
        {
            var candidate = sourceIterator.Current;
            var candidateProjected = selector(candidate);
            if (comparer.Compare(candidateProjected, mostKey) * factor < 0)
            {
                most = candidate;
                mostKey = candidateProjected;
            }
        }

        return most;
    }

    public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> subjects, Func<T, IEnumerable<T>> selector)
    {
        if (subjects == null)
            yield break;

        var stillToProcess = new Queue<T>(subjects);

        while (stillToProcess.Count > 0)
        {
            T item = stillToProcess.Dequeue();
            yield return item;

            foreach (T child in selector(item))
                stillToProcess.Enqueue(child);
        }
    }
}
