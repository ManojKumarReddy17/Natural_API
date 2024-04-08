using Microsoft.AspNetCore.Http;
using Natural_Core.Models;
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

        //Task<IEnumerable<Executive>> GetAllExecutives();
        //Task<IEnumerable<GetExecutive>> GetAllExecutiveDetailsAsync(string? prefix);
        Task<IEnumerable<InsertUpdateModel>> GetAllExecutiveDetailsAsync(string? prefix);

        //Task<Executive> GetExecutiveDetailsById(string DetailsId);
        Task<InsertUpdateModel> GetExecutiveDetailsPresignedUrlById(string DetailsId);

        Task<Executive> GetExecutiveByIdAsync(string ExecutiveId); //get table data of executive
        Task<InsertUpdateModel> GetExecutivePresignedUrlbyId(string ExecutiveId);

        Task<IEnumerable<Executive>> GetAllExecutives();

        Task<Executive> GetExecutiveDetailsById(string DetailsId); //get detail table data of executive

        //Task<Executive> GetExecutiveById(string ExecutiveId); //get table data of executive


        Task<List<ExecutiveArea>> GetExecutiveAreaById(string ExecutiveId); //get table data of executivearea

        //Task<ResultResponse> CreateExecutiveWithAssociationsAsync(Executive executive);

        Task<ProductResponse> CreateExecutiveAsync(Executive executive, List<ExecutiveArea> executiveArea);

        Task<ResultResponse> DeleteExecutive(string executiveId);

        //Task<ResultResponse> UpadateExecutive(Executive executive);

        Task<ProductResponse> UpadateExecutive(Executive executive, List<ExecutiveArea> executiveArea, string Id);

        //Task<IEnumerable<Executive>> SearchExecutives(SearchModel search);


        Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix);
        //Task UpadateExecutive(GetExecutive mappedexecutive);

        Task<IEnumerable<InsertUpdateModel>> SearchExecutives(SearchModel search);

        Task<AngularLoginResponse> LoginAsync(Executive credentials);

        Task<List<ExecutiveArea>> GetExectiveAreaDetailsByIdAsync(string id);  //get detail table data of executiveArea

        Task<List<InsertUpdateModel>> GetxecutiveAsync();

    }
}
