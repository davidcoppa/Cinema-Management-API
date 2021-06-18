namespace Cinema.DTOs
{
    public class MovieFilter
    {
        public int Page { get; set; } = 1;
        public int RegisterPerPage { get; set; } = 10;
        public PaginationDTO Pagination
        {
            get { return new PaginationDTO() { Page = Page, RegisterPerPage = RegisterPerPage }; }
        }

        public string Tittle { get; set; }
        public int GenderId { get; set; }
        public bool OnCinemas { get; set; }
        public bool OnComming { get; set; }
        public string OrderBy { get; set; }
        public bool OrderAsc { get; set; } = true;
    }
}
