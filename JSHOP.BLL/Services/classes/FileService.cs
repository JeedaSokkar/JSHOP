using JSHOP.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services.classes
{
    public class FileService : IFileService
    {
        public async Task<string?> UploadAsync(IFormFile file)
        {
            if (file is not null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(file.FileName);

                var uploadPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "Uploads"
                );

                Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }

            return null;
        }
      
    }
}
