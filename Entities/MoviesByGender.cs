namespace Cinema.Entities
{
    public class MoviesByGender
    {
        public int GenderId { get; set; }
        public int MovieId { get; set; }
        public Gender Gender { get; set; }
        public Movie movie { get; set; }
    }
}
