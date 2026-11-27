
using JSHOP.BLL.Common.Results;
using JSHOP.BLL.Services.Interfaces;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using JSHOP.DAL.Repository;
using Mapster;
using System.Linq.Expressions;

namespace JSHOP.BLL.Services.Classes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CategoryResponse>> CreateCategory(CategoryRequest request)
        {
            try
            {
                var category = request.Adapt<Category>();

                await _categoryRepository.CreateAsync(category);

                var response = category.Adapt<CategoryResponse>();

                return Result<CategoryResponse>.Success(response);
            }
            catch (Exception)
            {
                return Result<CategoryResponse>.Failure(
                    new Error(
                        "CATEGORY_CREATE_FAILED",
                        "An error occurred while creating the category."
                    )
                );
            }
        }

        public async Task<Result<List<CategoryResponse>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync(
                    new string[]
                    {
                        nameof(Category.Translations),
                        nameof(Category.CreatedBy)
                    }
                );

                var response = categories.Adapt<List<CategoryResponse>>();

                return Result<List<CategoryResponse>>.Success(response);
            }
            catch (Exception)
            {
                return Result<List<CategoryResponse>>.Failure(
                    new Error(
                        "CATEGORY_RETRIEVAL_FAILED",
                        "An error occurred while retrieving categories."
                    )
                );
            }
        }

        public async Task<Result<CategoryResponse>> GetCategory(
            Expression<Func<Category, bool>> filter)
        {
            try
            {
                var category = await _categoryRepository.GetOne(
                    filter,
                    new string[] { nameof(Category.Translations) }
                );

                if (category == null)
                {
                    return Result<CategoryResponse>.Failure(
                        new Error(
                            "CATEGORY_NOT_FOUND",
                            "The requested category was not found."
                        )
                    );
                }

                var response = category.Adapt<CategoryResponse>();

                return Result<CategoryResponse>.Success(response);
            }
            catch (Exception)
            {
                return Result<CategoryResponse>.Failure(
                    new Error(
                        "CATEGORY_RETRIEVAL_FAILED",
                        "An error occurred while retrieving the category."
                    )
                );
            }
        }

        public async Task<Result> DeleteCategory(int id)
        {
            try
            {
                var category = await _categoryRepository.GetOne(
                    c => c.Id == id
                );

                if (category == null)
                {
                    return Result.Failure(
                        new Error(
                            "CATEGORY_NOT_FOUND",
                            "The requested category was not found."
                        )
                    );
                }

                var isDeleted = await _categoryRepository.DeleteAsync(category);

                if (!isDeleted)
                {
                    return Result.Failure(
                        new Error(
                            "CATEGORY_DELETE_FAILED",
                            "The category could not be deleted."
                        )
                    );
                }

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Failure(
                    new Error(
                        "CATEGORY_DELETE_FAILED",
                        "An error occurred while deleting the category."
                    )
                );
            }
        }
    }
}
