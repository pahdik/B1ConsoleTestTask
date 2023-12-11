namespace B1ConsoleTestTask;

// Класс для генерации файлов с случайными данными
public class FileGenerator
{
    private Random random = new Random();

    // Метод для генерации файла с указанным именем
    public void GenerateFile(string fileName)
    {
        try
        {
            using StreamWriter writer = new StreamWriter(fileName);
            for (int j = 1; j <= 100000; j++)
            {
                string line = GenerateRandomLine();
                writer.WriteLine(line);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating file '{fileName}': {ex.Message}");
        }
    }

    // Метод для генерации случайной строки
    private string GenerateRandomLine()
    {
        DateTime randomDate = DateTime.Now.AddYears(-random.Next(1, 6));
        string latinChars = GenerateRandomString(10);
        string russianChars = GenerateRandomString(10, true);
        int evenInteger = random.Next(1, 50000000) * 2;
        double randomDouble = random.NextDouble() * 19 + 1;

        return $"{randomDate:dd.MM.yyyy}||{latinChars}||{russianChars}||{evenInteger}||{randomDouble:F8}||";
    }

    // Метод для генерации случайной строки заданной длины с латинскими или русскими символами
    private string GenerateRandomString(int length, bool russianChars = false)
    {
        string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (russianChars)
            chars += "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
