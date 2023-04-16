namespace YaPiMT.Core.AST;

public class StatementsNode : ExpressionNode
{
    public readonly List<ExpressionNode> CodeStrings;

    public StatementsNode()
    {
        CodeStrings = new List<ExpressionNode>();
    }

    public void AddNode(ExpressionNode node)
    {
        CodeStrings.Add(node);
    }
}