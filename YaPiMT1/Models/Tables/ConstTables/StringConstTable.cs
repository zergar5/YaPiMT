using YaPiMT1.IO;

namespace YaPiMT1.Models.Tables.ConstTables;

public class StringConstTable : ConstTable<string>
{
    public StringConstTable(string fileName, ConstTableReader constTableReader)
        : base(fileName, constTableReader)
        { }

    protected sealed override string Parse(string element)
    {
        return element;
    }
}