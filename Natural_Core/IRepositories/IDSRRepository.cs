using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IDsrRepository :IRepository<Dsr>
    {

        Task<IEnumerable<Product>> GetProductDetailsByDsrIdAsync(string dsrId);
       Task<IEnumerable<Dsr>> GetAllDsrAsync();
        Task<Dsr> GetDetails(string dsrid);
        Task<IEnumerable<DsrDistributor>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId);
        Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId);
        Task<IEnumerable<Dsr>> SearchDsr(Dsr search);
    }
}
