namespace YaPiMT.IO;

public class ConstTableIO
{
    private readonly string _path;

    public ConstTableIO(string path)
    {
        _path = path;
    }

    public string[] ReadFromFile(string fileName)
    {
        try
        {
            using var streamReader = new StreamReader(_path + fileName);

            return streamReader.ReadToEnd().Split(' ');
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}