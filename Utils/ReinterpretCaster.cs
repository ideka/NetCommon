using System.Runtime.InteropServices;

namespace Ideka.NetCommon;

[StructLayout(LayoutKind.Explicit)]
public struct ReinterpretCaster<TFrom, TTo>(TFrom from)
{
    [FieldOffset(0)]
    public TFrom From = from;
    [FieldOffset(0)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
    public TTo To;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

    public static TTo Cast(TFrom from)
        => new ReinterpretCaster<TFrom, TTo>(from).To;
}
