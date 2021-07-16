using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Excubo.Blazor.TreeViews.__Internal
{
    public sealed partial class ListItem<T> : ListItemBase, IDisposable
    {
        [Parameter] public T Item { get; set; }
        private bool Selected { get; set; }
        private bool Indeterminate { get; set; }
        private void SelectedChanged(bool newSelected)
        {
            if (disposed)
            {
                return;
            }
            if (newSelected == Selected && !Indeterminate)
            {
                return;
            }

            Indeterminate = false;
            Selected = newSelected;
            OnSelectedChanged?.Invoke(Selected);
            Parent?.ReevaluateSelected();
            TreeView.UpdateSelection(Item, Selected, Indeterminate);
            InvokeAsync(StateHasChangedIfNotDisposed);
        }
        private void SelectedAndIndeterminateChanged(bool newSelected, bool newIndeterminate)
        {
            if (disposed)
            {
                return;
            }
            if (newSelected == Selected && newIndeterminate == Indeterminate)
            {
                return;
            }

            Indeterminate = newIndeterminate;
            Selected = newSelected;
            OnSelectedChanged?.Invoke(Selected);
            Parent?.ReevaluateSelected();
            TreeView.UpdateSelection(Item, Selected, Indeterminate);
            InvokeAsync(StateHasChangedIfNotDisposed);
        }
        private void IndeterminateChanged(bool indeterminate)
        {
            if (disposed)
            {
                return;
            }

            Indeterminate = indeterminate;
            Parent?.ReevaluateSelected();
            TreeView.UpdateSelection(Item, Selected, Indeterminate);
            InvokeAsync(StateHasChangedIfNotDisposed);
        }
        private event Action<bool> OnSelectedChanged;
        [CascadingParameter] private TreeViewBase<T> TreeView { get; set; }
        [CascadingParameter] private ListItem<T> Parent { get; set; }
        private HashSet<ListItem<T>> Children = new HashSet<ListItem<T>>();
        private string Class => TreeView?.ItemClass;
        [Parameter] public EventCallback<bool> CollapseHasChanged { get; set; }
        [Parameter] public bool LoadingChild { get; set; }
        private bool _disabled { get; set; }
        public bool Disabled => TreeView.Disabled || Parent?.Disabled == true || _disabled;

        protected override void OnInitialized()
        {
            CollapsedChanged = async () => { await CollapseHasChanged.InvokeAsync(Collapsed); };
            if (TreeView.InitiallyCollapsed)
            {
                Collapsed = true;
            }
            if (!Disabled)
            {
                var this_should_be_selected = Parent?.Selected == true || TreeView.SelectedItems?.Contains(Item) == true;
                SelectedChanged(this_should_be_selected);
            }
            base.OnInitialized();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (disposed)
            {
                return;
            }
            if (firstRender)
            {
                if (TreeView.SelectedItems != null && TreeView.SelectedItems.Contains(Item))
                {
                    SelectedChanged(true);
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
            if (TreeView.ItemDisabled != null)
            {
                _disabled = TreeView.ItemDisabled.Invoke(Item);
            }
            base.OnParametersSet();
        }
        private void ReevaluateSelected()
        {
            if (disposed)
            {
                return;
            }
            if (!Children.Any())
            {
                return;
            }

            bool? state = null;
            // The state of indeterminate needs to be true if
            // - at least one child is indeterminate, OR
            // - at least two children differ in state
            var indeterminate = Children.Any(x => x.Indeterminate) || (Children.Any(x => x.Selected) && Children.Any(x => !x.Selected));
            if (Children.All(x => x.Selected && !x.Indeterminate))
            {
                state = true;
            }
            else if (Children.All(x => !x.Selected && !x.Indeterminate))
            {
                state = false;
            }
            if (state == null)
            {
                IndeterminateChanged(indeterminate);
            }
            else
            {
                SelectedAndIndeterminateChanged(state.Value, indeterminate);
            }
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
        private void ReactOnSelectedChanged(bool new_value)
        {
            if (disposed)
            {
                return;
            }
            if (Disabled)
            {
                return;
            }
            SelectedChanged(new_value);
        }
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }
            disposed = true;
            if (Parent != null)
            {
                Parent.OnSelectedChanged -= ReactOnSelectedChanged;
                Parent.Children.Remove(this);
                Parent.ReevaluateSelected();
            }
        }
        private RenderFragment LoadChildrenTemplate => (TreeView as TreeViewAsync<T>)?.LoadingTemplate;
    }
}