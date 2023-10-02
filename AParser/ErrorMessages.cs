namespace AParser;

public static class ErrorMessages
{
    public const string EmptySql = "String to parse can't be empty";

    public static string UnexpectedNode(NodeType expectedNodeType, TokenName currentTokenName)
    {
        return $"Expected to create a {expectedNodeType} node, but current token is: {currentTokenName}";
    }
}