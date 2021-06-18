
namespace Cinema.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        private int recordsPage = 10;
        private readonly int maxRecordsPage = 50;

        public int RegisterPerPage
        {
            get => recordsPage;
            set
            {
                recordsPage = (value > maxRecordsPage) ? maxRecordsPage : value;
            }
        }
    }
}
