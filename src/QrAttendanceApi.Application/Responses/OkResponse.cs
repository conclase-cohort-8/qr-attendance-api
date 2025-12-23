namespace QrAttendanceApi.Application.Responses
{
    public class OkResponse<T> : ApiBaseResponse
    {
        public T? Data { get; set; }

        public OkResponse(T data)
            : base(true, 200)
        {
            Data = data;
        }
    }
}