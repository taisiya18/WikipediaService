using System.Collections.Generic;
using System.Linq;

namespace project_2.Data
{
    public class MovieCatalog
    {
        public List<Movie> Movies { get; set; }

        public MovieCatalog()
        {
            Movies = new List<Movie>();
        }

        public void AddMovie(Movie movie)
        {
            Movies.Add(movie);
        }

        public void RemoveMovie(Movie movie)
        {
            Movies.Remove(movie);
        }

        public void UpdateRating(Movie movie, double newRating)
        {
            movie.Rating = newRating;
        }

        public List<Movie> GetRecommendations(List<string> favoriteGenres)
        {
            return Movies
                .Where(m => m.Genres.Any(g => favoriteGenres.Contains(g)))
                .OrderByDescending(m => m.Rating)
                .ToList();
        }

        public bool ContainsMovie(string title, int year)
        {
            return Movies.Any(m =>
                m.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && m.Year == year);
        }
    }
}
