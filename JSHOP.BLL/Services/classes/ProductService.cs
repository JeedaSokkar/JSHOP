using JSHOP.BLL.Common.Results;
using JSHOP.BLL.Services.Interfaces;
using JSHOP.DAL.Dto.Request;
using JSHOP.DAL.Dto.Response;
using JSHOP.DAL.Models;
using JSHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.BLL.Services.classes
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository = productRepository;
            _fileService = fileService;
        }


        public async Task<Result<ProductResponse>> CreateProduct(ProductRequest request)
        {
            if (request.MainImage is null || request.MainImage.Length == 0)
            {
                return Result<ProductResponse>.Failure(
                    new Error(
                        "MAIN_IMAGE_REQUIRED",
                        "Main image is required."
                    )
                );
            }

            var uploadResult = await _fileService.UploadAsync(request.MainImage);

            if (!uploadResult.IsSuccess)
            {
                return Result<ProductResponse>.Failure(
                    new Error(
                        "MAIN_IMAGE_UPLOAD_FAILED",
                        "Failed to upload main image."
                    )
                );
            }

            try
            {
                var product = request.Adapt<Product>();

                product.MainImage = uploadResult.Data!;

                await _productRepository.CreateAsync(product);

                return Result<ProductResponse>.Success(
                    product.Adapt<ProductResponse>()
                );
            }
            catch (Exception)
            {
                return Result<ProductResponse>.Failure(
                    new Error(
                        "PRODUCT_CREATE_FAILED",
                        "Failed to create product."
                    )
                );
            }
        }

        

        public Task<Result> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<ProductResponse>>> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProductResponse>> GetProduct(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}
