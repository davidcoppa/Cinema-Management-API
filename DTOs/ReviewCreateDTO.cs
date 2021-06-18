using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DTOs
{
    public class ReviewCreateDTO
    {
        public string Comment { get; set; }
        [Range(1, 5)]
        public int Starts { get; set; }
    }
}
