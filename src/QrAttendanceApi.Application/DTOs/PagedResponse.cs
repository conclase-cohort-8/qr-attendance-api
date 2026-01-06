namespace QrAttendanceApi.Application.DTOs
{
    public class PagedResponse<T>
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public List<T> Data { get; set; }

        public PagedResponse(List<T> data, int page, int size, int count)
        {
            Data = data;
            Page = page;
            PageCount = (int)Math.Ceiling((double)count / size);
            Total = count;
            HasPrevious = page > 1;
            HasNext = page < PageCount;
        }
    }
}
