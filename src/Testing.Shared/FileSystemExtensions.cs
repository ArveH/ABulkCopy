namespace Testing.Shared;

public static class FileSystemExtensions
{
    public static async Task<string> GetJsonDataText(
        this MockFileSystem fileSystem, 
        string path,
        SchemaTableTuple st)
    {
        var fullPath = Path.Combine(path, st.GetDataFileName());
        if (!fileSystem.FileExists(fullPath))
        {
            throw new FileNotFoundException("Data file should exist", fullPath);
        }

        return await fileSystem.File.ReadAllTextAsync(fullPath);
    }

    public static async Task<string> GetJsonSchemaText(
        this MockFileSystem fileSystem,
        string path,
        SchemaTableTuple st)
    {
        var fullPath = Path.Combine(path, st.GetSchemaFileName());
        if (!fileSystem.FileExists(fullPath))
        {
            throw new FileNotFoundException("Schema file should exist", fullPath);
        }

        return await fileSystem.File.ReadAllTextAsync(fullPath);
    }
}