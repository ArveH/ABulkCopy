﻿namespace AParser.KnownTokens;

public class CommaToken : IToken
{
    public CommaToken(int startPos)
    {
        StartPos = startPos;
        Name = TokenName.CommaToken;
    }
    public TokenName Name { get; }
    public string? ExpectedSpelling => ",";
    public int StartPos { get; }
    public int Length
    {
        get => 1;
        set => throw new SetLengthException(TokenName.CommaToken);
    }
}