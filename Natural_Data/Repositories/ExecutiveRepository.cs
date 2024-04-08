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



        public async Task<IEnumerable<InsertUpdateModel>> SearchExecutiveAsync(SearchModel search)

        {
            if (string.IsNullOrEmpty(search.Area))
            {
                var exec = await NaturalDbContext.Executives
                     .Include(c => c.CityNavigation)
                     .ThenInclude(ct => ct.State)
                     .Where(c =>
                      (c.IsDeleted != true) &&
                      (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
                      (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
                      (string.IsNullOrEmpty(search.FullName) || c.FirstName.StartsWith(search.FullName) ||
                       c.LastName.StartsWith(search.FullName) || (c.FirstName + c.LastName).StartsWith(search.FullName) ||
                      (c.FirstName + " " + c.LastName).StartsWith(search.FullName)))
                     .ToListAsync();
                var result = exec.Select(c => new InsertUpdateModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MobileNumber = c.MobileNumber,
                    Address = c.Address,
                    Email = c.Email,
                    UserName = c.UserName,
                    Password = c.Password,

                  
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    Image = c.Image,

                    City = c.CityNavigation.CityName,
                    State = c.CityNavigation.State.StateName,
                    Area = NaturalDbContext.ExecutiveAreas
                          .Where(execArea => execArea.Executive == c.Id)
                          .Select(ea => ea.AreaNavigation.AreaName)
                          .ToList()

                }).ToList();
                return result;
            }
            else
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
                               string.IsNullOrEmpty(search.FullName) || c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
                               c.LastName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) || (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
                               (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))
                               .ToList();
                    return exec;

                }
               

           

              


            }


        }
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }




        public async Task<IEnumerable<Executive>> GetAllExecutiveAsync()

        {
            {
                var exec = await NaturalDbContext.Executives
                .Include(c => c.CityNavigation)
                //.ThenInclude(a => a.City)
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



                return exec;
            }
        }


        public async Task<List<InsertUpdateModel>> GetxecutiveAsync()
        {
            {
              

                var query = NaturalDbContext.Executives
                    .Where(d => d.IsDeleted != true)
    //.AsEnumerable() // Execute query on client side
    .Select(exec => new InsertUpdateModel
    {
        Id = exec.Id,
        FirstName = exec.FirstName,
        LastName = exec.LastName,
        Address = exec.Address,
        MobileNumber = exec.MobileNumber,
        Longitude = exec.Longitude,
        Latitude = exec.Latitude,
        Email = exec.Email,
        Image = exec.Image,
        City = exec.CityNavigation.CityName,
        State = exec.StateNavigation.StateName,
        Area = NaturalDbContext.ExecutiveAreas
            .Where(execArea => execArea.Executive == exec.Id)
            .Select(ea => ea.AreaNavigation.AreaName)
            .ToList()
    })
    .ToList(); // Convert to list after executing the query

                return query;
            }
        }

        public async Task<Executive> GetWithExectiveByIdAsync(string id)
        {
            {
                var exec = await NaturalDbContext.Executives

                           .Include(c => c.CityNavigation)
                            .ThenInclude(ct => ct.State)
                           .Where(d => d.IsDeleted != true)
                            .FirstOrDefaultAsync(c => c.Id == id);

                if (exec != null)
                {
                    var result = new Executive
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
                        State = exec.CityNavigation.State.StateName

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

            var exec = await NaturalDbContext.Executives

                             .Where(d => d.Id == id)
                             .Where(d => d.IsDeleted != true)
                             .FirstOrDefaultAsync();
            return exec;
        }

        Task<IEnumerable<Executive>> IExecutiveRepository.GetAllExecutiveAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<InsertUpdateModel> GetxecutiveAsyncbyId(string id)
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

            return result;





        }

       
    }
}
