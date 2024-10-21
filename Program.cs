using CureitDownloader;

var config = ParseArguments(args);

if (config.NeedHelpTip)
{
    Console.WriteLine(
        """
        For download latest version of DrWeb Cureit:
        CureitDownloader <path>

        For download and automatically update every 12 hours:
        CureitDownloader updatable <path>

        Where <path> is path to save directory or filename.exe.
        """);
    return;
}

using var cureit = new Cureit(new PathToSave(config.Path));

if (config.NeedSchedule)
{
    Console.WriteLine("File will update every 12 hours.");
    Console.WriteLine("Press Q to exit...");
    await SetUpdateScheduleTimer(cureit);
    while (Console.ReadKey().Key != ConsoleKey.Q) await Task.Delay(500);
}
else await cureit.Download();

static async Task SetUpdateScheduleTimer(Cureit cureit)
{
    await cureit.Download();

    var updateInterval = new TimeSpan(hours: 12, minutes: 0, seconds: 0);
    var updateTimer = new System.Timers.Timer(updateInterval);
    updateTimer.Elapsed += async (_, _) => await cureit.Update();

    updateTimer.AutoReset = true;
    updateTimer.Enabled = true;
    updateTimer.Start();
}

static (bool NeedSchedule, bool NeedHelpTip, string Path) ParseArguments(string[] args)
{
    if (args.Length == 2 && args[0].Contains("updatable", StringComparison.OrdinalIgnoreCase)) 
        return (true, false, args[1]);
    if (args.Length == 1 && Directory.Exists(Path.GetDirectoryName(args[0]))) 
        return (false, false, args[0]);

    return (false, true, String.Empty);
}