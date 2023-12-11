using B1ConsoleTestTask.Data;
using System.IO;

namespace B1ConsoleTestTask;

public class Program
{
    private const string HintMessage = "Enter your command, or enter 'help' to get help.";

    private const int CommandHelpIndex = 0;
    private const int DescriptionHelpIndex = 1;
    private const int ExplanationHelpIndex = 2;

    private static string FolderPath = Directory.GetCurrentDirectory();

    private static bool isRunning = true;

    // Массив команд, где каждая команда представлена кортежем (название, метод)
    private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
    {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("generate", GenerateFiles),
            new Tuple<string, Action<string>>("setFolderPath", SetFolderPath),
            new Tuple<string, Action<string>>("combine", CombineFiles),
            new Tuple<string, Action<string>>("import", ImportFileToDatabase),
    };

    // Массив с описаниями команд для вывода подсказок
    private static string[][] helpMessages = new string[][]
    {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "generate", "generates files", "The 'generate' command generates 100 random files." },
            new string[] { "setFolderPath", "sets new folder", "The 'setFolderPath' command sets new folder." },
            new string[] { "combine", "combines all files and delete all records with substring", "The 'combine' combines all files and delete all records with substring." },
            new string[] { "import", "imports data to database", "The 'setFolderPath' command sets new folder." }
    };

    static void Main(string[] args)
    {
        Console.WriteLine(Program.HintMessage);
        Console.WriteLine();

        do
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            var inputs = line != null ? line.Split(' ', 2) : new string[] { string.Empty, string.Empty };
            const int commandIndex = 0;
            var command = inputs[commandIndex];

            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine(Program.HintMessage);
                continue;
            }

            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commands[index].Item2(parameters);
            }
            else
            {
                PrintMissedCommandInfo(command);
            }
        }
        while (isRunning);
    }

    // Метод для вывода сообщения о неизвестной команде
    private static void PrintMissedCommandInfo(string command)
    {
        Console.WriteLine($"There is no '{command}' command.");
        Console.WriteLine();
    }

    // Метод для вывода справки по командам
    private static void PrintHelp(string parameters)
    {
        if (!string.IsNullOrEmpty(parameters))
        {
            var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
            }
            else
            {
                Console.WriteLine($"There is no explanation for '{parameters}' command.");
            }
        }
        else
        {
            Console.WriteLine("Available commands:");

            foreach (var helpMessage in helpMessages)
            {
                Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
            }
        }

        Console.WriteLine();
    }

    // Метод для завершения приложения
    private static void Exit(string parameters)
    {
        Console.WriteLine("Exiting an application...");
        isRunning = false;
    }

    // Метод для генерации файлов
    private static void GenerateFiles(string parameters)
    {
        var fileGenerator = new FileGenerator();
        for(int i  = 1; i <= 100; i++)
        {
            fileGenerator.GenerateFile($"{FolderPath}file_{i}.txt");
        }
    }

    // Метод для изменения пути к папке
    private static void SetFolderPath(string parameters)
    {
        if (Path.IsPathRooted(parameters))
        {
            FolderPath = parameters;
        }
        else
        {
            Console.WriteLine("Folder path is incorrected. Try again...");
        }
    }

    // Метод для объединения файлов и удаления записей с подстрокой
    private static void CombineFiles(string parameters)
    {
        var fileCombiner = new FileCombiner();

        using StreamWriter writer = new StreamWriter($"{FolderPath}combinedFile.txt");
        var deletedRecords = fileCombiner.CombineFiles(FolderPath, writer, parameters);
        Console.WriteLine($"{deletedRecords} lines were deleted");
    }

    // Метод для импорта данных из объединенного файла в базу данных
    private static void ImportFileToDatabase(string parameters)
    {
        using ApplicationDbContext context= new ApplicationDbContext();
        context.Database.EnsureCreated();
        var fileImporter = new FileImporter(context);
        fileImporter.ImportFile($"{FolderPath}combinedFile.txt");   
    }
}