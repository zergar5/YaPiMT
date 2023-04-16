namespace YaPiMT.Core.AST;

public class BinaryOperationNode : ExpressionNode
{
    public readonly Token BinaryOperationToken;
    public readonly ExpressionNode LeftNode;
    public readonly ExpressionNode RightNode;

    public BinaryOperationNode(Token binaryOperationToken, ExpressionNode leftNode, ExpressionNode rightNode)
    {
        BinaryOperationToken = binaryOperationToken;
        LeftNode = leftNode;
        RightNode = rightNode;
    }
}