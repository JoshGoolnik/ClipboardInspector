using ClipboardInspector.Core;
using ClipboardInspector.Core.Enums;
using ClipboardInspector.Core.Utilities;
namespace ClipboardInspector.Cli;

internal class Program
{
    private static void Main()
    {
        var formats = ClipboardEnumeration.GetClipboardFormatsList();
        foreach (var format in formats)
        {
            Console.WriteLine($"ID: {format.Id}, Name: {format.Name}, Category: {format.Category}, Backing: {format.Backing}");
            var data = ClipboardEnumeration.GetData(format.Id);

            if (data is null)
            {
                Console.WriteLine(format.Backing == ClipboardBacking.GlobalMemory
                    ? "  <no data — not rendered>"
                    : $"  <not readable — {format.Backing}>");
                continue;
            }
            Console.WriteLine($"  {data.Length} bytes");
            Console.WriteLine(HexDump.ToHexDump(data));
        }
    }
}

