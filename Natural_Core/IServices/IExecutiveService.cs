using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IExecutiveService
    {
        Task<IEnumerable<Executive>> GetAllExecutive();
        Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive exec,
       string areaId, string cityId, string stateId);
    }
}
