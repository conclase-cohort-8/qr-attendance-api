using QrAttendanceApi.Application.Abstractions.Externals;

namespace QrAttendanceApi.Infrastructure.ExternalServices.Uploads
{
    public class UploadService : IUploadService
    {
        public Task<(bool Success, string Url, string PublicId)> UploadImageAsync(string fileName, 
                                                                                  string originalFileName, 
                                                                                  Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}