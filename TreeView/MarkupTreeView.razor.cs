using Microsoft.AspNetCore.Components;

namespace Excubo.Blazor.TreeViews
{
    public partial class MarkupTreeView : ComponentBase
    {
        /// <summary>
        /// Optional class(es) that are applied to every &lt;ul&gt; tag in the tree view.
        /// </summary>
        [Parameter] public string ListClass { get; set; }
        /// <summary>
        /// Optional class(es) that are applied to every &lt;li&gt; tag in the tree view.
        /// </summary>
        [Parameter] public string ItemClass { get; set; }
        /// <summary>
        /// When set to true, all items are initially collapsed.
        /// </summary>
        [Parameter] public bool InitiallyCollapsed { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
    }
}