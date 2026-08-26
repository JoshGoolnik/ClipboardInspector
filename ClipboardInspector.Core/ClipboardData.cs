using System.Runtime.InteropServices;
namespace ClipboardInspector.Core;
public static partial class ClipboardData

{
    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial IntPtr GetClipboardData(uint uFormat);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial IntPtr GlobalLock(IntPtr hMem);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GlobalUnlock(IntPtr hMem);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial nuint GlobalSize(IntPtr hMem);

    internal static byte[]? GetClipboardDataBytes(uint format)
    {

        IntPtr hData = GetClipboardData(format);
        if (hData == IntPtr.Zero)
        {
            return null;
        }
        IntPtr pData = GlobalLock(hData);
        if (pData == IntPtr.Zero)
        {
            throw new InvalidOperationException($"Failed to lock global memory for format {format}.");
        }
        try
        {
            nuint size = GlobalSize(hData);
            byte[] buffer = new byte[size];
            Marshal.Copy(pData, buffer, 0, (int)size);
            return buffer;
        }
        finally
        {
            GlobalUnlock(hData);
        }
    }
}
