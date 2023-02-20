namespace YaPiMT1.Models;

public record struct Lexeme(string Name, DataType Type, string Value)
{
    public Lexeme() : this(string.Empty, DataType.Undefined, string.Empty) { }
}