using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Natural_Core.Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IRepositories
{
    public interface IDistributorRepository : IRepository<Distributor>
    {
        Task<List<Distributor>> GetAllDistributorstAsync();
        Task<Distributor> GetDistributorDetailsByIdAsync(string id);
        Task<IEnumerable<Distributor>> SearchDistributorAsync(SearchModel search);

        Task<IEnumerable<Distributor>> GetNonAssignedDistributorsAsync();
        Task<IEnumerable<Distributor>> SearchNonAssignedDistributorsAsync(SearchModel search);
        Task<IEnumerable<string>> GetAllBucketAsync();
        Task<UploadResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix);
        Task<IEnumerable<Natural_Core.S3Models.S3Config>> GetAllFilesAsync(string bucketName, string? prefix);

        Task<AngularDistributor> GetAngularAsync(string DistributorId);




    }
}
