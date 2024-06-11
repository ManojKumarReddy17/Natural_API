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
using Microsoft.Extensions.Logging;
using Serilog;


namespace Natural_Services
{
    public class ExecutiveService : IExecutiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly S3Config _s3Config;
        private readonly IMapper _Mapper;
        private readonly ILogger<ExecutiveService> _logger;
        public ExecutiveService(IUnitOfWork unitOfWork, IOptions<S3Config> s3Config, IMapper mapper,ILogger<ExecutiveService>logger)
        {
            _unitOfWork = unitOfWork;
            _s3Config = s3Config.Value;
            _Mapper = mapper;
            _logger = logger;
        }

        //get bucket names//
        public async Task<IEnumerable<string>> GetAllBucketAsync()
        {
            Log.Information("Starting GetAllBucketAsync method");
            try
            {



                var bucketlist = await _unitOfWork.ExecutiveRepo.GetAllBucketAsync();
                Log.Information("Successfully retrieved bucket list");

                return bucketlist;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving bucket list");
                throw;
            }
        }


        //get all files //all images with presignedurl
        public async Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix)
        {
            Log.Information("Starting GetAllFilesAsync method for bucket: {BucketName}, prefix: {Prefix}", bucketName, prefix);
            try
            {


                var Allfilename = await _unitOfWork.ExecutiveRepo.GetAllFilesAsync(bucketName, prefix);
                Log.Information("Successfully retrieved files for bucket: {BucketName}, prefix: {Prefix}", bucketName, prefix);

                return Allfilename;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving files for bucket: {BucketName}, prefix: {Prefix}", bucketName, prefix);
                throw;
            }
        }

        //upload images to s3 bucket
        public async Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix)
        {
            Log.Information("Starting UploadFileAsync method");
            try
            {


                string bucketName = _s3Config.BucketName;
                var metadata = await _unitOfWork.ExecutiveRepo.UploadFileAsync(file, bucketName, prefix);
                Log.Information("Successfully uploaded file to bucket: {BucketName}, prefix: {Prefix}", bucketName, prefix);

                return metadata;
            }
             catch(Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while uploading file");
                throw;
            }

        }
       

        public async Task<IEnumerable<InsertUpdateModel>> GetAllExecutiveDetailsAsync(string? prefix, SearchModel? search)
        {
            Log.Information("Starting GetAllExecutiveDetailsAsync method");
            try
            {



                var executives = await _unitOfWork.ExecutiveRepo.GetxecutiveAsync(search);

                string bucketName = _s3Config.BucketName;
                Log.Information("Retrieving presigned URLs for bucket: {BucketName}, prefix: {Prefix}", bucketName, prefix);

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
                Log.Information("Successfully retrieved executive details with presigned URLs");


                return executivesList;
            }
             catch (Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving executive details");
                throw;
            }
        }


        public async Task<ExecutiveGetResource> GetExecutiveDetailsById(string DetailsId)
        {
            Log.Information("Starting GetExecutiveDetailsById method for Executive ID: {DetailsId}", DetailsId);

            try
            {


                var executiveDetailById = await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(DetailsId);
                string bucketName = _s3Config.BucketName;
                string prefix = executiveDetailById.Image;
                Log.Information("Retrieving presigned URL for image with prefix: {Prefix}", prefix);

                var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);


                if (PresignedUrl.Any())
                {
                    var exe = PresignedUrl.FirstOrDefault();
                    executiveDetailById.Image = exe.PresignedUrl;

                }
                Log.Information("Successfully retrieved executive details for Executive ID: {DetailsId}", DetailsId);

                return executiveDetailById;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving executive details for Executive ID: {DetailsId}", DetailsId);
                throw;
            }
        }
        public async Task<InsertUpdateModel> GetExecutiveDetailsPresignedUrlById(string DetailsId)
        {
            Log.Information("Starting GetExecutiveDetailsPresignedUrlById method for Executive ID: {DetailsId}", DetailsId);
            try
            {


                var executiveResults = await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(DetailsId);
                string bucketName = _s3Config.BucketName;
                string prefix = executiveResults.Image;
                Log.Information("Retrieving presigned URL for image with prefix: {Prefix}", prefix);

                var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);


                if (PresignedUrl.Any())
                {
                    var exe = PresignedUrl.FirstOrDefault();
                    var execuresoursze1 = _Mapper.Map<ExecutiveGetResource, InsertUpdateModel>(executiveResults);
                    execuresoursze1.PresignedUrl = exe.PresignedUrl;
                    Log.Information("Presigned URL retrieved successfully for Executive ID: {DetailsId}", DetailsId);





                    return execuresoursze1;
                }
                else
                {
                    var execuresoursze1 = _Mapper.Map<ExecutiveGetResource, InsertUpdateModel>(executiveResults);
                    Log.Warning("No presigned URL found for Executive ID: {DetailsId}", DetailsId);


                    return execuresoursze1;

                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving executive details with presigned URL for Executive ID: {DetailsId}", DetailsId);
                throw;
            }
        }

        public async Task<List<ExecutiveArea>> GetExectiveAreaDetailsByIdAsync(string id)
        {
            Log.Information("Starting GetExectiveAreaDetailsByIdAsync method for Executive ID: {Id}", id);
            try
            {


                return await _unitOfWork.ExecutiveAreaRepository.GetExectiveAreaByIdAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving executive area details for Executive ID: {Id}", id);
                throw;
            }

        }
        public async Task<Executive> GetExecutiveByIdAsync(string ExecutiveId)
        {
            Log.Information("Starting GetExecutiveByIdAsync method for Executive ID: {ExecutiveId}", ExecutiveId);
            try
            {


                var result = await _unitOfWork.ExecutiveRepo.GetExectiveTableByIdAsync(ExecutiveId);
                Log.Information("Successfully retrieved executive details for Executive ID: {ExecutiveId}", ExecutiveId);


                return result;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving executive details for Executive ID: {ExecutiveId}", ExecutiveId);
                throw;
            }
        }


        public async Task<InsertUpdateModel> GetExecutivePresignedUrlbyId(string ExecutiveId)
        {
            Log.Information("Starting GetExecutivePresignedUrlbyId method for Executive ID: {ExecutiveId}", ExecutiveId);
            try
            {


                var executiveResult = await GetExecutiveByIdAsync(ExecutiveId);

                string bucketName = _s3Config.BucketName;
                string prefix = executiveResult.Image;
                Log.Information("Retrieving presigned URL for image with prefix: {Prefix}", prefix);

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
            catch (Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving executive details with presigned URL for Executive ID: {ExecutiveId}", ExecutiveId);
                throw;
            }

        }

   
        public async Task<List<ExecutiveArea>> GetExecutiveAreaById(string ExecutiveId)
        {
            Log.Information("Starting GetExecutiveAreaById method for Executive ID: {ExecutiveId}", ExecutiveId);

            try
            {


                var result = await _unitOfWork.ExecutiveAreaRepository.GetExAreaByIdAsync(ExecutiveId);
                Log.Information("Successfully retrieved executive area details for Executive ID: {ExecutiveId}", ExecutiveId);


                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while retrieving executive area details for Executive ID: {ExecutiveId}", ExecutiveId);
                throw;
            }
        }

        public async Task<ProductResponse> UpadateExecutive(Executive executive, List<ExecutiveArea> executiveArea, string Id)

        {
            Log.Information("Starting UpadateExecutive method for Executive ID: {ExecutiveId}", Id);


            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {

                    var existingnotification = await GetExecutiveByIdAsync(Id);
                    Log.Information("Retrieved existing executive details for Executive ID: {ExecutiveId}", Id);


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
                    Log.Information("Committed changes for Executive ID: {ExecutiveId}", Id);



                    if (executiveArea.Count != 0)
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
                    Log.Information("Transaction committed successfully for Executive ID: {ExecutiveId}", Id);


                    response.Message = " Executive and ExectuiveArea Successful";
                    response.StatusCode = 200;
                    response.Id = Id;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error(ex.Message, "ExecutiveService" + "Error occurred while updating executive details for Executive ID: {ExecutiveId}", Id);
                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;

                }

                return response;
            }


        }
   

        public async Task<ProductResponse> CreateExecutiveAsync(Executive executive, List<ExecutiveArea> executiveArea)
        {
            Log.Information("Starting CreateExecutiveAsync method");

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                var response = new ProductResponse();
                try
                {
                    executive.Id = "NEXE" + new Random().Next(10000, 99999).ToString();
                    Log.Information("Generated new Executive ID: {ExecutiveId}", executive.Id);


                    await _unitOfWork.ExecutiveRepo.AddAsync(executive);
                    Log.Information("Added executive to repository");

                    var commit = await _unitOfWork.CommitAsync();
                    Log.Information("Committed executive to database");


                    var create = executiveArea.Select(c => new ExecutiveArea
                    {

                        Executive = executive.Id,
                        Area = c.Area

                    }).ToList();

                    await _unitOfWork.ExecutiveAreaRepository.AddRangeAsync(create);
                    Log.Information("Added executive areas to repository");

                    var commit1 = await _unitOfWork.CommitAsync();
                    Log.Information("Committed executive areas to database");


                    transaction.Commit();
                    Log.Information("Transaction committed successfully");


                    response.Message = " Executive and ExectiveArearea Insertion Successful";
                    response.StatusCode = 200;
                    response.Id = executive.Id;


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Error(ex.Message, "ExecutiveService" + "Error occurred while creating executive and areas");
                    response.Message = "Insertion Failed";
                    response.StatusCode = 401;

                }

                return response;
            }




        }


       

        public async Task<ResultResponse> DeleteExecutive(string executiveId)
        {
            Log.Information("Starting DeleteExecutive method for ExecutiveId: {ExecutiveId}", executiveId);

            var response = new ResultResponse();

            try
            {
                var exec = await _unitOfWork.ExecutiveRepo.GetByIdAsync(executiveId);
                Log.Information("Fetched Executive details for ExecutiveId: {ExecutiveId}", executiveId);


                if (exec != null)
                {
                    exec.IsDeleted = true;
                    _unitOfWork.ExecutiveRepo.Update(exec);
                    await _unitOfWork.CommitAsync();
                    Log.Information("Marked Executive as deleted and committed to database for ExecutiveId: {ExecutiveId}", executiveId);
                    response.Message = "Successfully Deleted";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "Executive Not Found";
                    Log.Warning("Executive not found for ExecutiveId: {ExecutiveId}", executiveId);

                    response.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Internal Server Error";
                Log.Error(ex.Message, "ExecutiveService" + "Error occurred while deleting Executive with ExecutiveId: {ExecutiveId}", executiveId);

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
            Log.Information("Starting LoginAsync method for UserName: {UserName}", credentials.UserName);

            AngularLoginResponse response = new AngularLoginResponse();
            try
            {
                var user = await _unitOfWork.ExecutiveRepo.GetAllExecutiveAsync();
                Log.Information("Fetched all executives from repository");


                var authenticatedUser = user.FirstOrDefault(u => u.UserName == credentials.UserName && u.Password == credentials.Password);

                if (authenticatedUser != null)
                {
                    Log.Information("Authenticated user found for UserName: {UserName}", credentials.UserName);
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
                    Log.Information("Login successful for UserName: {UserName}", credentials.UserName);

                    return response;
                }
                else
                {
                    response.Statuscode = 401;
                    response.Message = "INVALID CREDENTIALS";
                    Log.Warning("Invalid credentials provided for UserName: {UserName}", credentials.UserName);
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = "INTERNAL SERVER ERROR";
                Log.Error(ex.Message,"ExecutiveService"+ "Error occurred while logging in UserName: {UserName}", credentials.UserName);
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





