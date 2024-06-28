namespace ABulkCopy.Cmd.Internal;

public static class DataFolder
{
    public static CmdStatus CreateIfNotExists(string folder)
    {
        if (Directory.Exists(folder)) return CmdStatus.Exists;

        try
        {
            Directory.CreateDirectory(folder);
            Log.Information("Folder '{Folder}' created", folder);
            Console.WriteLine($"Folder '{folder}' created");
            return CmdStatus.Created;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Folder {Folder} couldn't be created.",
                folder);
            Console.WriteLine($"Folder {folder} couldn't be created.");
            Console.WriteLine(ex.Message);
            return CmdStatus.ShouldExit;
        }
    }
}