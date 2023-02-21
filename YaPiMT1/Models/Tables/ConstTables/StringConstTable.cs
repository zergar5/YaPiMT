using YaPiMT1.IO;

namespace YaPiMT1.Models.Tables.ConstTables;

public class StringConstTable : ConstTable<string>
{
    public StringConstTable(string fileName, ConstTableReader constTableReader)
    {
        TableElements = new SortedSet<string>();

        var elements = constTableReader.ReadFromFile(fileName);

        foreach (var element in elements)
        {
            TableElements.Add(element);
        }
    }

    protected sealed override string Parse(string element)
    {
        return element;
    }
}