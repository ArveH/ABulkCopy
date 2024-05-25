namespace ABulkCopy.Common.Extensions;

public static class TableDefinitionExtensions
{
    public static string GetFullName(this TableDefinition tableDefinition)
    {
        return tableDefinition.Header.Schema + "." + tableDefinition.Header.Name;
    }

    public static SchemaTableTuple GetNameTuple(this TableDefinition tableDefinition)
    {
        return (tableDefinition.Header.Schema, tableDefinition.Header.Name);
    }

    public static string GetSchemaFileName(this TableDefinition tableDefinition)
    {
        return tableDefinition.GetFullName() + Constants.SchemaSuffix;
    }
}