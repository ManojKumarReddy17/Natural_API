using Microsoft.AspNetCore.Http;
using Natural_Core.Models;
using Natural_Core.Models.CustomModels;
using Natural_Core.S3_Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IExecutiveService
    {
        Task<IEnumerable<InsertUpdateModel>> GetAllExecutiveDetailsAsync(string? prefix, SearchModel? search);

        Task<InsertUpdateModel> GetExecutiveDetailsPresignedUrlById(string DetailsId);

        Task<Models.ExecutiveGetResourcecs> GetExecutiveByIdAsync(string ExecutiveId); //get table data of executive
        Task<InsertUpdateModel> GetExecutivePresignedUrlbyId(string ExecutiveId);

        Task<ExecutiveGetResource> GetExecutiveDetailsById(string DetailsId); //get detail table data of executive

        Task<List<ExecutiveArea>> GetExecutiveAreaById(string ExecutiveId); //get table data of executivearea

        Task<ProductResponse> CreateExecutiveAsync(Models.ExecutiveGetResourcecs executive, List<ExecutiveArea> executiveArea);

        Task<ResultResponse> DeleteExecutive(string executiveId);

        Task<ProductResponse> UpadateExecutive(Models.ExecutiveGetResourcecs executive, List<ExecutiveArea> executiveArea, string Id);


        Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix);

        Task<AngularLoginResponse> LoginAsync(Models.ExecutiveGetResourcecs credentials);

        Task<List<ExecutiveArea>> GetExectiveAreaDetailsByIdAsync(string id);  

    }
}
