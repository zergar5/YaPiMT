namespace YaPiMT.Core.Errors.SyntaxErrors;

public record CannotResolveSymbolError(string Symbol) : IError
{
    public override string ToString()
    {
        return $"Cannot resolve symbol '{Symbol}'";
    }
}