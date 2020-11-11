using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Excubo.Blazor.TreeViews.__Internal
{
    public partial class ListItem<T> : ListItemBase, IDisposable
    {
        [Parameter] public T Item { get; set; }
        private bool? Selected { get; set; }
        private void SelectedChanged(bool? value)
        {
            if (value == Selected)
            {
                return;
            }
            Selected = value;
            OnSelectedChanged?.Invoke(Selected);
            Parent?.ReevaluateSelected();
            TreeView.UpdateSelection(Item, Selected);
            InvokeAsync(StateHasChangedIfNotDisposed);
        }
        protected event Action<bool?> OnSelectedChanged;
        [CascadingParameter] private TreeView<T> TreeView { get; set; }
        [CascadingParameter] private ListItem<T> Parent { get; set; }
        protected HashSet<ListItem<T>> Children = new HashSet<ListItem<T>>();
        protected override void OnInitialized()
        {
            if (TreeView.InitiallyCollapsed)
            {
                Collapsed = true;
            }
            SelectedChanged(Parent?.Selected);
            base.OnInitialized();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (TreeView.SelectedItems != null && TreeView.SelectedItems.Contains(Item))
                {
                    SelectedChanged(true);
                }
                else
                {
                    SelectedChanged(false);
                }
                Parent?.ReevaluateSelected();
            }
            base.OnAfterRender(firstRender);
        }
        protected override void OnParametersSet()
        {
            if (Parent != null)
            {
                if (Parent.Children.Add(this))
                {
                    Parent.OnSelectedChanged += ReactOnSelectedChanged;
                }
            }
            base.OnParametersSet();
        }
        protected void ReevaluateSelected()
        {
            if (!Children.Any())
            {
                return;
            }
            // The state of this needs to be indeterminate if
            // - at least one child is indeterminate, OR
            // - at least two children differ in state
            // Otherwise, the state of this needs to be the same as all the children, which is the same as the state of the first child.
            var state = Children.First().Selected;
            state = state == null ? null : (Children.Skip(1).Any(c => c.Selected != state) ? null : state);
            if (Selected == state)
            {
                return;
            }
            SelectedChanged(state);
        }
        private RenderFragment<ItemContent<T>> ItemTemplate => TreeView.ItemTemplate;
        private CheckboxFragment CheckboxTemplate => TreeView.CheckboxTemplate;
        private bool AllowSelection => TreeView.AllowSelection;
        private bool disposed;
        private void StateHasChangedIfNotDisposed()
        {
            if (!disposed)
            {
                try
                {
                    StateHasChanged();
                }
                catch
                {
                    // ignored
                }
            }
        }
        private void ReactOnSelectedChanged(bool? new_value)
        {
            if (new_value == null)
            {
                return;
            }
            SelectedChanged(new_value);
        }
        public void Dispose()
        {
            disposed = true;
            if (Parent != null)
            {
                Parent.OnSelectedChanged -= ReactOnSelectedChanged;
                Parent.Children.Remove(this);
            }
            SelectedChanged(false);
        }
    }
}
