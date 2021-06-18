using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DTOs
{
    public class CinemaRoomCreate
    {
        [Required]
        [StringLength(120)]
        public string name { get; set; }
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}
