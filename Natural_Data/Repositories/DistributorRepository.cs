using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using Natural_Core.S3Models;
using Natural_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Data.Repositories
{
    public class DistributorRepository : Repository<Distributor>, IDistributorRepository
    {
        private readonly IAmazonS3 _S3Client;
        public DistributorRepository(NaturalsContext context, IAmazonS3 S3Client) : base(context)
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
        public async Task<List<Distributor>> GetAllDistributorstAsync()
        {

            var distributors = await NaturalDbContext.Distributors
            .Include(c => c.AreaNavigation)
             .ThenInclude(a => a.City)
            .ThenInclude(ct => ct.State)
            .Where(d => d.IsDeleted != true)
             .ToListAsync();

            var result = distributors.Select(c => new Distributor
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                MobileNumber = c.MobileNumber,
                Address = c.Address,
                Email = c.Email,
                UserName = c.UserName,
                Password = c.Password,
                Area = c.AreaNavigation.AreaName,
                City = c.AreaNavigation.City.CityName,
                State = c.AreaNavigation.City.State.StateName,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                Image = c.Image
            }).ToList();

            return result;
        }



        public async Task<Distributor> GetDistributorDetailsByIdAsync(string distributorid)
        {
            var distributors = await NaturalDbContext.Distributors
                       .Include(c => c.AreaNavigation)
                        .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                        .FirstOrDefaultAsync(c => c.Id == distributorid && c.IsDeleted != true);

            if (distributors != null)
            {
                var result = new Distributor
                {
                    Id = distributors.Id,
                    FirstName = distributors.FirstName,
                    LastName = distributors.LastName,
                    MobileNumber = distributors.MobileNumber,
                    Address = distributors.Address,
                    Email = distributors.Email,
                    Area = distributors.AreaNavigation.AreaName,      
                    City = distributors.AreaNavigation.City.CityName,
                    State = distributors.AreaNavigation.City.State.StateName,
                    UserName = distributors.UserName,
                    Password = distributors.Password,
                    Latitude = distributors.Latitude,
                    Longitude= distributors.Longitude,
                    Image = distributors.Image,
                };

                return result;

            }
            else
            {
                    return null;
            }
            
        }

        public async Task<IEnumerable<Distributor>> SearchDistributorAsync(SearchModel search)
        {
            
                var exec = await NaturalDbContext.Distributors
                       .Include(c => c.AreaNavigation)
                       .ThenInclude(a => a.City)
                       .ThenInclude(ct => ct.State)
                       .Where(c =>
                       (c.IsDeleted != true) &&
        (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
        (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
        (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) &&
        (string.IsNullOrEmpty(search.FullName) || c.FirstName.StartsWith(search.FullName) ||
        c.LastName.StartsWith(search.FullName) || (c.FirstName + c.LastName).StartsWith(search.FullName) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName)))
       .ToListAsync();
            var result = exec.Select(c => new Distributor
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
                Latitude = c.Latitude,
                Longitude = c.Longitude
            }).ToList();
                return result;
            }

        public async Task<IEnumerable<Distributor>> GetNonAssignedDistributorsAsync()
        {
           
                var distributors = await NaturalDbContext.Distributors
                    .Include(c => c.AreaNavigation)
                    .ThenInclude(a => a.City)
                    .ThenInclude(ct => ct.State)
                    .ToListAsync();

                var assignedDistributorIds = await NaturalDbContext.DistributorToExecutives
                    .Select(de => de.DistributorId)
                    .ToListAsync();

                var nonAssignedDistributors = distributors
                    .Where(c => !assignedDistributorIds.Contains(c.Id)) 
                    .Where(c => c.IsDeleted != true) // this is added for soft delete

                    .Select(c => new Distributor
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        MobileNumber = c.MobileNumber,
                        Address = c.Address,
                        Email = c.Email,
                        UserName = c.UserName,
                        Password = c.Password,
                        Area = c.AreaNavigation.AreaName,
                        City = c.AreaNavigation.City.CityName,
                        State = c.AreaNavigation.City.State.StateName,
                        Latitude=c.Latitude, Longitude=c.Longitude,
                        Image = c.Image
                    })
                    .ToList();

                return nonAssignedDistributors;
            }

        public async Task<IEnumerable<Distributor>> SearchNonAssignedDistributorsAsync(SearchModel search)
        {
            var distirbutors = await NaturalDbContext.Distributors
                      .Include(c => c.AreaNavigation)
                       .ThenInclude(a => a.City)
                      .ThenInclude(ct => ct.State)
                      .Where(c =>
                       (c.IsDeleted != true) &&
       (string.IsNullOrEmpty(search.State) || c.State == search.State) &&
       (string.IsNullOrEmpty(search.City) || c.City == search.City) &&
       (string.IsNullOrEmpty(search.Area) || c.Area == search.Area) &&
       (string.IsNullOrEmpty(search.FullName) || c.FirstName.StartsWith(search.FullName) ||
       c.LastName.StartsWith(search.FullName) || (c.FirstName + c.LastName).StartsWith(search.FullName) ||
       (c.FirstName + " " + c.LastName).StartsWith(search.FullName)))
      .ToListAsync();

            var assignedDistributorIds = await NaturalDbContext.DistributorToExecutives
                 .Select(de => de.DistributorId)
                 .ToListAsync();

            var nonAssignedDistributors = distirbutors
                .Where(c => !assignedDistributorIds.Contains(c.Id))
                .Select(c => new Distributor
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MobileNumber = c.MobileNumber,
                    Address = c.Address,
                    Email = c.Email,
                    UserName = c.UserName,
                    Password = c.Password,
                    Area = c.AreaNavigation.AreaName,
                    City = c.AreaNavigation.City.CityName,
                    State = c.AreaNavigation.City.State.StateName,
                    Latitude = c.Latitude,
                    Longitude=c.Longitude,
                    Image = c.Image
                })
                .ToList();

            return nonAssignedDistributors;
        }

        public async Task<AngularDistributor> GetAngularAsync(string DistributorId)
        {


            var angularDistributors = await NaturalDbContext.DistributorToExecutives
    .Include(e => e.Executive)
    .Include(a => a.Distributor)
   .Where(x => x.DistributorId == DistributorId)
    .Select(distributor => new AngularDistributor
    {
        Id = distributor.Id,
        FirstName = distributor.Distributor.FirstName,
        LastName = distributor.Distributor.LastName,
        MobileNumber = distributor.Distributor.MobileNumber,
        Address = distributor.Distributor.Address,
        Email = distributor.Distributor.Email,
        PresignedUrl = distributor.Distributor.Image,
        UserName = distributor.Distributor.UserName,

        ExeId = distributor.Executive.Id,
        Executives = $"{distributor.Executive.FirstName} {distributor.Executive.LastName}"
    }).FirstOrDefaultAsync();

            return angularDistributors;



        }



        private NaturalsContext NaturalDbContext
        {
            get 
            
            { return Context as NaturalsContext; }
        }


    }
}