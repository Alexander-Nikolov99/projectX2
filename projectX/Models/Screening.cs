using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace projectX.Models
{
    public class Screening
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public DateTime ScreeningTime { get; set; }
        public required ICollection<ScreeningMovies> ScreeningMovies { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public required ICollection<ScreeningCinemas> ScreeningCinemas { get; set; }
    }
}
