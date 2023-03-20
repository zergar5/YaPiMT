namespace YaPiMT.Core.Errors;

public record UnexpectedTokenError(int LineNumber, string TokenName) : IError
{
    public override string ToString()
    {
        return $"Line {LineNumber}: Invalid character \'{TokenName}\'";
    }
}