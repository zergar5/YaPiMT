namespace YaPiMT.Core.Errors.SyntaxErrors;

public record ExpectedBracketError(string Terminal) : IError
{
    public override string ToString()
    {
        return $"Expected '{Terminal}'";
    }

}