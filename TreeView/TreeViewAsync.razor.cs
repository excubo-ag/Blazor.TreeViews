using Excubo.Blazor.TreeViews.__Internal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Excubo.Blazor.TreeViews
{
    public partial class TreeViewAsync<T> : TreeViewBase<T>
    {
        [Parameter]
        public Func<T, Task<List<T>>> GetChildrenAsync { get; set; }

        [Parameter]
        public Func<T, Task<bool>> HasChildrenAsync { get; set; }

        [Parameter]
        public RenderFragment LoadingTemplate { get; set; } = (builder) =>
        {
            builder.OpenComponent<DefaultLoadingTemplate>(0);
            builder.CloseComponent();
        };

        protected override void OnParametersSet()
        {
            InitiallyCollapsed = true;
            base.OnParametersSet();
        }
    }
}
