using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Natural_Core.Models;
using Natural_Core.S3Models;

namespace Natural_Core.IRepositories
{
	public interface IProductRepository : IRepository<Product>
	{

        Task<IEnumerable<string>> GetAllBucketAsync();
        Task<List<Product>> GetProducttAsync();
        
        Task<GetProduct> GetProductByIdAsync(string ProductId);
        Task<IEnumerable<Natural_Core.S3Models.S3Config>> GetAllFilesAsync(string bucketName, string? prefix);

        Task<UploadResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix);

        Task<bool> DeleteImageAsync(string bucketName, string key);
    }
}

