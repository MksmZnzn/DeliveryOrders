namespace Persistence.Configuration;

public class FilePathProvider
{
    private readonly string _solutionRootPath;

    public FilePathProvider()
    {
        string baseDirectory = AppContext.BaseDirectory;
        _solutionRootPath = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName
                            ?? throw new InvalidOperationException(
                                "Не удалось определить путь до корневой папки решения");
    }

    public string GetOrdersFilePath()
    {
        return Path.Combine(_solutionRootPath, "orders.xml");
    }
}