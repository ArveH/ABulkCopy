namespace Testing.Shared;

public class FileHelper
{
    private readonly string _tableName;
    private readonly DbServer _dbServer;

    public FileHelper(string tableName, DbServer dbServer)
    {
        _tableName = tableName;
        _dbServer = dbServer;
    }

    public MockFileSystem FileSystem { get; } = new();
    public string DataFolder { get; set; } = ".\\testdata";

    public void CreateSingleColMssSchemaFile(IColumn col)
    {
        var tableDefinition = MssTestData.GetEmpty(_tableName);
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
            Path.Combine(DataFolder, $"{_tableName}{DbServer.SqlServer.SchemaSuffix()}"),
            fileData);
    }

    public void CreateDataFile(string row1, string? row2=null)
    {
        var fileData = new MockFileData(row1, Encoding.UTF8);
        if (!string.IsNullOrEmpty(row2))
        {
            fileData.TextContents += Environment.NewLine + row2;
        }
        FileSystem.AddFile(
            Path.Combine(DataFolder, $"{_tableName}{_dbServer.DataSuffix()}"),
            fileData);
    }
}