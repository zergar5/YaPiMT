using System.ComponentModel.Design;
using YaPiMT.Core.Exceptions;

namespace YaPiMT.SyntaxAnalysis;

public class OperationsStack
{
    private readonly Stack<Operation> _stack;
    public readonly List<Operation> Operations;

    public OperationsStack()
    {
        _stack = new Stack<Operation>();
        Operations = new List<Operation>
        {
            new("(", 0),
            new("{", 0),
            new (";", 1),
            new (")", 1),
            new("}", 1),
            new (",", 1),
            new("-=", 2),
            new ("=", 2),
            new ("+=", 2),
            new ("*=", 2),
            new ("!=", 3),
            new ("==", 3),
            new (">", 3),
            new ("<", 3),
            new ("+", 4),
            new ("-", 4),
            new ("*", 4)
        };
    }

    public string PushOrPop(string operation)
    {
        var priority = Operations.FirstOrDefault(o => o.Name == operation).Priority;

        var operationsString = "";

        if (_stack.TryPeek(out var topOperation))
        {
            switch (operation)
            {
                case ")":
                    operationsString = HandleBrackets("(");

                    break;
                case "}":
                    operationsString = HandleBrackets("{");

                    break;
                case ";":
                    operationsString = HandleSemicolon();

                    break;
                default:
                {
                    if (priority != 0 && priority <= topOperation.Priority)
                    {
                        while (_stack.Count > 0 && priority <= topOperation.Priority)
                        {
                            operationsString += _stack.Pop().Name + " ";
                            if (!_stack.TryPeek(out topOperation)) break;
                        }
                    }

                    _stack.Push(new Operation(operation, priority));

                    break;
                }
            }
        }
        else _stack.Push(new Operation(operation, priority));

        return operationsString;
    }

    private string HandleBrackets(string bracket)
    {
        var operationsString = "";

        try
        {
            while (_stack.Peek().Name != $"{bracket}")
            {
                operationsString += _stack.Pop().Name + " ";
            }

            _stack.Pop();
        }
        catch (Exception)
        {
            throw new ExpectedBracketException($"Expected '{bracket}'");
        }

        return operationsString;
    }

    private string HandleSemicolon()
    {
        var operationsString = "";

        while (_stack.Peek().Name != "{")
        {
            operationsString += _stack.Pop().Name + " ";
        }

        return operationsString + "; ";
    }
}

public record struct Operation(string Name, int Priority);