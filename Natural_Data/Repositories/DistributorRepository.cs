﻿using Amazon.S3;
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
        //        public async Task<List<Distributor>> GetAllDistributorstAsync(SearchModel? search, bool? nonAssign)
        //        {

        //            var distributors = await NaturalDbContext.Distributors
        //            //.Include(c => c.AreaNavigation)
        //             .Include(a => a.CityNavigation)
        //            .ThenInclude(ct => ct.State)
        //            .Where(d => d.IsDeleted != true)
        //             .ToListAsync();

        //            if (search.City != null || search.State != null || search.FullName != null ||
        //                search.FirstName != null || search.LastName != null)
        //            {
        //                distributors = await searchDistributors(distributors, search);
        //                if (nonAssign == true)
        //                {
        //                    distributors = await searchNonAssignedDistributors(distributors);
        //                }
        //            }
        //            if (nonAssign == true)
        //            {
        //                distributors = await searchNonAssignedDistributors(distributors);
        //            }

        //            var result = distributors.Select(c => new Distributor
        //            {
        //                Id = c.Id,
        //                FirstName = c.FirstName,
        //                LastName = c.LastName,
        //                MobileNumber = c.MobileNumber,
        //                Address = c.Address,
        //                Email = c.Email,
        //                UserName = c.UserName,
        //                Password = c.Password,
        //                //Area = c.AreaNavigation.AreaName,
        //                City = c.CityNavigation?.CityName,
        //                State = c.StateNavigation?.StateName,
        //                Latitude = c.Latitude,
        //                Longitude = c.Longitude,
        //                Image = c.Image
        //            }).ToList();



        //            return result;
        //        }

        private async Task<List<Distributor>> searchNonAssignedDistributors(List<Distributor> distributorList)
        {
            var assignedDistributorIds = await NaturalDbContext.DistributorToExecutives
               .Select(de => de.DistributorId)
               .ToListAsync();

            var nonAssignedDistributors = distributorList
                .Where(c => !assignedDistributorIds.Contains(c.Id)).ToList();

            return nonAssignedDistributors;
        }

        private async Task<List<Distributor>> searchDistributors(List<Distributor> distributorsList, SearchModel search)
        {
            var exec = distributorsList.Where(c =>
       (c.IsDeleted != true) &&
(string.IsNullOrEmpty(search.State) || c.State == search.State) &&
(string.IsNullOrEmpty(search.City) || c.City == search.City) &&
//(string.IsNullOrEmpty(search.Area) || c.Area == search.Area) &&
(string.IsNullOrEmpty(search.FullName) ||
        c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
       (c.LastName?.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ?? false) ||
        (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))
).ToList();
            return exec;
        }
        private async Task<List<Distributor>> searchDistributorRecords(List<Distributor> distributorsList, SearchModel search)
        {
            var exec = distributorsList.Where(c =>
       (c.IsDeleted != true) &&

(string.IsNullOrEmpty(search.FullName) ||
        c.FirstName.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
       (c.LastName?.StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ?? false) ||
        (c.FirstName + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase) ||
        (c.FirstName + " " + c.LastName).StartsWith(search.FullName, StringComparison.OrdinalIgnoreCase))
).ToList();
            return exec;
        }
        public async Task<List<Distributor>> GetAllDistributorstAsync(SearchModel? search, bool? nonAssign)
        {

            if (nonAssign == true)
            {
                var distributor = await NaturalDbContext.Distributors
                 //.Include(c => c.AreaNavigation)
                 .Include(a => a.CityNavigation)
                .ThenInclude(ct => ct.State)
                .Where(d => d.IsDeleted != true)
                 .ToListAsync();

                if (search.City != null || search.State != null || search.FullName != null ||
                    search.FirstName != null || search.LastName != null)
                {
                    distributor = await searchDistributors(distributor, search);
                    if (nonAssign == true)
                    {
                        distributor = await searchNonAssignedDistributors(distributor);
                    }
                }
                if (nonAssign == true)
                {
                    distributor = await searchNonAssignedDistributors(distributor);
                }
                var result = distributor.Select(c => new Distributor
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    MobileNumber = c.MobileNumber,
                    Address = c.Address,
                    Email = c.Email,
                    UserName = c.UserName,
                    Password = c.Password,
                    //Area = c.AreaNavigation.AreaName,
                    City = c.CityNavigation?.CityName,
                    State = c.StateNavigation?.StateName,
                    Latitude = c.Latitude,
                    Longitude = c.Longitude,
                    Image = c.Image
                }).ToList();



                return result;
            }
            else
            {
                // Build the base query with eager loading and joins
                var query = from d in NaturalDbContext.Distributors
                            join de in NaturalDbContext.DistributorToExecutives
                            on d.Id equals de.DistributorId into deGroup
                            from de in deGroup.DefaultIfEmpty() // Left join to include distributors without executives
                            join e in NaturalDbContext.Executives
                            on de.ExecutiveId equals e.Id into eGroup
                            from e in eGroup.DefaultIfEmpty() // Left join to include executives without distributors
                            where d.IsDeleted != true
                            select new
                            {
                                Id = d.Id,
                                Distributor = d,
                                d.FirstName,
                                d.LastName,
                                //d.MobileNumber,
                                //d.Address,
                                //d.Email,
                                //d.UserName,
                                //d.Password,
                                ////City = d.CityNavigation.CityName,
                                ////State = d.StateNavigation.StateName,
                                ////d.City,
                                ////d.State,
                                //d.Latitude,
                                //d.Longitude,
                                //d.Image,
                                Executive = e == null ? null : $"{e.FirstName} {e.LastName}"
                                //Executive = e == null
                                //    ? "No Executive Assigned"
                                //    : $"{e.FirstName} {e.LastName}", // Format Executive field
                                //IsAssigned = e != null
                            };

                // Apply search filters
                if (search != null)
                {
                    if (!string.IsNullOrEmpty(search.State))
                    {
                        query = query.Where(q => q.Distributor.StateNavigation.Id == search.State);
                    }

                    if (!string.IsNullOrEmpty(search.City))
                    {
                        query = query.Where(q => q.Distributor.CityNavigation.Id == search.City);
                    }

                    
                  
                }

             

                // Execute the query and transform the results
                var distributors = await query
                    .Select(d => new Distributor
                    {
                        Id = d.Id,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        MobileNumber = d.Distributor.MobileNumber,
                        Address = d.Distributor.Address,
                        Email = d.Distributor.Email,
                        UserName = d.Distributor.UserName,
                        Password = d.Distributor.Password,
                        City = d.Distributor.CityNavigation.CityName,

                        State = d.Distributor.StateNavigation.StateName,
                        Latitude = d.Distributor.Latitude,
                        Longitude = d.Distributor.Longitude,
                        Image = d.Distributor.Image,
                        Executive = d.Executive // Include Executive information
                    })
                    .ToListAsync();

                if (search.FullName != null )
                {
                    distributors = await searchDistributorRecords(distributors, search);
                   
                }
                return distributors;
            }
        }

        public async Task<GetDistributor> GetDistributorDetailsByIdAsync(string distributorid)
        {
            var distributors = await NaturalDbContext.Distributors
                       //.Include(c => c.AreaNavigation)
                        .Include(a => a.CityNavigation)
                       .ThenInclude(ct => ct.State)
                        .FirstOrDefaultAsync(c => c.Id == distributorid && c.IsDeleted != true);

            if (distributors != null)
            {
                var result = new GetDistributor
                {
                    Id = distributors.Id,
                    FirstName = distributors.FirstName,
                    LastName = distributors.LastName,
                    MobileNumber = distributors.MobileNumber,
                    Address = distributors.Address,
                    Email = distributors.Email,
                    //Area = distributors.AreaNavigation.AreaName,
                    //AreaId = distributors.Area,
                    City = distributors.CityNavigation?.CityName,
                    CityId = distributors.City,
                    State = distributors.StateNavigation?.StateName,
                    StateId = distributors.State,
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
        //Area = distributor.Distributor.Area,
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