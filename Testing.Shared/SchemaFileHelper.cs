using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ABulkCopy.Common.Serialization;

namespace Testing.Shared;

public class SchemaFileHelper
{
    private readonly string _tableName;

    public SchemaFileHelper(string tableName)
    {
        _tableName = tableName;
    }

    public MockFileSystem FileSystem { get; } = new();

    public void CreateSingleColFile(string path, IColumn col)
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
            Path.Combine(path, $"{_tableName}.schema"),
            fileData);
    }
}