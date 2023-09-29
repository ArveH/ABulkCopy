namespace AParser
{
    public class AParser : IAParser
    {
        public void Parse(ITokenizer tokenizer)
        {
            ParseExpression(tokenizer);
        }

        private void ParseExpression(ITokenizer tokenizer)
        {
            var token = tokenizer.GetNext();
            if (token.Name == TokenName.LeftParenthesesToken)
            {
                ParseParentheses(tokenizer);
                return;
            }

        }

        private void ParseParentheses(ITokenizer tokenizer)
        {
            
        }
    }
}