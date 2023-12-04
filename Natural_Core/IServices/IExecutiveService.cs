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

        Task<IEnumerable<Executive>> GetAll();
        Task<Executive> GetExecutiveById(string ExecutiveId);

        Task<Executive> GetId(string ExecutiveId);
        Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive executive,
        string areaId, string cityId, string stateId);
        Task<Executive> CreateExecutive(Executive executive);

        Task UpadateExecutive (Executive existing, Executive  executive);
    }
}
