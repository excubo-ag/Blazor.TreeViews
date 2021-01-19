using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Excubo.Blazor.TreeViews
{
    /// <summary>
    /// An item of a <see cref="MarkupTreeView"/>
    /// </summary>
    public partial class Item : IDisposable
    {
        [CascadingParameter] public MarkupTreeView TreeView { get; set; }
        [CascadingParameter] public Item Parent { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool ContentIsCollapsable { get; set; }
        private bool Collapsed { get; set; }
        internal readonly HashSet<Item> Children = new HashSet<Item>();
        protected override void OnInitialized()
        {
            if (TreeView.InitiallyCollapsed)
            {
                Collapsed = true;
            }
            if (Parent != null)
            {
                Parent.Children.Add(this);
                Parent.StateHasChanged();
            }
            base.OnInitialized();
        }
        public void Dispose()
        {
            if (Parent != null)
            {
                Parent.Children.Remove(this);
            }
        }
    }
}
