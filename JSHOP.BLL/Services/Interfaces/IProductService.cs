using JSHOP.BLL.Common.Results;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task <Result<ProductResponse>> CreateProduct(ProductRequest request);
        Task<Result<List<ProductResponse>>> GetAllProducts();
        Task<Result<ProductResponse>> GetProduct(Expression<Func<Product, bool>> filter);
        Task<Result> DeleteProduct(int id);
    }
}
