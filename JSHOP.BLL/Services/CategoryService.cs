using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using JSHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService (ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        async Task<CategoryResponse> ICategoryService.CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>()  ;  
        }

        async Task<List<CategoryResponse>> ICategoryService.GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync(new string[] { nameof(Category.Translations)
            });
            return categories.Adapt<List<CategoryResponse>>();
        }
        async Task<CategoryResponse> ICategoryService.GetCategory(Expression<Func<Category, bool>> filter)
        {
            var category = await _categoryRepository.GetOne(filter, new string[] { nameof(Category.Translations) });
            return category.Adapt<CategoryResponse>();
        }
        
           
        
    }
}
