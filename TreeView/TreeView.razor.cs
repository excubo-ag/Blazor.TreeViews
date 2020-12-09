using Excubo.Blazor.TreeViews.__Internal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Excubo.Blazor.TreeViews
{
    public partial class TreeView<T> : ComponentBase
    {
        /// <summary>
        /// The items to be displayed in the <see cref="TreeView"/>
        /// </summary>
        [Parameter] public List<T> Items { get; set; }
        /// <summary>
        /// If this parameter is set, the TreeView is populated by grouping items under their respective parent. Items which have no parent (null) are put at the top of the hierarchy.
        /// Alternatively, use <see cref="GetChildren" />.
        /// </summary>
        [Parameter] public Func<T, T> GetParent { get; set; }
        /// <summary>
        /// If this parameter is set, the TreeView is populated by all the items in Items, placing all children returned by this method under the respective item in the Items list.
        /// Alternatively, use <see cref="GetParent"/>.
        /// </summary>
        [Parameter] public Func<T, List<T>> GetChildren { get; set; }
        /// <summary>
        /// If this parameter is set, the TreeView will be populate based on the result of this method. Only works in conjunction with <see cref="GetChildren" />, not with <see cref="GetParent"/>.
        /// </summary>
        [Parameter] public Func<T, bool> HasChildren { get; set; }
        /// <summary>
        /// Controls how each item is displayed. Defaults to printing the item itself.
        /// </summary>
        [Parameter] public RenderFragment<ItemContent<T>> ItemTemplate { get; set; } = (ItemContent<T> value) => (builder) => builder.AddContent(0, @value.Item);
        /// <summary>
        /// Controls in which order the items are displayed. Sorting affects each level.
        /// </summary>
        [Parameter] public Func<IEnumerable<T>, IEnumerable<T>> SortBy { get; set; } = (e) => e;
        /// <summary>
        /// Controls which items are ignored. When an item is filtered, it and all its children are not displayed.
        /// </summary>
        [Parameter] public Func<IEnumerable<T>, IEnumerable<T>> FilterBy { get; set; } = (e) => e;
        /// <summary>
        /// When set to true, checkboxes are displayed.
        /// </summary>
        [Parameter] public bool AllowSelection { get; set; }
        /// <summary>
        /// The items that should have a checked checkbox next to them. Requires <see cref="AllowSelection"/> to be set to true.
        /// </summary>
        [Parameter] public List<T> SelectedItems { get; set; } = new List<T>();
        /// <summary>
        /// Callback for when the selection of items changes by user interaction. Requires <see cref="AllowSelection"/> to be set to true.
        /// </summary>
        [Parameter] public EventCallback<List<T>> SelectedItemsChanged { get; set; }
        /// <summary>
        /// Controls the display of the checkbox. Requires <see cref="AllowSelection"/> to be set to true. Defaults to a pure HTML checkbox.
        /// </summary>
        [Parameter] public CheckboxFragment CheckboxTemplate { get; set; } = (value, value_changed) => (builder) =>
        {
            builder.OpenComponent<DefaultCheckbox>(0);
            builder.AddAttribute(1, nameof(DefaultCheckbox.Value), value);
            builder.AddAttribute(1, nameof(DefaultCheckbox.ValueChanged), value_changed);
            builder.CloseComponent();
        };
        /// <summary>
        /// When set to true, all items are initially collapsed.
        /// </summary>
        [Parameter] public bool InitiallyCollapsed { get; set; }
        internal void UpdateSelection(T item, bool? selected)
        {
            if (selected == null)
            {
                return;
            }
            if (SelectedItems == null)
            {
                SelectedItems = new List<T>();
            }
            if (selected == false)
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
            else if (selected == true && !SelectedItems.Contains(item))
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
        private bool has_children_initialized;
        protected override void OnParametersSet()
        {
            if (!has_children_initialized)
            {
                if (HasChildren != null)
                {
                    InitiallyCollapsed = true; // Collapse all if we want lazy loading, else we'll load everything at first
                }
                else
                {
                    // In general case, we stay with default HasChildren behaviour
                    HasChildren = item =>
                    {
                        var childItem = GetChildren(item);
                        return childItem != null && childItem.Any();
                    };
                }
                has_children_initialized = true;
            }
            base.OnParametersSet();
        }
    }
}
