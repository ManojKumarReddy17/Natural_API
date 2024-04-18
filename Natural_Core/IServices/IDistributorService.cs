using AutoMapper.Mappers;
using Microsoft.AspNetCore.Http;
using Natural_Core.Models;
using Natural_Core.S3_Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IServices
{
    public interface IDistributorService
    {
        //Task<IEnumerable<Distributor>> GetAllDistributors();
        Task<IEnumerable<GetDistributor>> GetAllDistributorDetailsAsync(string? prefix);
        Task<Distributor> GetDistributorById(string distributorId);
        Task<GetDistributor> GetDistributorPresignedUrlbyId(string distributorId);
        Task<Distributor> GetDistributorDetailsById(string distributorId);
        Task<ResultResponse> CreateDistributorWithAssociationsAsync(Distributor distributor);

        Task<ResultResponse> DeleteDistributor(string distributorId);
        Task<ResultResponse> UpdateDistributor(Distributor distributor);
        Task<IEnumerable<Distributor>> SearchDistributors(SearchModel search);

        Task<IEnumerable<Distributor>> GetNonAssignedDistributors();
        Task<IEnumerable<Distributor>> SearchNonAssignedDistributors(SearchModel search);
        public Task<AngularLoginResponse> LoginAsync(Distributor credentials);
        Task<ResultResponse> SoftDelete(string distributorId);
        Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix);
        Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix);


    }
}
