using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Settings;

namespace QrAttendanceApi.Infrastructure.ExternalServices.Uploads
{
    public class UploadService : IUploadService
    {

        private readonly Cloudinary _cloudinary;
        public UploadService(IOptions<CloudinarySettings> options)
        {
            
            var settings = options.Value;
            _cloudinary = new Cloudinary(new Account(settings.CloudName, 
                                                     settings.ApiKey, 
                                                     settings.ApiSecret));

        }
        public async Task<(bool Success, string Url, string PublicId)> UploadImageAsync(string fileName, 
                                                                                  string originalFileName, 
                                                                                  Stream stream)
        {
           var param = new ImageUploadParams
           {
               File = new FileDescription(fileName, stream),
               PublicId = $"images/{fileName}",
               UniqueFilename = true,
               UseFilename = false,
               Transformation = new Transformation()
           };
            var response = await _cloudinary.UploadAsync(param);
            return response != null && response.StatusCode == System.Net.HttpStatusCode.OK ?
                 (true, response.SecureUrl.ToString(), response.PublicId) :
                 (false, string.Empty, string.Empty);
        }

        public async Task<bool> DeleteAsync(string publicId, ResourceType type)
        {
            var deleteParam = new DeletionParams(publicId)
            {
                ResourceType = type
            };

            var response = await _cloudinary.DestroyAsync(deleteParam);
            return response != null &&
                response.StatusCode == System.Net.HttpStatusCode.OK &&
                response.Result.ToLower() == "ok";
        }
    }
}