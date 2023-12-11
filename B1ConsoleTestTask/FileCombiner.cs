namespace B1ConsoleTestTask;

// Класс для объединения файлов и удаления строк с указанным подстрокой
public class FileCombiner
{
    // Метод для объединения файлов из указанной папки
    // Возвращает количество удаленных строк
    public int CombineFiles(string folderPath, StreamWriter combinedWriter, string substringToRemove)
    {
        int deletedLinesCount = 0;

        try
        {
            for (int i = 1; i <= 100; i++)
            {
                string fileName = $"{folderPath}file_{i}.txt";
                deletedLinesCount += RemoveLinesWithSubstring(fileName, combinedWriter, substringToRemove);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error combining files: {ex.Message}");
        }

        return deletedLinesCount;
    }

    // Метод для удаления строк с указанной подстрокой из файла
    // Возвращает количество удаленных строк
    private int RemoveLinesWithSubstring(string sourceFileName, StreamWriter combinedWriter, string substringToRemove)
    {
        int deletedLinesCount = 0;

        try
        {
            using (StreamReader reader = new StreamReader(sourceFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.Contains(substringToRemove) && !string.IsNullOrEmpty(substringToRemove))
                    {
                        combinedWriter.WriteLine(line);
                    }
                    else
                    {
                        deletedLinesCount++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing lines from file '{sourceFileName}': {ex.Message}");
        }

        return deletedLinesCount;
    }
}
