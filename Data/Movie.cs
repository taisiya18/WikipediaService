namespace project_2.Data
{
    public class Movie
    {
        public string Title { get; set; }
        public List<string> Genres { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }

        public Movie(string title, List<string> genres, int year, double rating)
        {
            Title = title;
            Genres = genres;
            Year = year;
            Rating = rating;
        }

        public override string ToString()
        {
            return $"Название: {Title}\nЖанр: {string.Join(", ", Genres)}\nГод выпуска: {Year}\nРейтинг: {Rating}";
        }
    }
}
