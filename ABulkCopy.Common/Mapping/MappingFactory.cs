namespace ABulkCopy.Common.Mapping;

public class MappingFactory : IMappingFactory
{
    public IMapping GetDefaultMssToPgMappings()
    {
        return new Mapping("DefaultMssToPg", DbServer.SqlServer, DbServer.Postgres)
        {
            Schemas =
            {
                { "", "public" },
                { "dbo", "public" }
            },
            Locations =
            {
                { "", null },
                { "PRIMARY", null }
            },
            Collations =
            {
                { "SQL_Latin1_General_CP1_CI_AS", "en_ci_ai" }
            },
            Columns = new List<ColumnMap>
            {
                new (MssTypes.Bit, PgTypes.Boolean),
                new (MssTypes.DateTime, PgTypes.Timestamp),
                new (MssTypes.DateTime2, PgTypes.Timestamp),
                new (MssTypes.DateTimeOffset, PgTypes.TimestampTz),
                new (MssTypes.Float, PgTypes.DoublePrecision),
                new (MssTypes.NChar, PgTypes.Char),
                new (MssTypes.NVarChar, PgTypes.VarChar),
                new (MssTypes.SmallDateTime, PgTypes.Timestamp),
                new (MssTypes.TinyInt, PgTypes.SmallInt),
                new (MssTypes.UniqueIdentifier, PgTypes.Uuid),
            }
        };
    }
}