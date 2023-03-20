using YaPiMT.Core.Errors;

namespace YaPiMT.IO;

public class ErrorIO
{
    private readonly string _path;

    public ErrorIO(string path)
    {
        _path = path;
    }

    public void WriteToFile(IError error, string fileName)
    {
        try
        {
            using var streamWriter = new StreamWriter(_path + fileName);

            streamWriter.WriteLine(error);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void WriteToFile(IError error, StreamWriter streamWriter)
    {
        try
        {
            streamWriter.WriteLine(error);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void WriteToFile(List<IError> errors, string fileName)
    {
        using var streamWriter = new StreamWriter(_path + fileName);

        foreach (var error in errors)
        {
            WriteToFile(error, streamWriter);
        }
    }
}