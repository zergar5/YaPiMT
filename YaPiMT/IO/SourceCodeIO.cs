namespace YaPiMT.IO;

public class SourceCodeIO
{
    private readonly string _path;

    public SourceCodeIO(string path)
    {
        _path = path;
    }

    public string ReadFromFile(string fileName)
    {
        try
        {
            using var streamReader = new StreamReader(_path + fileName);

            return streamReader.ReadToEnd();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}