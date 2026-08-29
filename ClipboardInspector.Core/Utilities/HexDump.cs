using System.Text;

namespace ClipboardInspector.Core.Utilities;

public class HexDump
{

    // Method to convert the data into a traditional hex dump format
    public static string ToHexDump(byte[] data, int bytesPerLine = 16)
    {
        var sb = new StringBuilder();
        // Outer Loop, moves in chunks of 16 bytes
        for (int i = 0; i < data.Length; i += 16)
        {
            // Append the offset in hexadecimal format
            sb.Append($"{i:X8}  ");

            // Inner Loop, this is for the hexadecimal representation of the bytes
            for (int j = 0; j < 16; j++)
            {
                // Check if the current byte index is within the bounds of the data array
                if (i + j < data.Length)
                {
                    // If it is, append the byte in hexadecimal format
                    sb.Append($"{data[i + j]:X2} ");
                }
                else
                {
                    // If it isn't, we add spaces, so everything stays aligned in the output
                    sb.Append("   ");
                }
            }
            // End of the hex part, we add a separator and then the ASCII representation
            sb.Append(" |");

            // Inner Loop again, this time for the ASCII representation
            for (int j = 0; j < 16; j++)
            {
                // Again, we check if the current byte index is within the bounds of the data array
                if (i + j < data.Length)
                {
                    // If it is, we want to check if it's ASCII and then append it if it is (or a full stop if it isn't)
                    byte b = data[i + j];
                    sb.Append(b >= 0x20 && b <= 0x7E ? (char)b : '.');
                }
            }
            // End of the ASCII part, we add a closing separator and a new line
            sb.AppendLine("|");
        }
        return sb.ToString();
    }
}
