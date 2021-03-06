# Static Web Assets Storage ![License](https://img.shields.io/github/license/BizStream/xperience-static-web-assets-storage) [![NuGet Version](https://img.shields.io/nuget/v/BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage)](https://nuget.org/packages/bizstream.kentico.xperience.aspnetcore.staticwebassetsstorage)

This package provides an `AbstractStorageProvider` implementation, and an accompanying `Module`, that allow Xperience Page Builder to auto-discover static assets bundled from [Razor Class Libraries](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/ui-class#create-an-rcl-with-static-assets) during Localhost development.

## Usage

- Install the package into your Xperience Mvc project:

```bash
dotnet add package BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage
```

OR

```csproj
<PackageReference Include="BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage" Version="x.x.x" />
```

- Register the `Module` (within the Mvc Application **only**):

```csharp
using BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;
using CMS;

[assembly: RegisterModule( typeof( StaticWebAssetsStorageModule ) )]
```

- Configure services in `Startup.cs`:

```csharp
using BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ...

public void ConfigureServices( IServiceCollection services )
{
    services.AddControllersWithViews();

    Assembly rclAssembly = /* ... */
    services.AddStaticWebAssetsStorage()
        .AddOptions<PageBuilderBundlesOptions>()
        .ConfigureRCLBundle( rclAssembly, "dist\\PageBuilder" );
}
```

### Configuring Environments

As RCL static assets are included when packed (via `dotnet pack` or `dotnet publish`), the `StaticWebAssetsStorageProvider` only needs to be registered for localhost development environments. If a custom environment name is used for localhost development, expose the environment name to the `StaticWebAssetsStorageModule` via the `StaticWebAssetsStorageOptions.EnvironmentNames` option:

```csharp
using BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ...

public void ConfigureServices( IServiceCollection services )
{
    services.AddControllersWithViews();

    services.AddStaticWebAssetsStorage(
        options => options.EnvironmentNames.Add( "MyCustomEnvironment" )
    );

    // OR, using OptionsBuilder:
    services.AddOptions<StaticWebAssetsStorageOptions>()
        .PostConfigure(
            options => options.EnvironmentNames.Add( "MyCustomEnvironment" )
        );

    // ...
}
```

> _`StaticWebAssetsStorageOptions.EnvironmentNames` is initialized with a default value containing the `Development` environment name._
