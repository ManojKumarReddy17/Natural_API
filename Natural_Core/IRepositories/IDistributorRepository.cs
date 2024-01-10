using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Core.IRepositories
{
    public interface IDistributorRepository : IRepository<Distributor>
    {
        Task<List<Distributor>> GetAllDistributorstAsync();

        Task<Distributor> GetDistributorDetailsByIdAsync(string id);
        Task<IEnumerable<Distributor>> SearchDistributorAsync(SearchModel search);




    }
}
