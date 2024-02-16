using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IAssignRetailorToDistributorRepository : IRepository<RetailorToDistributor>
    {
        Task<IEnumerable<RetailorToDistributor>> GetAssignedRetailorsIdByDistributorIdAsync(string distributorId);
        Task<IEnumerable<Retailor>> GetAssignedRetailorDetailsByDistributorIdAsync(string distributorId);
        Task<bool> IsRetailorAssignedToDistirbutor(List<string> retailorid);
        Task<RetailorToDistributor> DeleteRetailorAsync(string RetailorID, string DistributorId);
    }
}
