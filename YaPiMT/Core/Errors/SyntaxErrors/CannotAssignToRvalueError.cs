namespace YaPiMT.Core.Errors.SyntaxErrors;

public record CannotAssignToRvalueError(string Operator) : IError
{
    public override string ToString()
    {
        return $"'{Operator}': left operand must be l-value";
    }
}