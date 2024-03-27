using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Http;
using Natural_Core.S3Models;
using Microsoft.Extensions.Options;
using Natural_Core.S3_Models;
using System.Linq;
using AutoMapper;

namespace Natural_Services
{
    public class ExecutiveService : IExecutiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly S3Config _s3Config;
        private readonly IMapper _Mapper;
        public ExecutiveService(IUnitOfWork unitOfWork, IOptions<S3Config> s3Config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _s3Config = s3Config.Value;
            _Mapper = mapper;
        }

        //get bucket names//
        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            var bucketlist = await _unitOfWork.ProductRepository.GetAllBucketAsync();
            return bucketlist;
        }


        //get all files //all images with presignedurl
        public async Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            var Allfilename = await _unitOfWork.ProductRepository.GetAllFilesAsync(bucketName, prefix);
            return Allfilename;
        }

        //upload images to s3 bucket
        public async Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix)
        {

            string bucketName = _s3Config.BucketName;
            var metadata = await _unitOfWork.ExecutiveRepo.UploadFileAsync(file, bucketName, prefix);
            return metadata;

        }

        public async Task<IEnumerable<Executive>> GetAllExecutives()
        {
            var result = await _unitOfWork.ExecutiveRepo.GetAllExecutiveAsync();
            return result;
        }

        public async Task<IEnumerable<GetExecutive>> GetAllExecutiveDetailsAsync(string? prefix)
        {

            var executives = await GetAllExecutives();

            string bucketName = _s3Config.BucketName;
            var presignedUrls = await GetAllFilesAsync(bucketName, prefix);

            var leftJoinQuery = from executive in executives
                                join presigned in presignedUrls
                                on executive.Image equals presigned.Image into newUrl
                                from sub in newUrl.DefaultIfEmpty()
                                select new GetExecutive
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
                                    PresignedUrl = sub?.PresignedUrl
                                };

            return leftJoinQuery;
        }



        public async Task<Executive> GetExecutiveDetailsById(string DetailsId)
        {
            return await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(DetailsId);
        }
        public async Task<GetExecutive> GetExecutiveDetailsPresignedUrlById(string DetailsId)
        {
            var executiveResults = await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(DetailsId);
            string bucketName = _s3Config.BucketName;
            string prefix = executiveResults.Image;
            var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);

            if (PresignedUrl.Any())
            {
                var exe = PresignedUrl.FirstOrDefault();
                var execuresoursze1 = _Mapper.Map<Executive, GetExecutive>(executiveResults);
                execuresoursze1.PresignedUrl = exe.PresignedUrl;

                return execuresoursze1;
            }
            else
            {
                var execuresoursze1 = _Mapper.Map<Executive, GetExecutive>(executiveResults);

                return execuresoursze1;

            }
        }


        public async Task<Executive> GetExecutiveByIdAsync(string ExecutiveId)
        {
            return await _unitOfWork.ExecutiveRepo.GetByIdAsync(ExecutiveId);
        }

        public async Task<GetExecutive> GetExecutivePresignedUrlbyId(string ExecutiveId)
        {
            var executiveResult = await _unitOfWork.ExecutiveRepo.GetByIdAsync(ExecutiveId);

            string bucketName = _s3Config.BucketName;
            string prefix = executiveResult.Image;
            var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);

            if (PresignedUrl.Any())
            {
                var exe = PresignedUrl.FirstOrDefault();
                var execuresoursze1 = _Mapper.Map<Executive, GetExecutive>(executiveResult);
                execuresoursze1.PresignedUrl = exe.PresignedUrl;

                return execuresoursze1;
            }
            else
            {
                var execuresoursze1 = _Mapper.Map<Executive, GetExecutive>(executiveResult);

                return execuresoursze1;

            }

        }
        
        public async Task<ResultResponse> UpadateExecutive(Executive executive)
        {
            var response = new ResultResponse();
            try
            {
                _unitOfWork.ExecutiveRepo.Update(executive);
                var updated = await _unitOfWork.CommitAsync();
                if (updated != 0)
                {

                    response.Message = "updatesuceesfull";
                    response.StatusCode = 200;
                }
            }
            catch (Exception)
            {
                response.Message = "Failed";
                response.StatusCode = 500;
            }

            return (response);
        }



        public async Task<ResultResponse> CreateExecutiveWithAssociationsAsync(Executive executive)
        {
            {
                var response = new ResultResponse();

                try
                {

                    executive.Id = "NEXE" + new Random().Next(10000, 99999).ToString();


                    await _unitOfWork.ExecutiveRepo.AddAsync(executive);


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
        }

        public async Task<ResultResponse> DeleteExecutive(string executiveId)
        {
            var response = new ResultResponse();

            try
            {
                var exec = await _unitOfWork.ExecutiveRepo.GetByIdAsync(executiveId);

                if (exec != null)
                {
                    _unitOfWork.ExecutiveRepo.Remove(exec);
                    await _unitOfWork.CommitAsync();
                    response.Message = "Successfully Deleted";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "Executive Not Found";
                    response.StatusCode = 404;
                }
            }
            catch (Exception)
            {
                response.Message = "Internal Server Error";
            }

            return response;
        }

        public async Task<IEnumerable<Executive>> SearchExecutives(SearchModel search)
        {
            var exec = await _unitOfWork.ExecutiveRepo.SearchExecutiveAsync(search);
            return exec;
        }
    }
}






