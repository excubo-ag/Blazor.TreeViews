using Microsoft.AspNetCore.Components;
using System;

namespace Excubo.Blazor.TreeViews.__Internal
{
    public class ListItemBase : ComponentBase
    {
        [Parameter] public int Level { get; set; }
        [Parameter] public RenderFragment<bool> ChildContent { get; set; }
        [Parameter] public bool HasChildren { get; set; }
        internal bool Collapsed { get; set; }
        internal Action CollapsedChanged { get; set; }
    }
}