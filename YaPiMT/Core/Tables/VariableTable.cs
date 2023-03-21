namespace YaPiMT.Core.Tables;

public class VariableTable
{
    private readonly List<Lexeme> _table;

    public VariableTable()
    {
        _table = new List<Lexeme>();
    }

    public int AddLexeme(Lexeme lexeme)
    {
        if (!_table.Contains(lexeme)) _table.Add(lexeme);

        return _table.FindIndex(x => x == lexeme);
    }

    public int FindLexemeIndex(string name)
    {
        return _table.FindIndex(x => x.Name == name);
    }

    public Lexeme FindLexeme(int index)
    {
        return _table[index];
    }

    public Lexeme RemoveLexeme(int index)
    {
        var lexeme = _table[index];
        _table.RemoveAt(index);
        return lexeme;
    }

    public void SetLexemeType(int index, DataType type)
    {
        _table[index] = _table[index] with { Type = type };
    }

    public DataType GetLexemeType(int index)
    {
        return _table[index].Type;
    }

    public void SetLexemeIsInitialized(int index, bool isInitialized)
    {
        _table[index] = _table[index] with { IsInitialized = isInitialized };
    }

    public bool GetLexemeIsInitialized(int index)
    {
        return _table[index].IsInitialized;
    }

    public void PrintTable()
    {
        foreach (var lexeme in _table)
        {
            Console.WriteLine(lexeme.ToString());
        }
    }
}