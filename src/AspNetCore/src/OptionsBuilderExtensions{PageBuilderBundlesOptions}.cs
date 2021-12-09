using System.Reflection;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.Extensions.Options;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;

/// <summary> Extensions to <see cref="OptionsBuilder{PageBuilderBundlesOptions}"/>. </summary>
public static class OptionsBuilderExtensions
{
    /// <summary> Configures Page Builder to discover static files bundled via an RCL. </summary>
    /// <param name="assembly"> The Assembly representing the RCL (the <see cref="AssemblyName.Name"/> is used to generating the RCL file path). </param>
    /// <param name="rootPath"> A custom path within the RCL in which PageBuilder assets are located. </param>
    public static OptionsBuilder<PageBuilderBundlesOptions> ConfigureRCLBundle(
        this OptionsBuilder<PageBuilderBundlesOptions> options,
        Assembly assembly,
        string rootPath = "PageBuilder"
    )
    {
        if( options is null )
        {
            throw new ArgumentNullException( nameof( options ) );
        }

        if( assembly is null )
        {
            throw new ArgumentNullException( nameof( assembly ) );
        }

        return options.Configure( options => options.AddRCLBundle( assembly, rootPath ) );
    }
}
