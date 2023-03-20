using YaPiMT.IO;
using YaPiMT.Models.Tables.ConstTables;

namespace YaPiMT.Core.Tables.ConstTables;

public class CharConstTable : ConstTable<char>
{
    public CharConstTable(string fileName, ConstTableIO constTableReader)
        : base(fileName, constTableReader) { }

    protected sealed override char Parse(string element)
    {
        if (char.TryParse(element, out var result)) return result;
        throw new ArgumentException(nameof(element), "Can't parse string to char");
    }
}