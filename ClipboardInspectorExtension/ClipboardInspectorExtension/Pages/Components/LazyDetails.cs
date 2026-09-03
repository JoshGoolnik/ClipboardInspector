using ClipboardInspector.Core;
using ClipboardInspector.Core.Entities;
using ClipboardInspector.Core.Utilities;
using Microsoft.CommandPalette.Extensions.Toolkit;

internal sealed partial class LazyDetails : Details
{
    private readonly ClipboardFormat _format;
    private string? _body;

    public LazyDetails(ClipboardFormat format)
    {
        _format = format;
        Title = format.Name;
    }

    public override string Body => _body ??= BuildBody(_format);

    private static string BuildBody(ClipboardFormat format)
    {
        if (!ClipboardFormat.IsReadable(format.Id))
        {
            return $"Not readable — backed by {format.Backing}.";
        }

        var data = ClipboardEnumeration.GetData(format.Id);

        if (data is null)
        {
            return "No data — the owning application did not render this format.";
        }

        return $"{data.Length:N0} bytes\n\n```\n{HexDump.ToHexDump(data)}```";
    }
}