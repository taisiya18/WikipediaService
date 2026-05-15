using System;
using System.IO;
using project_2.Data;
using project_2.Services;
using project_2.UI;

namespace project_2
{
    class Program
    {
        static void Main(string[] args)
        {
            var catalog = new MovieCatalog();
            var fileService = new FileService();
            var wikipediaService = new WikipediaService();

            string filePath = GetFilePathFromUser();

            if (filePath == null)
            {
                Console.WriteLine("Выход из программы.");
                return;
            }

            if (File.Exists(filePath))
            {
                catalog.Movies = fileService.LoadMovies(filePath);
                Console.WriteLine("Фильмы успешно загружены.");
            }
            else
            {
                Console.WriteLine("Файл не найден. Начинаем с пустого каталога.");
            }

            var consoleUI = new ConsoleUI(catalog, wikipediaService);
            consoleUI.Run();

            fileService.SaveMovies(filePath, catalog.Movies);
            Console.WriteLine("Изменения сохранены. Выход из программы.");
        }

        static string GetFilePathFromUser()
        {
            Console.Write("Лежит ли файл movies.txt в папке проекта? (y/n): ");
            var response = Console.ReadLine();

            string filePath;

            if (response.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), "movies.txt");
                Console.WriteLine($"Ищем файл по пути: {filePath}");
            }
            else
            {
                while (true)
                {
                    Console.Write("Введите полный путь к файлу movies.txt: ");
                    filePath = Console.ReadLine();

                    if (File.Exists(filePath))
                    {
                        break; // Файл найден
                    }

                    Console.WriteLine("Файл не найден.");
                    Console.Write("Хотите ввести путь заново? (y/n): ");
                    var retryResponse = Console.ReadLine();

                    if (!retryResponse.Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
            }

            return filePath;
        }
    }
}