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
            [1] = new FormatInfo("CF_TEXT", ClipboardBacking.GlobalMemory),
            [2] = new FormatInfo("CF_BITMAP", ClipboardBacking.GdiHandle),
            [3] = new FormatInfo("CF_METAFILEPICT", ClipboardBacking.Structure),
            [4] = new FormatInfo("CF_SYLK", ClipboardBacking.GlobalMemory),
            [5] = new FormatInfo("CF_DIF", ClipboardBacking.GlobalMemory),
            [6] = new FormatInfo("CF_TIFF", ClipboardBacking.GlobalMemory),
            [7] = new FormatInfo("CF_OEMTEXT", ClipboardBacking.GlobalMemory),
            [8] = new FormatInfo("CF_DIB", ClipboardBacking.GlobalMemory),
            [9] = new FormatInfo("CF_PALETTE", ClipboardBacking.GdiHandle),
            [10] = new FormatInfo("CF_PENDATA", ClipboardBacking.GlobalMemory),
            [11] = new FormatInfo("CF_RIFF", ClipboardBacking.GlobalMemory),
            [12] = new FormatInfo("CF_WAVE", ClipboardBacking.GlobalMemory),
            [13] = new FormatInfo("CF_UNICODETEXT", ClipboardBacking.GlobalMemory),
            [14] = new FormatInfo("CF_ENHMETAFILE", ClipboardBacking.GdiHandle),
            [15] = new FormatInfo("CF_HDROP", ClipboardBacking.Structure),
            [16] = new FormatInfo("CF_LOCALE", ClipboardBacking.GlobalMemory),
            [17] = new FormatInfo("CF_DIBV5", ClipboardBacking.GlobalMemory),
        };
    }
}
