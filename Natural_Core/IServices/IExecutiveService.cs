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
        Task<Executive> GetExecutiveById(string ExecutiveId);
        Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive executive,
        string areaId, string cityId, string stateId);

    }
}
