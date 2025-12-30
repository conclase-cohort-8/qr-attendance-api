namespace QrAttendanceApi.Application.Abstractions.Externals
{
    public interface IUploadService
    {
        Task<(bool Success, string Url, string PublicId)> UploadImageAsync(string fileName, string originalFileName, Stream stream);
    }
}
