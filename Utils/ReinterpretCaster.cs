using System.Runtime.InteropServices;

namespace Ideka.NetCommon;

[StructLayout(LayoutKind.Explicit)]
public struct ReinterpretCaster<TFrom, TTo>(TFrom from)
{
    [FieldOffset(0)]
    public TFrom From = from;
    [FieldOffset(0)]
    public TTo To;

    public static TTo Cast(TFrom from)
        => new ReinterpretCaster<TFrom, TTo>(from).To;
}
