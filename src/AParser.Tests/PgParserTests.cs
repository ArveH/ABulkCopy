namespace AParser.Tests;

public class PgParserTests : ParserTestBase
{
    [Theory]
    [InlineData("123", "123")]
    [InlineData("-123", "-123")]
    [InlineData("(123)", "(123)")]
    [InlineData("((99.9))", "((99.9))")]
    [InlineData("((-1))", "((-1))")]
    [InlineData("123.2444", "123.2444")]
    [InlineData("1.arve", "1.")]
    [InlineData(".1", ".1")]
    [InlineData("23.1", "23.1")]
    [InlineData("-.1", "-.1")]
    [InlineData("-0.1", "-0.1")]
    [InlineData("(CONVERT([bit],(0)))", "((0)::integer)")]
    [InlineData("convert(bit, 1)", "1::integer")]
    [InlineData("'arve'", "'arve'")]
    [InlineData("N'arve'", "N'arve'")]
    [InlineData("''", "''")]
    [InlineData("' '", "' '")]
    [InlineData("''''", "''''")]
    [InlineData("'5'''", "'5'''")]
    [InlineData("'''5'", "'''5'")]
    [InlineData("'s''5'", "'s''5'")]
    [InlineData("(CONVERT([datetime],N'19000101 00:00:00:000',(9)))", "(to_timestamp('19000101 00:00:00:000', 'YYYYMMDD HH24:MI:SS:FF3'))")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900 00:00:01:000',(9)))", "(cast('JAN 1 1900 00:00:01' as timestamp))")]
    [InlineData("(CONVERT([datetime],'20991231 23:59:59:998',(9)))", "(to_timestamp('20991231 23:59:59:998', 'YYYYMMDD HH24:MI:SS:FF3'))")]
    [InlineData("(CONVERT([datetime],'JAN 1 1900',(9)))", "(cast('JAN 1 1900' as timestamp))")]
    [InlineData("(CONVERT([smalldatetime],'JAN 1 1900',(9)))", "(cast('JAN 1 1900' as timestamp))")]
    [InlineData("(CONVERT([smalldatetime],'JAN 1 1900'))", "(cast('JAN 1 1900' as timestamp))")]
    [InlineData("(CONVERT([smalldatetime],'1900-01-01',(9)))", "(cast('1900-01-01' as timestamp))")]
    [InlineData("(CONVERT([smalldatetime],'1900-01-01'))", "(cast('1900-01-01' as timestamp))")]
    [InlineData("(getdate())", "(localtimestamp)")]
    [InlineData("(newid())", "(gen_random_uuid())")]
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