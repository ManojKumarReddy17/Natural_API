using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IDSRService 
    {
        Task<Dsr> GetDsrById(string dsrId);
        Task<IEnumerable<Product>> GetProductsByDsrIdAsync(string dsrId);
        Task<Dsr> GetAllDetails(String id);
        Task<Dsr> GetDsrDetailsById(string DsrId);
        Task<DsrResponse> DeleteDsr(string dsrId);

        Task<Dsr> GetDsrById(string dsrId);
        Task<ResultResponse> CreateDsrWithAssociationsAsync(Dsr dsr);
        Task<IEnumerable<Dsr>> GetAllDsr();
    }
}
