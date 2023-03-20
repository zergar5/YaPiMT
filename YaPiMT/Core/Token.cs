namespace YaPiMT.Core;

public readonly record struct Token(string TableName, string Name, int IndexInTable)
{
    public Token(string TableName, char Name, int IndexInTable) : this(TableName, Name.ToString(), IndexInTable) { }

    public override string ToString()
    {
        return $"{TableName} {Name} {IndexInTable}";
    }
}