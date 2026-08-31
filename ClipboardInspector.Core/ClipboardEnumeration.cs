using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ClipboardInspector.Core.Enums;
using ClipboardInspector.Core.Entities;
[assembly: DisableRuntimeMarshalling]
namespace ClipboardInspector.Core;
public static partial class ClipboardEnumeration
{

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool OpenClipboard(IntPtr hWndNewOwner);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CloseClipboard();

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial uint EnumClipboardFormats(uint format);

    [LibraryImport("user32.dll", EntryPoint = "GetClipboardFormatNameW")]
    private static partial int GetClipboardFormatName(
    uint format, [Out] char[] lpszFormatName, int cchMaxCount);


    public static IReadOnlyList<ClipboardFormat> GetClipboardFormatsList()
    {
        if (!OpenClipboard(IntPtr.Zero))
        {
            throw new InvalidOperationException("Failed to open clipboard.");
        }
        try
        {
            var formats = new List<ClipboardFormat>();
            uint format = 0;
            while ((format = EnumClipboardFormats(format)) != 0)
            {
                var formatName = GetFormatName(format);
                formats.Add(ClipboardFormat.FromId(format, GetFormatName(format)));
            }
            return formats;
        }
        finally
        {
            CloseClipboard();
        }
    }

    private static string GetFormatName(uint format)
    {
        if (PredefinedFormats.PredefinedFormats.TryGetValue(format, out var knownFormat))
        {
            return knownFormat.Name;
        }

        var buffer = new char[256];
        var length = GetClipboardFormatName(format, buffer, buffer.Length);

        return length > 0
            ? new string(buffer, 0, length)
            : $"Unknown (0x{format:X4})";
    }

    public static byte[]? GetData(uint format)
    {
        if (!ClipboardFormat.IsReadable(format))
        {
            return null;
        }

        if (!OpenClipboard(IntPtr.Zero))
        {
            throw new InvalidOperationException("Failed to open clipboard.");
        }

        try
        {
            return ClipboardData.GetClipboardDataBytes(format);
        }
        finally
        {
            CloseClipboard();
        }
    }

}
