using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IExecutiveService
    {
<<<<<<< HEAD
        Task<IEnumerable<Executive>> GetAllExecutive();
        Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive exec,
       string areaId, string cityId, string stateId);
=======
        Task<IEnumerable<Executive>> GetAllExecutives();
        Task<Executive> GetExecutiveById(string ExecutiveId);
        Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive executive,
        string areaId, string cityId, string stateId);

>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa
    }
}
