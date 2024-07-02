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
        Task<IEnumerable<Dsr>> GetAllDsrAsync(EdittDSR? search);
        Task<IEnumerable<DsrDistributor>> GetAssignedDistributorDetailsByExecutiveId(string ExecutiveId);
        Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId);
        Task<Dsr> GetDsrbyId(string dsrid);
       Task<IEnumerable<Dsr>> GetRetailorDetailsByDistributorId(string distributorId);
        Task<IEnumerable<Dsr>> GetRetailorDetailsByDate(string Id, DateTime date);
        Task<IEnumerable<Dsr>> GetRetailorDetailsByExecutiveId(string executiveId);

        Task<IEnumerable<DSRretailorDetails>> GetRetailorDetailsbyId(string Id);
        Task<IEnumerable<DSRretailorDetails>> GetRetailorDetailsbyDistributorId(string distributorId);
    }
}
