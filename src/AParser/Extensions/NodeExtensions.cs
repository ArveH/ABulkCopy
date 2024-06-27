namespace AParser.Extensions;

public static class NodeExtensions
{
    public static bool IsSimpleValue(this INode node)
    {
        if (IsNodeConstantValue(node))
        {
            return true;
        }

        if (node.Type == NodeType.RightParenthesesNode)
        {
            return IsSimpleValue(node.Children[1]);
        }

        return false;
    }

    public static INode? StripParentheses(this INode node)
    {
        if (IsNodeConstantValue(node))
        {
            return node;
        }

        if (node.Type == NodeType.ParenthesesNode)
        {
            return StripParentheses(node.Children[1]);
        }

        return null;
    }

    private static bool IsNodeConstantValue(INode node)
    {
        return node.Type is NodeType.NStringNode or NodeType.StringNode
               || node.Type == NodeType.NStringNode
               || node.Type == NodeType.NumberNode;
    }
}