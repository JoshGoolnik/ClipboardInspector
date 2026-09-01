// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using ClipboardInspector.Core;
using ClipboardInspector.Core.Entities;
using ClipboardInspector.Core.Utilities;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Linq;

namespace ClipboardInspectorExtension;

internal sealed partial class ClipboardInspectorExtensionPage : ListPage
{
    public ClipboardInspectorExtensionPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "Clipboard Inspector";
        Name = "Open";
        ShowDetails = true;
    }

    public override IListItem[] GetItems()
    {
        var formats = ClipboardEnumeration.GetClipboardFormatsList();

        return formats
            .Select(f => new ListItem(new NoOpCommand())
            {
                Title = f.Name,
                Subtitle = $"{f.Id} · {f.Category} · {f.Backing}",
                Details = new Details
                {
                    Title = f.Name,
                    Body = BuildBody(f),
                }
            })
            .ToArray();
    }

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
