namespace ABulkCopy.Common.Mapping;

public class MappingFactory : IMappingFactory
{
    public IMapping GetDefaultMssToPgMappings()
    {
        return new Mapping(
            columns:new Dictionary<string, string>
            {
                {MssTypes.Bit, PgTypes.Boolean},
                {MssTypes.DateTime, PgTypes.Timestamp},
                {MssTypes.DateTime2, PgTypes.Timestamp},
                {MssTypes.DateTimeOffset, PgTypes.TimestampTz},
                {MssTypes.Float, PgTypes.DoublePrecision},
                {MssTypes.NChar, PgTypes.Char},
                {MssTypes.NVarChar, PgTypes.VarChar},
                {MssTypes.SmallDateTime, PgTypes.Timestamp},
                {MssTypes.TinyInt, PgTypes.SmallInt},
                {MssTypes.UniqueIdentifier, PgTypes.Uuid }
            })
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
            }
        };
    }
}