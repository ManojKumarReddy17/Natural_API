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

        Task<ResultRepsonse> CreateExecutiveWithAssociationsAsync(Executive executive);
        Task<ResultRepsonse> DeleteExecutive(string executiveId);

        Task<ResultRepsonse> UpadateExecutive(Executive executive);

        Task<IEnumerable<Executive>> SearchExecutives(SearchModel search);






    }
}
