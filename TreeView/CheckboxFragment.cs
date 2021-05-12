using Microsoft.AspNetCore.Components;
using System;

namespace Excubo.Blazor.TreeViews
{
    public delegate RenderFragment CheckboxFragment(bool value, bool indeterminate, Action<bool> value_changed, bool disabled);
}