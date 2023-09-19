namespace APostgres.Test;

public class QuoterForTest : IQuoter
{
    public bool AddQuotes { get; set; } = true;

    public string Quote(string identifier)
    {
        return AddQuotes ? $"\"{identifier}\"" : identifier;
    }
}