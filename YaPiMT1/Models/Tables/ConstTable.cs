namespace YaPiMT1.Models.Tables;

public class ConstTable
{
    private readonly SortedSet<string> _tableElements;

    public ConstTable()
    {
        _tableElements = new SortedSet<string>();
    }

    public ConstTable(string fileName)
    {
        _tableElements = new SortedSet<string>();

        using var streamReader = new StreamReader(fileName);

        var elements = streamReader.ReadToEnd().Split("\r\n");

        foreach (var name in elements)
        {
            _tableElements.Add(name);
        }
    }

    public string FindElement(string element)
    {
        return _tableElements.TryGetValue(element, out var item) ? item : default;
    }

    public void PrintTable()
    {
        var i = 0;

        foreach (var tableElement in _tableElements)
        {
            Console.WriteLine($"{i}: {tableElement}");
            i++;
        }
    }
}