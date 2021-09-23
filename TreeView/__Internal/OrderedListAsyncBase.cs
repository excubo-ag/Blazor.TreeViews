using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Excubo.Blazor.TreeViews.__Internal
{
    public class OrderedListAsyncBase<T> : ComponentBase
    {
        [CascadingParameter] protected TreeViewAsync<T> TreeViewAsync { get; set; }
        protected RenderFragment<ItemContent<T>> ItemTemplate => TreeViewAsync.ItemTemplate;
        protected Func<IEnumerable<T>, IEnumerable<T>> FilterBy => TreeViewAsync.FilterBy;
        protected Func<IEnumerable<T>, IEnumerable<T>> SortBy => TreeViewAsync.SortBy;
        protected string Class => TreeViewAsync?.ListClass;
        [Parameter] public int Level { get; set; }
        [Parameter] public bool Collapsed { get; set; }
    }
}