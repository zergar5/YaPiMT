namespace YaPiMT.Core.AST;

public class VariableNode : ExpressionNode
{
    public readonly Token VariableToken;

    public VariableNode(Token variableToken)
    {
        VariableToken = variableToken;
    }
}