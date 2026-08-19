using System;
using System.Collections.Generic;
using System.Text;

namespace ClipboardInspector.Core
{
    internal class FormatHelper
    {
        public static readonly Dictionary<uint, string> PredefinedFormats = new()
        {
            [1] = "CF_TEXT",
            [2] = "CF_BITMAP",
            [3] = "CF_METAFILEPICT",
            [4] = "CF_SYLK",
            [5] = "CF_DIF",
            [6] = "CF_TIFF",
            [7] = "CF_OEMTEXT",
            [8] = "CF_DIB",
            [9] = "CF_PALETTE",
            [10] = "CF_PENDATA",
            [11] = "CF_RIFF",
            [12] = "CF_WAVE",
            [13] = "CF_UNICODETEXT",
            [14] = "CF_ENHMETAFILE",
            [15] = "CF_HDROP",
            [16] = "CF_LOCALE",
            [17] = "CF_DIBV5",
        };
    }
}
