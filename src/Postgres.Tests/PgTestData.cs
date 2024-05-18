namespace Postgres.Tests;

public static class PgTestData
{
    public static TableDefinition GetEmpty(SchemaTableTuple st)
    {
        return new TableDefinition(Rdbms.Pg)
        {
            Header = new TableHeader
            {
                Id = 1,
                Location = "default",
                Name = st.tableName,
                Schema = st.schemaName
            }
        };
    }
}