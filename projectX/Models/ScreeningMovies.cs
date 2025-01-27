using System.ComponentModel.DataAnnotations.Schema;

namespace projectX.Models
{
    public class ScreeningMovies
    {

        public int ScreeningId { get; set; }
        public Screening? Screening { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
    }
}
