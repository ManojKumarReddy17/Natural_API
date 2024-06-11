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
using Microsoft.Extensions.Logging;

#nullable disable

namespace Natural_Services
{
    public class RetailorService : IRetailorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly S3Config _s3Config;
        private readonly IDistributorService _DistributorService;
        private readonly IMapper _Mapper;
        private readonly IRetailorRepository _RetailorRepository;
        private readonly ILogger<RetailorService> _logger;

      


        public RetailorService(IUnitOfWork unitOfWork, IOptions<S3Config> s3Config, IDistributorService distributorService, IMapper mapper, ILogger<RetailorService> logger)
        {
            _unitOfWork = unitOfWork;
            _s3Config = s3Config.Value;
            _DistributorService = distributorService;
            _Mapper = mapper;
            _logger = logger;

        }

        //get bucket names//
        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            try
            {
                var bucketlist = await _unitOfWork.ExecutiveRepo.GetAllBucketAsync();
                return bucketlist;
            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorService-GetAllBucketAsync", ex.Message);
                return null;

            }

        }

        public async Task<IEnumerable<GetRetailor>> GetAllRetailorDetailsAsync(SearchModel? search, bool? NonAssign, string? prefix)
        {
            try
            {
                var Retailors = await _unitOfWork.RetailorRepo.GetAllRetailorsAsync(search, NonAssign);
                _logger.LogInformation("GetRetailers Successful {Retailors}", Retailors);
                string bucketName = _s3Config.BucketName;
                var presignedUrls = await _DistributorService.GetAllFilesAsync(bucketName, prefix);
                _logger.LogInformation("GetAllFiles Successful {presignedUrls}", presignedUrls);

                var leftJoinQuery = from retailor in Retailors
                                    join presigned in presignedUrls
                                    on retailor.Image equals presigned.Image into newUrl
                                    from sub in newUrl.DefaultIfEmpty()
                                    select new GetRetailor
                                    {
                                        Id = retailor.Id,
                                        FullName = retailor.FirstName,
                                      // LastName = retailor.LastName,
                                        MobileNumber = retailor.MobileNumber,
                                        Address = retailor.Address,
                                        Area = retailor.Area,
                                        Email = retailor.Email,
                                        City = retailor.City,
                                        State = retailor.State,
                                        Image = sub?.PresignedUrl,
                                        Latitude = retailor.Latitude,
                                        Longitude = retailor.Longitude
                                    };


                return leftJoinQuery;

            }
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "An error occurred while deleting product with ID {ProductId}", id);
            //    throw;
            //}
            catch (Exception ex)
            {
                _logger.LogError("RetailorService-GetAllRetailorDetailsAsync", ex.Message);
                return null;

            }













        }

        public async Task<GetRetailor> GetRetailorDetailsById(string retailorId)
        {
            try
            {

            
            var retailorDetails = await _unitOfWork.RetailorRepo.GetRetailorDetailsByIdAsync(retailorId);
            _logger.LogInformation("GetRetailerDetails Successful {retailorId}", retailorId);
            string bucketName = _s3Config.BucketName;
            string prefix = retailorDetails.Image;
            var PresignedUrl = await _DistributorService.GetAllFilesAsync(bucketName, prefix);
            _logger.LogInformation("GetAllFilesAsync Successful {PresignedUrl}", PresignedUrl);

            if (PresignedUrl.Any())
            {
                var exe = PresignedUrl.FirstOrDefault();
                retailorDetails.Image = exe.PresignedUrl;
                return retailorDetails;
            }
            else
            {
                return retailorDetails;
            }
            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorService-GetRetailorDetailsById", ex.Message);
                return null;

            }
        }

        public async Task<Retailor> GetRetailorsById(string retailorId)
        {
            try
            {
                var result = await _unitOfWork.RetailorRepo.GetByIdAsync(retailorId);

                if (result.IsDeleted == false)
                {
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("RetailorService-GetRetailorsById", ex.Message);
                return null;

            }

        }

        public async Task<ResultResponse> CreateRetailorWithAssociationsAsync(Retailor retailor)
        {
            var response = new ResultResponse();
            try
            {
               

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
            catch (Exception ex)
            {
                _logger.LogError("RetailorService-CreateRetailorWithAssociationsAsync", ex.Message);
                return null;

            }

        }
        public async Task<ResultResponse> UpdateRetailors(Retailor existingRetailor, Retailor retailor)
        {
            var response = new ResultResponse();
            try
            {

              

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
            catch (Exception ex)
            {
                _logger.LogError("RetailorService-UpdateRetailors", ex.Message);
                return null;

            }



        }

        public async Task<IEnumerable<RetailorDetailsByArea>> GetRetailordetailsByAreaId(string areaId)
        {
            var reatailorbyarea = await _unitOfWork.RetailorRepo.GetRetailorDetailsByAreaId(areaId);
            return reatailorbyarea;
        }
        public async Task<ResultResponse> SoftDelete(string retailorId)
        {
            var response = new ResultResponse();
            try
            {
               

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
            catch (Exception ex)
            {
                _logger.LogError("RetailorService-SoftDelete", ex.Message);
                return null;

            }

        }
    }
}
 
       
