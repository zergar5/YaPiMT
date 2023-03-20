namespace YaPiMT.Core.Errors;

public record UnreachableCodeError(int LineNumber, string Code) : IError
{
    public override string ToString()
    {
        return $"Line {LineNumber}: Unrecognized lexeme \"{Code}\"";
    }
}