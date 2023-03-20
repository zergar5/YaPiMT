namespace YaPiMT.Core;

public record struct Lexeme(string Name, DataType Type, bool IsInitialized)
{
    public Lexeme() : this(string.Empty, DataType.Undefined, default) { }

    public override string ToString()
    {
        return $"Name: {Name} Type: {Type} IsInitialized: {IsInitialized}";
    }
}