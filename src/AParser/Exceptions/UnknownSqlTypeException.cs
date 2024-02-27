namespace AParser.Exceptions;

public class UnknownSqlTypeException : Exception
{
    public string Name { get; }

    public UnknownSqlTypeException(string name)
        : base(ErrorMessages.UnknownSqlType(name))
    {
        Name = name;
    }
}