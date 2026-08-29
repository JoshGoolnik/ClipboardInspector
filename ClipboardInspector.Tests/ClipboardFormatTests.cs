using ClipboardInspector.Core.Entities;
using ClipboardInspector.Core.Enums;

namespace ClipboardInspector.Tests;

public class ClipboardFormatTests
{
    [Fact]
    public void FromId_UsesPredefinedFormatInformation()
    {
        var result = ClipboardFormat.FromId(0x000D, "ignored name");

        Assert.Equal((uint)0x000D, result.Id);
        Assert.Equal("ignored name", result.Name);
        Assert.Equal(FormatCategory.Predefined, result.Category);
        Assert.Equal(ClipboardBacking.GlobalMemory, result.Backing);
    }

    [Fact]
    public void FromId_ClassifiesPrivateApplicationFormatAsGlobalMemory()
    {
        var result = ClipboardFormat.FromId(0x0201, "Private format");

        Assert.Equal(FormatCategory.PrivateApplication, result.Category);
        Assert.Equal(ClipboardBacking.GlobalMemory, result.Backing);
        Assert.True(ClipboardFormat.IsReadable(0x0201));
    }

    [Fact]
    public void FromId_ClassifiesGdiFormatAsGdiHandle()
    {
        var result = ClipboardFormat.FromId(0x0301, "GDI format");

        Assert.Equal(FormatCategory.GdiObject, result.Category);
        Assert.Equal(ClipboardBacking.GdiHandle, result.Backing);
        Assert.False(ClipboardFormat.IsReadable(0x0301));
    }

    [Theory]
    [InlineData(0xC000u)]
    [InlineData(0xFFFFu)]
    public void IsReadable_RegisteredFormatsAreReadable(uint formatId)
    {
        var result = ClipboardFormat.FromId(formatId, "Registered format");

        Assert.Equal(FormatCategory.Registered, result.Category);
        Assert.Equal(ClipboardBacking.GlobalMemory, result.Backing);
        Assert.True(ClipboardFormat.IsReadable(formatId));
    }

    [Fact]
    public void FromId_ClassifiesOutOfRangeFormatAsUnknown()
    {
        var result = ClipboardFormat.FromId(0x0100, "Unknown format");

        Assert.Equal(FormatCategory.Unknown, result.Category);
        Assert.Equal(ClipboardBacking.Unknown, result.Backing);
        Assert.False(ClipboardFormat.IsReadable(0x0100));
    }

    [Theory]
    [InlineData(0x0001u, FormatCategory.Predefined, ClipboardBacking.GlobalMemory)]
    [InlineData(0x0011u, FormatCategory.Predefined, ClipboardBacking.GlobalMemory)]
    [InlineData(0x0012u, FormatCategory.Unknown, ClipboardBacking.Unknown)]
    [InlineData(0xBFFFu, FormatCategory.Unknown, ClipboardBacking.Unknown)]
    [InlineData(0xC000u, FormatCategory.Registered, ClipboardBacking.GlobalMemory)]
    public void FromId_ClassifiesBoundaryIds(uint id, FormatCategory category, ClipboardBacking backing)
    {
        var result = ClipboardFormat.FromId(id, "test");

        Assert.Equal(category, result.Category);
        Assert.Equal(backing, result.Backing);
    }
}
