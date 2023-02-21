namespace YaPiMT1.Models.Tables.ConstTables;

public abstract class ConstTable<T>
{
    protected SortedSet<T> TableElements;

    protected abstract T Parse(string element);

    public virtual T FindElement(T element)
    {
        return TableElements.TryGetValue(element, out var item) ? item : default;
    }

    public void PrintTable()
    {
        var i = 0;

        foreach (var tableElement in TableElements)
        {
            Console.WriteLine($"{i}: {tableElement}");
            i++;
        }
    }
}