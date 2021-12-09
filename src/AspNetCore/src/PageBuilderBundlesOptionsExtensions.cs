using System.Reflection;
using Kentico.PageBuilder.Web.Mvc;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;

/// <summary> Extensions to <see cref="PageBuilderBundlesOptions"/>. </summary>
public static class PageBuilderBundlesOptionsExtensions
{
    /// <summary> Adds the given RCL Assembly to to <see cref="PageBuilderBundlesOptions"/>. </summary>
    /// <param name="assembly"> The Assembly representing the RCL (the <see cref="AssemblyName.Name"/> is used to generating the RCL file path). </param>
    /// <param name="rootPath"> A custom path within the RCL in which PageBuilder assets are located. </param>
    public static PageBuilderBundlesOptions AddRCLBundle( this PageBuilderBundlesOptions options, Assembly assembly, string rootPath = "PageBuilder" )
    {
        if( options is null )
        {
            throw new ArgumentNullException( nameof( options ) );
        }

        if( assembly is null )
        {
            throw new ArgumentNullException( nameof( assembly ) );
        }

        if( string.IsNullOrWhiteSpace( rootPath ) )
        {
            throw new ArgumentNullException( nameof( rootPath ) );
        }

        var basePath = $"_content\\{assembly.GetName().Name}\\{rootPath}";
        var adminPath = $"{basePath}\\Admin";
        var publicPath = $"{basePath}\\Public";

        options.PageBuilderAdminScripts
            .Contents
            .IncludedWebRootDirectories
            .Add( adminPath );

        options.PageBuilderAdminStyles
            .Contents
            .IncludedWebRootDirectories
            .Add( adminPath );

        options.PageBuilderPublicScripts
            .Contents
            .IncludedWebRootDirectories
            .Add( publicPath );

        options.PageBuilderPublicStyles
            .Contents
            .IncludedWebRootDirectories
            .Add( publicPath );

        return options;
    }

}