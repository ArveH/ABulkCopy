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
        var sb = new StringBuilder();
        sb.AppendLine("{");
        sb.AppendLine("  \"Header\": {");
        sb.AppendLine("    \"Id\": 1, ");
        sb.AppendLine($"    \"Name\": \"{_tableName}\", ");
        sb.AppendLine("    \"Schema\": \"dbo\", ");
        sb.AppendLine("    \"Location\": \"PRIMARY\"");
        sb.AppendLine("    }");
        sb.AppendLine("  \"  Columns\": [");
        sb.AppendLine("    {");
        sb.AppendLine($"      \"Name\":\"{col.Name}\",");
        sb.AppendLine($"      \"Type\":\"{col.Type}\",");
        sb.AppendLine("      \"IsNullable\": false");
        sb.AppendLine("      \"Identity\": null");
        sb.AppendLine("      \"ComputedDefinition\": null");
        sb.AppendLine($"      \"Length\": {col.Length}");
        sb.AppendLine($"      \"Precision\": {col.Precision}");
        sb.AppendLine($"      \"Scale\": {col.Scale}");
        sb.AppendLine("      \"DefaultConstraint\": null");
        sb.AppendLine("      \"Collation\": null");
        sb.AppendLine("    }");
        sb.AppendLine("  ],");
        sb.AppendLine("  \"PrimaryKey\": null,");
        sb.AppendLine("  \"ForeignKeys\": []");
        sb.AppendLine("}");

        var fileData = new MockFileData(sb.ToString());
        FileSystem.AddFile(
            Path.Combine(path, $"{_tableName}.schema"),
            fileData);
    }
}