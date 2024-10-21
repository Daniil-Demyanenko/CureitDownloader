namespace CureitDownloader;

public class Cureit(PathToSave _path) : IDisposable
{
    private readonly HttpClient _client = new();
    private string _oldFile = null!;

    public async Task<string> Download()
    {
        string fileName = _path.IsDirectoryPath ? Path.Combine(_path.Path, DateTime.Now.ToString("dd.MM.yyyy_HH-mm.exe")) : _path.Path;
        Console.WriteLine($"File downloading: {fileName}");

        try
        {
            await using var stream = await _client.GetStreamAsync("https://free.drweb.ru/download+cureit/gr/");
            await using var fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            await stream.CopyToAsync(fileStream);

            Console.WriteLine("Download completed");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }

        return fileName;
    }

    public async Task Update()
    {
        try
        {
            if (File.Exists(_oldFile)) File.Delete(_oldFile);
        }
        catch (Exception e)
        {
            Console.WriteLine("Can't delete old file: " + e.Message);
        }

        _oldFile = await Download();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}