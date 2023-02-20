using System.Collections;

namespace YaPiMT1.Models.Tables;

public class VariableTable
{
    private HashSet<Lexeme> _tableElements;

    public VariableTable()
    {
        _tableElements = new HashSet<Lexeme>();
    }

    public void AddLexeme(Lexeme lexeme)
    {
        if(_tableElements.Contains(lexeme)) throw new ArgumentException(nameof(lexeme), "Lexeme already exists");
        _tableElements.Add(lexeme);
    }

    public Lexeme FindLexeme(string name)
    {
        var lexeme = _tableElements.FirstOrDefault(x => x.Name == name);
        return lexeme == default ? default : lexeme;
    }

    public Lexeme RemoveLexeme(string name)
    {
        var lexeme = _tableElements.FirstOrDefault(x => x.Name == name);
        if (lexeme == default) throw new ArgumentNullException(nameof(lexeme), "Lexeme doesn't exist");
        _tableElements.Remove(lexeme);
        return lexeme;
    }

    public void SetLexemeType(string name, DataType type)
    {
        var lexeme = RemoveLexeme(name);
        lexeme.Type = type;
        AddLexeme(lexeme);
    }

    public void SetLexemeValue(string name, string value)
    {
        var lexeme = RemoveLexeme(name);
        lexeme.Value = value;
        AddLexeme(lexeme);
    }
}