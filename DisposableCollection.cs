using System;
using System.Collections.Generic;

namespace Ideka.NetCommon;

public class DisposableCollection : IDisposable
{
    private readonly HashSet<IDisposable> _disposables = [];

    public T Add<T>(T disposable) where T : notnull, IDisposable
    {
        _disposables.Add(disposable);
        return disposable;
    }

    public void Dispose()
    {
        foreach (var disposable in _disposables)
            disposable.Dispose();

        _disposables.Clear();
    }
}
