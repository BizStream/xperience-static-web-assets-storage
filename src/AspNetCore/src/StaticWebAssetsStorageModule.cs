using BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage.IO;
using CMS.Core;
using CMS.DataEngine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using StorageHelper = CMS.IO.StorageHelper;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;

/// <summary> A <see cref="Module"/> implementation that registers <see cref="StaticWebAssetsStorageProvider"/>s for discovered RCL bundles. </summary>
public class StaticWebAssetsStorageModule : Module
{
    public StaticWebAssetsStorageModule( )
        : base( nameof( StaticWebAssetsStorageModule ), false )
    {
    }

    protected override void OnInit( )
    {
        base.OnInit();

        var environment = Service.Resolve<IWebHostEnvironment>();
        var options = Service.Resolve<IOptions<StaticWebAssetsStorageOptions>>();

        if( options.Value.EnsureWebContentRoot && string.IsNullOrEmpty( environment.WebRootPath ) )
        {
            environment.WebRootPath = Path.Combine( environment.ContentRootPath, "wwwroot" );
        }

        if( !options.Value.EnvironmentNames.Any( name => environment.IsEnvironment( name ) ) )
        {
            // storage provider is not configured to run
            return;
        }

        var configuration = Service.Resolve<IConfiguration>();
        var paths = StaticWebAssetsHelper.GetRCLPaths( environment, configuration );
        foreach( (var basePath, var path) in paths )
        {
            if( basePath == StaticWebAssetsStorageStrings.KenticoMvcRCLBasePath )
            {
                // ignore Kentico's RCL; it's supported by Kentico's logic
                continue;
            }

            // `BuilderAssetsProvider` prepends the `IWebHostEnvironment.ContentRootPath` when resolving configured bundles/scripts/styles
            var rclPath = Path.Combine(
                environment.WebRootPath,
                basePath.Replace( "/", "\\" )
            );

            StorageHelper.MapStoragePath( rclPath, new StaticWebAssetsStorageProvider( rclPath, path ) );
        }
    }
}