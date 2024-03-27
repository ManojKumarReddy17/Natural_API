using Microsoft.EntityFrameworkCore;
using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Natural_Data.Models;
using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Natural_Core.S3Models;
using Amazon.Util;
using Natural_Core.S3_Models;

#nullable disable
namespace Natural_Data.Repositories

{
    public class ExecutiveRepository : Repository<Executive>, IExecutiveRepository
    {
        private readonly IAmazonS3 _S3Client;
        public ExecutiveRepository(NaturalsContext context, IAmazonS3 S3Client) : base(context)
        {
            _S3Client = S3Client;
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

        public async Task<IEnumerable<Executive>> GetAllExecutiveAsync()
        {
            {
                var exec = await NaturalDbContext.Executives
                .Include(c => c.AreaNavigation)
                 .ThenInclude(a => a.City)
                .ThenInclude(ct => ct.State)
                 .ToListAsync();

                var result = exec.Select(c => new Executive
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MobileNumber = c.MobileNumber,
                    Address = c.Address,
                    Area = c.AreaNavigation.AreaName,
                    Email = c.Email,
                    UserName = c.UserName,
                    Password = c.Password,
                    City = c.AreaNavigation.City.CityName,
                    State = c.AreaNavigation.City.State.StateName,
                    Image = c.Image
                }).ToList();

                return result;
            }
        }

        
        public async Task<Executive> GetWithExectiveByIdAsync(string execid)
        {
            {
                var exec = await NaturalDbContext.Executives
                           .Include(c => c.AreaNavigation)
                            .ThenInclude(a => a.City)
                           .ThenInclude(ct => ct.State)
                            .FirstOrDefaultAsync(c => c.Id == execid);

                if (exec != null)
                {
                    var result = new Executive
                    {
                        Id = exec.Id,
                        FirstName = exec.FirstName,
                        LastName = exec.LastName,
                        MobileNumber = exec.MobileNumber,
                        Address = exec.Address,
                        Area = exec.AreaNavigation.AreaName,
                        Email = exec.Email,
                        UserName= exec.UserName,
                        Password= exec.Password,
                        Image = exec.Image,
                        City = exec.AreaNavigation.City.CityName,
                        State = exec.AreaNavigation.City.State.StateName
                    };

                    return result;

                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<IEnumerable<Executive>> SearchExecutiveAsync(SearchModel search)
        {
            var exec = await NaturalDbContext.Executives
                   .Include(c => c.AreaNavigation)
                    .ThenInclude(a => a.City)
                   .ThenInclude(ct => ct.State)
                   .Where(c =>
    (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
    (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
    (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) &&
    (string.IsNullOrEmpty(search.FullName) || c.FirstName.StartsWith(search.FullName) ||
    c.LastName.StartsWith(search.FullName) || (c.FirstName + c.LastName).StartsWith(search.FullName)||
    (c.FirstName + " "+ c.LastName).StartsWith(search.FullName)))
   .ToListAsync();
            var result = exec.Select(c => new Executive
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                Address = c.Address,
                Area = c.AreaNavigation.AreaName,
                Email = c.Email,
                UserName = c.UserName,
                Password = c.Password,
                City = c.AreaNavigation.City.CityName,
                State = c.AreaNavigation.City.State.StateName
            }).ToList();
            return result;
        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
    }
}
