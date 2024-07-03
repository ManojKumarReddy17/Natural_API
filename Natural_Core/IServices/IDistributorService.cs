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
        Task<Pagination<Distributor>> GetAllDistributorDetailsAsync(string? prefix, SearchModel? search, bool? NonAssign,int? page);
        Task<Distributor> GetDistributorById(string distributorId);
        Task<GetDistributor> GetDistributorPresignedUrlbyId(string distributorId);
        Task<GetDistributor> GetDistributorDetailsById(string distributorId);
        Task<ResultResponse> CreateDistributorWithAssociationsAsync(Distributor distributor);

        Task<ResultResponse> DeleteDistributor(string distributorId);
        Task<ResultResponse> UpdateDistributor(Distributor distributor);
        Task<ResultResponse> SoftDelete(string distributorId);
        Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix);
        Task<IEnumerable<S3Config>> GetAllFilesAsync(string bucketName, string? prefix);
        Task<AngularDistributor> LoginAsync(Distributor credentials);

    }
}
