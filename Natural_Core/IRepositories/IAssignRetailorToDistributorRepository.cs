using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IAssignRetailorToDistributorRepository : IRepository<RetailorToDistributor>
    {
        Task<IEnumerable<RetailorToDistributor>> GetRetailorsIdByDistributorIdAsync(string distributorId);
        Task<IEnumerable<RetailorToDistributor>> GetAssignedRetailorDetailsByDistributorIdAsync(string distributorId);
        Task<bool> DistributorAssignedToRetailor(List<string> retailorid);

    }
}
