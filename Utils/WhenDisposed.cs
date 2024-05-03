using System;

namespace Ideka.NetCommon;

public readonly struct WhenDisposed(Action action) : IDisposable
{
    public void Dispose() => action();
}
