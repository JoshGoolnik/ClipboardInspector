using ClipboardInspector.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClipboardInspector.Core
{
    internal class FormatHelper
    {
        public sealed record FormatInfo(string Name, ClipboardBacking Backing);
        public static readonly Dictionary<uint, FormatInfo> PredefinedFormats = new()
        {
            [0x0001] = new FormatInfo("CF_TEXT", ClipboardBacking.GlobalMemory),
            [0x0002] = new FormatInfo("CF_BITMAP", ClipboardBacking.GdiHandle),
            [0x0003] = new FormatInfo("CF_METAFILEPICT", ClipboardBacking.Structure),
            [0x0004] = new FormatInfo("CF_SYLK", ClipboardBacking.GlobalMemory),
            [0x0005] = new FormatInfo("CF_DIF", ClipboardBacking.GlobalMemory),
            [0x0006] = new FormatInfo("CF_TIFF", ClipboardBacking.GlobalMemory),
            [0x0007] = new FormatInfo("CF_OEMTEXT", ClipboardBacking.GlobalMemory),
            [0x0008] = new FormatInfo("CF_DIB", ClipboardBacking.GlobalMemory),
            [0x0009] = new FormatInfo("CF_PALETTE", ClipboardBacking.GdiHandle),
            [0x000A] = new FormatInfo("CF_PENDATA", ClipboardBacking.GlobalMemory),
            [0x000B] = new FormatInfo("CF_RIFF", ClipboardBacking.GlobalMemory),
            [0x000C] = new FormatInfo("CF_WAVE", ClipboardBacking.GlobalMemory),
            [0x000D] = new FormatInfo("CF_UNICODETEXT", ClipboardBacking.GlobalMemory),
            [0x000E] = new FormatInfo("CF_ENHMETAFILE", ClipboardBacking.GdiHandle),
            [0x000F] = new FormatInfo("CF_HDROP", ClipboardBacking.Structure),
            [0x0010] = new FormatInfo("CF_LOCALE", ClipboardBacking.GlobalMemory),
            [0x0011] = new FormatInfo("CF_DIBV5", ClipboardBacking.GlobalMemory),
        };
    }
}
