namespace APostgres.Test;

public class QBFactoryForTest : IQueryBuilderFactory
{
    public List<string> KeyWords { get; } = new();
    public bool AddQuotes { get; set; } = true;

    public IQueryBuilder GetQueryBuilder()
    {
        return new QueryBuilder(new IdentifierForTest(AddQuotes, KeyWords));
    }

    class IdentifierForTest : IIdentifier
    {
        private readonly bool _addQuotes;
        private readonly List<string> _keyWords;

        public IdentifierForTest(bool addQuotes, List<string> keyWords)
        {
            _addQuotes = addQuotes;
            _keyWords = keyWords;
        }

        public string Get(string identifier)
        {
            if (_addQuotes || _keyWords.Contains(" "))
                return $"\"{identifier}\"";
            return identifier;
        }
    }
}

