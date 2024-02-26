using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Natural_Core.Models;

namespace Natural_Core.IRepositories
{
	public interface IDsrdetailRepository: IRepository<Dsrdetail>
    {
        Task<IEnumerable<Dsrdetail>> GetDsrDetailsByDsrIdAsync(string dsrId);
        //Task<IEnumerable<GetProduct>> GetDsrDetailsByDsrIdAsync(string dsrId);

    }
}

