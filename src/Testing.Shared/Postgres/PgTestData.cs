namespace Testing.Shared.Postgres;

public static class PgTestData
{
    public static TableDefinition GetEmpty(string name)
    {
        return new TableDefinition(Rdbms.Pg)
        {
            Header = new TableHeader
            {
                Id = 1,
                Location = "default",
                Name = name,
                Schema = "dbo"
            }
        };
    }
}