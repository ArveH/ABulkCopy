﻿using Testing.Shared.SqlServer;

namespace Testing.Shared;

public class FileHelper
{
    public MockFileSystem FileSystem { get; } = new();
    public string DataFolder { get; set; } = Path.Combine(".", "testdata");

    public void CreateSingleColMssSchemaFile(SchemaTableTuple st, IColumn col)
    {
        var tableDefinition = MssTestData.GetEmpty(st);
        tableDefinition.Columns.Add(col);
        CreateSingleColMssSchemaFile(tableDefinition);
    }

    public void CreateSingleColMssSchemaFile(TableDefinition tableDefinition)
    {
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
            Path.Combine(DataFolder, tableDefinition.GetSchemaFileName()),
            fileData);
    }

    public void CreateDataFile(SchemaTableTuple st, List<string> rows)
    {
        var fileData = new MockFileData(string.Join(Environment.NewLine, rows), Encoding.UTF8);
        FileSystem.AddFile(
            Path.Combine(DataFolder, st.GetDataFileName()),
            fileData);
    }
}