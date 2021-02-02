#region Arx One
// Arx One
// The ass kicking online backup
// © Arx One 2009-2019
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Excubo.Blazor.TreeViews.__Internal
{
    public partial class ListItemAsync<T>
    {
        [Parameter] public T Item { get; set; }
        protected bool? selected;
        protected bool? Selected
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
        protected void SelectedChanged(bool? value)
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
        [CascadingParameter] protected TreeViewAsync<T> TreeView { get; set; }
        [CascadingParameter] protected ListItemAsync<T> Parent { get; set; }
        protected HashSet<ListItemAsync<T>> Children = new HashSet<ListItemAsync<T>>();
        [Parameter] public EventCallback<bool> CollapseHasChanged { get; set; }
        [Parameter] public bool LoadingChild { get; set; }

        protected RenderFragment<ItemContent<T>> ItemTemplate => TreeView.ItemTemplate;
        protected RenderFragment LoadChildrenTemplate => TreeView.LoadingTemplate;
        protected CheckboxFragment CheckboxTemplate => TreeView.CheckboxTemplate;
        protected bool AllowSelection => TreeView.AllowSelection;

        protected override void OnParametersSet()
        {
            CollapsedChanged = async () => { await CollapseHasChanged.InvokeAsync(Collapsed); };
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
        }

        protected override void OnInitialized()
        {
            Collapsed = true;
            Selected = Parent?.Selected;
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
    }
}