using ClipboardInspector.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClipboardInspector.Core
{
    internal static class PredefinedFormats
    {
        public sealed record FormatInfo(string Name, ClipboardBacking Backing);
        public static readonly Dictionary<uint, FormatInfo> AllPredefinedFormats = new()
        {
            [0x0001u] = new FormatInfo("CF_TEXT", ClipboardBacking.GlobalMemory),
            [0x0002u] = new FormatInfo("CF_BITMAP", ClipboardBacking.GdiHandle),
            [0x0003u] = new FormatInfo("CF_METAFILEPICT", ClipboardBacking.Structure),
            [0x0004u] = new FormatInfo("CF_SYLK", ClipboardBacking.GlobalMemory),
            [0x0005u] = new FormatInfo("CF_DIF", ClipboardBacking.GlobalMemory),
            [0x0006u] = new FormatInfo("CF_TIFF", ClipboardBacking.GlobalMemory),
            [0x0007u] = new FormatInfo("CF_OEMTEXT", ClipboardBacking.GlobalMemory),
            [0x0008u] = new FormatInfo("CF_DIB", ClipboardBacking.GlobalMemory),
            [0x0009u] = new FormatInfo("CF_PALETTE", ClipboardBacking.GdiHandle),
            [0x000Au] = new FormatInfo("CF_PENDATA", ClipboardBacking.GlobalMemory),
            [0x000Bu] = new FormatInfo("CF_RIFF", ClipboardBacking.GlobalMemory),
            [0x000Cu] = new FormatInfo("CF_WAVE", ClipboardBacking.GlobalMemory),
            [0x000Du] = new FormatInfo("CF_UNICODETEXT", ClipboardBacking.GlobalMemory),
            [0x000Eu] = new FormatInfo("CF_ENHMETAFILE", ClipboardBacking.GdiHandle),
            [0x000Fu] = new FormatInfo("CF_HDROP", ClipboardBacking.Structure),
            [0x0010u] = new FormatInfo("CF_LOCALE", ClipboardBacking.GlobalMemory),
            [0x0011u] = new FormatInfo("CF_DIBV5", ClipboardBacking.GlobalMemory),
            [0x0080u] = new FormatInfo("CF_OWNERDISPLAY", ClipboardBacking.GdiHandle),
            [0x0081u] = new FormatInfo("CF_DSPTEXT", ClipboardBacking.GlobalMemory),
            [0x0082u] = new FormatInfo("CF_DSPBITMAP", ClipboardBacking.GdiHandle),
            [0x0083u] = new FormatInfo("CF_DSPMETAFILEPICT", ClipboardBacking.Structure),
            [0x008Eu] = new FormatInfo("CF_DSPENHMETAFILE", ClipboardBacking.GdiHandle)
        };
    }
}
