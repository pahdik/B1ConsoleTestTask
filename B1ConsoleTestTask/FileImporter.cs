using B1ConsoleTestTask.Data;
using B1ConsoleTestTask.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace B1ConsoleTestTask;

// Класс для импорта данных из файла в базу данных
public class FileImporter
{
    private readonly ApplicationDbContext dbContext;

    public FileImporter(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // Метод для импорта данных из файла в базу данных
    public void ImportFile(string fileName)
    {
        try
        {
            int totalLines = File.ReadLines(fileName).Count();
            int importedLines = 0;

            using (StreamReader reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    string[] values = reader.ReadLine().Split("||");
                    ImportRow(values);
                    importedLines++;
                    Console.WriteLine($"Imported: {importedLines} lines, Remaining: {totalLines - importedLines} lines");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing file '{fileName}': {ex.Message}");
        }
    }

    // Метод для импорта отдельной строки данных в базу данных
    private async Task ImportRow(string[] values)
    {
        try
        {
            if (DateTime.TryParse(values[0], out DateTime date) &&
                int.TryParse(values[3], out int evenInteger) &&
                double.TryParse(values[4], out double randomDouble))
            {
                Entity entity = new Entity
                {
                    Date = date,
                    LatinChars = values[1],
                    RussianChars = values[2],
                    EvenInteger = evenInteger,
                    RandomDouble = randomDouble
                };

                await dbContext.Entities.AddAsync(entity);
                
            }
            else
            {
                Console.WriteLine($"Invalid data format in file: {string.Join("||", values)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing row: {ex.Message}");
        }
    }
}
