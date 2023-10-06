namespace AParser.Test;

public class PgParserTests : ParserTestBase
{
    [Theory]
    [InlineData("123", "123")]
    [InlineData("(123)", "(123)")]
    [InlineData("(CONVERT([bit],(0)))", "(to_number((0)))")]
    [InlineData("convert(bit, 0)", "to_number(0)")]
    public void TestParseExpression(string testSql, string expected)
    {
        var tokenizer = GetTokenizer();
        var node = CreateExpressionNode(testSql, tokenizer);
        IPgParser parser = new PgParser();

        var result = parser.Parse(tokenizer, node);

        result.Should().Be(expected);
    }


    private INode CreateExpressionNode(string sql, ITokenizer tokenizer)
    {
        var tree = GetParseTree();
        tokenizer.Initialize(sql);
        tokenizer.GetNext();
        var node = tree.CreateExpression(tokenizer);
        return node;
    }
}