using CMS.IO;

using DirectoryInfo = CMS.IO.DirectoryInfo;
using FileAccess = CMS.IO.FileAccess;
using FileInfo = CMS.IO.FileInfo;
using FileMode = CMS.IO.FileMode;
using FileShare = CMS.IO.FileShare;
using FileStream = CMS.IO.FileStream;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage.IO;

/// <summary> Implementation of an <see cref="AbstractStorageProvider"/> that allows "ProviderObject" creation of RCL assets. </summary>
/// <remarks> This implementation is not intended to be used directly, nor does it guarantee proper implementation of the complete <see cref="AbstractStorageProvider"/> contract. </remarks>
/// <seealso cref="StaticWebAssetsStorageModule"/>
public class StaticWebAssetsStorageProvider : AbstractStorageProvider
{
    #region Fields
    private readonly string rclPath;
    private readonly string rootPath;
    #endregion

    /// <param name="rclPath"> The RCL path the provider is mapped to (<c>_content/{AssemblyName}/</c>). </param>
    /// <param name="rootPath"> The absolute path to the underlying folder containing the RCL assets. </param>
    public StaticWebAssetsStorageProvider( string rclPath, string rootPath )
    {
        this.rclPath = rclPath;
        this.rootPath = rootPath;
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override DirectoryInfo GetDirectoryInfo( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileInfo GetFileInfo( string fileName )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileStream GetFileStream( string path, FileMode mode )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileStream GetFileStream( string path, FileMode mode, FileAccess access )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileStream GetFileStream( string path, FileMode mode, FileAccess access, FileShare share )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileStream GetFileStream( string path, FileMode mode, FileAccess access, FileShare share, int bufferSize )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    protected override AbstractDirectory CreateDirectoryProviderObject( )
    {
        return new StaticWebAssetsDirectory( rclPath, rootPath );
    }

    /// <inheritdoc/>
    protected override AbstractFile CreateFileProviderObject( )
    {
        return new StaticWebAssetsFile( rclPath, rootPath );
    }
}