using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
[assembly: DisableRuntimeMarshalling]
namespace ClipboardInspector.Core
{
    public static partial class InspectionService
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
        
        public static string GetClipboardFormats()
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                throw new InvalidOperationException("Failed to open clipboard.");
            }
            try
            {
                var formats = new List<string>();
                uint format = 0;
                while ((format = EnumClipboardFormats(format)) != 0)
                {
                    formats.Add(GetFormatName(format));
                }
                return string.Join(Environment.NewLine, formats);
            }
            finally
            {
                CloseClipboard();
            }
        }
        private static string GetFormatName(uint format)
        {
            if (FormatHelper.PredefinedFormats.TryGetValue(format, out var known))
            {
                return known;
            }

            var buffer = new char[256];
            var length = GetClipboardFormatName(format, buffer, buffer.Length);

            return length > 0
                ? new string(buffer, 0, length)
                : $"Unknown (0x{format:X4})";
        }
    }
}
