using WikiDotNet;
using System;
using System.Text.RegularExpressions;

namespace project_2.Services
{
    public class WikipediaService
    {
        public (string Title, string Genre, int Year, double Rating) GetMovieInfo(string title)
        {
            var searcher = new WikiSearcher();
            var searchSettings = new WikiSearchSettings
            {
                RequestId = "Request ID",
                ResultLimit = 5,
                ResultOffset = 0,
                Language = "ru"
            };

            var query = $"{title} фильм";
            var response = searcher.Search(query, searchSettings);

            var searchResults = response.Query.SearchResults;

            if (searchResults.Length > 0)
            {
                var result = searchResults[0];

                Console.WriteLine("Полный текст из первичного поиска:");
                Console.WriteLine($"Название: {result.Title}");
                Console.WriteLine($"Превью: {result.Preview}");
                Console.WriteLine($"Слов: {result.WordCount}, Размер: {result.Size} байт");
                Console.WriteLine($"ID страницы: {result.PageId}");
                Console.WriteLine($"URL: {result.Url.AbsoluteUri}");
                Console.WriteLine($"Постоянный URL: {result.ConstantUrl.AbsoluteUri}");
                Console.WriteLine($"Последнее редактирование: {result.LastEdited}");
                Console.WriteLine();

                var (genre, year) = ExtractGenreAndYear(result.Preview);
                return (result.Title, genre, year, 0);
            }

            return (null, null, 0, 0);
        }

        private (string Genre, int Year) ExtractGenreAndYear(string preview)
        {
            string genre = "Неизвестно";
            int year = 0;

            var yearMatch = Regex.Match(preview, @"\b\d{4}\b");
            if (yearMatch.Success)
            {
                int.TryParse(yearMatch.Value, out year);
            }

            var genreKeywords = new[] 
            { 
                "комедия", "фантастика", "драма", "ужасы", "боевик", 
                "триллер", "мультфильм", "аниме", "мелодрама", "приключения", 
                "фэнтези", "детектив", "биография", "история", "военный" 
            };

            foreach (var keyword in genreKeywords)
            {
                if (preview.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    genre = keyword;
                    break;
                }
            }

            return (genre, year);
        }

        public string NormalizeTitle(string title)
        {
            var normalizedTitle = Regex.Replace(title, @"\s*\([^)]*\)", "");
            return normalizedTitle.Trim();
        }
    }
}