using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Entities
{
    public class Actor : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }
        public string Photo { get; set; }
        public List<MovieByActors> MovieByActors { get; set; }
    }
}
