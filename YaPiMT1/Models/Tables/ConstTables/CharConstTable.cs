using YaPiMT1.IO;

namespace YaPiMT1.Models.Tables.ConstTables;

public class CharConstTable : ConstTable<char>
{
    public CharConstTable(string fileName, ConstTableReader constTableReader)
    {
        TableElements = new SortedSet<char>();

        var elements = constTableReader.ReadFromFile(fileName);

        foreach (var element in elements)
        {
            TableElements.Add(Parse(element));
        }
    }
    protected sealed override char Parse(string element)
    {
        if (char.TryParse(element, out var result)) return result;
        throw new ArgumentException(nameof(element), "Can't parse string to char");
    }
}