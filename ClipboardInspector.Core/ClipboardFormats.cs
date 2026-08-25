using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ClipboardInspector.Core.Enums;
using ClipboardInspector.Core.Entities;
[assembly: DisableRuntimeMarshalling]
namespace ClipboardInspector.Core
{
    public static partial class ClipboardFormats
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

        [LibraryImport("user32.dll", SetLastError = true)]
        private static partial IntPtr GetClipboardData(uint uFormat);

        [LibraryImport("kernel32.dll", SetLastError = true)]
        private static partial IntPtr GlobalLock(IntPtr hMem);

        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool GlobalUnlock(IntPtr hMem);

        [LibraryImport("kernel32.dll", SetLastError = true)]
        private static partial nuint GlobalSize(IntPtr hMem);

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
                    var formatCategory = GetFormatCategory(format);
                    var backing = GetBacking(format, formatCategory);
                    formats.Add(new ClipboardFormat(format, formatName, formatCategory, backing));
                }
                return formats;
            }
            finally
            {
                CloseClipboard();
            }
        }

        public static byte[]? GetData(uint format)
        {
            if (GetBacking(format, GetFormatCategory(format)) != ClipboardBacking.GlobalMemory)
            {
                return null;
            }

            if (!OpenClipboard(IntPtr.Zero))
            {
                throw new InvalidOperationException("Failed to open clipboard.");
            }

            try
            {
                return GetClipboardDataBytes(format);
            }
            finally
            {
                CloseClipboard();
            }
        }

        private static byte[]? GetClipboardDataBytes(uint format)
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


        private static string GetFormatName(uint format)
        {
            if (FormatHelper.PredefinedFormats.TryGetValue(format, out var knownFormat))
            {
                return knownFormat.Name;
            }

            var buffer = new char[256];
            var length = GetClipboardFormatName(format, buffer, buffer.Length);

            return length > 0
                ? new string(buffer, 0, length)
                : $"Unknown (0x{format:X4})";
        }

        private static FormatCategory GetFormatCategory(uint format) => format switch
        {
            >= 0x0001 and <= 0x0011 => FormatCategory.Predefined,
            >= 0x0200 and <= 0x02FF => FormatCategory.PrivateApplication,
            >= 0x0300 and <= 0x03FF => FormatCategory.GdiObject,
            >= 0xC000 and <= 0xFFFF => FormatCategory.Registered,
            _ => FormatCategory.Unknown,
        };

        private static ClipboardBacking GetBacking(uint format, FormatCategory category) => FormatHelper.PredefinedFormats.TryGetValue(format, out var info)
                ? info.Backing
                : GetBackingFromCategory(category);

        private static ClipboardBacking GetBackingFromCategory(FormatCategory category) => category switch
        {
            FormatCategory.Registered or FormatCategory.PrivateApplication => ClipboardBacking.GlobalMemory,
            FormatCategory.GdiObject => ClipboardBacking.GdiHandle,
            _ => ClipboardBacking.Unknown,
        };
    }
}
