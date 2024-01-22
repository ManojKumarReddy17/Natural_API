
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IAssignDistributorToExecutiveRepository : IRepository<DistributorToExecutive>
    {

        Task<IEnumerable<DistributorToExecutive>> GetAssignedDistributorByExecutiveId(string ExecutiveId);
        Task<IEnumerable<DistributorToExecutive>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId);
        Task<bool> IsExecutiveAssignedToDistributor(List<string> distributorIds);


    }
}
