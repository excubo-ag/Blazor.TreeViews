## Excubo.Blazor.TreeView

![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.TreeView)
![Nuget](https://img.shields.io/nuget/dt/Excubo.Blazor.TreeView)
![GitHub](https://img.shields.io/github/license/excubo-ag/Blazor.TreeView)

Excubo.Blazor.TreeView is a native-Blazor tree-view component

[Demo on github.io using Blazor Webassembly](https://excubo-ag.github.io/Blazor.TreeView/)

## Key features

## How to use

### 1. Install the nuget package Excubo.Blazor.TreeView

Excubo.Blazor.TreeView is distributed [via nuget.org](https://www.nuget.org/packages/Excubo.Blazor.TreeView/).
![Nuget](https://img.shields.io/nuget/v/Excubo.Blazor.TreeView)

#### Package Manager:
```ps
Install-Package Excubo.Blazor.TreeView -Version 1.0.0
```

#### .NET Cli:
```cmd
dotnet add package Excubo.Blazor.TreeView --version 1.0.0
```

#### Package Reference
```xml
<PackageReference Include="Excubo.Blazor.TreeView" Version="1.0.0" />
```

### 2. Add the `TreeView` component to your app

```html
@using Excubo.Blazor.TreeViews

<TreeView Items="Items" />
```

Have a look at the fully working examples provided in [the sample project](https://github.com/excubo-ag/Blazor.TreeView/tree/master/TestProject_Components).

## Design principles

- Blazor API

The API should feel like you're using Blazor, not a javascript library.

- Minimal js, minimal css, lazy-loaded only when you use the component

The non-C# part of the code of the library should be as tiny as possible. We set ourselves a maximum amount of 10kB for combined js+css.
The current payload is 0kB.

## Checkbox style from another component library

It is possible to configure the checkbox style for a `TreeView` with `AllowSelection` enabled. Use the `CheckboxTemplate` parameter:

```
<TreeView Items="Items" AllowSelection="true" CheckboxTemplate="checkbox_template" />
```

Because of a [quirk in Blazor](https://github.com/dotnet/aspnetcore/issues/24655), writing the correct code for the `CheckboxTemplate` can be quite tricky.
See below for a list of snippets of correct checkboxes in some of the common component libraries.
If you can't find your favorite library, please consider contributing to this list.

### [MatBlazor](https://github.com/SamProf/MatBlazor)

```
@code {
    private static readonly object no_render = new object();
    private static readonly CheckboxFragment checkbox_template =
        (value, value_changed) =>
            (builder) =>
            {
                builder.OpenComponent<MatBlazor.MatCheckbox<bool?>>(0);
                builder.AddAttribute(1, nameof(MatBlazor.MatCheckbox<bool?>.Value), value);
                builder.AddAttribute(2, nameof(MatBlazor.MatCheckbox<bool?>.ValueChanged), EventCallback.Factory.Create<bool?>(no_render, value_changed));
                builder.AddAttribute(3, nameof(MatBlazor.MatCheckbox<bool?>.Indeterminate), true);
                builder.AddEventStopPropagationAttribute(4, "onclick", true);
                builder.CloseComponent();
            };
}
```

