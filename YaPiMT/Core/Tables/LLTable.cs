using YaPiMT.IO;

namespace YaPiMT.Core.Tables;

public class LLTable
{
    private readonly List<Row> _table;
    private readonly Stack<int> _stateStack;
    public int CurrentStateNumber { get; private set; }

    public Row CurrentState => _table[CurrentStateNumber];

    public LLTable(string fileName, LLTableIO llTableI)
    {
        _table = new List<Row>();

        _stateStack = new Stack<int>();
        _stateStack.Push(-1);

        var elements = llTableI.ReadFromFile(fileName);

        foreach (var element in elements)
        {
            var row = element.Split(' ');

            var terminals = row[0];

            if (row.Length > 6)
            {
                var end = row.Length - 5;
                terminals = string.Join(' ', row[..end]);
            }

            _table.Add(new Row(
                        terminals,
                        int.Parse(row[^5]),
                        bool.Parse(row[^4]),
                        bool.Parse(row[^3]),
                        bool.Parse(row[^2]),
                        bool.Parse(row[^1])
                        ));
        }
    }

    public Row MoveNext(bool containsTerminal)
    {
        var row = _table[CurrentStateNumber];

        if (containsTerminal)
        {
            if (row.Jump != -1)
            {
                if (row.Stack) _stateStack.Push(CurrentStateNumber + 1);
                CurrentStateNumber = row.Jump;
            }
            else if (row.Return) CurrentStateNumber = _stateStack.Pop();
        }
        else if (CurrentStateNumber < _table.Count - 1) CurrentStateNumber++;

        return CurrentStateNumber != -1 ? _table[CurrentStateNumber] : default;
    }
}

public record struct Row(string Terminals, int Jump, bool Accept, bool Stack, bool Return, bool Error);