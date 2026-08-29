using ClipboardInspector.Core.Utilities;

namespace ClipboardInspector.Tests;

public class HexDumpTests
{
    [Fact]
    public void ToHexDump_EmptyData_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, HexDump.ToHexDump([]));
    }

    [Fact]
    public void ToHexDump_FormatsBytesAndPrintableCharacters()
    {
        var result = HexDump.ToHexDump([0x00, 0x20, 0x41, 0x7E, 0x7F]);

        Assert.StartsWith("00000000  00 20 41 7E 7F ", result);
        Assert.Contains("|. A~.|", result);
        Assert.EndsWith(Environment.NewLine, result);
    }

    [Fact]
    public void ToHexDump_StartsSecondLineAtSixteenByteOffset()
    {
        var result = HexDump.ToHexDump(Enumerable.Range(0, 17).Select(i => (byte)i).ToArray());

        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        Assert.Equal(2, lines.Length);
        Assert.StartsWith("00000000  ", lines[0]);
        Assert.StartsWith("00000010  10", lines[1]);
    }
}