using JSHOP.BLL.Common.Results;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using System.Linq.Expressions;


namespace JSHOP.BLL.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<Result<List<CategoryResponse>>> GetAllCategories();
        Task<Result<CategoryResponse>> CreateCategory(CategoryRequest request);
        Task<Result<CategoryResponse>> GetCategory(Expression<Func<Category, bool>> filter);
        Task<Result> DeleteCategory(int id);
    }
}
