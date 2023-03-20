using YaPiMT.Core;

namespace YaPiMT.IO;

public class TokenIO
{
    private readonly string _path;

    public TokenIO(string path)
    {
        _path = path;
    }

    public void WriteToFile(Token token, string fileName)
    {
        try
        {
            using var streamWriter = new StreamWriter(_path + fileName);

            streamWriter.WriteLine(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void WriteToFile(Token token, StreamWriter streamWriter)
    {
        try
        {
            streamWriter.WriteLine(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void WriteToFile(List<Token> tokens, string fileName)
    {
        using var streamWriter = new StreamWriter(_path + fileName);

        foreach (var token in tokens)
        {
            WriteToFile(token, streamWriter);
        }
    }
}