namespace YaPiMT.Core.AST;

public class ConstantNode : ExpressionNode
{
    public readonly Token ConstantToken;

    public ConstantNode(Token constantToken)
    {
        ConstantToken = constantToken;
    }
}