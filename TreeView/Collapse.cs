using Excubo.Blazor.TreeViews.__Internal;
using Microsoft.AspNetCore.Components;

namespace Excubo.Blazor.TreeViews
{
    [Excubo.Generators.Blazor.GenerateSetParametersAsync]
    /// <summary>
    /// This is a helper to interact with the list items collapse/expand state.
    /// Bind any boolean value to this, e.g. context.Item.Collapsed
    /// </summary>
    public partial class Collapse : ComponentBase
    {
        [Parameter] public bool Value { get; set; }
        [Parameter] public EventCallback<bool> ValueChanged { get; set; }
        [CascadingParameter] public ListItemBase ListItem { get; set; }
        protected override void OnParametersSet()
        {
            ListItem.Collapsed = Value;
            ListItem.CollapsedChanged = () =>
            {
                Value = ListItem.Collapsed;
                ValueChanged.InvokeAsync(Value);
            };
            base.OnParametersSet();
        }
    }
}