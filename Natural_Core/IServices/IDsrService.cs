using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IDsrService 
    {
        Task<Dsr> GetDsrDetailsById(string DsrId);
        Task<DsrResponse> DeleteDsr(string dsrId);
        Task<ResultResponse> CreateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails);
        Task<IEnumerable<Dsr>> GetAllDsr();
        Task<IEnumerable<DsrDistributor>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId);
        Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId);
        Task<IEnumerable<Product>> GetProductAsync();
        Task<IEnumerable<Dsr>> SearchDsr(Dsr search);
    }
}
