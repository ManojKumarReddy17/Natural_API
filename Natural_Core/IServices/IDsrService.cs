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
        Task<ResultResponse> CreateDsrWithAssociationsAsync(Dsr dsr);
        Task<IEnumerable<Dsr>> GetAllDsr();
    }
}
