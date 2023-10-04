namespace AParser;

public static class ErrorMessages
{
    public const string EmptySql = "String to parse can't be empty";

    public static string UnexpectedToken(TokenType current)
    {
        return $"Did not expect a {current} token at this point";
    }

    public static string UnexpectedToken(TokenType expected, TokenType current)
    {
        return $"Expected to get a {expected} token, but current token is: {current}";
    }

    public static string NullToken(string expected)
    {
        return $"Expected argument to be {expected}, but found null";
    }

    public static string CreateNode(NodeType nodeType)
    {
        return $"Can't create node of type {nodeType}";
    }

    public static string UnknownFunction(string name)
    {
        return $"Can't parse function {name}";
    }

    public static string UnknownSqlType(string name)
    {
        return $"Illegal type name: {name}";
    }

    public static string Unclosed(char expectedChar)
    {
        return $"Didn't find expected closing character: {expectedChar}";
    }
}