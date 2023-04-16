namespace YaPiMT.Core.Errors.SyntaxErrors;

public record RedefinitionError(string Identifier) : IError
{
    public override string ToString()
    {
        return $"'{Identifier}': redefinition; different basic types";
    }
}