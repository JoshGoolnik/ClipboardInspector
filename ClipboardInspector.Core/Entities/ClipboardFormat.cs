
using ClipboardInspector.Core.Enums;
namespace ClipboardInspector.Core.Entities;

public sealed record ClipboardFormat(uint Id, string Name, FormatCategory Category, ClipboardBacking Backing)
{
    // Determines if the clipboard format is readable based on its backing type
    public static bool IsReadable(uint id) =>
    GetBacking(id, GetFormatCategory(id)) == ClipboardBacking.GlobalMemory;

    // Factory method to create a ClipboardFormat instance from an ID and name
    public static ClipboardFormat FromId(uint id, string name)
    {
        var category = GetFormatCategory(id);
        var backing = GetBacking(id, category);
        return new ClipboardFormat(id, name, category, backing);
    }

    private static ClipboardBacking GetBacking(uint format, FormatCategory category) => PredefinedFormats.All.TryGetValue(format, out var info)
        ? info.Backing
        : GetBackingFromCategory(category);

    private static ClipboardBacking GetBackingFromCategory(FormatCategory category) => category switch
    {
        FormatCategory.Registered or FormatCategory.PrivateApplication => ClipboardBacking.GlobalMemory,
        FormatCategory.GdiObject => ClipboardBacking.GdiHandle,
        _ => ClipboardBacking.Unknown,
    };

    private static FormatCategory GetFormatCategory(uint format)
    {
        return PredefinedFormats.All.ContainsKey(format)
            ? FormatCategory.Predefined
            : format switch
            {
                >= 0x0001 and <= 0x0011 => FormatCategory.Predefined,
                >= 0x0200 and <= 0x02FF => FormatCategory.PrivateApplication,
                >= 0x0300 and <= 0x03FF => FormatCategory.GdiObject,
                >= 0xC000 and <= 0xFFFF => FormatCategory.Registered,
                _ => FormatCategory.Unknown,
            };
    }
}
