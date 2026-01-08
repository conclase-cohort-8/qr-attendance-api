using System;
using System.Collections.Generic;

namespace QrAttendanceApi.Application.Responses
{
    public class PagedResponse<T>
    {
        public IReadOnlyList<T> Data { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public PagedResponse(
            IReadOnlyList<T> data,
            int page,
            int pageSize,
            int totalCount)
        {
            Data = data ?? Array.Empty<T>();
            Page = page < 1 ? 1 : page;
            PageSize = pageSize < 1 ? 10 : pageSize;
            TotalCount = totalCount < 0 ? 0 : totalCount;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        }
    }
}
