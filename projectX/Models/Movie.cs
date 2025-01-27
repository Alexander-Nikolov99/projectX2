using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace projectX.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Duration { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public double Rating { get; set; }
        public ICollection<MovieActors>? MovieActors { get; set; }
        public ICollection<ScreeningMovies>? ScreeningMovies { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<MovieGenres>? MovieGenres { get; set; }
    }
}
