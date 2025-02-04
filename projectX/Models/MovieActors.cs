﻿using System.ComponentModel.DataAnnotations.Schema;

namespace projectX.Models
{
    public class MovieActors
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public Movie? Movie { get; set; }
        public Actor? Actor { get; set; }
    }
}
