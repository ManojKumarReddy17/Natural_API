
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

        Task<IEnumerable<DistributorToExecutive>> GetAssignedDistributorByExecutiveId(string ExecutiveId);
        Task<IEnumerable<Distributor>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId);
        Task<bool> IsExecutiveAssignedToDistributor(List<string> distributorIds);
        Task<DistributorToExecutive>DeleteDistributor(string distributorId, string ExecutiveId);



    }
}
