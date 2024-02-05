using System;

namespace Ideka.NetCommon;

public readonly struct WhenDisposed(Action act) : IDisposable
{
    private readonly Action _act = act;

    public void Dispose()
    {
        _act();
    }
}
