using ClipboardInspector.Core;
using System.Runtime.CompilerServices;
namespace ClipboardInspector.Cli;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine(InspectionService.GetClipboardFormats());
    }
}

