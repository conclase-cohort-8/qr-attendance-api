using CloudinaryDotNet.Actions;

namespace QrAttendanceApi.Application.Abstractions.Externals
{
    public interface IUploadService
    {
     
        Task<bool> DeleteAsync(string publicId, ResourceType type);
        Task<(bool Success, string Url, string PublicId)> UploadImageAsync(string fileName, string originalFileName, Stream stream);
    }
}
