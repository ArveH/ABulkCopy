namespace Testing.Shared;

public class FileHelper
{
    private readonly Rdbms _rdbms;

    public FileHelper(Rdbms rdbms)
    {
        _rdbms = rdbms;
    }

    public MockFileSystem FileSystem { get; } = new();
    public string DataFolder { get; set; } = ".\\testdata";

    public void CreateSingleColMssSchemaFile(string tableName, IColumn col)
    {
        var tableDefinition = MssTestData.GetEmpty(tableName);
        tableDefinition.Columns.Add(col);
        var options = new JsonSerializerOptions
        {
            // Contrast is going to have a field day with me allowing stuff like ' :-)
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(), new ColumnInterfaceConverter() }
        };
        var jsonText = JsonSerializer.Serialize(tableDefinition, options);

        var fileData = new MockFileData(jsonText);
        FileSystem.AddFile(
            Path.Combine(DataFolder, $"{tableName}{Rdbms.Mss.SchemaSuffix()}"),
            fileData);
    }

    public void CreateDataFile(string tableName, List<string> rows)
    {
        var fileData = new MockFileData(string.Join(Environment.NewLine, rows), Encoding.UTF8);
        FileSystem.AddFile(
            Path.Combine(DataFolder, $"{tableName}{_rdbms.DataSuffix()}"),
            fileData);
    }
}