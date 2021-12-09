using System.Security.AccessControl;
using System.Text;
using CMS.IO;

using FileAccess = CMS.IO.FileAccess;
using FileAttributes = CMS.IO.FileAttributes;
using FileMode = CMS.IO.FileMode;
using FileStream = CMS.IO.FileStream;
using StreamReader = CMS.IO.StreamReader;
using StreamWriter = CMS.IO.StreamWriter;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage.IO;

internal class StaticWebAssetsFile : AbstractFile
{
    #region Fields
    private readonly AbstractFile file;
    private readonly string rclPath;
    private readonly string rootPath;
    #endregion

    public StaticWebAssetsFile( string rclPath, string rootPath )
    {
        file = new CMS.FileSystemStorage.File();
        this.rclPath = rclPath;
        this.rootPath = rootPath;
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void AppendAllText( string path, string contents )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void AppendAllText( string path, string contents, Encoding encoding )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void Copy( string sourceFileName, string destFileName )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void Copy( string sourceFileName, string destFileName, bool overwrite )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileStream Create( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override StreamWriter CreateText( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void Delete( string path )
    {
        throw new NotImplementedException();
    }

    public override bool Exists( string path )
    {
        if( TryGetRCLPath( path, out var rclPath ) )
        {
            return file.Exists( rclPath );
        }

        return file.Exists( path );
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileSecurity GetAccessControl( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override string GetFileUrl( string path, string siteName )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override DateTime GetLastWriteTime( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void Move( string sourceFileName, string destFileName )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileStream Open( string path, FileMode mode, FileAccess access )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override FileStream OpenRead( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override StreamReader OpenText( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override byte[] ReadAllBytes( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override string ReadAllText( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override string ReadAllText( string path, Encoding encoding )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void SetAttributes( string path, FileAttributes fileAttributes )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void SetLastWriteTime( string path, DateTime lastWriteTime )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void SetLastWriteTimeUtc( string path, DateTime lastWriteTimeUtc )
    {
        throw new NotImplementedException();
    }

    private bool TryGetRCLPath( string path, out string? rclPath )
    {
        if( path?.StartsWith( this.rclPath, StringComparison.InvariantCultureIgnoreCase ) is true )
        {
            rclPath = string.Concat( rootPath.TrimEnd( '\\', '/' ), path.AsSpan( this.rclPath.Length ) );
            return true;
        }

        rclPath = null;
        return false;
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void WriteAllBytes( string path, byte[] bytes )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void WriteAllText( string path, string contents )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void WriteAllText( string path, string contents, Encoding encoding )
    {
        throw new NotImplementedException();
    }
}
