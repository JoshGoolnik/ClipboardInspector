using ClipboardInspector.Core;
using System.Runtime.CompilerServices;
namespace ClipboardInspector.Cli;

internal class Program
{
    private static void Main()
    {
        var formats = ClipboardFormats.GetClipboardFormatsList();
        foreach (var format in formats)
        {
            Console.WriteLine($"ID: {format.Id}, Name: {format.Name}");
        }
    }
}

