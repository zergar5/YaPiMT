namespace YaPiMT.Core.Errors.SyntaxErrors;

public record ExpectedError(string Terminal) : IError
{
    public override string ToString()
    {
        return $"Expected '{Terminal}'";
    }
}