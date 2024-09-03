using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace Natural_Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IAmazonS3 _S3Client;

        public ProductRepository(NaturalsContext context, IAmazonS3 S3Client) : base(context)
        {
            _S3Client = S3Client;
        }

        //get products with category name 
        public async Task<List<GetProduct>> GetProducttAsync()
        {

            var Products = await NaturalDbContext.Products
           .Include(p => p.CategoryNavigation)
           .Include(p=>p.ProductTypeNavigation)
           .Where(c => c.IsDeleted != true)
           .ToListAsync();
         
            var result = Products.Select(p => new GetProduct
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Quantity = p.Quantity,
                Price = p.Price,
                DisplayPrice = p.DisplayPrice,
                Image = p.Image,
                Weight = p.Weight,
                ProductType = p.ProductTypeNavigation?.ProductTypeName,
                ProductTypeCode = p.ProductTypeNavigation?.ProductTypeCode,
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate,
                Category = p.CategoryNavigation?.CategoryName
            }).ToList();

            return result;
        }
        public async Task<List<ProductType>> GetProductType()
        {

            return await NaturalDbContext.ProductTypes.ToListAsync();
           
          }

        //Getbucket name
        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            var data = await _S3Client.ListBucketsAsync();
            var buckets = data.Buckets.Select(b => { return b.BucketName; });

            return buckets;
        }


        //all images with presignedurl
         public async Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix)
        {
          
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = prefix
            };
            var result = await _S3Client.ListObjectsV2Async(request);
            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(10)
                };
                return new Natural_Core.S3Models.S3Config()
                {
                    Image = s.Key.ToString(),
                    PresignedUrl = _S3Client.GetPreSignedURL(urlRequest),
                };
            });
            return (s3Objects);
        }


        //upload images to s3bucket
        [Obsolete]
        public async Task<UploadResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
        {
            var bucketExists = await _S3Client.DoesS3BucketExistAsync(bucketName);
            if (!bucketExists)
            {
                return new UploadResult { Success = false, Message = $"Bucket {bucketName} does not exist." };
            }
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);
            try
            {
                await _S3Client.PutObjectAsync(request);
                return new UploadResult { Success = true, Message = file.FileName };
            }

            catch (Exception ex)
            { return new UploadResult { Success = false, Message = $"Error uploading file to S3:{ex.Message}" }; }

        }


        //get products by id with category name
        public async Task<GetProduct> GetProductByIdAsync(string ProductId)
        {
            {
                var prod = await NaturalDbContext.Products
                           .Include(p => p.CategoryNavigation)
                           .Include(p => p.ProductTypeNavigation)
                           .Where(c => c.IsDeleted != true)
                            .FirstOrDefaultAsync(p => p.Id == ProductId);

                if (prod != null)
                {
                    var result = new GetProduct
                    {
                        Id = prod.Id,
                        ProductName = prod.ProductName,
                        Quantity = prod.Quantity,
                        Price = prod.Price,
                        DisplayPrice = prod.DisplayPrice,
                        Image = prod.Image,
                        Weight = prod.Weight,
                        CreatedDate = prod.CreatedDate,
                        ModifiedDate = prod.ModifiedDate,
                        Category = prod.CategoryNavigation?.CategoryName,
                        CategoryId = prod.Category,
                        ProductType = prod.ProductTypeNavigation?.ProductTypeName

                    };

                    return result;
                }
                else
                {
                    return null;
                }
            }
        }


        //if while updating i want to delete image
        public async Task<bool> DeleteImageAsync(string bucketName, string key)
        {
          
            try
            {
            
                // Attempt to delete the object
                await _S3Client.DeleteObjectAsync(bucketName, key);

                // Return true indicating successful deletion
                return true;
            }
            catch (Exception)
            {
                // Handle exceptions, log errors, or return false if deletion fails
                return false;
            }
        }

        private NaturalsContext NaturalDbContext
        {
            get

            { return Context as NaturalsContext; }
        }
    }
}

