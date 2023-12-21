using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface IDSRdetailsService 
    {
        Task<IEnumerable<Dsrdetail>> GetAllDsrdetail();
        Task<ResultResponse> CreateDsrDetailsWithAssociationsAsync(Dsrdetail dsr);

    }
}
