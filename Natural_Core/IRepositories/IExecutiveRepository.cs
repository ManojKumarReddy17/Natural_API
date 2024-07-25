using Microsoft.AspNetCore.Http;
using Natural_Core.Models;
using Natural_Core.Models.CustomModels;
using Natural_Core.S3_Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IExecutiveRepository : IRepository<Executive>
    {
        Task<IEnumerable<Executive>> GetAllExecutiveAsync();
        Task<ExecutiveGetResource> GetWithExectiveByIdAsync(string id);
        Task<IEnumerable<string>> GetAllBucketAsync();
        Task<UploadResult> UploadFileAsync(IFormFile file, string bucketName, string? prefix);
        Task<IEnumerable<Natural_Core.S3Models.S3Config>> GetAllFilesAsync(string bucketName, string? prefix);
        Task<Executive> GetExectiveTableByIdAsync(string id);
        Task<List<InsertUpdateModel>> GetxecutiveAsync(SearchModel? search);

        Task<IEnumerable<GetExecutivesByArea>> GetExecutiveDetailsByAreaId(string areaId);

        Task<IEnumerable<Area>> GetAreaIdByExecutiveDetails(string exid);

    }
}
