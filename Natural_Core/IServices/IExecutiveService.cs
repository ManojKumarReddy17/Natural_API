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
        Task<IEnumerable<GetExecutive>> GetAllExecutiveDetailsAsync(string? prefix);


        Task<Executive> GetExecutiveDetailsById(string DetailsId);
        Task<GetExecutive> GetExecutiveDetailsPresignedUrlById(string DetailsId);

        Task<Executive> GetExecutiveByIdAsync(string ExecutiveId);
        Task<GetExecutive> GetExecutivePresignedUrlbyId(string ExecutiveId);

        Task<ResultResponse> CreateExecutiveWithAssociationsAsync(Executive executive);

        Task<ResultResponse> DeleteExecutive(string executiveId);

        Task<ResultResponse> UpadateExecutive(Executive executive);

        Task<IEnumerable<Executive>> SearchExecutives(SearchModel search);

        public Task<AngularLoginResponse> LoginAsync(Executive credentials);

        Task<UploadResult> UploadFileAsync(IFormFile file, string? prefix);
        //Task UpadateExecutive(GetExecutive mappedexecutive);
    }
}
