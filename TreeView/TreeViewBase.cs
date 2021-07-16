using Excubo.Blazor.TreeViews.__Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Excubo.Blazor.TreeViews
{
    public partial class TreeViewBase<T> : ComponentBase
    {
        /// <summary>
        /// Controls how each item is displayed. Defaults to printing the item itself.
        /// </summary>
        [Parameter]
        public RenderFragment<ItemContent<T>> ItemTemplate { get; set; } = (ItemContent<T> value) => (builder) => builder.AddContent(0, @value.Item);
        /// <summary>
        /// Optional class(es) that are applied to every &lt;ul&gt; tag in the tree view.
        /// </summary>
        [Parameter]
        public string ListClass { get; set; }
        /// <summary>
        /// Optional class(es) that are applied to every &lt;li&gt; tag in the tree view.
        /// </summary>
        [Parameter]
        public string ItemClass { get; set; }
        /// <summary>
        /// Controls in which order the items are displayed. Sorting affects each level.
        /// </summary>
        [Parameter]
        public Func<IEnumerable<T>, IEnumerable<T>> SortBy { get; set; } = (e) => e;

        /// <summary>
        /// Controls which items are ignored. When an item is filtered, it and all its children are not displayed.
        /// </summary>
        [Parameter]
        public Func<IEnumerable<T>, IEnumerable<T>> FilterBy { get; set; } = (e) => e;
        /// <summary>
        /// When set to true, the checkboxes are disabled and the tree will not respond to user's actions.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }
        /// <summary>
        /// Controls which nodes are disabled. When an node is disabled, its children are disabled too.
        /// </summary>
        [Parameter]
        public Func<T, bool> ItemDisabled { get; set; }
        /// <summary>
        /// When set to true, checkboxes are displayed.
        /// </summary>
        [Parameter]
        public bool AllowSelection { get; set; }

        /// <summary>
        /// The items that should have a checked checkbox next to them. Requires <see cref="AllowSelection"/> to be set to true.
        /// </summary>
        [Parameter]
        public List<T> SelectedItems { get; set; }

        /// <summary>
        /// Callback for when the selection of items changes by user interaction. Requires <see cref="AllowSelection"/> to be set to true.
        /// </summary>
        [Parameter]
        public EventCallback<List<T>> SelectedItemsChanged { get; set; }
        /// <summary>
        /// Controls the display of the checkbox. Requires <see cref="AllowSelection"/> to be set to true. Defaults to a pure HTML checkbox.
        /// </summary>
        [Parameter]
        public CheckboxFragment CheckboxTemplate { get; set; } = (value, indeterminate, value_changed, disabled) => (builder) =>
        {
            builder.OpenComponent<DefaultCheckbox>(0);
            builder.AddAttribute(1, nameof(DefaultCheckbox.Value), value);
            builder.AddAttribute(2, nameof(DefaultCheckbox.Indeterminate), indeterminate);
            builder.AddAttribute(3, nameof(DefaultCheckbox.ValueChanged), value_changed);
            builder.AddAttribute(4, nameof(DefaultCheckbox.Disabled), disabled);
            builder.CloseComponent();
        };
        /// <summary>
        /// When set to true, all items are initially collapsed.
        /// </summary>
        [Parameter]
        public bool InitiallyCollapsed { get; set; }

        internal void UpdateSelection(T item, bool selected, bool indeterminate)
        {
            if (SelectedItems == null)
            {
                SelectedItems = new List<T>();
            }
            if (!selected || indeterminate)
            {
                SelectedItems.Remove(item);
                InvokeAsync(async () =>
                {
                    try
                    {
                        await SelectedItemsChanged.InvokeAsync(SelectedItems);
                    }
                    catch
                    {
                    }
                });
            }
            else
            {
                if (!SelectedItems.Contains(item))
                {
                    SelectedItems.Add(item);
                    InvokeAsync(async () =>
                    {
                        try
                        {
                            await SelectedItemsChanged.InvokeAsync(SelectedItems);
                        }
                        catch
                        {
                        }
                    });
                }
            }
        }
    }
}
