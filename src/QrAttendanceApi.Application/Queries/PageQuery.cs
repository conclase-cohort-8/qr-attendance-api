namespace QrAttendanceApi.Application.Queries
{
    public class PageQuery
    {
        private int _minPage = 1;
        private int _minPageSize = 5;
        private int _maxPageSize = 200;
        public int Page 
        { 
            get => _minPage;
            set
            {
                _minPage = value < _minPage ? _minPage : value;
            }
        }
        public int PageSize 
        { 
            get => _maxPageSize;
            set
            {
                _maxPageSize = value < _minPageSize ? _minPageSize : 
                    value > _maxPageSize ? _maxPageSize : value;
            }
        }
    }
}
