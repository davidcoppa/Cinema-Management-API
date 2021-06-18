using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DTOs
{
    public class CinemaRoomCloserFilterDTO
    {
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
        private int distanceKM = 10;
        private int distanceMaxKM = 80;
        public int DistanceKM
        {
            get
            {
                return distanceKM;
            }
            set
            {
                distanceKM = (value > distanceMaxKM) ? distanceMaxKM : value;
            }
        }
    }
}
