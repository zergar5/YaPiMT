namespace YaPiMT.Core.Errors;

public record InvalidCharacterError(int LineNumber, char Character) : IError
{
    public override string ToString()
    {
        return $"Line {LineNumber}: Invalid character \'{Character}\'";
    }
}