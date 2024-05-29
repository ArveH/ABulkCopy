namespace ABulkCopy.Common.Identifier;

public class Identifier : IIdentifier
{
    private readonly bool _addQuotes;
    private readonly bool _toLower;
    private readonly Rdbms _rdbms;
    private readonly HashSet<string> _keywords;

    public Identifier(
        IConfiguration config,
        IDbContext dbContext)
    {
        _addQuotes = Convert.ToBoolean(config[Constants.Config.AddQuotes]);
        _toLower = Convert.ToBoolean(config[Constants.Config.ToLower]);
        var keywords = config.GetSection(Constants.Config.PgKeywords)
                           .Get<List<string>>()
                       ?? new List<string>();

        _keywords = keywords.ToHashSet(StringComparer.CurrentCultureIgnoreCase);
        _rdbms = dbContext.Rdbms;
    }

    public string AdjustForSystemTable(string name)
    {
        return ShouldAddQuotes(name) ? name : name.ToLower();
    }

    public string Get(string name)
    {
        name = _toLower ? name.ToLower() : name;
        return ShouldAddQuotes(name) ? "\"" + name + "\"" : name;
    }

    private bool ShouldAddQuotes(string name)
    {
        return _rdbms == Rdbms.Mss || _addQuotes || _keywords.Contains(name);
    }
}