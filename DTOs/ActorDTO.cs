using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DataBirth { get; set; }
        public string Photo { get; set; }
    }
}
