using System.Text;

namespace YaPiMT.SyntaxAnalysis;

public class RpnBuilder
{
    private readonly OperationsStack _operationsStack;
    private readonly StringBuilder _stringBuilder;

    public RpnBuilder()
    {
        _stringBuilder = new StringBuilder();
        _operationsStack = new OperationsStack();
    }

    public void Append(string text)
    {
        if (_operationsStack.Operations.Exists(o => o.Name == text))
        {
            text = _operationsStack.PushOrPop(text);
        }

        _stringBuilder.Append(text);
    }

    public override string ToString()
    {
        return _stringBuilder.ToString();
    }
}