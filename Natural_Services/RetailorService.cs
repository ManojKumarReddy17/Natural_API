using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Data.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Natural_Core.S3Models;
using Microsoft.Extensions.Options;
using AutoMapper;

#nullable disable

namespace Natural_Services
{
    public class RetailorService : IRetailorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly S3Config _s3Config;
        private readonly IDistributorService _DistributorService;
        private readonly IMapper _Mapper;


        public RetailorService(IUnitOfWork unitOfWork, IOptions<S3Config> s3Config, IDistributorService distributorService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _s3Config = s3Config.Value;
            _DistributorService = distributorService;
            _Mapper = mapper;
        }

        //get bucket names//
        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            var bucketlist = await _unitOfWork.ExecutiveRepo.GetAllBucketAsync();
            return bucketlist;
        }


        public async Task<IEnumerable<Retailor>> GetAllRetailors()
        {
            var result = await _unitOfWork.RetailorRepo.GetAllRetailorsAsync();
            var presentRetailor = result.Where(d => d.IsDeleted != true).ToList();
            
            return presentRetailor;
        }

        public async Task<IEnumerable<GetRetailor>> GetAllRetailorDetailsAsync(string? prefix)
        {

            var Retailors = await GetAllRetailors();

            string bucketName = _s3Config.BucketName;
            var presignedUrls = await _DistributorService.GetAllFilesAsync(bucketName, prefix);

            var leftJoinQuery = from retailor in Retailors
                                join presigned in presignedUrls
                                on retailor.Image equals presigned.Image into newUrl
                                from sub in newUrl.DefaultIfEmpty()
                                select new GetRetailor
                                {
                                    Id = retailor.Id,
                                    FirstName = retailor.FirstName,
                                    LastName = retailor.LastName,
                                    MobileNumber = retailor.MobileNumber,
                                    Address = retailor.Address,
                                    Area = retailor.Area,
                                    Email = retailor.Email,
                                    City = retailor.City,
                                    State = retailor.State,
                                    PresignedUrl = sub?.PresignedUrl,
                                    Latitude = retailor.Latitude,
                                    Longitude = retailor.Longitude
                                };

            return leftJoinQuery;
        }


        public async Task<IEnumerable<Retailor>> GetNonAssignedRetailors()
        {
            var result = await _unitOfWork.RetailorRepo.GetNonAssignedRetailorsAsync();
            return result;
        }



        public async Task<Retailor> GetRetailorDetailsById(string retailorId)
        {
            return await _unitOfWork.RetailorRepo.GetRetailorDetailsByIdAsync(retailorId);
        }




        public async Task<Retailor> GetRetailorsById(string retailorId)
        {
            var result = await _unitOfWork.RetailorRepo.GetByIdAsync(retailorId);

            if (result.IsDeleted == false)
            {
                return result;
            }

            return null;
        }

        public async Task<GetRetailor> GetRetailorPresignedUrlbyId(string retailorId)
        {
            var Retailorresult = await _unitOfWork.RetailorRepo.GetByIdAsync(retailorId);

            string bucketName = _s3Config.BucketName;
            string prefix = Retailorresult.Image;
            //var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);
            var PresignedUrl = await _DistributorService.GetAllFilesAsync(bucketName, prefix);

            if (PresignedUrl.Any())
            {
                var exe = PresignedUrl.FirstOrDefault();
                var execuresoursze1 = _Mapper.Map<Retailor, GetRetailor>(Retailorresult);
                execuresoursze1.PresignedUrl = exe.PresignedUrl;

                return execuresoursze1;
            }
            else
            {
                var execuresoursze1 = _Mapper.Map<Retailor, GetRetailor>(Retailorresult);

                return execuresoursze1;

            }

        }




        public async Task<ResultResponse> CreateRetailorWithAssociationsAsync(Retailor retailor)
        {
            var response = new ResultResponse();

            try
            {
                retailor.Id = "NRET" + new Random().Next(10000, 99999).ToString();

                await _unitOfWork.RetailorRepo.AddAsync(retailor);

                var created = await _unitOfWork.CommitAsync();

                if (created != 0)
                {
                    response.Message = "Insertion Successful";
                    response.StatusCode = 200;
                }
            }
            catch (Exception)
            {

                response.Message = "Insertion Failed";
                response.StatusCode = 401;
            }

            return response;
        }
        public async Task<ResultResponse> UpdateRetailors(Retailor existingRetailor, Retailor retailor)
        {

            var response = new ResultResponse();

            try
            {
                await _unitOfWork.RetailorRepo.UpdateRetailorAsync(existingRetailor, retailor);

                await _unitOfWork.CommitAsync();


                response.Message = "Update Successfull";
                response.StatusCode = 200;
            }

            catch (Exception)
            {

                response.Message = "Update Failed";
                response.StatusCode = 400;

            }
            return response;

        }


        public async Task<ResultResponse> SoftDelete(string retailorId)
        {
            var response = new ResultResponse();

            try
            {
                var retailor = await _unitOfWork.RetailorRepo.GetByIdAsync(retailorId);

                if (retailor != null)
                {
                    retailor.IsDeleted = true;
                    _unitOfWork.RetailorRepo.Update(retailor);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "RETAILER NOT FOUND";
                    response.StatusCode = 404;
                }
            }
            catch (Exception)
            {
                response.Message = "Internal Server Error";
            }

            return response;
        }

        public async Task<IEnumerable<Retailor>> SearcRetailors(SearchModel search)
        {
            var exec = await _unitOfWork.RetailorRepo.SearchRetailorAsync(search);
            return exec;
        }


        public async Task<IEnumerable<Distributor>> SearchNonAssignedDistributors(SearchModel search)
        {
            var searchdistributors = await _unitOfWork.DistributorRepo.SearchNonAssignedDistributorsAsync(search);
            return searchdistributors;
        }
    }
}
 
       
