using Natural_Core.Models;
using Natural_Core.S3Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IDsrService 
    {
        //Task<Dsr> GetDsrDetailsById(string DsrId);
        //Task<DsrResponse> DeleteDsr(string dsrId);
        //Task<ResultResponse> CreateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails);
        Task<ProductResponse> CreateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails);
        Task<IEnumerable<Dsr>> GetAllDsr();
        Task<IEnumerable<DsrDistributor>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId);
        Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId);
        Task<IEnumerable<Product>> GetProductAsync();
        Task<IEnumerable<Dsr>> SearchDsr(Dsr search);
        //Task<IEnumerable<Dsrdetail>> GetDsrDetailsByDsrIdAsync(string dsrId);
        Task<IEnumerable<DsrProduct>> GetDsrDetailsByDsrIdAsync(string dsrId);
        Task<Dsr> GetDsrbyId(string dsrid);
        Task<DsrResponse> DeleteDsr(Dsr dsr, List<Dsrdetail> dsrdetails, string dsrId);
        Task<Dsr> GetbyId(string dsrid);
        Task<IEnumerable<Dsrdetail>> GetDetailTableByDsrIdAsync(string dsrId);
        Task<IEnumerable<GetProduct>> GetDetTableByDsrIdAsync(string dsrId);
        Task<ProductResponse> UpdateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails);


    }

}
