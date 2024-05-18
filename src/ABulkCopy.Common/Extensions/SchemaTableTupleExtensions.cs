using ABulkCopy.Common.Identifier;

namespace ABulkCopy.Common.Extensions;

public static class SchemaTableTupleExtensions
{
    public static string GetFullName(this SchemaTableTuple st)
    {
        return st.schemaName + "." + st.tableName;
    }

    public static string GetFullName(this SchemaTableTuple st, IIdentifier identifier)
    {
        return identifier.Get(st.schemaName) + "." + identifier.Get(st.tableName);
    }

    public static string GetDataFileName(this SchemaTableTuple st)
    {
        return st.GetFullName() + Constants.DataSuffix;
    }

    public static string GetSchemaFileName(this SchemaTableTuple st)
    {
        return st.GetFullName() + Constants.SchemaSuffix;
    }
}