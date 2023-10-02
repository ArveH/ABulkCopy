namespace AParser
{
    public class AParser : IAParser
    {
        private readonly ITokenizer _tokenizer;

        public AParser(
            ITokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public void Parse(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new AParserException(ErrorMessages.EmptySql);
            }

            _tokenizer.Initialize(sql);
        }
    }
}