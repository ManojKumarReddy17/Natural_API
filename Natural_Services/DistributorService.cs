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
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;


#nullable disable

namespace Natural_Services
{
    public class DistributorService : IDistributorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly S3Config _s3Config;
        private readonly IMapper _Mapper;
        private readonly PaginationSettings _paginationSettings;
         
        public DistributorService(IUnitOfWork unitOfWork, IOptions<S3Config> s3Config, IMapper mapper,IOptions<PaginationSettings> paginationsetting)
        {
            _unitOfWork = unitOfWork;
            _s3Config = s3Config.Value;
            _Mapper = mapper;
            _paginationSettings = paginationsetting.Value;
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

        public async Task<Pagination<Distributor>> GetAllDistributorDetailsAsync(string? prefix, SearchModel? search, bool? nonAssign, int? page)
        {
            // Fetch all distributors based on the search criteria and nonAssign flag
            var distributors = await _unitOfWork.DistributorRepo.GetAllDistributorstAsync(search, nonAssign);

            // Fetch all presigned URLs from S3 bucket based on the prefix
            string bucketName = _s3Config.BucketName;
            var presignedUrls = await GetAllFilesAsync(bucketName, prefix);

           

            // Join distributors with presigned URLs based on Image property
            var leftJoinQuery = from distributor in distributors
                                join presigned in presignedUrls
                                on distributor.Image equals presigned.Image into newUrl
                                from sub in newUrl.DefaultIfEmpty()
                                select new Distributor
                                {
                                    Id = distributor.Id,
                                    FirstName = distributor.FirstName,
                                    LastName = distributor.LastName,
                                    MobileNumber = distributor.MobileNumber,
                                    Address = distributor.Address,
                                    //Area = distributor.Area,
                                    Email = distributor.Email,
                                    UserName = distributor.UserName,
                                    Password = distributor.Password,
                                    City = distributor.City,
                                    State = distributor.State,
                                    Image = sub?.PresignedUrl,
                                    Latitude = distributor.Latitude,
                                    Longitude = distributor.Longitude,
                                    Executive = distributor.Executive
                                };

            // Apply pagination
            if (page > 0)
            {
                // Define page size from the pagination settings
                var pageSize = _paginationSettings.PageSize;
                var totalItems = distributors.Count();
                var totalPageCount = (int)Math.Ceiling(totalItems / (double)pageSize);
                var paginatedItems = leftJoinQuery.Skip((int)((page - 1) * pageSize)).Take(pageSize).ToList();

                // Return paginated result
                return new Pagination<Distributor>
                {
                    TotalPageCount = totalPageCount,
                    TotalItems = totalItems,
                    Items = paginatedItems
                };
            }
            else
            {
                var paginatedItems = leftJoinQuery;
                return new Pagination<Distributor>
                {
                    Items = paginatedItems.ToList(),
                };
            }
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
                execuresoursze1.Image = exe.PresignedUrl;

                return execuresoursze1;
            }
            else
            {
                var execuresoursze1 = _Mapper.Map<Distributor, GetDistributor>(executiveResult);

                return execuresoursze1;

            }

        }


        public async Task<GetDistributor> GetDistributorDetailsById(string distributorId)
        {
            var getDistributorById = await _unitOfWork.DistributorRepo.GetDistributorDetailsByIdAsync(distributorId);
            string bucketName = _s3Config.BucketName;
            string? prefix = getDistributorById.Image;
            var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);

            if (prefix != null)
            {
                var exe = PresignedUrl.FirstOrDefault();

                getDistributorById.Image = exe.PresignedUrl;
            }

                return getDistributorById;

        }

        public async Task<ResultResponse> CreateDistributorWithAssociationsAsync(Distributor distributor)
        {
            var response = new ResultResponse();

            try
            {
                distributor.Id = "NDIS" + new Random().Next(10000, 99999).ToString();

                await _unitOfWork.DistributorRepo.AddAsync(distributor);

                Login loginDistributor = new Login();
                loginDistributor.Id = distributor.Id;
                loginDistributor.UserName = distributor.UserName;
                loginDistributor.Password = distributor.Password;
                loginDistributor.FirstName = distributor.FirstName;
                loginDistributor.LastName = distributor.LastName;
                loginDistributor.IsAdmin = false;
                await _unitOfWork.Login.AddAsync(loginDistributor);

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
                IEnumerable<Login> loginDetails = await _unitOfWork.Login.GetAllAsync();
                Login loginDistributor =loginDetails.Where(x => x.Id == distributor.Id).FirstOrDefault();
                if (loginDistributor != null)
                {
                    loginDistributor.Id = distributor.Id;
                    loginDistributor.UserName = distributor.UserName;
                    loginDistributor.Password = distributor.Password;
                    loginDistributor.FirstName = distributor.FirstName;
                    loginDistributor.LastName = distributor.LastName;
                    loginDistributor.IsAdmin = false;
                }
                _unitOfWork.Login.Update(loginDistributor);
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
                var user = await _unitOfWork.DistributorRepo.GetAllAsync();

                var authenticatedUser = user.FirstOrDefault(u => u.UserName == credentials.UserName && u.Password == credentials.Password);

                var user1 = await _unitOfWork.DistributorRepo.GetAngularAsync(authenticatedUser.Id);

                if (authenticatedUser != null)
                {
                    response.Id = authenticatedUser.Id;
                    response.FirstName = authenticatedUser.FirstName;
                    response.LastName = authenticatedUser.LastName;
                    response.Email = authenticatedUser.Email;
                    response.Address = authenticatedUser.Address;
                    response.MobileNumber = authenticatedUser.MobileNumber;
                    //response.Area = authenticatedUser.Area;
                    response.Executives = user1.Executives;
                    response.UserName = credentials.UserName;
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
                    Login login = new Login();
                    login.Id= distributorId;
                    login.UserName = distributor.UserName;
                    login.Password = distributor.Password;
                    login.FirstName = distributor.FirstName;
                    login.LastName = distributor.LastName;
                    login.IsAdmin = false;
                    login.IsDeleted = true;
                    _unitOfWork.Login.Update(login);
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