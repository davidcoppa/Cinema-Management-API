using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DTOs
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }
    }
}
