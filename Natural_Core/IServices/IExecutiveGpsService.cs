using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IExecutiveGpsService
    {
        // Task<IEnumerable<ExecutiveGp>> InsertOrUpdateExecutive(string executiveId, string latitude, string longitude);
        Task<ResultResponse> CreateOrUpdate(ExecutiveGp executive);
        Task<ExecutiveGp> GetExeId(string executiveId);
        Task<IEnumerable<ExecutiveGp>> GetAllLatLung();

    }
}
