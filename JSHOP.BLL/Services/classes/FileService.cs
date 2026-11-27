using JSHOP.BLL.Common.Results;
using JSHOP.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace JSHOP.BLL.Services.Classes
{
    public class FileService : IFileService
    {
        private readonly string[] _allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public async Task<Result<string>> UploadAsync(IFormFile file)
        {
            try
            {
                if (file is null || file.Length == 0)
                {
                    return Result<string>.Failure(
                        new Error("INVALID_FILE", "No file was provided.")
                    );
                }

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_allowedExtensions.Contains(extension))
                {
                    return Result<string>.Failure(
                        new Error("INVALID_FILE_TYPE", "This file type is not allowed.")
                    );
                }

                if (file.Length > MaxFileSize)
                {
                    return Result<string>.Failure(
                        new Error("FILE_TOO_LARGE", "File size exceeds the 5MB limit.")
                    );
                }

                var fileName = Guid.NewGuid().ToString() + extension;

                var uploadPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Uploads"
                );

                Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Result<string>.Success(fileName);
            }
            catch (Exception)
            {
                return Result<string>.Failure(
                    new Error("FILE_UPLOAD_FAILED", "An error occurred while uploading the file.")
                );
            }
        }
    }
}