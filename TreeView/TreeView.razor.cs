using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Excubo.Blazor.TreeViews
{
    public partial class TreeView<T> : TreeViewBase<T>
    {
        /// <summary>
        /// The items to be displayed in the <see cref="TreeView"/>
        /// </summary>
        [Parameter] public List<T> Items { get; set; }
        /// <summary>
        /// If this parameter is set, the TreeView is populated by grouping items under their respective parent. Items which have no parent (null) are put at the top of the hierarchy.
        /// Alternatively, use <see cref="GetChildren" />.
        /// <br />
        /// <br />
        /// Note: the type parameter <c>T</c> must be nullable, as the top level items are determined by checking <c>GetParent(item) == null</c>.
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
        private bool has_children_initialized;
        internal bool IsLazyLoading;
        protected override void OnParametersSet()
        {
            if (!has_children_initialized)
            {
                IsLazyLoading = HasChildren != null;
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