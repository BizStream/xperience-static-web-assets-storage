using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

#if NET6_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
#else
using System.Xml.Linq;
#endif

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;

/// <summary> Static helper class for retrieving values from the RCL Bundle paths from the default manifest. </summary>
/// <seealso href="https://docs.microsoft.com/en-us/aspnet/core/razor-pages/ui-class"> Reusable UI with Razor Class Libraries. </seealso>
public static class StaticWebAssetsHelper
{
#if NET5_0_OR_GREATER
    private const string ManifestFileName = "staticwebassets.runtime.json";
#else
    private const string ManifestFileName = "StaticWebAssets.xml";
#endif

    /// <summary> Retrieve <c>(BasePath, Path)</c> pairs from the default RCL Bundle Manifest. </summary>
    /// <param name="environment"> The environment to resolve the manifest within. </param>
    /// <param name="configuration"> If specified, the configuration object used to retrieve the manifest <see cref="WebHostDefaults.StaticWebAssetsKey">default location</see>. </param>
    public static IEnumerable<(string BasePath, string Path)> GetRCLPaths( IWebHostEnvironment environment, IConfiguration? configuration = null )
    {
        if( environment is null )
        {
            throw new ArgumentNullException( nameof( environment ) );
        }

        using var source = ResolveManifest( environment, configuration );
        if( source is not null )
        {
#if NET6_0_OR_GREATER
            var manifest = StaticWebAssetManifest.Parse( source );
            var contentNode = manifest.Root.Children![ "_content" ];
            foreach( var entry in contentNode!.Children! )
            {
                if( !entry.Value.HasPatterns() )
                {
                    continue;
                }

                string? path = manifest.ContentRoots.ElementAt(
                    entry.Value.Patterns.First().ContentRoot
                );

                var basePath = $"_content/{entry.Key}";
                yield return (basePath, path);
            }
#else
            var manifest = XDocument.Load( source );
            foreach( var element in manifest.Root!.Elements() )
            {
                var basePath = element.Attribute( "BasePath" )!.Value;
                var path = element.Attribute( "Path" )!.Value;

                yield return (basePath, path);
            }
#endif
        }
    }

    /*
     * The following methods were originally implemented based on the StaticWebAssets source code for net5.0:
     *  https://github.com/dotnet/aspnetcore/blob/3e9ae8e5eee2930da0096ab4ca4976f5938df648/src/Hosting/Hosting/src/StaticWebAssets/StaticWebAssetsLoader.cs#L55
     *
     *  MS decided to make them `internal`, but we need them for resolving the absolute paths to configure PageBuilder to locate _our_ RCL's static files
     *  This is all required due to Kentico's `BuilderAssetsProvider` (also `internal`), being hardcoded to only use the `IWebHostEnvironment.IFileProvder`
     *  for Kentico's RCL package (`BuilderAssetsProvider.GetLibraryBundleVirtualPaths`), and `CMS.IO.FileInfo` being used for the configurable
     *  page builder assets (`BuilderAssetsProvider.GetWebRootBundleVirtualPaths`).
     */
    private static Stream? ResolveManifest( IWebHostEnvironment environment, IConfiguration? configuration = null )
    {
        if( environment is null )
        {
            throw new ArgumentNullException( nameof( environment ) );
        }

        try
        {
            var manifestPath = configuration?.GetValue<string>( WebHostDefaults.StaticWebAssetsKey );
            var filePath = string.IsNullOrEmpty( manifestPath )
                ? ResolveRelativeToAssembly( environment )
                : manifestPath;

            if( !string.IsNullOrEmpty( filePath ) && File.Exists( filePath ) )
            {
                return File.OpenRead( filePath );
            }
            else
            {
                // A missing manifest might simply mean that the feature is not enabled, so we simply
                // return early. Misconfigurations will be uncommon given that the entire process is automated
                // at build time.
                return null;
            }
        }
        catch
        {
            return null;
        }
    }

