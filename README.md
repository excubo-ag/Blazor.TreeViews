## Excubo.Blazor.TreeViews

[![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.TreeViews)](https://www.nuget.org/packages/Excubo.Blazor.TreeViews/)
[![Nuget](https://img.shields.io/nuget/dt/Excubo.Blazor.TreeViews)](https://www.nuget.org/packages/Excubo.Blazor.TreeViews/)
[![GitHub](https://img.shields.io/github/license/excubo-ag/Blazor.TreeViews)](https://github.com/excubo-ag/Blazor.TreeViews/)

Excubo.Blazor.TreeViews is a native-Blazor tree-view component.

[Demo on github.io using Blazor Webassembly](https://excubo-ag.github.io/Blazor.TreeViews/)

## How to use

### 1. Install the nuget package Excubo.Blazor.TreeViews

Excubo.Blazor.TreeViews is distributed [via nuget.org](https://www.nuget.org/packages/Excubo.Blazor.TreeViews/).
[![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.TreeViews)](https://www.nuget.org/packages/Excubo.Blazor.TreeViews/)

#### Package Manager:
```ps
Install-Package Excubo.Blazor.TreeViews
```

#### .NET Cli:
```cmd
dotnet add package Excubo.Blazor.TreeViews
```

#### Package Reference
```xml
<PackageReference Include="Excubo.Blazor.TreeViews" />
```

### 2. Add the `TreeView` component to your app

```html
@using Excubo.Blazor.TreeViews

<TreeView Items="Items" />
```

Have a look at the fully working examples provided in [the sample project](https://github.com/excubo-ag/Blazor.TreeViews/tree/main/TestProject_Components).

## Design principles

- Blazor API

The API should feel like you're using Blazor, not a javascript library.

- Minimal js, minimal css, lazy-loaded only when you use the component

The non-C# part of the code of the library should be as tiny as possible. We set ourselves a maximum amount of 10kB for combined js+css.
The current payload is 0kB.

## Breaking changes

### Version 3.X.Y

Starting with version 3.0.0, only the net6.0 TFM is targeted. This is a change to simplify dependency management and we strongly encourage all users to upgrade to net6.0.

### Version 2.X.Y

In this version, [@qinhuaihe](https://github.com/qinhuaihe) added support for `Disabled`, both on individual elements, and the entire tree view. This required a few changes to the API, including how custom checkbox styles are handled.
Please refer to the section below to learn how to implement a custom checkbox style.

## Checkbox style from another component library

It is possible to configure the checkbox style for a `TreeView` with `AllowSelection` enabled. Use the `CheckboxTemplate` parameter:

```
<TreeView Items="Items" AllowSelection="true" CheckboxTemplate="checkbox_template" />
```

Because of a [quirk in Blazor](https://github.com/dotnet/aspnetcore/issues/24655), writing the correct code for the `CheckboxTemplate` can be quite tricky.
See below for a list of snippets of correct checkboxes in some of the common component libraries.
If you can't find your favorite library, please consider contributing to this list.

### [MatBlazor](https://github.com/SamProf/MatBlazor)

```cs
@code {
    private static readonly object no_render = new object();
    private static readonly CheckboxFragment checkbox_template_matblazor =
        (value, indeterminate, value_changed, disabled) =>
            (builder) =>
            {
                builder.OpenComponent<MatBlazor.MatCheckbox<bool?>>(0);
                builder.AddAttribute(1, nameof(MatBlazor.MatCheckbox<bool?>.Value), indeterminate ? null : value);
                builder.AddAttribute(2, nameof(MatBlazor.MatCheckbox<bool?>.ValueChanged), EventCallback.Factory.Create<bool?>(no_render, (v) => { if (v != null) { value_changed(v.Value); } }));
                builder.AddAttribute(3, nameof(MatBlazor.MatCheckbox<bool?>.Indeterminate), true);
                builder.AddAttribute(4, nameof(MatBlazor.MatCheckbox<bool?>.Disabled), disabled);
                builder.AddEventStopPropagationAttribute(5, "onclick", true);
                builder.CloseComponent();
            };
}
```

### [Material.Blazor](https://github.com/Material-Blazor/Material.Blazor)

```cs
@code {
    private static readonly object no_render = new object();
    private static readonly CheckboxFragment checkbox_template_material_blazor =
        (value, indeterminate, value_changed, disabled) =>
            (builder) =>
            {
                builder.OpenComponent<Material.Blazor.MBCheckbox>(0);
                builder.AddAttribute(1, nameof(Material.Blazor.MBCheckbox.Value), value);
                builder.AddAttribute(2, nameof(Material.Blazor.MBCheckbox.ValueChanged), EventCallback.Factory.Create<bool>(no_render, value_changed));
                builder.AddAttribute(3, nameof(Material.Blazor.MBCheckbox.IsIndeterminate), indeterminate);
                builder.AddAttribute(4, nameof(Material.Blazor.MBCheckbox.Disabled), disabled);
                builder.AddEventStopPropagationAttribute(5, "onclick", true);
                builder.CloseComponent();
            };
}
```


### [Radzen.Blazor](https://github.com/radzenhq/radzen-blazor)

```cs
@code {
    private static readonly object no_render = new object();
    private static readonly CheckboxFragment checkbox_template_radzen =
        (value, indeterminate, value_changed, disabled) =>
            (builder) =>
            {
                builder.OpenComponent<Radzen.Blazor.RadzenCheckBox<bool?>>(0);
                builder.AddAttribute(1, nameof(Radzen.Blazor.RadzenCheckBox<bool?>.Value), indeterminate ? null : value);
                builder.AddAttribute(2, nameof(Radzen.Blazor.RadzenCheckBox<bool?>.ValueChanged), EventCallback.Factory.Create<bool?>(no_render, (v) => { if (v != null) { value_changed(v.Value); } }));
                builder.AddAttribute(3, nameof(Radzen.Blazor.RadzenCheckBox<bool?>.TriState), false);
                builder.AddAttribute(4, nameof(Radzen.Blazor.RadzenCheckBox<bool?>.Disabled), disabled);
                builder.AddEventStopPropagationAttribute(5, "onclick", true);
                builder.CloseComponent();
            };
}
```
### Samples and Tutorials

The official usage examples can be found on https://excubo-ag.github.io/Blazor.TreeViews/.

All following links provided here are not **affiliated with excubo ag** and have been **contributed by the community**.

- [Blazor Tree Creator with Checkboxes](https://blazorhelpwebsite.com/ViewBlogPost/51) by [@ADefWebserver](https://github.com/ADefWebserver/)
