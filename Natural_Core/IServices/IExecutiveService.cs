using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IExecutiveService
    {
        Task<IEnumerable<Executive>> GetAllExecutives();
 
        Task<Executive> GetExecutiveDetailsById(string DetailsId);

        Task<Executive> GetExecutiveById(string ExecutiveId);

        Task<ResultResponse> CreateExecutiveWithAssociationsAsync(Executive executive);

        Task<ResultResponse> DeleteExecutive(string executiveId);

        Task<ResultResponse> UpadateExecutive(Executive executive);

        Task<IEnumerable<Executive>> SearchExecutives(SearchModel search);

        public Task<AngularLoginResponse> LoginAsync(Executive credentials);




    }
}
