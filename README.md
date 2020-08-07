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
