using BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage.IO;
using Kentico.Content.Web.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage;

/// <summary> Represents options that configure the behavior of <see cref="StaticWebAssetsStorageProvider"/>s. </summary>
public class StaticWebAssetsStorageOptions
{
    /// <summary> Indicate whether the <see cref="StaticWebAssetsStorageModule"/> should set the <see cref="IWebHostEnvironment.WebRootPath"/> is it <c>null</c>. </summary>
    /// <remarks>
    /// In some Mvc configurations (net5.0, net6.0), the <see cref="IWebHostEnvironment.WebRootPath"/> is not set to the <c>wwwroot</c> folder by default.
    /// This behavior prevents Kentico's <c>AssetsBuilderProvider</c> from enumerating the <see cref="BundleConfiguration{TBundler}.Contents"/>, thus preventing the <see cref="StaticWebAssetsStorageProvider"/> from providing RCL assets.
    /// </remarks>
    public bool EnsureWebContentRoot { get; set; } = true;

    /// <summary> The names of <see cref="Environments"/> in which to register <see cref="StaticWebAssetsStorageProvider"/>s. </summary>
    /// <value> <see cref="Environments.Development"/>. </value>
    public IList<string> EnvironmentNames { get; } = new List<string> { Environments.Development };
}