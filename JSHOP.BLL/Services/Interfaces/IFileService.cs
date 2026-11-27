using JSHOP.BLL.Common.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<Result<string>> UploadAsync(IFormFile file);
    }
}
