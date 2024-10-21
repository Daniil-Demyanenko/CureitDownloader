namespace CureitDownloader;

public class PathToSave
{
    public bool IsDirectoryPath => !_path.EndsWith(".exe", StringComparison.OrdinalIgnoreCase);
    public string Path
    {
        get => _path;
        init => CheckSet(value);
    }
    private string _path = String.Empty;

    public PathToSave(string path) => Path = path;
    
    private void CheckSet(string path)
    {
        _path = path;
        
        if (IsDirectoryPath && !Directory.Exists(_path)) throw new IOException("Directory not exist");
        if (IsDirectoryPath) return;
        
        var dir= System.IO.Path.GetDirectoryName(_path);
        if(!Directory.Exists(dir)) throw new IOException($"Directory not exist ({dir})");
    }
}