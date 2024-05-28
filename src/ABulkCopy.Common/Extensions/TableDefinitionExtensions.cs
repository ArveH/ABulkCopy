namespace ABulkCopy.Common.Extensions;

public static class TableDefinitionExtensions
{
    public static string GetFullName(this TableDefinition tableDefinition)
    {
        return tableDefinition.Header.Schema + "." + tableDefinition.Header.Name;
    }

    public static string GetFullName(this ForeignKey fk)
    {
        return fk.SchemaReference + "." + fk.TableReference;
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