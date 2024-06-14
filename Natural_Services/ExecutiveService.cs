using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Natural_Core.S3_Models;
using AutoMapper;
using Natural_Core.Models.CustomModels;


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
            var bucketlist = await _unitOfWork.ExecutiveRepo.GetAllBucketAsync();
            return bucketlist;
        }


        //get all files //all images with presignedurl
        public async Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            var Allfilename = await _unitOfWork.ExecutiveRepo.GetAllFilesAsync(bucketName, prefix);
            return Allfilename;
        }

        //upload images to s3 bucket
        public async Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix)
        {

            string bucketName = _s3Config.BucketName;
            var metadata = await _unitOfWork.ExecutiveRepo.UploadFileAsync(file, bucketName, prefix);
            return metadata;

        }
       

        public async Task<IEnumerable<InsertUpdateModel>> GetAllExecutiveDetailsAsync(string? prefix, SearchModel? search)
        {

            var executives = await _unitOfWork.ExecutiveRepo.GetxecutiveAsync(search);

            string bucketName = _s3Config.BucketName;
            var presignedUrls = await GetAllFilesAsync(bucketName, prefix);

            var executivesList = from executive in executives
                                join presigned in presignedUrls
                                on executive.Image equals presigned.Image into newUrl
                                from sub in newUrl.DefaultIfEmpty()
                                select new InsertUpdateModel
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
                                    Latitude = executive.Latitude,
                                    Longitude = executive.Longitude,
                                    Image = sub?.PresignedUrl
                                };

            return executivesList;
        }


        public async Task<ExecutiveGetResource> GetExecutiveDetailsById(string DetailsId)
        {

            var executiveDetailById =  await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(DetailsId);
            string bucketName = _s3Config.BucketName;
            string? prefix = executiveDetailById.Image;
            var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);


            if (prefix != null)
            {
                var exe = PresignedUrl.FirstOrDefault();
                executiveDetailById.Image = exe.PresignedUrl;
                
            }
            return executiveDetailById;
        }
        public async Task<InsertUpdateModel> GetExecutiveDetailsPresignedUrlById(string DetailsId)
        {
            var executiveResults = await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(DetailsId);
            string bucketName = _s3Config.BucketName;
            string prefix = executiveResults.Image;
            var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);


            if (PresignedUrl.Any())
            {
                var exe = PresignedUrl.FirstOrDefault();
                var execuresoursze1 = _Mapper.Map<ExecutiveGetResource, InsertUpdateModel>(executiveResults);
                execuresoursze1.PresignedUrl = exe.PresignedUrl;




                return execuresoursze1;
            }
            else
            {
                var execuresoursze1 = _Mapper.Map<ExecutiveGetResource, InsertUpdateModel>(executiveResults);

                return execuresoursze1;

            }
        }

        public async Task<List<ExecutiveArea>> GetExectiveAreaDetailsByIdAsync(string id)
        {
            return await _unitOfWork.ExecutiveAreaRepository.GetExectiveAreaByIdAsync(id);

        }
        public async Task<Executive> GetExecutiveByIdAsync(string ExecutiveId)
        {
            var result = await _unitOfWork.ExecutiveRepo.GetExectiveTableByIdAsync(ExecutiveId);

            return result;
        }


        public async Task<InsertUpdateModel> GetExecutivePresignedUrlbyId(string ExecutiveId)
        {
           
            var executiveResult = await GetExecutiveByIdAsync(ExecutiveId);

            string bucketName = _s3Config.BucketName;
            string prefix = executiveResult.Image;
            var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);

            if (PresignedUrl.Any())
            {
                var exe = PresignedUrl.FirstOrDefault();
                var execuresoursze1 = _Mapper.Map<Executive, InsertUpdateModel>(executiveResult);
                execuresoursze1.PresignedUrl = exe.PresignedUrl;

                return execuresoursze1;
            }
            else
            {
                var execuresoursze1 = _Mapper.Map<Executive, InsertUpdateModel>(executiveResult);

                return execuresoursze1;

            }

        }

   
        public async Task<List<ExecutiveArea>> GetExecutiveAreaById(string ExecutiveId)
        {
            var result = await _unitOfWork.ExecutiveAreaRepository.GetExAreaByIdAsync(ExecutiveId);
           
            return result;
        }

        public async Task<ProductResponse> UpadateExecutive(Executive executive, List<ExecutiveArea> executiveArea, string Id)

        {

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {

                    var existingnotification = await GetExecutiveByIdAsync(Id);

                    existingnotification.FirstName = executive.FirstName;
                    existingnotification.LastName = executive.LastName;
                    existingnotification.Email = executive.Email;
                    existingnotification.MobileNumber = executive.MobileNumber;
                    existingnotification.Address = executive.Address;
                    existingnotification.City = executive.City;
                    existingnotification.State = executive.State;
                    existingnotification.UserName = executive.UserName;
                    existingnotification.Password = executive.Password;
                    existingnotification.Image = executive.Image;
                    existingnotification.Latitude = executive.Latitude;
                    existingnotification.Longitude = executive.Longitude;

                    _unitOfWork.ExecutiveRepo.Update(existingnotification);


                    var commit = await _unitOfWork.CommitAsync();



                    if(executiveArea.Count != 0)
                    {
                        var result = await GetExecutiveAreaById(Id);

                        var differentRecords = executiveArea.Except(result, new ExecutiveComparer()).ToList();

                        await _unitOfWork.ExecutiveAreaRepository.AddRangeAsync(differentRecords);

                        var created = await _unitOfWork.CommitAsync();

                        var deletingRecords = result.Except(executiveArea, new ExecutiveComparer()).ToList();


                        _unitOfWork.ExecutiveAreaRepository.RemoveRange(deletingRecords);
                        var deted = await _unitOfWork.CommitAsync();
                    }
                   
                    transaction.Commit();

                    response.Message = " Executive and ExectuiveArea Successful";
                    response.StatusCode = 200;
                    response.Id = Id;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;

                }

                return response;
            }


        }
   

        public async Task<ProductResponse> CreateExecutiveAsync(Executive executive, List<ExecutiveArea> executiveArea)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {
                    executive.Id = "NEXE" + new Random().Next(10000, 99999).ToString();


                    await _unitOfWork.ExecutiveRepo.AddAsync(executive);
                    var commit = await _unitOfWork.CommitAsync();

                    var create = executiveArea.Select(c => new ExecutiveArea
                    {

                        Executive = executive.Id,
                        Area = c.Area

                    }).ToList();

                    await _unitOfWork.ExecutiveAreaRepository.AddRangeAsync(create);
                    var commit1 = await _unitOfWork.CommitAsync();

                    transaction.Commit();

                    response.Message = " Executive and ExectiveArearea Insertion Successful";
                    response.StatusCode = 200;
                    response.Id = executive.Id;


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
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
                    exec.IsDeleted = true;
                    _unitOfWork.ExecutiveRepo.Update(exec);
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

        private async Task<string?> GetPresignedUrlForImage(string imageName)
        {
            string bucketName = _s3Config.BucketName;
            var presignedUrls = await GetAllFilesAsync(bucketName, ""); 
            return presignedUrls.FirstOrDefault(p => p.Image == imageName)?.PresignedUrl;
        }

        public async Task<AngularLoginResponse> LoginAsync(Executive credentials)
        {
            AngularLoginResponse response = new AngularLoginResponse();
            try
            {
                var user = await _unitOfWork.ExecutiveRepo.GetAllExecutiveAsync();

                var authenticatedUser = user.FirstOrDefault(u => u.UserName == credentials.UserName && u.Password == credentials.Password);

                if (authenticatedUser != null)
                {
                    response.Id = authenticatedUser.Id;
                    response.FirstName = authenticatedUser.FirstName;
                    response.LastName = authenticatedUser.LastName;
                    response.Email = authenticatedUser.Email;
                    response.Address = authenticatedUser.Address;
                    response.MobileNumber = authenticatedUser.MobileNumber;
                    response.UserName = authenticatedUser.UserName;

                    response.PresignedUrl = await GetPresignedUrlForImage(authenticatedUser.Image);
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

    }


    class ExecutiveComparer : IEqualityComparer<ExecutiveArea>
    
    {
        public bool Equals(ExecutiveArea x, ExecutiveArea y)
        {
            if (x.Area == y.Area)
                return true;

            return false;
        }

        public int GetHashCode(ExecutiveArea obj)
        {
            return obj.Area.GetHashCode();
        }


    }
}





