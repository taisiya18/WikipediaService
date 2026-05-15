using Spectre.Console;
using System;
using System.Linq;
using System.Threading.Tasks;
using project_2.Data;
using project_2.Services;

namespace project_2.UI
{
    public class ConsoleUI
    {
        private readonly MovieCatalog _catalog;
        private readonly WikipediaService _wikipediaService;

        public ConsoleUI(MovieCatalog catalog, WikipediaService wikipediaService)
        {
            _catalog = catalog;
            _wikipediaService = wikipediaService;
        }

        public void DisplayMenu()
        {
            Console.WriteLine("\nМеню каталога фильмов:");
            Console.WriteLine("1. Просмотреть все фильмы");
            Console.WriteLine("2. Добавить фильм");
            Console.WriteLine("3. Редактировать рейтинг фильма");
            Console.WriteLine("4. Удалить фильм");
            Console.WriteLine("5. Показать фильмы в таблице");
            Console.WriteLine("6. Показать диаграмму по жанрам");
            Console.WriteLine("7. Получить рекомендации");
            Console.WriteLine("8. Добавить фильм из Wikipedia");
            Console.WriteLine("9. Выйти и сохранить");
            Console.Write("Выберите опцию: ");
        }

        public async Task Run()
        {
            while (true)
            {
                DisplayMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewMovies();
                        break;
                    case "2":
                        AddMovie();
                        break;
                    case "3":
                        EditRating();
                        break;
                    case "4":
                        DeleteMovie();
                        break;
                    case "5":
                        ShowMoviesTable();
                        break;
                    case "6":
                        ShowGenresChart();
                        break;
                    case "7":
                        ShowRecommendations();
                        break;
                    case "8":
                        AddMovieFromWikipedia();
                        break;
                    case "9":
                        return;
                    default:
                        Console.WriteLine("Неверная опция. Попробуйте снова.");
                        break;
                }
            }
        }

        private void ViewMovies()
        {
            if (_catalog.Movies.Any())
            {
                Console.WriteLine("\nКаталог фильмов:");
                Console.WriteLine("--------------------------------------------------");
                foreach (var movie in _catalog.Movies)
                {
                    Console.WriteLine(movie);
                    Console.WriteLine("--------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("Каталог фильмов пуст.");
            }
        }

        private void ShowMoviesTable()
        {
            var table = new Table();
            table.AddColumn("Название");
            table.AddColumn("Жанры");
            table.AddColumn("Год");
            table.AddColumn("Рейтинг");

            foreach (var movie in _catalog.Movies)
            {
                table.AddRow(
                    movie.Title,
                    string.Join(", ", movie.Genres),
                    movie.Year.ToString(),
                    movie.Rating.ToString()
                );
            }

            AnsiConsole.Write(table);
        }

        private void ShowGenresChart()
        {
            var genresCount = _catalog.Movies
                .SelectMany(m => m.Genres)
                .GroupBy(g => g)
                .ToDictionary(g => g.Key, g => g.Count());

            var chart = new BarChart()
                .Width(60)
                .Label("[green]Распределение фильмов по жанрам[/]");

            foreach (var genre in genresCount)
            {
                chart.AddItem(genre.Key, genre.Value, Color.Blue);
            }

            AnsiConsole.Write(chart);
        }

        private void ShowRecommendations()
        {
            Console.Write("Введите ваши любимые жанры (через запятую): ");
            var favoriteGenres = StringHelper.NormalizeString(Console.ReadLine()).Split(',').ToList();

            var recommendations = _catalog.GetRecommendations(favoriteGenres);

            if (recommendations.Any())
            {
                Console.WriteLine("\nРекомендации:");
                foreach (var movie in recommendations)
                {
                    Console.WriteLine(movie);
                    Console.WriteLine("--------------------------------------------------");
                }
            }
            else
            {
                Console.WriteLine("Нет рекомендаций по выбранным жанрам.");
            }
        }

 private void AddMovieFromWikipedia()
{
    Console.Write("Введите название фильма: ");
    var title = StringHelper.NormalizeString(Console.ReadLine());

    var (movieTitle, genre, year, rating) = _wikipediaService.GetMovieInfo(title);

    if (!string.IsNullOrEmpty(movieTitle))
    {
        Console.WriteLine("\nНайдена информация:");
        Console.WriteLine($"Название: {movieTitle}");
        Console.WriteLine($"Жанр: {genre}");
        Console.WriteLine($"Год: {year}");
        Console.WriteLine($"Рейтинг: {rating}");

        Console.Write("\nДобавить этот фильм в каталог? (y/n): ");
        var choice = Console.ReadLine();

        if (choice.Equals("y", StringComparison.OrdinalIgnoreCase))
        {
            if (genre == "Неизвестно")
            {
                Console.Write("Введите жанр (через запятую, если несколько): ");
                genre = StringHelper.NormalizeString(Console.ReadLine());
            }

            if (year == 0)
            {
                Console.Write("Введите год выпуска: ");
                int.TryParse(Console.ReadLine(), out year);
            }

            if (rating == 0)
            {
                Console.Write("Введите рейтинг (от 1 до 10): ");
                double.TryParse(Console.ReadLine(), out rating);
            }

            var genres = genre.Split(',').Select(g => g.Trim()).ToList();
            _catalog.AddMovie(new Movie(movieTitle, genres, year, rating));
            Console.WriteLine("Фильм добавлен.");
        }
    }
    else
    {
        Console.WriteLine("Фильм не найден в Wikipedia.");
    }
}
        private void AddMovie()
        {
            Console.Write("Введите название фильма: ");
            var title = StringHelper.NormalizeString(Console.ReadLine());
            Console.Write("Введите жанры (через запятую): ");
            var genres = StringHelper.NormalizeString(Console.ReadLine()).Split(',').ToList();
            Console.Write("Введите год выпуска: ");
            var year = int.Parse(Console.ReadLine());
            Console.Write("Введите рейтинг (от 1 до 10): ");
            var rating = double.Parse(Console.ReadLine());

            _catalog.AddMovie(new Movie(title, genres, year, rating));
            Console.WriteLine("Фильм добавлен.");
        }

        private void EditRating()
        {
            Console.Write("Введите название фильма для редактирования рейтинга: ");
            var title = StringHelper.NormalizeString(Console.ReadLine());
            var movie = _catalog.Movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (movie != null)
            {
                Console.Write("Введите новый рейтинг (от 1 до 10): ");
                var newRating = double.Parse(Console.ReadLine());
                _catalog.UpdateRating(movie, newRating);
                Console.WriteLine("Рейтинг обновлен.");
            }
            else
            {
                Console.WriteLine("Фильм не найден.");
            }
        }

        private void DeleteMovie()
        {
            Console.Write("Введите название фильма для удаления: ");
            var title = StringHelper.NormalizeString(Console.ReadLine());
            var movie = _catalog.Movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (movie != null)
            {
                _catalog.RemoveMovie(movie);
                Console.WriteLine("Фильм удален.");
            }
            else
            {
                Console.WriteLine("Фильм не найден.");
            }
        }
    }
}
