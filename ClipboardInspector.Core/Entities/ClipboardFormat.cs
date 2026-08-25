
using ClipboardInspector.Core.Enums;
namespace ClipboardInspector.Core.Entities;
public sealed record ClipboardFormat(uint Id, string Name, FormatCategory Category, ClipboardBacking Backing);