using System.Formats.Asn1;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ClipboardInspector.Core.Enums;
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

        [LibraryImport("user32.dll")]
        private static partial uint EnumClipboardFormats(uint format);

        [LibraryImport("user32.dll", EntryPoint = "GetClipboardFormatNameW")]
        private static partial int GetClipboardFormatName(
        uint format, [Out] char[] lpszFormatName, int cchMaxCount);

        public sealed record ClipboardFormat(uint Id, string Name, FormatCategory Category, ClipboardBacking Backing);

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
                    formats.Add(new ClipboardFormat(format, GetFormatName(format), GetFormatCategory(format), GetBacking(format, GetFormatCategory(format))));
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
