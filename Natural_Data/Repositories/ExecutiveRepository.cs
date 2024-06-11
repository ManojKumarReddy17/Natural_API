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
using Natural_Core.Models.CustomModels;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

#nullable disable
namespace Natural_Data.Repositories

{
    public class ExecutiveRepository : Repository<Executive>, IExecutiveRepository
    {
        private readonly IAmazonS3 _S3Client;
        private readonly ILogger<ExecutiveRepository> _logger;
        private IAmazonS3 s3Client;

        public ExecutiveRepository(NaturalsContext context, IAmazonS3 S3Client, ILogger<ExecutiveRepository> logger) : base(context)
        {
            _S3Client = S3Client;
            _logger = logger;
        }

        public ExecutiveRepository(DbContext context, IAmazonS3 s3Client) : base(context)
        {
            this.s3Client = s3Client;
        }

        //Getbucket name
        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            try
            {
                _logger.LogInformation("Starting to list all S3 buckets.");
                var data = await _S3Client.ListBucketsAsync();
                _logger.LogInformation("Successfully fetched bucket list. Found {BucketCount} buckets.", data.Buckets.Count);
                var buckets = data.Buckets.Select(b => { return b.BucketName; });
                _logger.LogInformation("Extracted bucket names successfully.");
                return buckets;
            }
               
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while listing S3 buckets.");
            throw;
        }

}

        //all images with presignedurl
        public async Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            try
            {
                _logger.LogInformation("Listing all files from bucket: {BucketName} with prefix: {Prefix}", bucketName, prefix);

                var request = new ListObjectsV2Request()
                {
                    BucketName = bucketName,
                    Prefix = prefix
                };
                var result = await _S3Client.ListObjectsV2Async(request);
                _logger.LogInformation("Listing all files from bucket: {BucketName} with prefix: {Prefix}", bucketName, prefix);
                var s3Objects = result.S3Objects.Select(s =>
                {
                    _logger.LogInformation("Generated presigned URL for file: {FileKey}", s.Key);
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
            _logger.LogInformation("Successfully processed {FileCount} files from bucket: {BucketName}", s3Objects.Count(), bucketName);
            return (s3Objects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while listing files from bucket: {BucketName}", bucketName);
                throw;
            }

        }


        //upload images to s3bucket
        public async Task<UploadResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix)
        {
            try
            {
                _logger.LogInformation("Checking if bucket {BucketName} exists.", bucketName);
                var bucketExists = await _S3Client.DoesS3BucketExistAsync(bucketName);
                if (!bucketExists)
                {
                    _logger.LogWarning("Bucket {BucketName} does not exist.", bucketName);
                    return new UploadResult { Success = false, Message = $"Bucket {bucketName} does not exist." };
                }
                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                    InputStream = file.OpenReadStream()
                };
                request.Metadata.Add("Content-Type", file.ContentType);
                _logger.LogInformation("Uploading file {FileName} to bucket {BucketName} with key {Key}.", file.FileName, bucketName);
                try
                {
                    await _S3Client.PutObjectAsync(request);
                    _logger.LogInformation("File {FileName} successfully uploaded to bucket {BucketName}.", file.FileName, bucketName);
                    return new UploadResult { Success = true, Message = file.FileName };
                }

                catch (Exception ex)
                { return new UploadResult { Success = false, Message = $"Error uploading file to S3:{ex.Message}" }; }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {FileName} to bucket {BucketName}.", file.FileName, bucketName);
                return new UploadResult { Success = false, Message = $"Error uploading file to S3: {ex.Message}" };
            }

        }

        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }




        public async Task<IEnumerable<Executive>> GetAllExecutiveAsync()

        {

                 _logger.LogInformation("GetAllExecutiveAsync started.");
            try
            {
                var exec = await NaturalDbContext.Executives
                .Include(c => c.CityNavigation)
                .ThenInclude(ct => ct.State)
                .Where(d => d.IsDeleted != true)
                 .Select(c => new Executive
                 {
                     Id = c.Id,
                     FirstName = c.FirstName,
                     LastName = c.LastName,
                     MobileNumber = c.MobileNumber,
                     Address = c.Address,
                     
                     Email = c.Email,
                     UserName = c.UserName,
                     Password = c.Password,
                     City = c.CityNavigation.CityName,
                     State = c.CityNavigation.State.StateName,
                     Longitude = c.Longitude,
                     Latitude = c.Latitude,
                     Image = c.Image,
                   
                 })
                 .ToListAsync();


                _logger.LogInformation("GetAllExecutiveAsync completed successfully.");
                return exec;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing GetAllExecutiveAsync.");
                throw;
            }

        }


        public async Task<List<InsertUpdateModel>> GetxecutiveAsync(SearchModel? search)
        {

            _logger.LogInformation("GetxecutiveAsync started.");
            try
            {
                {
                    var executiveList = await NaturalDbContext.Executives
                         .Include(c => c.CityNavigation)
                         .ThenInclude(ct => ct.State)
                         .Where(c => c.IsDeleted != true).ToListAsync();
                    var allExecutiveList = new List<InsertUpdateModel>();
                    if (search != null)
                    {
                        if (string.IsNullOrEmpty(search.Area))
                        {
                            executiveList = await SearchExecutiveAsync(executiveList, search);
                        }
                        else
                        {
                            allExecutiveList = await searchExecutiveAsyncWithArea(search);
                            _logger.LogInformation("GetxecutiveAsync completed with search area.");
                            return allExecutiveList;
                        }

                    }

                    allExecutiveList = executiveList.Select(exec => new InsertUpdateModel
                    {
                        Id = exec.Id,
                        FirstName = exec.FirstName,
                        LastName = exec.LastName,
                        MobileNumber = exec.MobileNumber,
                        Address = exec.Address,
                        Email = exec.Email,
                        UserName = exec.UserName,
                        Password = exec.Password,
                        City = exec.CityNavigation.CityName,
                        State = exec.CityNavigation.State.StateName,
                        Longitude = exec.Longitude,
                        Latitude = exec.Latitude,
                        Image = exec.Image,
                        Area = NaturalDbContext.ExecutiveAreas
                                .Where(execArea => execArea.Executive == exec.Id)
                                .Select(ea => ea.AreaNavigation.AreaName)
                                .ToList()
                    }).ToList();
                    _logger.LogInformation("GetxecutiveAsync completed successfully.");

                    return allExecutiveList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing GetxecutiveAsync.");
                throw;
            }
        }

        private async Task<List<InsertUpdateModel>> searchExecutiveAsyncWithArea(SearchModel search)
        {
            var executiveNames = await NaturalDbContext.ExecutiveAreas
                .Where(x => x.Area == search.Area)
                .Select(x => x.Executive)
                .ToListAsync();
            List<InsertUpdateModel> resultList = new List<InsertUpdateModel>();
            foreach (var executiveName in executiveNames)
            {
                // Call the GetMethodById method and store the result in a variable
                var result = await GetxecutiveAsyncbyId(executiveName); // Assuming GetMethodById returns an instance of YourModel
                if (result != null)
                // Add the result to the resultList
                { resultList.Add(result); }
            }

            if (string.IsNullOrEmpty(search.FullName) && string.IsNullOrEmpty(search.FirstName) && string.IsNullOrEmpty(search.LastName))

            { return resultList; }
            else
            {
                var exec = resultList

                .Where(c =>
                          (string.IsNullOrEmpty(search.FullName) ||
        c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        c.LastName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))
).ToList();
                return exec;

            }
        }

        private async Task<List<Executive>> SearchExecutiveAsync(List<Executive> executiveList, SearchModel search)
        {
                var exec = executiveList.Where(c =>
                      (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
                      (string.IsNullOrEmpty(search.City) || c.City == search.City) && (string.IsNullOrEmpty(search.FullName) ||
        c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        c.LastName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))
).ToList();

            return exec;

        }

        public async Task<ExecutiveGetResource> GetWithExectiveByIdAsync(string id)
        {
            {
                var exec = await NaturalDbContext.Executives

                           .Include(c => c.CityNavigation)
                            .ThenInclude(ct => ct.State)
                           .Where(d => d.IsDeleted != true)
                            .FirstOrDefaultAsync(c => c.Id == id);

                if (exec != null)
                {
                    var result = new ExecutiveGetResource
                    {
                        Id = exec.Id,
                        FirstName = exec.FirstName,
                        LastName = exec.LastName,
                        MobileNumber = exec.MobileNumber,
                        Address = exec.Address,
                        Email = exec.Email,
                        UserName = exec.UserName,
                        Password = exec.Password,
                        Image = exec.Image,
                        Latitude = exec.Latitude,
                        Longitude = exec.Longitude,
                        City = exec.CityNavigation.CityName,
                        CityId = exec.City,
                        State = exec.CityNavigation.State.StateName,
                        StateId = exec.State,

                    };

                    return result;

                }
                else
                {
                    return null;
                }
            }
        }


        // get table data as it is
        public async Task<Executive> GetExectiveTableByIdAsync(string id)
        {
            _logger.LogInformation("GetExectiveTableByIdAsync started with Id: {Id}", id);
            try
            {



                var exec = await NaturalDbContext.Executives

                                 .Where(d => d.Id == id)
                                 .Where(d => d.IsDeleted != true)
                                 .FirstOrDefaultAsync();
                //return exec;
                if (exec == null)
                {
                    _logger.LogWarning("Executive with Id: {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("GetExectiveTableByIdAsync completed successfully for Id: {Id}", id);
                }
                return exec;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing GetExectiveTableByIdAsync for Id: {Id}", id);
                throw;
            }
        }



        public async Task<InsertUpdateModel> GetxecutiveAsyncbyId(string id)
        {
            _logger.LogInformation("GetExecutiveAsyncById started with Id: {Id}", id);
            try
            {



                var result = await NaturalDbContext.Executives
                            .Include(c => c.CityNavigation)
                            .ThenInclude(ct => ct.State)

                            .Where(d => d.Id == id)
                            .Where(d => d.IsDeleted != true)
                            .Select(c => new InsertUpdateModel
                            {
                                Id = c.Id,
                                FirstName = c.FirstName,
                                LastName = c.LastName,
                                MobileNumber = c.MobileNumber,
                                Address = c.Address,
                                Email = c.Email,
                                UserName = c.UserName,
                                Password = c.Password,
                                Longitude = c.Longitude,
                                Latitude = c.Latitude,
                                Image = c.Image,
                                City = c.CityNavigation.CityName,
                                State = c.CityNavigation.State.StateName,
                                Area = NaturalDbContext.ExecutiveAreas
                                    .Where(execArea => execArea.Executive == c.Id)
                                    .Select(ea => ea.AreaNavigation.AreaName)
                                    .ToList()
                            }).FirstOrDefaultAsync();
                if (result == null)
                {
                    _logger.LogWarning("Executive with Id: {Id} not found", id);
                }
                else
                {
                    _logger.LogInformation("GetExecutiveAsyncById completed successfully for Id: {Id}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing GetExecutiveAsyncById for Id: {Id}", id);
                throw;
            }





        }

    }
}
