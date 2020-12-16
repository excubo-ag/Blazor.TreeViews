using Microsoft.AspNetCore.Components;
using System;

namespace Excubo.Blazor.TreeViews
{
    public delegate RenderFragment CheckboxFragment(bool? value, Action<bool?> value_changed);
    public delegate RenderFragment CheckboxFragment2(bool value, bool indeterminate, Action<bool> value_changed, Action<bool> indeterminate_changed);
    public static class CheckboxFragmentConverter
    {
        public static CheckboxFragment ToCheckboxFragment(this CheckboxFragment2 fragment)
        {
            return (value, value_changed) =>
            {
                return (builder) =>
                {
                    builder.AddContent(0, fragment(value == true, value == null, (v) => value_changed(v), (v) => value_changed(v ? default(bool?) : true)));
                };
            };
        }
    }
}