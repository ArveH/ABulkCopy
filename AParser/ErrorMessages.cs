namespace AParser;

public static class ErrorMessages
{
    public const string EmptySql = "String to parse can't be empty";

    public static string UnexpectedNode(NodeType expectedNodeType, TokenName currentTokenName)
    {
        return $"Expected to create a {expectedNodeType} node, but current token is: {currentTokenName}";
    }

    public static string NullToken(string expected)
    {
        return $"Expected argument to be {expected}, but found null";
    }

    public static string CreateNode(NodeType nodeType)
    {
        return $"Can't create node of type {nodeType}";
    }
}