using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Excubo.Blazor.TreeViews.__Internal
{
    public partial class ListItem<T> : ListItemBase, IDisposable
    {
        [Parameter] public T Item { get; set; }
        private bool Selected { get; set; }
        private bool Indeterminate { get; set; }
        private void SelectedChanged(bool? selected, bool? indeterminate = null)
        {
            if (selected.HasValue && selected.Value == Selected && indeterminate.HasValue && indeterminate.Value == Indeterminate)
            {
                return;
            }

            if (indeterminate.HasValue)
            {
                Indeterminate = indeterminate.Value;
            }
            if (selected.HasValue)
            {
                Selected = selected.Value;
                OnSelectedChanged?.Invoke(Selected);
            }
            Parent?.ReevaluateSelected();
            TreeView.UpdateSelection(Item, Selected);
            InvokeAsync(StateHasChangedIfNotDisposed);
        }
        protected event Action<bool> OnSelectedChanged;
        [CascadingParameter] private TreeViewBase<T> TreeView { get; set; }
        [CascadingParameter] private ListItem<T> Parent { get; set; }
        protected HashSet<ListItem<T>> Children = new HashSet<ListItem<T>>();
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
            if (TreeView.ItemDisabled != null)
            {
                _disabled = TreeView.ItemDisabled.Invoke(Item);
            }
            base.OnParametersSet();
        }
        protected void ReevaluateSelected()
        {
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
            SelectedChanged(state, indeterminate);
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
            if (Disabled)
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
        protected RenderFragment LoadChildrenTemplate => (TreeView as TreeViewAsync<T>)?.LoadingTemplate;
    }
}