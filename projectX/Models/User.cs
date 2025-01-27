using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;

namespace projectX.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