    private static string? ResolveRelativeToAssembly( IWebHostEnvironment environment )
    {
        var assembly = Assembly.Load( environment.ApplicationName );
        string? basePath = string.IsNullOrEmpty( assembly.Location )
            ? AppContext.BaseDirectory
            : Path.GetDirectoryName( assembly.Location );

        return Path.Combine( basePath!, $"{environment.ApplicationName}.{ManifestFileName}" );
    }

#if NET6_0_OR_GREATER
    /*
     * The following types were copied from StaticWebAssets source code:
     * https://github.com/dotnet/aspnetcore/blob/73db5e40bbdeb4f04e5e05b9090f253760d1e1ee/src/Shared/StaticWebAssets/ManifestStaticWebAssetFileProvider.cs#L314
     */

    internal sealed class StaticWebAssetManifest
    {
        internal static readonly StringComparer PathComparer =
            OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        public string[] ContentRoots { get; set; } = Array.Empty<string>();

        public StaticWebAssetNode Root { get; set; } = null!;

        internal static StaticWebAssetManifest Parse( Stream manifest )
        {
            return JsonSerializer.Deserialize<StaticWebAssetManifest>( manifest )!;
        }
    }

    internal sealed class StaticWebAssetNode
    {
        [JsonPropertyName( "Asset" )]
        public StaticWebAssetMatch? Match { get; set; }

        [JsonConverter( typeof( OSBasedCaseConverter ) )]
        public Dictionary<string, StaticWebAssetNode>? Children { get; set; }

        public StaticWebAssetPattern[]? Patterns { get; set; }

        [MemberNotNullWhen( true, nameof( Children ) )]
        internal bool HasChildren( )
        {
            return Children?.Count > 0 is true;
        }

        [MemberNotNullWhen( true, nameof( Patterns ) )]
        internal bool HasPatterns( )
        {
            return Patterns?.Length > 0 is true;
        }
    }

    internal sealed class StaticWebAssetMatch
    {
        [JsonPropertyName( "ContentRootIndex" )]
        public int ContentRoot { get; set; }

        [JsonPropertyName( "SubPath" )]
        public string Path { get; set; } = null!;
    }

    internal sealed class StaticWebAssetPattern
    {
        [JsonPropertyName( "ContentRootIndex" )]
        public int ContentRoot { get; set; }

        public int Depth { get; set; }

        public string Pattern { get; set; } = null!;
    }

    private sealed class OSBasedCaseConverter : JsonConverter<Dictionary<string, StaticWebAssetNode>>
    {
        public override Dictionary<string, StaticWebAssetNode> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
        {
            var parsed = JsonSerializer.Deserialize<IDictionary<string, StaticWebAssetNode>>( ref reader, options )!;
            var result = new Dictionary<string, StaticWebAssetNode>( StaticWebAssetManifest.PathComparer );
            MergeChildren( parsed, result );
            return result;

            static void MergeChildren(
                IDictionary<string, StaticWebAssetNode> newChildren,
                IDictionary<string, StaticWebAssetNode> existing )
            {
                foreach( (string? key, var value) in newChildren )
                {
                    if( !existing.TryGetValue( key, out var existingNode ) )
                    {
                        existing.Add( key, value );
                    }
                    else
                    {
                        if( value.Patterns != null )
                        {
                            if( existingNode.Patterns == null )
                            {
                                existingNode.Patterns = value.Patterns;
                            }
                            else
                            {
                                if( value.Patterns.Length > 0 )
                                {
                                    var newList = new StaticWebAssetPattern[ existingNode.Patterns.Length + value.Patterns.Length ];
                                    existingNode.Patterns.CopyTo( newList, 0 );
                                    value.Patterns.CopyTo( newList, existingNode.Patterns.Length );
                                    existingNode.Patterns = newList;
                                }
                            }
                        }

                        if( value.Children != null )
                        {
                            if( existingNode.Children == null )
                            {
                                existingNode.Children = value.Children;
                            }
                            else
                            {
                                if( value.Children.Count > 0 )
                                {
                                    MergeChildren( value.Children, existingNode.Children );
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void Write( Utf8JsonWriter writer, Dictionary<string, StaticWebAssetNode> value, JsonSerializerOptions options )
        {
            JsonSerializer.Serialize( writer, value, options );
        }
    }
#endif
}