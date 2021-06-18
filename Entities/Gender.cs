using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Entities
{
    public class Gender : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public List<MoviesByGender> MoviesByGender { get; set; }
    }
}
