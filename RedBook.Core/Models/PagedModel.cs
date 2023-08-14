namespace RedBook.Core.Models
{
    public class PagedModel<T> where T : class
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public IEnumerable<T>? SourceData { get; set; }

        private int pageNumber { get; set; }
        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = value is <= 0 ? 0 : value;
        }

        private int pageSize { get; set; }
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value is <= 0 ? 10 : value;
        }

        public int Skip
        {
            get { return pageSize * (pageNumber - 1); }
            private set { }
        }
    }
}
