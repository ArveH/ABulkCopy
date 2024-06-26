﻿using System.Runtime.Serialization;

namespace AParser;

public static class ErrorMessages
{
    public const string EmptySql = "String to parse can't be empty";
    public const string EmptyQuote = "No characters found between quotes";

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

    public static string CreateNode(NodeType type)
    {
        return $"Can't create node of type {type}";
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

    public static string Conversion(string from, string toTypeName)
    {
        return $"Can't convert {from} to {toTypeName}";
    }

    public static string UnexpectedNode(NodeType type)
    {
        return $"Unexpected node type: {type}";
    }
}