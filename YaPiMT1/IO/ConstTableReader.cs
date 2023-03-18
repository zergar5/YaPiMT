namespace YaPiMT.IO;

public class ConstTableReader
{
    private readonly string _path;

    public ConstTableReader(string path)
    {
        _path = path;
    }

    public string[] ReadFromFile(string fileName)
    {
        try
        {
            using var streamReader = new StreamReader(_path + fileName);

            return streamReader.ReadToEnd().Split("\r\n");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}