using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Natural_Core.Models;

namespace Natural_Core.IRepositories
{
	public interface IDsrdetailRepository: IRepository<Dsrdetail>
    {
        
        Task<IEnumerable<DsrProduct>> GetDsrDetailsByDsrIdAsync(string dsrId);
        Task<IEnumerable<Dsrdetail>> GetDetailTableByDsrIdAsync(string dsrId);
        Task<IEnumerable<GetProduct>> GetDetailTableAsync(string dsrId);

    }
}

