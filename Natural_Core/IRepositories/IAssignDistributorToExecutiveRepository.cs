
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Natural_Core.IRepositories
{
    public interface IAssignDistributorToExecutiveRepository : IRepository<DistributorToExecutive>
    {

        Task<IEnumerable<DistributorToExecutive>> GetAssignedDistributorByExecutiveIdAsync(string ExecutiveId);
        Task<IEnumerable<Distributor>> GetAssignedDistributorDetailsByExecutiveIdAsync(string ExecutiveId);
        Task<bool> IsDistirbutorAssignedToExecutiveAsync(List<string> distributorIds);
        Task<DistributorToExecutive>DeleteDistributorAsync(string distributorId, string ExecutiveId);



    }
}
