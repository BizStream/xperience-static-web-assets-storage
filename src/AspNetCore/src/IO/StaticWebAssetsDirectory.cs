using System.Security.AccessControl;
using CMS.IO;

using DirectoryInfo = CMS.IO.DirectoryInfo;
using SearchOption = CMS.IO.SearchOption;

namespace BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage.IO;

internal class StaticWebAssetsDirectory : AbstractDirectory
{
    #region Fields
    private readonly AbstractDirectory directory;
    private readonly string rclPath;
    private readonly string rootPath;
    #endregion

    public StaticWebAssetsDirectory( string rclPath, string rootPath )
    {
        directory = new CMS.FileSystemStorage.Directory();
        this.rclPath = rclPath;
        this.rootPath = rootPath;
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override DirectoryInfo CreateDirectory( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void Delete( string path, bool recursive )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void Delete( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void DeleteDirectoryStructure( string path )
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<string> EnumerateDirectories( string path, string searchPattern, SearchOption searchOption )
    {
        if( TryGetRCLPath( path, out string? rclPath ) )
        {
            return directory.EnumerateDirectories( rclPath, searchPattern, searchOption )
                .Select( file => string.Concat( this.rclPath, "\\", file.AsSpan( rootPath.Length ) ) );
        }

        return directory.EnumerateDirectories( path, searchPattern, searchOption );
    }

    public override IEnumerable<string> EnumerateFiles( string path, string searchPattern )
    {
        if( TryGetRCLPath( path, out string? rclPath ) )
        {
            return directory.EnumerateFiles( rclPath, searchPattern )
                .Select( file => string.Concat( this.rclPath, "\\", file.AsSpan( rootPath.Length ) ) );
        }

        return directory.EnumerateFiles( path, searchPattern );
    }

    public override bool Exists( string path )
    {
        if( TryGetRCLPath( path, out string? rclPath ) )
        {
            return directory.Exists( rclPath );
        }

        return directory.Exists( path );
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override DirectorySecurity GetAccessControl( string path )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override string[] GetDirectories( string path, string searchPattern, SearchOption searchOption )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override string[] GetFiles( string path, string searchPattern )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void Move( string sourceDirName, string destDirName )
    {
        throw new NotImplementedException();
    }

    /// <summary> Not supported. </summary>
    /// <exception cref="NotImplementedException"/>
    public override void PrepareFilesForImport( string path )
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
}