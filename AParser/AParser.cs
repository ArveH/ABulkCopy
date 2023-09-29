namespace AParser
{
    public class AParser : IAParser
    {
        public void Parse(ITokenizer tokenizer)
        {
            var token = tokenizer.GetNext();
        }
    }
}