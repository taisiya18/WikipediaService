using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using project_2.Data;

namespace project_2.Services
{
    public class FileService
    {
        public List<Movie> LoadMovies(string filePath)
        {
            var movies = new List<Movie>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 4)
                    {
                        var title = StringHelper.NormalizeString(parts[0]);
                        var genres = StringHelper.NormalizeString(parts[1]).Split(',').ToList();
                        var year = int.Parse(parts[2]);
                        var rating = double.Parse(parts[3]);
                        movies.Add(new Movie(title, genres, year, rating));
                    }
                }
            }
            return movies;
        }

        public void SaveMovies(string filePath, List<Movie> movies)
        {
            var lines = movies.Select(m => $"{m.Title}|{string.Join(",", m.Genres)}|{m.Year}|{m.Rating}");
            File.WriteAllLines(filePath, lines);
        }
    }
}