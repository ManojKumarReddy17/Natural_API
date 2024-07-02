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
        Task<ProductResponse> CreateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails);

        Task<IEnumerable<DsrDistributor>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId);
        Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId);
        Task<IEnumerable<Dsr>> GetAllDsr(EdittDSR? search);
        Task<IEnumerable<DsrProduct>> GetDsrDetailsByDsrIdAsync(string dsrId);
        Task<Dsr> GetDsrbyId(string dsrid);
        Task<DsrResponse> DeleteDsr(Dsr dsr, List<Dsrdetail> dsrdetails, string dsrId);
        Task<Dsr> GetbyId(string dsrid);
        Task<IEnumerable<Dsrdetail>> GetDetailTableByDsrIdAsync(string dsrId);
        Task<IEnumerable<GetProduct>> GetDetTableByDsrIdAsync(string dsrId);
        Task<ProductResponse> UpdateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails);
        Task<IEnumerable<Dsr>> getRetailorListByDistributorId(string distributorId);
        Task<IEnumerable<Dsr>> GetRetailorListByDate(string Id, DateTime date);
        Task<IEnumerable<Dsr>> getRetailorListByExecutiveId(string executiveId);
        Task<IEnumerable<DSRretailorDetails>> GetDetailsByIdAsync(string Id);
        Task<IEnumerable<DSRretailorDetails>> GetDetailsByIdAsyncdis(string DistributorId);


    }

}
