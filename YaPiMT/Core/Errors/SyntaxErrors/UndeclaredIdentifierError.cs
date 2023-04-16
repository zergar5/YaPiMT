namespace YaPiMT.Core.Errors.SyntaxErrors;

public record UndeclaredIdentifierError(string Identifier) : IError
{
    public override string ToString()
    {
        return $"'{Identifier}' : undeclared identifier";
    }
}