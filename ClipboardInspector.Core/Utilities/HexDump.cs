using System.Text;

namespace ClipboardInspector.Core.Utilities;

public class HexDump
{
    public static string ToHexDump(byte[] data, int bytesPerLine = 16)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < data.Length; i += 16)
        {
            sb.Append($"{i:X8}  ");
            for (int j = 0; j < 16; j++)
            {
                if (i + j < data.Length)
                {
                    sb.Append($"{data[i + j]:X2} ");
                }
                else
                {
                    sb.Append("   ");
                }
            }
            sb.Append(" |");
            for (int j = 0; j < 16; j++)
            {
                if (i + j < data.Length)
                {
                    byte b = data[i + j];
                    sb.Append(b >= 0x20 && b <= 0x7E ? (char)b : '.');
                }
            }
            sb.AppendLine("|");
        }
        return sb.ToString();
    }
}
