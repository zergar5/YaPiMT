using YaPiMT.IO;

namespace YaPiMT.Core.Tables.ConstTables;

public class StringConstTable : ConstTable<string>
{
    public StringConstTable(string fileName, ConstTableIO constTableReader)
        : base(fileName, constTableReader) { }

    protected sealed override string Parse(string element)
    {
        return element;
    }
}