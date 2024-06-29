using Microsoft.AspNetCore.Http;
using Natural_Core.Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProduct();
        Task<ProductResponse> CreateProduct(Product product);
        Task<GetProduct> GetProductDetailsByIdAsync(string ProductId);
        Task<Product> GetProductByIdAsync(string ProductId);
        Task<GetProduct> GetProductpresignedurlByIdAsync(string ProductId);
        Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix);
        Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix);
        Task<IEnumerable<string>> GetAllBucketAsync();
        Task<IEnumerable<GetProduct>> GetAllPrtoductDetails(string? prefix, SearchProduct? search);
        Task<ProductResponse> UpadateProduct(Product product);
        Task<ProductResponse> DeleteImage(string Id);
        Task<ProductResponse> DeleteProduct(string ProductId, bool? deleteEntireProduct);
        Task<IEnumerable<ProductType>> GetAllProductType();
    }
}

