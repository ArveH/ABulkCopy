namespace ABulkCopy.Cmd.Internal;

public static class DataFolder
{
    public static CmdStatus CreateIfNotExists(string folder)
    {
        if (Directory.Exists(folder)) return CmdStatus.Exists;

        Console.Write($"Create '{folder}' [y|n] (n is default)? ");
        var key = Console.ReadKey();
        Console.WriteLine();
        if (key.KeyChar is 'y' or 'Y' or '\n')
        {
            Directory.CreateDirectory(folder);
            Log.Information("Folder '{Folder}' created", folder);
            Console.WriteLine($"Folder '{folder}' created");
            return CmdStatus.Created;
        }

        return CmdStatus.ShouldExit;
    }
}