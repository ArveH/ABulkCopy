namespace AParser;

public interface ITokenFactory
{
    IToken GetToken(TokenName name, int startPos);
}