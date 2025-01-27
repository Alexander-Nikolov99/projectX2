using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectX.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="Въведи правилна стойност")]
        public double Price { get; set; }

        [Required]
        [ForeignKey("Screening")]
        public int ScreeningId { get; set; }
        public Screening? Screening { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
