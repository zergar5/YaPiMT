namespace YaPiMT1.Models;

public record struct Lexeme(string Name, DataType Type, bool IsInitialized)
{
    public Lexeme() : this(string.Empty, DataType.Undefined, default) { }
}