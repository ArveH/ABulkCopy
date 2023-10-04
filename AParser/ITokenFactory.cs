namespace AParser;

public interface ITokenFactory
{
    IToken GetToken(TokenType type, int startPos);
}