using AParser;
using AParser.Parsers.Pg;
using AParser.Tree;
using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace Benchmark.Test;

public class ParseConvertFunction
{
    private static readonly Regex FunctionRegex = new("(convert\\s*?\\((\\[*[a-z]*\\]*)\\s*,(\\((?>\\((?<DEPTH>)|\\)(?<-DEPTH>)|[^()]+)*\\)(?(DEPTH)(?!))|\\w+)\\s*\\))",
        RegexOptions.IgnoreCase);

    private ITokenFactory? _tokenFactory;
    private IParseTree? _parseTree;

    [GlobalSetup]
    public void Setup()
    {
        _tokenFactory = new TokenFactory();
        _parseTree = new ParseTree(new NodeFactory(), new SqlTypes());
    }

    [Benchmark]
    public void TestRegEx()
    {
        var testSql = "(CONVERT([bit],(0)))";
        var match = FunctionRegex.Match(testSql);
        if (match.Groups.Count < 3)
        {
            throw new Exception("Invalid match");
        }

        string functionName;
        var sqlType = match.Groups[2].Value.ToLower();
        switch (sqlType)
        {
            case "bit":
            case "[bit]":
                functionName = "to_number";
                break;
            default:
                throw new Exception("Unknown function");
        }
        var result = functionName + "(" + match.Groups[3].Value;
    }

    [Benchmark]
    public void TestParseExpression()
    {
        var tokenizer = new Tokenizer(_tokenFactory!);
        tokenizer.Initialize("(CONVERT([bit],(0)))");
        tokenizer.GetNext();
        var node = _parseTree!.CreateExpression(tokenizer);
        IPgParser parser = new PgParser();

        var result = parser.Parse(tokenizer, node);
    }
}