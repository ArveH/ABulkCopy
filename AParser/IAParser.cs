namespace AParser;

public interface IAParser
{
    INode Parse(string sql);
}