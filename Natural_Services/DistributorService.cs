using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Natural_Core.S3Models;
using AutoMapper;
using Microsoft.Extensions.Options;
using Natural_Core.S3_Models;


#nullable disable

namespace Natural_Services
{
    public class DistributorService : IDistributorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly S3Config _s3Config;
        private readonly IMapper _Mapper;

        public DistributorService(IUnitOfWork unitOfWork, IOptions<S3Config> s3Config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _s3Config = s3Config.Value;
            _Mapper = mapper;
        }

        //get bucket names//
        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            var bucketlist = await _unitOfWork.DistributorRepo.GetAllBucketAsync();
            return bucketlist;
        }


        //get all files //all images with presignedurl
        public async Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            var Allfilename = await _unitOfWork.DistributorRepo.GetAllFilesAsync(bucketName, prefix);
            return Allfilename;
        }

        //upload images to s3 bucket
        public async Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix)
        {

            string bucketName = _s3Config.BucketName;
            var metadata = await _unitOfWork.DistributorRepo.UploadFileAsync(file, bucketName, prefix);
            return metadata;

        }


        public async Task<IEnumerable<Distributor>> GetAllDistributors()
        {
            var result = await _unitOfWork.DistributorRepo.GetAllDistributorstAsync();
            var presentDistributors = result.Where(d => d.IsDeleted != true ).ToList();
            return presentDistributors;
        }

        public async Task<IEnumerable<GetDistributor>> GetAllDistributorDetailsAsync(string? prefix)
        {

            var executives = await GetAllDistributors();

            string bucketName = _s3Config.BucketName;
            var presignedUrls = await GetAllFilesAsync(bucketName, prefix);

            var leftJoinQuery = from executive in executives
                                join presigned in presignedUrls
                                on executive.Image equals presigned.Image into newUrl
                                from sub in newUrl.DefaultIfEmpty()
                                select new GetDistributor
                                {
                                    Id = executive.Id,
                                    FirstName = executive.FirstName,
                                    LastName = executive.LastName,
                                    MobileNumber = executive.MobileNumber,
                                    Address = executive.Address,
                                    Area = executive.Area,
                                    Email = executive.Email,
                                    UserName = executive.UserName,
                                    Password = executive.Password,
                                    City = executive.City,
                                    State = executive.State,
                                    PresignedUrl = sub?.PresignedUrl,
                                    Latitude = executive.Latitude,
                                    Longitude = executive.Longitude
                                };

            return leftJoinQuery;
        }


        public async Task<IEnumerable<Distributor>> GetNonAssignedDistributors()
        {
            var result = await _unitOfWork.DistributorRepo.GetNonAssignedDistributorsAsync();
            return result;
        }




        public async Task<Distributor> GetDistributorById(string distributorId)
        {
            var result = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);
            if(result.IsDeleted == false)
            {
                return result;
            }
            return null;
        }

        public async Task<GetDistributor> GetDistributorPresignedUrlbyId(string distributorId)
        {
            var executiveResult = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);

            string bucketName = _s3Config.BucketName;
            string prefix = executiveResult.Image;
            var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);

            if (PresignedUrl.Any())
            {
                var exe = PresignedUrl.FirstOrDefault();
                var execuresoursze1 = _Mapper.Map<Distributor, GetDistributor>(executiveResult);
                execuresoursze1.PresignedUrl = exe.PresignedUrl;

                return execuresoursze1;
            }
            else
            {
                var execuresoursze1 = _Mapper.Map<Distributor, GetDistributor>(executiveResult);

                return execuresoursze1;

            }

        }


        public async Task<Distributor> GetDistributorDetailsById(string distributorId)
        {
            return await _unitOfWork.DistributorRepo.GetDistributorDetailsByIdAsync(distributorId);
        }

        public async Task<ResultResponse> CreateDistributorWithAssociationsAsync(Distributor distributor)
        {
            var response = new ResultResponse();

            try
            {
                distributor.Id = "NDIS" + new Random().Next(10000, 99999).ToString();

                await _unitOfWork.DistributorRepo.AddAsync(distributor);

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

        public async Task<ResultResponse> UpdateDistributor(Distributor distributor)

        {
            var response = new ResultResponse();
            try
            {
                _unitOfWork.DistributorRepo.Update(distributor);
                var created = await _unitOfWork.CommitAsync();
                if (created != 0)
                {
                    response.Message = "update Successful";
                    response.StatusCode = 200;
                }
            }
            catch (Exception)
            {

                response.Message = "update Failed";
                response.StatusCode = 401;
            }

            return response;
        }
        public async Task<ResultResponse> DeleteDistributor(string distributorId)
        {
            var response = new ResultResponse();

            try
            {
                var distributor = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);

                if (distributor != null)
                {
                    _unitOfWork.DistributorRepo.Update(distributor);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "DISTRIBUTOR NOT FOUND";
                    response.StatusCode = 404;
                }
            }
            catch (Exception)
            {
                response.Message = "Internal Server Error";
            }

            return response;
        }

        public async Task<IEnumerable<Distributor>> SearchDistributors(SearchModel search)
        {
            var distributors = await _unitOfWork.DistributorRepo.SearchDistributorAsync(search);
            return distributors;
        }

        public async Task<IEnumerable<Distributor>> SearchNonAssignedDistributors(SearchModel search)
        {
            var searchdistributors = await _unitOfWork.DistributorRepo.SearchNonAssignedDistributorsAsync(search);
            return searchdistributors;
        }
        private async Task<string?> GetPresignedUrlForImage(string imageName)
        {
            string bucketName = _s3Config.BucketName;
            var presignedUrls = await GetAllFilesAsync(bucketName, "");
            return presignedUrls.FirstOrDefault(p => p.Image == imageName)?.PresignedUrl;
        }
        public async Task<AngularDistributor> LoginAsync(Distributor credentials)
        {
            AngularDistributor response = new AngularDistributor();
            try
            {
                var user = await _unitOfWork.DistributorRepo.GetAllDistributorstAsync();

                var authenticatedUser = user.FirstOrDefault(u => u.UserName == credentials.UserName && u.Password == credentials.Password);

                var user1 = await _unitOfWork.DistributorRepo.GetAngularAsync(authenticatedUser.Id);

                if (authenticatedUser != null)
                {
                    response.Id = user1.Id;
                    response.FirstName = user1.FirstName;
                    response.LastName = user1.LastName;
                    response.Email = user1.Email;
                    response.Address = user1.Address;
                    response.MobileNumber = user1.MobileNumber;
                    response.Executives = user1.Executives;
                    response.ExeId = user1.ExeId;
                    response.PresignedUrl = await GetPresignedUrlForImage(user1.PresignedUrl);

                    response.Statuscode = 200;
                    response.Message = "LOGIN SUCCESSFUL";
                    return response;
                }
                else
                {
                    response.Statuscode = 401;
                    response.Message = "INVALID CREDENTIALS";
                    return response;

                }


            }

            catch (Exception)
            {
                response.Message = "INTERNAL SERVER ERROR";
                response.Statuscode = 500;
                return response;
            }



        }

        public async Task<ResultResponse> SoftDelete(string distributorId)
        {
            var response = new ResultResponse();

            try
            {
                var distributor = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);
                
                
                if (distributor != null)
                {
                    
                    distributor.IsDeleted = true;
                    _unitOfWork.DistributorRepo.Update(distributor);
                     await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "DISTRIBUTOR NOT FOUND";
                    response.StatusCode = 404;
                }
            }
            catch (Exception)
            {
                response.Message = "Internal Server Error";
            }

            return response;
        }
    }
}