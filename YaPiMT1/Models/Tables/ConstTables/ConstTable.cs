using YaPiMT1.IO;

namespace YaPiMT1.Models.Tables.ConstTables;

public abstract class ConstTable<T>
{
    protected List<T> Table;

    protected ConstTable(string fileName, ConstTableReader constTableReader)
    {
        Table = new List<T>();

        var elements = constTableReader.ReadFromFile(fileName);

        foreach (var element in elements)
            Table.Add(Parse(element));
    }

    protected abstract T Parse(string element);

    public T FindElement(int index)
    {
        return Table[1];
    }

    public void PrintTable()
    {
        var i = 0;

        foreach (var tableElement in Table)
        {
            Console.WriteLine($"{i}: {tableElement}");
            i++;
        }
    }
}