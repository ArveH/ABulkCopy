namespace AParser.Exceptions;

public class UnknownFunctionException : Exception
{
    public string Name { get; }

    public UnknownFunctionException(string name)
        : base(ErrorMessages.UnknownFunction(name))
    {
        Name = name;
    }
}