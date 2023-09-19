namespace ABulkCopy.Common.Utils;

public class Quoter : IQuoter
{
    private readonly bool _addQuotes;

    public Quoter(IConfiguration config)
    {
        _addQuotes = Convert.ToBoolean(config[Constants.Config.AddQuotes]);
    }

    public string Quote(string identifier)
    {
        return _addQuotes ? $"\"{identifier}\"" : identifier;
    }
}