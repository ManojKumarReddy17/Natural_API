using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface IDSRRepository :IRepository<Dsr>
    {

        Task<IEnumerable<Product>> GetProductDetailsByDsrIdAsync(string dsrId);
       Task<IEnumerable<Dsr>> GetAllDsrAsync();
        Task<Dsr> GetDetails(string dsrid);
    }
}
