using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Excubo.Blazor.TreeViews.__Internal
{
    public partial class ListItem<T>
    {
        [Parameter] public T Item { get; set; }
        private bool? selected;
        private bool? Selected
        {
            get => selected;
            set
            {
                if (value == selected)
                {
                    return;
                }
                selected = value;
                SelectedChanged(selected);
                TreeView.UpdateSelection(Item, Selected);
                StateHasChanged();
            }
        }
        private void SelectedChanged(bool? value)
        {
            if (value == Selected)
            {
                return;
            }
            Selected = value;
            Parent?.ReevaluateSelected();
            OnSelectedChanged?.Invoke(this, Selected);
        }
        protected event EventHandler<bool?> OnSelectedChanged;
        [CascadingParameter] private TreeView<T> TreeView { get; set; }
        [CascadingParameter] private ListItem<T> Parent { get; set; }
        protected HashSet<ListItem<T>> Children = new HashSet<ListItem<T>>();
        protected override void OnInitialized()
        {
            if (TreeView.InitiallyCollapsed)
            {
                Collapsed = true;
            }
            base.OnInitialized();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (TreeView.SelectedItems != null && TreeView.SelectedItems.Contains(Item))
                {
                    Selected = true;
                }
                else
                {
                    Selected = false;
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
                    Parent.OnSelectedChanged += (object _, bool? new_value) =>
                    {
                        if (new_value == null)
                        {
                            return;
                        }
                        Selected = new_value;
                        OnSelectedChanged?.Invoke(this, Selected);
                        StateHasChanged();
                    };
                }
            }
            base.OnParametersSet();
        }
        protected void ReevaluateSelected()
        {
            // of course we have at least one child! this method is only called from children.
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
            Selected = state;
            StateHasChanged();
            Parent?.ReevaluateSelected();
        }
        private RenderFragment<ItemContent<T>> ItemTemplate => TreeView.ItemTemplate;
        private CheckboxFragment CheckboxTemplate => TreeView.CheckboxTemplate;
        private bool AllowSelection => TreeView.AllowSelection;
    }
}
