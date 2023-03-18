using YaPiMT.IO;

namespace YaPiMT.Models.Tables.ConstTables;

public class StringConstTable : ConstTable<string>
{
    public StringConstTable(string fileName, ConstTableReader constTableReader)
        : base(fileName, constTableReader) { }

    protected sealed override string Parse(string element)
    {
        return element;
    }
}